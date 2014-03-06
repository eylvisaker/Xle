using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

using AgateLib;
using AgateLib.InputLib;
using AgateLib.Geometry;
using AgateLib.Serialization.Xle;
using ERY.Xle.Maps;
using ERY.Xle.Maps.XleMapTypes.Extenders;
using ERY.Xle.XleEventTypes.Extenders;
using ERY.Xle.XleEventTypes;
using ERY.Xle.Commands;

namespace ERY.Xle
{
	public abstract class XleMap : IXleSerializable
	{
		//static int cyclesDraw = 0;

		string mMapName;                    // map name

		XleEventList mEvents = new XleEventList();
		List<EntryPoint> mEntryPoints = new List<EntryPoint>();

		// map number
		int mMapID;

		// stores which bitmap contains map tiles
		string mTileImage;

		int mDefaultTile;

		TileSet mTileSet;

		protected IMapExtender mBaseExtender;

		#region --- Construction and Seralization ---

		public XleMap()
		{
			mBaseExtender = new NullMapExtender();
			ColorScheme = new ColorScheme();
		}

		/// <summary>
		/// Initializes the map to the given width and height.
		/// Destroys existing map data.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public abstract void InitializeMap(int width, int height);


		#region IXleSerializable Members

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
			info.Write("DefaultTile", mDefaultTile);
			info.Write("Events", mEvents.ToArray());
			info.Write("EntryPoints", EntryPoints);

			if (this is IHasRoofs)
				info.Write("Roofs", ((IHasRoofs)this).Roofs);
			if (this is IHasGuards)
			{
				IHasGuards guard = (IHasGuards)this;

				info.Write("Guards", guard.Guards);
				info.Write("GuardDefaultAttack", guard.DefaultAttack);
				info.Write("GuardDefaultDefense", guard.DefaultDefense);
				info.Write("GuardDefaultHP", guard.DefaultHP);
				info.Write("GuardDefaultColor", guard.DefaultColor.ToArgb());
			}

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

			mDefaultTile = info.ReadInt32("DefaultTile");
			mEvents.AddRange(info.ReadArray<XleEvent>("Events"));

			if (this is IHasRoofs)
			{
				((IHasRoofs)this).Roofs.AddRange(info.ReadArray<Roof>("Roofs"));
			}
			if (this is IHasGuards)
			{
				IHasGuards guard = (IHasGuards)this;

				guard.Guards.AddRange(info.ReadArray<Guard>("Guards"));
				guard.DefaultAttack = info.ReadInt32("GuardDefaultAttack");
				guard.DefaultColor = Color.FromArgb(info.ReadInt32("GuardDefaultColor"));
				guard.DefaultDefense = info.ReadInt32("GuardDefaultDefense");
				guard.DefaultHP = info.ReadInt32("GuardDefaultHP");

				guard.InitializeGuardData();
			}

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


		public static XleMap LoadMap(string filename, int id)
		{
			if (System.IO.Path.GetExtension(filename).ToLower() != ".xmf")
				throw new ArgumentException("File extension not recognized.");

			XleSerializer ser = new XleSerializer(typeof(XleMap));
			ser.Binder = new XleTypeBinder(ser.Binder);

			XleMap retval;

			using (System.IO.Stream file = System.IO.File.Open(filename,
				System.IO.FileMode.Open, System.IO.FileAccess.Read))
			{
				retval = (XleMap)ser.Deserialize(file);
			}

			retval.MapID = id;
			retval.ConstructRenderTimeData();
			retval.mBaseExtender = retval.CreateExtender();
			retval.CreateEventExtenders();
			retval.GameState = XleCore.GameState;

			return retval;
		}

		protected internal virtual void ConstructRenderTimeData()
		{
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

		private IMapExtender CreateExtender()
		{
			var retval = CreateExtenderImpl();

			retval.TheMap = this;

			return retval;
		}
		protected virtual IMapExtender CreateExtenderImpl()
		{
			return new NullMapExtender();
		}

		protected virtual void CreateEventExtenders()
		{
			foreach (var evt in Events)
			{
				evt.CreateExtender(this);
			}
		}


		public T CreateEventExtender<T>(XleEvent evt) where T : IEventExtender, new()
		{
			return (T)CreateEventExtender(evt, typeof(T));
		}
		public IEventExtender CreateEventExtender(XleEvent evt, Type defaultExtender)
		{
			if (mBaseExtender != null)
				return mBaseExtender.CreateEventExtender(evt, defaultExtender);
			else
				return (IEventExtender)Activator.CreateInstance(defaultExtender);
		}

		#endregion
		#region --- Public Properties ---

		GameState theState;

		public GameState GameState
		{
			get { return theState; }
			set
			{
				theState = value;
			}
		}

		[Browsable(false)]
		public abstract int Width { get; }
		[Browsable(false)]
		public abstract int Height { get; }

		/// <summary>
		/// Gets or sets the default tile used in the map editor.
		/// </summary>
		[Browsable(false)]
		public int DefaultTile
		{
			get { return mDefaultTile; }
			set { mDefaultTile = value; }
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



		#endregion

		#region --- Abstract Properties ---

		/// <summary>
		/// Gets or sets map data.  Used for collision detection and the like.
		/// </summary>
		/// <param name="xx"></param>
		/// <param name="yy"></param>
		/// <returns></returns>
		public abstract int this[int xx, int yy] { get; set; }

		#endregion
		#region --- Virtual functions ---

		/// <summary>
		/// Returns what tile should be drawn at xx, yy.  Not used for collision
		/// detection.
		/// </summary>
		/// <param name="xx"></param>
		/// <param name="yy"></param>
		/// <returns></returns>
		public virtual int DrawTile(int xx, int yy)
		{
			return this[xx, yy];
		}

		/// <summary>
		/// Default color for message.
		/// </summary>
		public virtual Color DefaultColor
		{
			get { return XleColor.White; }
		}

		public void AfterExecuteCommand(Player player, KeyCode cmd)
		{
			AfterExecuteCommandImpl(player, cmd);

			mBaseExtender.AfterExecuteCommand(XleCore.GameState, cmd);
		}

		protected virtual void AfterExecuteCommandImpl(Player player, KeyCode cmd)
		{
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

			MapData retval = new MapData(rect.Width, rect.Height);

			for (int j = 0; j < rect.Height; j++)
			{
				for (int i = 0; i < rect.Width; i++)
				{
					retval[i, j] = this[rect.X + i, rect.Y + j];
				}
			}

			return retval;
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

		#endregion

		#region --- Old stuff ---

		void LoadMonsters()
		{
			/*
			DWORD error;
			int ptr = 0;

			HANDLE hMonst = LoadResource (g.hInstance(), 
				FindResource (g.hInstance(), MAKEINTRESOURCE(dat_Monst1), TEXT("MONST")));

			error = GetLastError();
			_ASSERT(!FAILED(error));
		

			BYTE *data = (unsigned char*)LockResource (hMonst);

			error = GetLastError();
			_ASSERT(!FAILED(error));

			for (int i = 0; i < 32; i++)
			{
				Monster current;
				string sTemp, sTemp2;
				int j;

				while(data[ptr] != 0x0D)
				{
					if (data[ptr] != 0x0A)
						sTemp += (char)data[ptr++];
					else
						ptr++;
				}
				ptr++;

				for (j = 0; j < len(sTemp) && sTemp[j] != 0x09; j++);
				current.mName = ltrim(left(sTemp, j));
				sTemp = mid(sTemp, j, len(sTemp));

				if (current.mName == "Name" || current.mName == "")
				{
					i--;
					continue;
				}

				current.mTerrain = ParseMonstValue(sTemp);
				current.mHP = ParseMonstValue(sTemp);
				current.mAttack = ParseMonstValue(sTemp);
				current.mDefense = ParseMonstValue(sTemp);
				current.mGold = ParseMonstValue(sTemp);
				current.mFood = ParseMonstValue(sTemp);
				current.mWeapon = ParseMonstValue(sTemp);
				current.mImage = ParseMonstValue(sTemp);
				current.mTalks = (ParseMonstValue(sTemp)) ? true : false;
				current.mFriendly = ParseMonstValue(sTemp);

				mMonst[i] = current;
			}

			FreeResource(hMonst);
		*/

		}
		int ParseMonstValue(ref string sTemp)
		{
			int j;
			for (j = 0; j < sTemp.Length && sTemp[j] != 0x09; j++) ;

			sTemp = sTemp.Substring(j);

			while (sTemp[0] == 0x09)
			{
				sTemp = sTemp.Substring(1);
			}

			return int.Parse(sTemp);

		}

		/*
		int CheckSpecial()
		{
			return CheckSpecial(g.player.X, g.player.Y);
		}

		int CheckSpecial(int x, int y)			// checks for special events at player coordinates
		{

			SpecialEvent se = GetSpecial(x, y);

			if (se.type != 0)
			{
				return se.type;
			}
			else
			{
				return 0;
			}

		}

		bool specialmarked(int i)
		{
			return spcMarked[i];
		}

		SpecialEvent GetSpecial()
		{
			return GetSpecial(g.player.X, g.player.Y);
		}
		*/
		#endregion

		#region --- Drawing ---

		protected Point centerPoint;

		public void Draw(int x, int y, Direction faceDirection, Rectangle inRect)
		{
			DrawImpl(x, y, faceDirection, inRect);
		}

		protected abstract void DrawImpl(int x, int y, Direction faceDirection, Rectangle inRect);
		protected void Draw2D(int x, int y, Direction faceDirction, Rectangle inRect)
		{
			int i, j;
			int initialxx = inRect.X;
			int width = inRect.Width / 16;
			int height = inRect.Height / 16;
			int wAdjust = 0;
			int hAdjust = 0;
			int tile;

			centerPoint = new Point(x, y);

			wAdjust = 1;
			hAdjust = 1;

			int xx = initialxx;
			int yy = 16;

			int startx = x - 11;
			int starty = y - 7;

			Rectangle tileRect = new Rectangle(startx, starty, width, height);
			AnimateTiles(tileRect);

			for (j = starty; j < starty + height; j++)
			{
				for (i = startx; i < startx + width; i++)
				{
					tile = DrawTile(i, j);

					XleCore.Renderer.DrawTile(xx, yy, tile);

					xx += 16;
				}

				yy += 16;
				xx = initialxx;
			}
		}

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

		[Obsolete("Use ColorScheme instead.", true)]
		public void GetBoxColors(out Color boxColor, out Color innerColor, out Color fontColor, out int vertLine)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region --- Events ---

		/// <summary>
		/// returns the special event at the player coordinates
		/// 
		/// null if there is none.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="border">How many tiles away from the player to consider</param>
		/// <returns></returns>
		[Obsolete("Use EventsAt or EnabledEventsAt instead.")]
		public XleEvent GetEvent(Player player, int border)
		{
			XleEvent evt = GetEvent(player.X, player.Y, border);
			if (evt != null) return evt;


			evt = GetEvent(player.X, player.Y + 1, border);
			if (evt != null) return evt;

			evt = GetEvent(player.X + 1, player.Y, border);
			if (evt != null) return evt;

			evt = GetEvent(player.X + 1, player.Y + 1, border);
			if (evt != null) return evt;

			return null;
		}
		[Obsolete("Use EventsAt or EnabledEventsAt instead.")]
		public T GetEvent<T>(Player player, int border) where T : XleEvent
		{
			XleEvent evt = GetEvent(player, border);

			return evt as T;
		}

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
		private IEnumerable<XleEvent> EventsAt(int px, int py, int border)
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
		#region --- Menu Stuff ---

		#endregion
		#region --- Player movement stuff ---

		public TileSet TileSet { get { return mTileSet; } set { mTileSet = value; } }

		public TerrainType Terrain(int xx, int yy)
		{
			int[,] t = new int[2, 2] { { 0, 0 }, { 0, 0 } };
			int[] tc = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };

			for (int j = 0; j < 2; j++)
			{
				for (int i = 0; i < 2; i++)
				{
					t[j, i] = this[xx + i, yy + j];
				}
			}

			for (int j = 0; j < 2; j++)
			{
				for (int i = 0; i < 2; i++)
				{
					tc[t[j, i] / 32]++;

					if (t[j, i] % 32 <= 1)
						tc[t[j, i] / 32] += 1;
				}
			}

			if (tc[(int)TerrainType.Mountain] > 4)
			{
				return TerrainType.Mountain;
			}

			if (tc[(int)TerrainType.Mountain] > 0)
			{
				return TerrainType.Foothills;
			}

			if (tc[(int)TerrainType.Desert] >= 1)
			{
				return TerrainType.Desert;
			}

			if (tc[(int)TerrainType.Swamp] > 1)
			{
				return TerrainType.Swamp;
			}

			for (int i = 0; i < 8; i++)
			{
				if (tc[i] > 3)
				{
					return (TerrainType)i;
				}
				else if (tc[i] == 2 && i != 1)
				{
					return TerrainType.Mixed;
				}
			}

			return (TerrainType)2;
		}

		/// <summary>
		/// Checks to see if the player can move in the specified direction.
		/// Returns true if the player can move in that direction.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		/// <returns></returns>
		public bool CheckMovement(Player player, int dx, int dy)
		{
			XleEvent evt = GetEvent(player.X + dx, player.Y + dy, 0);

			if (evt != null)
			{
				bool allowStep;

				evt.TryToStepOn(GameState, dx, dy, out allowStep);

				if (allowStep == false)
					return false;
			}

			return CheckMovementImpl(player, dx, dy);
		}
		/// <summary>
		/// Checks to see if the player can move in the specified direction.
		/// Returns true if the player can move in that direction.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		/// <returns></returns>
		protected abstract bool CheckMovementImpl(Player player, int dx, int dy);


		public void BeforeStepOn(Player player, int x, int y)
		{
			foreach (var evt in EventsAt(x, y, 0))
			{
				evt.BeforeStepOn(GameState);
			}
		}
		public void PlayerStep(Player player)
		{
			bool didEvent = false;

			foreach (var evt in EventsAt(player.X, player.Y, 0))
			{
				evt.StepOn(GameState);
				didEvent = true;
			}

			PlayerStepImpl(player, didEvent);
			mBaseExtender.PlayerStep(GameState);
		}
		/// <summary>
		/// Called after the player steps.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="didEvent">True if there was an event that occured at this location</param>
		protected virtual void PlayerStepImpl(Player player, bool didEvent)
		{

		}

		public abstract bool CanPlayerStepInto(Player player, int xx, int yy);

		public abstract void PlayerCursorMovement(Player player, Direction dir);

		protected void _Move2D(Player player, Direction dir, string textStart, out string command, out Point stepDirection)
		{
			player.FaceDirection = dir;

			command = textStart + " " + dir.ToString();
			int stepSize = mBaseExtender.StepSize;

			switch (dir)
			{
				case Direction.West:
					stepDirection = new Point(-stepSize, 0);
					break;

				case Direction.North:
					stepDirection = new Point(0, -stepSize);
					break;

				case Direction.East:
					stepDirection = new Point(stepSize, 0);
					break;

				case Direction.South:
					stepDirection = new Point(0, stepSize);
					break;

				default:
					stepDirection = Point.Empty;
					break;
			}

		}
		protected void _MoveDungeon(Player player, Direction dir, bool haveCompass, out string command, out Point stepDirection)
		{
			Direction newDirection;
			command = "";
			stepDirection = Point.Empty;

			switch (dir)
			{
				case Direction.East:
					command = "Turn Right";

					newDirection = player.FaceDirection - 1;

					if (newDirection < Direction.East)
						newDirection = Direction.South;


					player.FaceDirection = (Direction)newDirection;

					if (haveCompass)
						command = "Turn " + player.FaceDirection.ToString();

					break;

				case Direction.North:
					command = "Move Forward";

					stepDirection = StepDirection(player.FaceDirection);

					if (haveCompass)
						command = "Walk " + player.FaceDirection.ToString();

					player.TimeQuality += StepQuality;

					break;

				case Direction.West:
					command = "Turn Left";

					newDirection = player.FaceDirection + 1;


					if (newDirection > Direction.South)
						newDirection = Direction.East;

					player.FaceDirection = (Direction)newDirection;

					if (haveCompass)
						command = "Turn " + player.FaceDirection.ToString();

					break;

				case Direction.South:
					command = "Move Backward";

					if (player.FaceDirection == Direction.East)
						stepDirection = new Point(-1, 0);
					if (player.FaceDirection == Direction.North)
						stepDirection = new Point(0, 1);
					if (player.FaceDirection == Direction.West)
						stepDirection = new Point(1, 0);
					if (player.FaceDirection == Direction.South)
						stepDirection = new Point(0, -1);

					if (haveCompass)
					{
						// we're walking backwards here, so make the text work right!
						command = "Walk ";
						switch (player.FaceDirection)
						{
							case Direction.East: command += "West"; break;
							case Direction.West: command += "East"; break;
							case Direction.North: command += "South"; break;
							case Direction.South: command += "North"; break;
						}
					}

					player.TimeQuality += StepQuality;


					break;
			}
		}

		protected virtual double StepQuality { get { return 1; } }

		public static Point StepDirection(Direction dir)
		{
			switch (dir)
			{
				case Direction.East: return new Point(1, 0);
				case Direction.North: return new Point(0, -1);
				case Direction.West: return new Point(-1, 0);
				case Direction.South: return new Point(0, 1);

				default: throw new ArgumentException("Invalid direction!");
			}
		}
		public static Point LeftDirection(Direction dir)
		{
			switch (dir)
			{
				case Direction.East: return StepDirection(Direction.North);
				case Direction.North: return StepDirection(Direction.West);
				case Direction.West: return StepDirection(Direction.South);
				case Direction.South: return StepDirection(Direction.East);

				default: throw new ArgumentException("Invalid direction!");
			}
		}
		public static Point RightDirection(Direction dir)
		{
			switch (dir)
			{
				case Direction.East: return StepDirection(Direction.South);
				case Direction.North: return StepDirection(Direction.East);
				case Direction.West: return StepDirection(Direction.North);
				case Direction.South: return StepDirection(Direction.West);

				default: throw new ArgumentException("Invalid direction!");
			}
		}

		#endregion
		#region --- Player commands ---

		[Obsolete]
		public bool PlayerSpeak(Player player)
		{
			return PlayerSpeak(XleCore.GameState);
		}
		public bool PlayerSpeak(GameState state)
		{
			foreach (var evt in EnabledEventsAt(state.Player, 1))
			{
				bool handled = evt.Speak(state);

				if (handled)
					return handled;
			}

			return PlayerSpeakImpl(state.Player);
		}




		public virtual bool PlayerRob(GameState state)
		{
			return PlayerRobImpl(state);
		}
		[Obsolete]
		public virtual bool PlayerRob(Player player)
		{
			return PlayerRobImpl(player);
		}

		protected virtual bool PlayerRobImpl(Xle.GameState state)
		{
			return false;
		}

		protected virtual bool PlayerSpeakImpl(Player player)
		{
			return false;
		}
		protected virtual bool PlayerRobImpl(Player player)
		{
			return false;
		}

		public abstract bool PlayerFight(Player player);

		/// <summary>
		/// Function called when the player executes the Climb command.
		/// Returns true if the command was handled by this function, false
		/// if the caller should display a "Nothing to Climb" type message.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool PlayerClimb(Player player)
		{
			return false;
		}
		/// <summary>
		/// Function called when the player executes the Xamine command.
		/// Returns true if the command was handled by this function, false
		/// if the caller should display a "Nothing to Xamine" type message.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool PlayerXamine(Player player)
		{
			return false;
		}

		public virtual bool PlayerLeave(Player player)
		{
			return false;
		}

		public virtual bool PlayerTake(Player player)
		{
			foreach (var evt in EnabledEventsAt(player, 1))
			{
				if (evt.Take(GameState))
					return true;
			}

			return false;
		}

		public virtual bool PlayerOpen(Player player)
		{
			foreach (var evt in EnabledEventsAt(player, 1))
			{
				if (evt.Open(GameState))
					return true;
			}

			return false;
		}

		/// <summary>
		/// Returns true if there was an effect of using the item.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual bool PlayerUse(Player player, int item)
		{
			var state = GameState;
			bool handled = false;

			foreach (var evt in EventsAt(player, 1))
			{
				handled = evt.Use(state, item);

				if (handled)
					return handled;
			}

			mBaseExtender.PlayerUse(state, item, ref handled);

			return handled;
		}

		#endregion
		#region --- Animation ---

		public virtual bool AutoDrawPlayer
		{
			get { return true; }
		}

		#endregion

		public virtual void CheckSounds(Player player)
		{
		}

		public virtual void LeaveMap(Player player)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Leave " + XleCore.Map.MapName);
			XleCore.TextArea.PrintLine();

			XleCore.Wait(2000);

			player.ReturnToPreviousMap();

			XleCore.TextArea.PrintLine();
		}

		/// <summary>
		/// Called after a map is loaded.
		/// </summary>
		/// <param name="player"></param>
		public virtual void OnLoad(Player player)
		{
			mBaseExtender.OnLoad(GameState);
			mBaseExtender.SetColorScheme(this.ColorScheme);

			SetChestIDs();

			foreach (var evt in Events)
			{
				evt.OnLoad(XleCore.GameState);
			}
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



		public static Direction DirectionFromPoint(Point point)
		{
			if (point.X < 0 && point.Y == 0) return Direction.West;
			if (point.X > 0 && point.Y == 0) return Direction.East;
			if (point.X == 0 && point.Y < 0) return Direction.North;
			if (point.X == 0 && point.Y > 0) return Direction.South;

			throw new ArgumentException();
		}

		public string ExtenderName { get; set; }



		public void BeforeEntry(GameState state, ref int targetEntryPoint)
		{
			mBaseExtender.BeforeEntry(state, ref targetEntryPoint);
		}

		public virtual void OnAfterEntry(Xle.GameState state)
		{
			mBaseExtender.OnAfterEntry(state);
		}

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

		public void SetCommands(CommandList commands)
		{
			mBaseExtender.SetCommands(commands);
		}
	}

	public class Roof : IXleSerializable
	{
		Rectangle mRect;
		int[] mData;

		private bool mOpen;


		#region --- Construction and Serialization ---



		public Roof()
		{ }

		void IXleSerializable.WriteData(XleSerializationInfo info)
		{
			info.Write("X", X);
			info.Write("Y", Y);
			info.Write("Width", Width);
			info.Write("Height", Height);
			info.Write("RoofData", mData, NumericEncoding.Csv);
		}

		void IXleSerializable.ReadData(XleSerializationInfo info)
		{
			mRect.X = info.ReadInt32("X");
			mRect.Y = info.ReadInt32("Y");
			mRect.Width = info.ReadInt32("Width");
			mRect.Height = info.ReadInt32("Height");
			mData = info.ReadInt32Array("RoofData");
		}

		#endregion

		public Point Location
		{
			get { return mRect.Location; }
			set { mRect.Location = value; }
		}
		public int X
		{
			get { return mRect.X; }
			set { mRect.X = value; }
		}
		public int Y
		{
			get { return mRect.Y; }
			set { mRect.Y = value; }
		}

		public bool Open
		{
			get { return mOpen; }
			set { mOpen = value; }
		}

		public void SetSize(int width, int height)
		{
			int[] newData = new int[height * width];

			// copy old data to new data
			if (mData != null)
			{
				for (int i = 0; i < Math.Min(Width, width); i++)
				{
					for (int j = 0; j < Math.Min(Height, height); j++)
					{
						newData[i + j * width] = mData[i + j * Width];
					}
				}
			}

			mRect.Width = width;
			mRect.Height = height;

			mData = newData;
		}
		public int Width
		{
			get { return mRect.Width; }
		}
		public int Height
		{
			get { return mRect.Height; }
		}

		public int this[int x, int y]
		{
			get { return mData[x + y * Width]; }
			set { mData[x + y * Width] = value; }
		}


		public Rectangle Rectangle
		{
			get
			{
				return new Rectangle(Location, new Size(Width, Height));
			}
		}

		public bool CharIn(int ptx, int pty)
		{
			return CharIn(ptx, pty, false);
		}
		public bool CharIn(int ptx, int pty, bool ignoreTransparency)
		{
			if (PointInRoof(ptx, pty, ignoreTransparency))
			{
				return true;
			}
			else if (PointInRoof(ptx + 1, pty, ignoreTransparency))
			{
				return true;
			}
			else if (PointInRoof(ptx, pty + 1, ignoreTransparency))
			{
				return true;
			}
			else if (PointInRoof(ptx + 1, pty + 1, ignoreTransparency))
			{
				return true;
			}
			else
				return false;
		}
		public bool CharIn(Point pt, bool ignoreTransparency)
		{
			return CharIn(pt.X, pt.Y, ignoreTransparency);
		}
		public bool CharIn(Point pt)
		{
			return CharIn(pt.X, pt.Y, false);
		}

		public bool PointInRoof(int ptx, int pty)
		{
			return PointInRoof(ptx, pty, false);
		}
		public bool PointInRoof(int ptx, int pty, bool ignoreTransparency)
		{
			if (Rectangle.Contains(ptx, pty))
			{
				if (ignoreTransparency == false && (this[ptx - X, pty - Y] == 127 || this[ptx - X, pty - Y] == 0))
					return false;

				return true;
			}

			return false;
		}

	}

	[Serializable]
	public class Guard : IXleSerializable
	{
		public Guard()
		{
			Name = "Guard";
			Facing = Direction.South;
			Color = XleColor.Yellow;
		}

		public Point Location
		{
			get { return new Point(X, Y); }
			set
			{
				X = value.X;
				Y = value.Y;
			}
		}
		public int HP { get; set; }
		public Direction Facing { get; set; }
		public Color Color { get; set; }

		public int Attack { get; set; }
		public int Defense { get; set; }

		public int X { get; set; }
		public int Y { get; set; }

		/// <summary>
		/// Method called when attacked by the player.
		/// Return true to cancel further processing of the attack.
		/// </summary>
		public Func<GameState, Guard, bool> OnPlayerAttack;
		public Func<GameState, Guard, bool> OnGuardDead;
		public bool SkipAttacking { get; set; }

		#region IXleSerializable Members

		void IXleSerializable.WriteData(XleSerializationInfo info)
		{
			info.Write("X", X);
			info.Write("Y", Y);
			info.Write("Color", Color.ToArgb());

			info.Write("HP", HP);
			info.Write("Attack", Attack);
			info.Write("Defense", Defense);
		}

		void IXleSerializable.ReadData(XleSerializationInfo info)
		{
			X = info.ReadInt32("X");
			Y = info.ReadInt32("Y");
			Color = Color.FromArgb(info.ReadInt32("Color"));

			HP = info.ReadInt32("HP");
			Attack = info.ReadInt32("Attack");
			Defense = info.ReadInt32("Defense");
		}

		#endregion

		public string Name { get; set; }
	}
}