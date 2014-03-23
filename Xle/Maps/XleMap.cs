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

namespace ERY.Xle.Maps
{
	public abstract class XleMap : IXleSerializable
	{
		string mMapName;
		int mMapID;
		int mDefaultTile;

		string mTileImage;
		TileSet mTileSet;

		List<Roof> mRoofs;
		GuardList mGuards;

		protected MapExtender mBaseExtender;

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
			info.Write("DefaultTile", mDefaultTile);
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

			mDefaultTile = info.ReadInt32("DefaultTile");
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
			retval.mBaseExtender = retval.CreateExtender();
			retval.CreateEventExtenders();
			retval.GameState = XleCore.GameState;

			return retval;
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
			var retval = CreateExtenderImpl();

			retval.TheMap = this;

			return retval;
		}
		protected virtual MapExtender CreateExtenderImpl()
		{
			return new MapExtender();
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
			mBaseExtender.OnLoad(GameState);
			mBaseExtender.SetColorScheme(this.ColorScheme);

			SetChestIDs();

			foreach (var evt in Events)
			{
				evt.OnLoad(XleCore.GameState);
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
		public virtual int TileToDraw(int xx, int yy)
		{
			return this[xx, yy];
		}

		/// <summary>
		/// Default color for text messages.
		/// </summary>
		[Obsolete("Move this to the color scheme class.")]
		public virtual Color DefaultColor
		{
			get { return XleColor.White; }
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
			int tile;

			centerPoint = new Point(x, y);

			int xx = initialxx;
			int yy = 16;

			int startx = x - 11;
			int starty = y - 7;

			Rectangle tileRect = new Rectangle(startx, starty, width, height);

			if (HasGuards)
				Guards.AnimateGuards();

			AnimateTiles(tileRect);

			for (j = starty; j < starty + height; j++)
			{
				for (i = startx; i < startx + width; i++)
				{
					tile = TileToDraw(i, j);

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

		public bool CanPlayerStep(Player player, Point stepDirection)
		{
			return CanPlayerStep(player, stepDirection.X, stepDirection.Y);
		}

		/// <summary>
		/// Checks to see if the player can move in the specified direction.
		/// Returns true if the player can move in that direction.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		/// <returns></returns>
		public bool CanPlayerStep(Player player, int dx, int dy)
		{
			throw new NotImplementedException();
			//return mBaseExtender.CanPlayerStep(XleCore.GameState, dx, dy);
		}

		public void BeforeStepOn(Player player, int x, int y)
		{
			mBaseExtender.BeforeStepOn(XleCore.GameState, x, y);
		}
		public void AfterPlayerStep(Player player)
		{
			mBaseExtender.AfterPlayerStep(XleCore.GameState);
		}

		/// <summary>
		/// Called after the player steps.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="didEvent">True if there was an event that occured at this location</param>
		protected virtual void AfterStepImpl(Player player, bool didEvent)
		{

		}

		public virtual bool CanPlayerStepIntoImpl(Player player, int xx, int yy)
		{
			return mBaseExtender.CanPlayerStepIntoImpl(player, xx, yy);
		}

		public virtual void PlayerCursorMovement(Player player, Direction dir)
		{
			mBaseExtender.PlayerCursorMovement(XleCore.GameState, dir);
		}

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

		public virtual double StepQuality { get { return 1; } }

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

		public void AfterExecuteCommand(Player player, KeyCode cmd)
		{
			AfterExecuteCommandImpl(player, cmd);

			mBaseExtender.AfterExecuteCommand(XleCore.GameState, cmd);
		}

		protected virtual void AfterExecuteCommandImpl(Player player, KeyCode cmd)
		{
		}

		public bool PlayerSpeak(GameState state)
		{
			return mBaseExtender.PlayerSpeak(XleCore.GameState);
		}

		public virtual bool PlayerRob(GameState state)
		{
			return mBaseExtender.PlayerRob(XleCore.GameState);
		}

		protected virtual bool PlayerRobImpl(Xle.GameState state)
		{
			return mBaseExtender.PlayerRob(XleCore.GameState);
		}

		protected virtual bool PlayerSpeakImpl(Player player)
		{
			return mBaseExtender.PlayerSpeak(XleCore.GameState);
		}
		protected virtual bool PlayerRobImpl(Player player)
		{
			return mBaseExtender.PlayerRob(XleCore.GameState);
		}

		public virtual bool PlayerFight(Player player)
		{
			return mBaseExtender.PlayerFight(XleCore.GameState);
		}

		/// <summary>
		/// Function called when the player executes the Climb command.
		/// Returns true if the command was handled by this function, false
		/// if the caller should display a "Nothing to Climb" type message.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool PlayerClimb(Player player)
		{
			return mBaseExtender.PlayerClimb(XleCore.GameState);
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
			return mBaseExtender.PlayerXamine(XleCore.GameState);
		}

		public virtual bool PlayerLeave(Player player)
		{
			return mBaseExtender.PlayerLeave(XleCore.GameState);
		}

		public virtual bool PlayerTake(Player player)
		{
			return mBaseExtender.PlayerTake(XleCore.GameState);
		}

		public virtual bool PlayerOpen(Player player)
		{
			return mBaseExtender.PlayerOpen(XleCore.GameState);
		}

		/// <summary>
		/// Returns true if there was an effect of using the item.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual bool PlayerUse(Player player, int item)
		{
			return mBaseExtender.PlayerUse(XleCore.GameState, item);

		}


		public virtual bool PlayerDisembark(Xle.GameState state)
		{
			return mBaseExtender.PlayerDisembark(state);
		}

		public virtual void PlayerMagic(GameState state)
		{
			mBaseExtender.PlayerMagic(state);
		}

		protected virtual void PlayerMagicImpl(GameState state, MagicSpell magic)
		{
		}


		#endregion

		GameState theState;
		public GameState GameState
		{
			get { return theState; }
			set
			{
				theState = value;
			}
		}

		public virtual void CheckSounds(Player player)
		{
		}

		public virtual void LeaveMap(Player player)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Leave " + MapName);
			XleCore.TextArea.PrintLine();

			XleCore.Wait(2000);

			player.ReturnToPreviousMap();

			XleCore.TextArea.PrintLine();
		}

		public static Direction DirectionFromPoint(Point point)
		{
			if (point.X < 0 && point.Y == 0) return Direction.West;
			if (point.X > 0 && point.Y == 0) return Direction.East;
			if (point.X == 0 && point.Y < 0) return Direction.North;
			if (point.X == 0 && point.Y > 0) return Direction.South;

			throw new ArgumentException();
		}

		public void BeforeEntry(GameState state, ref int targetEntryPoint)
		{
			mBaseExtender.BeforeEntry(state, ref targetEntryPoint);
		}

		public virtual void OnAfterEntry(Xle.GameState state)
		{
			mBaseExtender.AfterEntry(state);
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
	}
}