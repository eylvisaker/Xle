using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

using AgateLib;
using AgateLib.InputLib;
using AgateLib.IO;
using AgateLib.Geometry;
using AgateLib.Serialization.Xle;
using ERY.Xle.Maps;
using ERY.Xle.Maps.Extenders;
using ERY.Xle.Services.Implementation;
using ERY.Xle.XleEventTypes.Extenders;
using ERY.Xle.XleEventTypes;


namespace ERY.Xle.Maps
{
	public abstract class XleMap : IXleSerializable
	{
		string mMapName;
		int mMapID;
		int mOutsideTile;

		string mTileImage;
		TileSet mTileSet;

		List<Roof> mRoofs;
		GuardList mGuards;

		public MapExtender mBaseExtender;

		XleEventList mEvents = new XleEventList();
		List<EntryPoint> mEntryPoints = new List<EntryPoint>();

		#region --- Construction and Seralization ---

		public XleMap()
		{
			mBaseExtender = new MapExtender();
			ColorScheme = new ColorScheme();
		}

		/// <summary>
		/// Initializes the map to the given width and height.
		/// Destroys existing map data.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public abstract void InitializeMap(int width, int height);


		#region --- IXleSerializable Members ---

		void IXleSerializable.WriteData(XleSerializationInfo info)
		{
			if (EntryPoints == null)
				EntryPoints = new List<EntryPoint>();
			while (EntryPoints.Count > 0 && EntryPoints.Last().Location == Point.Empty)
				EntryPoints.RemoveAt(EntryPoints.Count - 1);

			info.Write("MapName", mMapName);
			info.Write("ExtenderName", ExtenderName);

			info.Write("MapID", mMapID);
			info.Write("TileSet", mTileSet);
			info.Write("TileImage", mTileImage);
			info.Write("OutsideTile", mOutsideTile);
			info.Write("Events", mEvents.ToArray());
			info.Write("EntryPoints", EntryPoints);

			if (HasRoofs) info.Write("Roofs", Roofs);
			if (HasGuards) info.Write("Guards", Guards);

			WriteData(info);
		}
		void IXleSerializable.ReadData(XleSerializationInfo info)
		{
			mMapName = info.ReadString("MapName");
			mMapID = info.ReadInt32("MapID");

			ExtenderName = info.ReadString("ExtenderName", "");

			if (info.ContainsKey("EntryPoints"))
			{
				EntryPoints = info.ReadList<EntryPoint>("EntryPoints");
			}
			if (info.ContainsKey("Tileset"))
			{
				mTileImage = info.ReadString("Tileset");
			}
			else
			{
				mTileImage = info.ReadString("TileImage");
				mTileSet = info.ReadObject<TileSet>("TileSet");
			}

			var defaultTile = info.ReadInt32("DefaultTile", 0);
			mOutsideTile = info.ReadInt32("OutsideTile", 0);

			if (mOutsideTile == 0)
				mOutsideTile = defaultTile;

			mEvents.AddRange(info.ReadArray<XleEvent>("Events"));

			if (info.ContainsKey("Roofs")) Roofs = info.ReadList<Roof>("Roofs");
			if (info.ContainsKey("Guards")) Guards = info.ReadObject<GuardList>("Guards");

			// read events
			ReadData(info);
		}

		/// <summary>
		/// Override this to read type specific data
		/// </summary>
		/// <param name="info"></param>
		protected virtual void ReadData(XleSerializationInfo info)
		{
		}

		/// <summary>
		/// Writes any data that is a member of the base class, or the IHasRoofs or IHasGuards interfaces.
		/// </summary>
		/// <param name="info"></param>
		protected virtual void WriteData(XleSerializationInfo info)
		{
		}

		#endregion

        [Obsolete("Use MapLoader service instead")]
		public static XleMap LoadMap(string filename, int id)
		{
			if (System.IO.Path.GetExtension(filename).ToLower() != ".xmf")
				throw new ArgumentException("File extension not recognized.");

			XleSerializer ser = new XleSerializer(typeof(XleMap));
			ser.Binder = new XleTypeBinder(ser.Binder);

			XleMap result;

			using (var file = FileProvider.Assets.OpenRead(filename))
			{
				result = (XleMap)ser.Deserialize(file);
			}

			result.MapID = id;
			result.mBaseExtender = result.CreateExtender();
			result.CreateEventExtenders();

			return result;
		}

		public static void SaveMap(XleMap map, string filename)
		{
			if (System.IO.Path.GetExtension(filename).ToLower() == ".xmf")
			{
				XleSerializer ser = new XleSerializer(typeof(XleMap));

				using (System.IO.Stream file = System.IO.File.Open(filename,
				   System.IO.FileMode.Create, System.IO.FileAccess.Write))
				{
					ser.Serialize(file, map);
				}
			}
		}

		private MapExtender CreateExtender()
		{
			var result = CreateExtenderImpl();

			result.TheMap = this;

			return result;
		}
		protected virtual MapExtender CreateExtenderImpl()
		{
			return new MapExtender();
		}

		public virtual void CreateEventExtenders()
		{
			foreach (var evt in Events)
			{
				evt.CreateExtender(this);
			}
		}

		public T CreateEventExtender<T>(XleEvent evt) where T : EventExtender, new()
		{
			return (T)CreateEventExtender(evt, typeof(T));
		}
		public EventExtender CreateEventExtender(XleEvent evt, Type defaultExtender)
		{
			if (mBaseExtender != null)
				return mBaseExtender.CreateEventExtender(evt, defaultExtender);
			else
				return (EventExtender)Activator.CreateInstance(defaultExtender);
		}

		private void SetChestIDs()
		{
			int index = 0;
			foreach (TreasureChestEvent chest in Events.OfType<TreasureChestEvent>())
			{
				chest.ChestID = index;
				index++;
			}
		}

		/// <summary>
		/// Called after a map is loaded.
		/// </summary>
		/// <param name="player"></param>
		public virtual void OnLoad(Player player)
		{
			mBaseExtender.OnLoad(XleCore.GameState);
			mBaseExtender.SetColorScheme(this.ColorScheme);

			SetChestIDs();

			foreach (var evt in Events)
			{
				evt.Extender.OnLoad(XleCore.GameState);
			}
		}

		#endregion
		#region --- Public Properties ---

		public MapExtender Extender { get { return mBaseExtender; } }

		public string ExtenderName { get; set; }

		[Browsable(false)]
		public abstract int Width { get; }
		[Browsable(false)]
		public abstract int Height { get; }

		/// <summary>
		/// Gets or sets the default tile used in the map editor.
		/// </summary>
		[Browsable(false)]
		public int OutsideTile
		{
			get { return mOutsideTile; }
			set { mOutsideTile = value; }
		}

		/// <summary>
		/// Gets the number of levels on this map.
		/// </summary>
		public virtual int Levels
		{
			get { return 1; }
		}
		/// <summary>
		/// Gets whether or not this type of map supports multiple levels.
		/// </summary>
		[Browsable(false)]
		public virtual bool IsMultiLevelMap
		{
			get { return false; }
		}

		/// <summary>
		/// Sets the number of levels on this map.
		/// 
		/// This should preserve existing map data, for levels between 1 and Math.Min(Levels, count).
		/// </summary>
		/// <param name="count"></param>
		public virtual void SetLevels(int count)
		{
			if (IsMultiLevelMap == false)
				throw new InvalidOperationException("Maptype does not support multiple levels.");
			else
				throw new NotImplementedException("SetLevels is not implemented.");
		}

		public List<EntryPoint> EntryPoints
		{
			get
			{
				return mEntryPoints;
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException();

				mEntryPoints = value;
			}
		}

		[Browsable(false)]
		public string MapName
		{
			get
			{
				return mMapName;
			}
			set
			{
				mMapName = value;
			}
		}
		[Browsable(false)]
		public string TileImage
		{
			get { return mTileImage; }
			set { mTileImage = value; }
		}

		public virtual IEnumerable<string> AvailableTileImages
		{
			get
			{
				yield break;
			}
		}

		[Browsable(false)]
		public int MapID
		{
			get { return mMapID; }
			set { mMapID = value; }
		}

		[Browsable(false)]
		public XleEventList Events
		{
			get { return mEvents; }
			set { mEvents = value; }
		}


		public bool HasRoofs
		{
			get { return mRoofs != null; }
			set { mRoofs = new List<Roof>(); }
		}
		public bool HasGuards
		{
			get { return mGuards != null; }
			set { mGuards = new GuardList(); }
		}

		public List<Roof> Roofs
		{
			get { return mRoofs; }
			set { mRoofs = value; }
		}
		public GuardList Guards
		{
			get { return mGuards; }
			set { mGuards = value; }
		}

		public TileSet TileSet { get { return mTileSet; } set { mTileSet = value; } }

		#endregion

		#region --- Abstract Properties ---

		/// <summary>
		/// Gets or sets map data.  Used for collision detection and the like.
		/// </summary>
		/// <param name="xx"></param>
		/// <param name="yy"></param>
		/// <returns></returns>
		public abstract int this[int x, int y] { get; set; }

		public int this[Point location]
		{
			get { return this[location.X, location.Y]; }
			set { this[location.X, location.Y] = value; }
		}

		#endregion

		#region --- Reading / Writing map data ---

		public void CopyMapDataFrom(XleMap map)
		{
			InitializeMap(map.Width, map.Height);

			for (int j = 0; j < map.Height; j++)
			{
				for (int i = 0; i < map.Width; i++)
				{
					this[i, j] = map[i, j];
				}
			}
		}

		public MapData ReadMapData(int x, int y, int width, int height)
		{
			return ReadMapDataImpl(new Rectangle(x, y, width, height));
		}
		public MapData ReadMapData(Rectangle rect)
		{
			return ReadMapDataImpl(rect);
		}
		protected virtual MapData ReadMapDataImpl(Rectangle rect)
		{
			if (rect.Right >= this.Width) throw new ArgumentOutOfRangeException();
			if (rect.Bottom >= this.Height) throw new ArgumentOutOfRangeException();

			MapData result = new MapData(rect.Width, rect.Height);

			for (int j = 0; j < rect.Height; j++)
			{
				for (int i = 0; i < rect.Width; i++)
				{
					result[i, j] = this[rect.X + i, rect.Y + j];
				}
			}

			return result;
		}

		public void WriteMapData(MapData data, int x, int y)
		{
			WriteMapDataImpl(data, new Point(x, y));
		}
		public void WriteMapData(MapData data, Point target)
		{
			WriteMapDataImpl(data, target);
		}
		private void WriteMapDataImpl(MapData data, Point target)
		{
			if (target.X + data.Width > this.Width) throw new ArgumentOutOfRangeException();
			if (target.Y + data.Height > this.Height) throw new ArgumentOutOfRangeException();

			for (int j = 0; j < data.Height; j++)
			{
				for (int i = 0; i < data.Width; i++)
				{
					this[target.X + i, target.Y + j] = data[i, j];
				}
			}
		}

		[Obsolete("Use ClosedRoofAt instead.", true)]
		public int PointInRoof(int ptx, int pty)
		{
			for (int i = 0; i < mRoofs.Count; i++)
			{
				if (mRoofs[i].PointInRoof(ptx, pty, false))
				{
					if (mRoofs[i].Open)
						return -1;
					else
						return i;
				}
			}

			return -1;
		}

		public Roof ClosedRoofAt(int x, int y)
		{
			Roof result = null;

			foreach(var roof in Roofs)
			{
				if (roof.PointInRoof(x, y, false))
				{
					if (roof.Open)
						return null;
					else
						result = roof;
				}
			}

			return result;
		}

		#endregion

		#region --- Drawing ---

		protected Point centerPoint;

		public void Draw(int x, int y, Direction faceDirection, Rectangle inRect)
		{
			DrawImpl(x, y, faceDirection, inRect);
		}

		protected virtual void DrawImpl(int x, int y, Direction faceDirection, Rectangle inRect)
		{ }

		private IEnumerable<TileGroup> GetGroupsToAnimate()
		{
			if (TileSet == null)
				yield break;

			foreach (var group in TileSet.TileGroups)
			{
				if (group.AnimationType == AnimationType.None)
					continue;
				if (group.Tiles.Count < 2)
					continue;

				group.TimeSinceLastAnim += AgateLib.DisplayLib.Display.DeltaTime;

				if (group.TimeSinceLastAnim >= group.AnimationTime)
				{
					group.TimeSinceLastAnim %= group.AnimationTime;
					yield return group;
				}
			}
		}

		protected virtual void AnimateTiles(Rectangle rectangle)
		{
			List<TileGroup> groupsToAnimate = GetGroupsToAnimate().ToList();

			if (groupsToAnimate.Count == 0)
				return;

			for (int j = 0; j <= Height; j++)
			{
				for (int i = 0; i <= Width; i++)
				{
					int current = this[i, j];
					TileGroup group = groupsToAnimate.FirstOrDefault(x => x.Tiles.Contains(current));

					if (group == null) continue;
					if (group.AnimationType == AnimationType.None) continue;

					int nextTile = current;

					switch (group.AnimationType)
					{
						case AnimationType.Loop:
							int index = group.Tiles.IndexOf(current);
							if (index + 1 >= group.Tiles.Count)
							{
								index = 0;
							}
							else
								index++;

							nextTile = group.Tiles[index];
							break;

						case AnimationType.Random:
							while (nextTile == current)
								nextTile = group.Tiles[XleCore.random.Next(group.Tiles.Count)];

							break;
					}

					if (group.AnimateChance == 100 || XleCore.random.Next(100) < group.AnimateChance)
					{
						this[i, j] = nextTile;
					}
				}
			}
		}

		public ColorScheme ColorScheme { get; private set; }

		#endregion
		#region --- Animation ---

		public virtual bool AutoDrawPlayer
		{
			get { return true; }
		}

		#endregion
		#region --- Events ---

		public IEnumerable<XleEvent> EnabledEventsAt(Player player, int border)
		{
			return EventsAt(player, border).Where(e => e.Enabled);
		}
		public IEnumerable<XleEvent> EventsAt(Player player, int border)
		{
			int px = player.X;
			int py = player.Y;

			return EventsAt(px, py, border);
		}
		public IEnumerable<XleEvent> EventsAt(int px, int py, int border)
		{
			foreach (var e in mEvents)
			{
				bool found = false;

				if (e.Enabled == false)
					continue;

				for (int j = 0; j < 2; j++)
				{
					for (int i = 0; i < 2; i++)
					{
						int x = px + i;
						int y = py + j;

						if (x >= e.Rectangle.X - border && y >= e.Rectangle.Y - border &&
							x < e.Rectangle.Right + border && y < e.Rectangle.Bottom + border)
						{
							found = true;

						}
					}
				}

				if (found)
					yield return e;
			}
		}

		/// <summary>
		/// returns the special event at the specified location
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public XleEvent GetEvent(int x, int y, int border)
		{
			for (int i = 0; i < mEvents.Count; i++)
			{
				XleEvent e = mEvents[i];

				if (x >= e.Rectangle.X - border && y >= e.Rectangle.Y - border &&
					x < e.Rectangle.Right + border && y < e.Rectangle.Bottom + border)
				{
					return e;
				}
			}

			return null;
		}

		#endregion

		#region --- Player movement stuff ---


		public virtual double StepQuality { get { return 1; } }

		#endregion


		public void RemoveJailBars(Rectangle rectangle, int replacementTile)
		{
			for (int j = 0; j < rectangle.Height; j++)
			{
				for (int i = 0; i < rectangle.Width; i++)
				{
					var tile = this[rectangle.X + i, rectangle.Y + j];
					var group = TileSet.TileGroups.FirstOrDefault(
						x => x.Tiles.Contains(tile) &&
						x.GroupType == GroupType.PrisonBars);

					if (group == null)
						continue;

					this[rectangle.X + i, rectangle.Y + j] = replacementTile;
				}
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual int WaitTimeAfterStep
		{
			get
			{
				return XleCore.GameState.GameSpeed.GeneralStepTime;
			}
		}

		public virtual void GuardAttackPlayer(Player player, Guard guard)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Executes the movement of the player in a certain direction.
		/// Assumes validation has already been performed. Call CanPlayerStep
		/// first to check to see if the movement is valid.
		/// </summary>
		/// <param name="state"></param>
		/// <param name="stepDirection"></param>
		protected virtual void MovePlayer(GameState state, Point stepDirection)
		{
			mBaseExtender.MovePlayer(state, stepDirection);
		}

		public void OnUpdate(GameState state, double deltaTime)
		{
			foreach(var evt in Events)
			{
				evt.OnUpdate(state, deltaTime);
			}
		}
	}
}