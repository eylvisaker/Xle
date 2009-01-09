using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

using AgateLib;
using AgateLib.InputLib;
using AgateLib.Geometry;
using ERY.Xle.Serialization;
using System.ComponentModel;

namespace ERY.Xle
{
    // Map Type definitions
    //public enum MapTypes
    //{
    //    Museum = 1,
    //    Outside,
    //    Town,
    //    Dungeon,
    //    Castle
    //}

    [Serializable]
    public abstract class XleMap : IXleSerializable
    {
        static int lastTimeDraw = 0;
        static int cyclesDraw = 0;

        string mMapName;                    // map name

        List<XleEvent> mEvents = new List<XleEvent>();

        int mMapID;					// map number
        string mTileSet;				// stores which bitmap contains map tiles
        int mDefaultTile;

        #region --- Construction and Seralization ---

        public XleMap()
        {
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
            info.Write("MapName", mMapName);
            info.Write("MapID", mMapID);
            info.Write("Tileset", mTileSet);
            info.Write("DefaultTile", mDefaultTile);
            info.Write("Events", mEvents);

            if (this is IHasRoofs)
                info.Write("Roofs", ((IHasRoofs)this).Roofs);
            if (this is IHasGuards)
            {
                IHasGuards guard = (IHasGuards)this;

                info.Write("Guards",guard.Guards);
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
            mTileSet = info.ReadString("Tileset");
            mDefaultTile = info.ReadInt32("DefaultTile");
            mEvents.AddRange((XleEvent[])info.ReadArray("Events"));

            if (this is IHasRoofs)
            {
                ((IHasRoofs)this).Roofs.AddRange((Roof[])info.ReadArray("Roofs"));
            }
            if (this is IHasGuards)
            {
                IHasGuards guard = (IHasGuards)this;

                guard.Guards.AddRange((Guard[])info.ReadArray("Guards"));
                guard.DefaultAttack = info.ReadInt32("GuardDefaultAttack");
                guard.DefaultColor = Color.FromArgb(info.ReadInt32("GuardDefaultColor"));
                guard.DefaultDefense = info.ReadInt32("GuardDefaultDefense");
                guard.DefaultHP = info.ReadInt32("GuardDefaultHP");
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
            if (System.IO.Path.GetExtension(filename).ToLower() == ".bmf")
            {
                IFormatter formatter = new BinaryFormatter();

                XleMap retval;

                using (System.IO.Stream file = System.IO.File.Open(filename,
                    System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    retval = (XleMap)formatter.Deserialize(file);
                }

                retval.MapID = id;

                return retval;
            }
            else if (System.IO.Path.GetExtension(filename).ToLower() == ".xmf")
            {
                Serialization.XleSerializer ser = new ERY.Xle.Serialization.XleSerializer(typeof(XleMap));

                XleMap retval;

                using (System.IO.Stream file = System.IO.File.Open(filename,
                    System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    retval = (XleMap)ser.Deserialize(file);
                }

                retval.MapID = id;

                return retval;
            }
            else
                throw new ArgumentException("File extension not recognized.");
        }

        public static void SaveMap(XleMap map, string filename)
        {
            if (System.IO.Path.GetExtension(filename).ToLower() == ".xmf")
            {
                Serialization.XleSerializer ser = new ERY.Xle.Serialization.XleSerializer(typeof(XleMap));

                using (System.IO.Stream file = System.IO.File.Open(filename,
                   System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    ser.Serialize(file, map);
                }
            }
        }

        #endregion
        #region --- Public Properties ---

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
            if (IsMultiLevelMap)
                throw new InvalidOperationException("Maptype does not support multiple levels.");
            else
                throw new NotImplementedException("SetLevels is not implemented.");
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
        public string TileSet
        {
            get { return mTileSet; }
            set { mTileSet = value; }
        }
        public virtual IEnumerable<string> AvailableTilesets
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
        public List<XleEvent> Events
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
        public abstract int this[int xx, int yy] { get;set; }

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

        public virtual void AfterExecuteCommand(Player player, KeyCode cmd)
        {
        }

        #endregion

        #region --- Reading / Writing map data ---

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



        [Obsolete]
        int GetMapResource(int map)
        {
            return 0;
            /*
            int resID = 0;
	
            switch (map)
            {
            case 0:
                return 0;

            case 1:
                resID = map_OUTSIDE1;

                break;

            case 2:
                resID = map_OUTSIDE2;

                break;
            case 3:
                //resID = map_OUTSIDE3;
                break;

            case 10:
                resID = map_ThompsonCrossing;
                break;
	
            case 11:
                resID = map_Thornberry;
                break;

            case 12:
                resID = map_Alanville;
                break;

            case 13:
                resID = map_IsleCity;
                break;

            case 14:
                resID = map_Cobbleton;
                break;

            case 15:
                resID = map_GrandLedge;
                break;

            case 16:
                resID = map_BigRapids;
                break;

            case 17:
                resID = map_Mazelton;
                break;

            case 18:
                resID = map_MerchantSquare;
                break;

            case 19:
                resID = map_Laingsburg;
                break;

            case 20:
                resID = map_HolyPoint;
                break;

            case 21:
                resID = map_EagleHollow;
                break;

            case 51:
                resID = map_Castle;
                break;

            case 52:
                resID = map_Castle2;
                break;

            case 61:
                resID = Dng_Pirates;
                break;

            default:
                OutputDebugString("GetMapResource: Failed to load map " + string(map) + "resource number.\n");

            }
	
            return resID;
            */
        }

        [Obsolete]
        static string GetName(int mapNum)
        {
            return "MapName";
            /*
            HANDLE tempMap;
            int resID;
            string	tempName;
            resID = GetMapResource(mapNum);
            char*	tempM;

            if (resID > 0) 
            {
                tempMap = LoadResource (g.hInstance(), 
                          FindResource (g.hInstance(), MAKEINTRESOURCE(resID), TEXT("map")));
	
                tempM = (char*)LockResource(tempMap);

                for (int i = 0; i < 16; i++)
                {
                    tempName += tempM[7 + i];
                }
		
                tempName = rtrim(tempName);

                FreeResource (tempMap);

            }
	
            if (tempName == "Thomson Crossing")
                tempName = "Thompson Crossing";

            return tempName;
            */
        }

        [Obsolete("Use this[xx,yy]")]
        public int M(int yy, int xx)
        {
            return this[xx, yy];
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

        public void Draw(int x, int y, Rectangle inRect)
        {
            DrawImpl(x, y, inRect);
        }

        protected abstract void DrawImpl(int x, int y, Rectangle inRect);
        protected void Draw2D(int x, int y, Rectangle inRect)
        {
            int i, j;
            int initialxx = inRect.X;
            int width = inRect.Width / 16;
            int height = inRect.Height / 16;
            int wAdjust = 0;
            int hAdjust = 0;
            int tile;
            //Point mDrawMonst = new Point(0, 0);
            int now = (int)Timing.TotalMilliseconds;
            bool setLastTime = false;

            width = width / 2;
            height = height / 2;

            wAdjust = 1;
            hAdjust = 1;


            int xx = initialxx;
            int yy = 16;


            if (lastTimeDraw + 150 < now)
                setLastTime = true;


            for (j = y - height; j < y + height + hAdjust; j++)
            {

                for (i = x - width; i < x + width + wAdjust; i++)
                {
                    tile = DrawTile(i, j);

                    //if (i == monstPoint.X && j == monstPoint.Y)
                    //{
                    //    mDrawMonst.X = xx;
                    //    mDrawMonst.Y = yy;
                    //}

                    XleCore.DrawTile(xx, yy, tile);

                    xx += 16;
                }

                yy += 16;
                xx = initialxx;
            }

            g.waterReset = false;


            if (setLastTime)
            {
                lastTimeDraw = (int)Timing.TotalMilliseconds;
                cyclesDraw++;
            }


        }

        public abstract void GetBoxColors(out Color boxColor, out Color innerColor, out Color fontColor, out int vertLine);

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

        /*
        SpecialEvent GetSpecial(int x, int y)			// retuns the special event at the player coordinates
        {
            int i;
            SpecialEvent dave = new SpecialEvent ();

            for (i = 0; i < 120; i++)
            {
                if (x >= specialx(i) && y >= specialy(i) &&
                    x <= specialx(i) + specialwidth(i) - 1 && y <= specialy(i) + specialheight(i) - 1)
                {
                    dave.sx = specialx(i);
                    dave.sy = specialy(i);
                    dave.swidth = specialwidth(i);
                    dave.sheight = specialheight(i);
                    dave.type = specialType(i);
                    dave.id = i;
                    dave.marked = specialmarked(i);
                    dave.robbed = 0;

                    //specialData(i, dave.data);

                    return dave;

                }
            }

            dave.type = 0;

            return dave;

        }
        /*
        void MarkSpecial(SpecialEvent dave)
        {
            spcMarked[dave.id] = true;
        }

        int specialType(int i)
        {
            int off = Height * Width + offset;
            int type;

            off += i * (SpecialDataLength() + 5);

            type = m[off];

            return type;
        }

        int specialx(int i)
        {
            int off = mapHeight * mapWidth + offset;
            int type;

            off += i * (SpecialDataLength() + 5) + 1;

            type = m[off++] * 256;
            type += m[off];

            return type;
        }

        int specialy(int i)
        {
            int type;
            int off = mapHeight * mapWidth + offset;

            off += i * (SpecialDataLength() + 5) + 3;

            type = m[off++] * 256;
            type += m[off];

            return type;
        }

        int specialwidth(int i)
        {
            int type;
            int off = mapHeight * mapWidth + offset;

            off += i * (SpecialDataLength() + 5) + 5;

            type = m[off++] * 256;
            type += m[off];

            return type;
        }

        int specialheight(int i)
        {
            int type;
            int off = mapHeight * mapWidth + offset;

            off += i * (SpecialDataLength() + 5) + 7;

            type = m[off++] * 256;
            type += m[off];

            return type;
        }

        string specialData(int i)
        {
            return null;
            
            byte[] buffer = new byte[200];

            int off = mapHeight * mapWidth + offset;

            off += i * (SpecialDataLength() + 5) + 9;

            for (int j = 0; j < SpecialDataLength(); j++)
            {
                buffer[j] = m[off + j];
            }

            buffer[j] = 0;

            return new string(buffer);
            
        }
        */


        /// <summary>
        /// Returns whether or not the specified type is present in this map's special events.
        /// Passed type must derive from XleEvent.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool HasEventType(Type t)
        {
            Type basetype = typeof(XleEvent);

            if (basetype.IsAssignableFrom(t) == false)
            {
                throw new Exception("Argument to HasSpecialType must derive from XleEvent");
            }

            for (int i = 0; i < mEvents.Count; i++)
            {
                if (t.IsAssignableFrom(mEvents[i].GetType()))
                    return true;
            }

            return false;
        }



        #endregion
        #region --- Menu Stuff ---

        public abstract string[] MapMenu();

        #endregion
        #region --- Player movement stuff ---

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

            if (evt != null && evt.TryToStepOn(player, dx, dy) == false)
                return false;

            else
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
        
        public void PlayerStep(Player player)
        {
            if (GetEvent(player, 0) != null)
            {
                XleEvent evt = XleCore.Map.GetEvent(player, 0);

                evt.StepOn(player);
            }

            PlayerStepImpl(player);
        }
        protected virtual void PlayerStepImpl(Player player)
        {
        }

        public abstract bool CanPlayerStepInto(Player player, int xx, int yy);


        public abstract void PlayerCursorMovement(Player player, Direction dir);
        protected void _Move2D(Player player, Direction dir, string textStart, out string command, out Point stepDirection)
        {
            player.FaceDirection = dir;

            command = textStart + " " + dir.ToString();

            switch (dir)
            {
                case Direction.West:
                    stepDirection = new Point(-1, 0);
                    break;

                case Direction.North:
                    stepDirection = new Point(0, -1);
                    break;

                case Direction.East:
                    stepDirection = new Point(1, 0);
                    break;

                case Direction.South:
                    stepDirection = new Point(0, 1);
                    break;

                default:
                    stepDirection = Point.Empty;
                    break;
            }

        }
        protected static void _MoveDungeon(Player player, Direction dir, out string command, out Point stepDirection)
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

                    break;

                case Direction.North:
                    command = "Move Forward";


                    if (player.FaceDirection == Direction.East)
                        stepDirection = new Point(1, 0);
                    if (player.FaceDirection == Direction.North)
                        stepDirection = new Point(0, -1);
                    if (player.FaceDirection == Direction.West)
                        stepDirection = new Point(-1, 0);
                    if (player.FaceDirection == Direction.South)
                        stepDirection = new Point(0, 1);

                    break;

                case Direction.West:
                    command = "Turn Left";

                    newDirection = player.FaceDirection + 1;


                    if (newDirection > Direction.South)
                        newDirection = Direction.East;

                    player.FaceDirection = (Direction)newDirection;

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

                    break;
            }
        }

        #endregion
        #region --- Player commands ---

        public bool PlayerSpeak(Player player)
        {
            XleEvent evt = GetEvent(player, 1);
            bool handled = false;

            if (evt != null)
            {
                handled = evt.Speak(player);

                if (handled)
                    return handled;
            }

            return PlayerSpeakImpl(player);
        }
        protected virtual bool PlayerSpeakImpl(Player player)
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
            return false;
        }

        public virtual bool PlayerOpen(Player player)
        {
            return false;
        }

        #endregion
        #region --- Animation ---

        public virtual void UpdateAnim()
        {

        }

        #endregion

        public virtual void CheckSounds(Player player)
        {
        }

    }


    [Serializable]
    public class Roof : IXleSerializable
    {
        private int[,] mData;
        Point mLocation;
        Rectangle mRect;
        int[] mRoofData;

        private bool mOpen;


        #region --- Construction and Serialization ---



        public Roof()
        { }

        Roof(SerializationInfo info, StreamingContext context)
        {
            int[,] data = (int[,])info.GetValue("mData", typeof(int[,]));
            Point loc = (Point)info.GetValue("mLocation", typeof(Point));

            mRect.X = loc.X;
            mRect.Y = loc.Y;
            mRect.Width = data.GetUpperBound(1) + 1;
            mRect.Height = data.GetUpperBound(0) + 1;
            mRoofData = new int[Width * Height];

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    mRoofData[i + j * Width] = data[j, i];
                }
            }
        }

        void IXleSerializable.WriteData(XleSerializationInfo info)
        {
            if (mData != null)
            {
                int[,] data = mData;
                Point loc = mLocation;

                mRect.X = loc.X;
                mRect.Y = loc.Y;
                mRect.Width = data.GetUpperBound(1) + 1;
                mRect.Height = data.GetUpperBound(0) + 1;
                mRoofData = new int[Width * Height];

                for (int i = 0; i < Width; i++)
                {
                    for (int j = 0; j < Height; j++)
                    {
                        mRoofData[i + j * Width] = data[j, i];
                    }
                }
            }

            info.Write("X", X);
            info.Write("Y", Y);
            info.Write("Width", Width);
            info.Write("Height", Height);
            info.Write("RoofData", mRoofData);
        }

        void IXleSerializable.ReadData(XleSerializationInfo info)
        {
            mRect.X = info.ReadInt32("X");
            mRect.Y = info.ReadInt32("Y");
            mRect.Width = info.ReadInt32("Width");
            mRect.Height = info.ReadInt32("Height");
            mRoofData = info.ReadInt32Array("RoofData");
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
            if (mRoofData != null)
            {
                for (int i = 0; i < Math.Min(Width, width); i++)
                {
                    for (int j = 0; j < Math.Min(Height, height); j++)
                    {
                        newData[i + j * width] = mRoofData[i + j * Width];
                    }
                }
            }

            mRect.Width = width;
            mRect.Height = height;

            mRoofData = newData;
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
            get { return mRoofData[x + y * Width]; }
            set { mRoofData[x + y * Width] = value; }
        }


        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle(Location, new Size(Width - 1, Height - 1));
            }
        }

        public bool CharIn(int ptx, int pty )
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
                if (ignoreTransparency == false && this[ptx - X, pty - Y] == 127)
                    return false;
                
                return true;
            }

            return false;
        }

    }

    [Serializable]
    public class Guard : IXleSerializable 
    {
        public Point Location;				// the guards' locations on the map
        public int HP;			// stores the current hp for the guards
        public Direction Facing;		// direction the guards are facing
        public Color Color = XleColor.Yellow;

        public int Attack;			// attack and defense for the guards
        public int Defense;

        public int X { get { return Location.X; } set { Location.X = value; } }
        public int Y { get { return Location.Y; } set { Location.Y = value; } }


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
    }

    namespace XleMapTypes
    {
        [Serializable]
        public class Outside : XleMap, ISerializable 
        {
            int[] mData;
            int mHeight;
            int mWidth;

            List<Monster> mMonst = new List<Monster>();		// stores monster structs;
            List<Monster> currentMonst = new List<Monster>();

            int mEncounterState;

            int stepCount;
            int displayMonst = -1;
            Direction monstDir;
            Point mDrawMonst;
            int monstCount, initMonstCount;
            bool friendly;

            
            #region --- Construction and Serialization ---

            public Outside()
            { }

            Outside(SerializationInfo info, StreamingContext context)
            {
                MapName = info.GetString("XleMap+mMapName");
                TileSet = info.GetString("XleMap+mTileSet");

                int[,] data = (int[,])info.GetValue("mData", typeof(int[,]));

                mWidth = data.GetUpperBound(1)+1;
                mHeight = data.GetUpperBound(0)+1;

                mData = new int[mWidth * mHeight];

                for (int i = 0; i < mWidth; i++)
                {
                    for (int j = 0; j < mHeight; j++)
                    {
                        mData[i + mWidth * j] = data[j, i];
                    }
                }
            }
            void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
            {
                throw new NotImplementedException();
            }

            protected override void WriteData(XleSerializationInfo info)
            {
                info.Write("Width", Width);
                info.Write("Height", Height);
                info.Write("MapData", mData);
            }
            protected override void ReadData(XleSerializationInfo info)
            {
                mWidth = info.ReadInt32("Width");
                mHeight = info.ReadInt32("Height");

                mData = info.ReadInt32Array("MapData");

            }

            #endregion

            [NonSerialized]
            int stormy;

            public override void InitializeMap(int width, int height)
            {
                mWidth = width;
                mHeight = height;
                mData = new int[mWidth * mHeight];
            }
            public override IEnumerable<string> AvailableTilesets
            {
                get
                {
                    yield return "tiles.png";
                }
            }

            public override int Height
            {
                get { return mHeight; }
            }
            public override int Width
            {
                get { return mWidth; }
            }
            public override int this[int xx, int yy]
            {
                get
                {
                    if (yy < 0 || yy >= Height || xx < 0 || xx >= Width)
                    {
                        return 0;
                    }
                    else
                    {
                        return mData[xx + mWidth * yy];
                    }
                }
                set
                {
                    if (yy < 0 || yy >= Height ||
                        xx < 0 || xx >= Width)
                    {
                        return;
                    }
                    else
                    {
                       // mData[yy, xx] = value;
                        //mData[(yy + mapExtend) * (mapWidth + 2 * mapExtend) + (xx + mapExtend)] = (byte)val;

                        mData[xx + mWidth * yy] = value;
                    }

                }

            }

            public override void CheckSounds(Player player)
            {
                if (player.OnRaft > 0)
                {
                    if (SoundMan.IsPlaying(LotaSound.Raft1) == false)
                        SoundMan.PlaySound(LotaSound.Raft1);

                    SoundMan.StopSound(LotaSound.Ocean1);
                    SoundMan.StopSound(LotaSound.Ocean2);
                }
                else
                {
                    SoundMan.StopSound(LotaSound.Raft1);
                    int ocean = 0;


                    for (int i = -1; i <= 2 && ocean == 0; i++)
                    {
                        for (int j = -1; j <= 2 && ocean == 0; j++)
                        {
                            if (Math.Sqrt(Math.Pow(i, 2) + Math.Pow(j, 2)) <= 5)
                            {
                                if (this[player.X + i, player.Y + j] < 16)
                                {
                                    ocean = 1;
                                }
                            }
                        }
                    }

                    //  If we're not near the ocean, fade the sound out
                    if (ocean == 0)
                    {
                        /*
                        if (LotaGetSoundStatus(LotaSound.Ocean1) & (DSBSTATUS_LOOPING | DSBSTATUS_PLAYING))
                        {
                            //SoundMan.PlaySound(LotaSound.Ocean1, 0, false);
                            LotaFadeSound(LotaSound.Ocean1, -2);
                        }
                        if (LotaGetSoundStatus(LotaSound.Ocean2) & (DSBSTATUS_LOOPING | DSBSTATUS_PLAYING))
                        {
                            //SoundMan.PlaySound(LotaSound.Ocean2, 0, false);
                            LotaFadeSound(LotaSound.Ocean2, -2);
                        }
                        */
                    }
                    //  we are near the ocean, so check to see if we need to play the next
                    //  sound (at 1 second intervals)
                    else
                    {
                        /*
                        if (lastOceanSound + 1000 < Timing.TotalMilliseconds )
                        {
                            if (1 + Lota.random.Next(2) == 1)
                            {
                                SoundMan.PlaySound(LotaSound.Ocean1, 0, false);
                            }
                            else
                            {
                                SoundMan.PlaySound(LotaSound.Ocean2, 0, false);
                            }

                            lastOceanSound = clock();
                        }
                         * */
                    }
                    /*
                    //  Play mountain sounds...
                    if (player.Terrain() == TerrainType.Mountain)
                    {
                        if (!(LotaGetSoundStatus(LotaSound.Mountains) & DSBSTATUS_PLAYING))
                        {
                            SoundMan.PlaySound(LotaSound.Mountains, DSBPLAY_LOOPING, true);
                            //LotaFadeSound(LotaSound.Mountains, 2, DSBPLAY_LOOPING);
                        }
                    }
                    else if (LotaGetSoundStatus(LotaSound.Mountains) & (DSBSTATUS_LOOPING | DSBSTATUS_PLAYING))
                    {
                        //if (LotaGetSoundStatus(LotaSound.Mountains) & DSBSTATUS_PLAYING)
                        {
                            LotaFadeSound(LotaSound.Mountains, -1, 0);
                            //LotaStopSound(LotaSound.Mountains);
                        }

                    }
                    */
                }
            }
            public override bool PlayerXamine(Player player)
            {

                string terrain;
                string travel;
                string food;
                string thestring;

                switch (player.Terrain)
                {
                    case TerrainType.Grass:
                        terrain = "grasslands";
                        travel = "easy";
                        food = "low";
                        break;
                    case TerrainType.Water:
                        terrain = "water";
                        travel = "easy";
                        food = "low";
                        break;
                    case TerrainType.Mountain:
                        terrain = "the mountains";
                        travel = "slow";
                        food = "high";
                        break;
                    case TerrainType.Forest:
                        terrain = "a forest";
                        travel = "easy";
                        food = "low";
                        break;
                    case TerrainType.Desert:
                        terrain = "a desert";
                        travel = "slow";
                        food = "high";
                        break;
                    case TerrainType.Swamp:
                        terrain = "a swamp";
                        travel = "average";
                        food = "medium";
                        break;
                    case TerrainType.Foothills:
                        terrain = "mountain foothills";
                        travel = "average";
                        food = "medium";
                        break;

                    default:
                    case TerrainType.Mixed:
                        terrain = "mixed terrain";
                        travel = "average";
                        food = "medium";
                        break;

                }

                g.AddBottom("");

                thestring = "You are in " + terrain.ToString() + ".";

                g.AddBottom(thestring);

                ColorStringBuilder builder = new ColorStringBuilder();

                builder.AddText("Travel: ", XleColor.White);
                builder.AddText(travel.ToString(), XleColor.Green);
                builder.AddText("  -  Food use: ", XleColor.White);
                builder.AddText(food.ToString(), XleColor.Green);

                g.AddBottom(builder);

                return true;
            }
            public override bool PlayerFight(Player player)
            {

                string weaponName;
                Point attackPt = Point.Empty, attackPt2 = Point.Empty;
                Color[] colors = new Color[40];
                ColorStringBuilder builder;

                weaponName = player.CurrentWeaponName;

                g.AddBottom("");


                if (EncounterState == 10)
                {
                    int dam = attack(player);

                    builder = new ColorStringBuilder();
                    builder.AddText("Attack ", XleColor.White);
                    builder.AddText(MonstName(), XleColor.Cyan);

                    g.AddBottom(builder);
                    builder.Clear();

                    builder.AddText("with ", XleColor.White);
                    builder.AddText(weaponName, XleColor.Cyan);

                    if (dam > 0)
                    {
                        SoundMan.PlaySound(LotaSound.PlayerHit);

                        builder = new ColorStringBuilder();

                        builder.AddText("Enemy hit by blow of ", XleColor.White);
                        builder.AddText(dam.ToString(), XleColor.Cyan);
                        builder.AddText(".", XleColor.White);

                        g.AddBottom(builder);
                    }
                    else
                    {
                        SoundMan.PlaySound(LotaSound.PlayerMiss);

                        g.AddBottom("Your Attack missed.", XleColor.Yellow);
                    }

                    XleCore.wait(XleCore.Redraw, 250 + 100 * player.Gamespeed, true);

                    if (KilledOne())
                    {
                        SoundMan.PlaySound(LotaSound.EnemyDie);

                        g.AddBottom("");
                        g.AddBottom("the " + MonstName() + " dies.");

                        int gold, food;
                        bool finished = FinishedCombat(out gold, out food);

                        XleCore.wait(500 + 100 * player.Gamespeed);

                        if (finished)
                        {
                            g.AddBottom("");

                            if (food > 0)
                            {
                                MenuItemList menu = new MenuItemList("Yes", "No");
                                int choice;

                                g.AddBottom("Would you like to use the");
                                g.AddBottom(MonstName() + "'s flesh for food?");
                                g.AddBottom("");

                                choice = XleCore.QuickMenu(menu, 3, 0);

                                if (choice == 1)
                                    food = 0;
                                else
                                {
                                    builder = new ColorStringBuilder();
                                    builder.AddText("You gain ", XleColor.White);
                                    builder.AddText(food.ToString(), XleColor.Green);
                                    builder.AddText(" days of food.", XleColor.White);

                                    player.Food += food;
                                }

                            }


                            if (gold < 0)
                            {
                                // gain weapon or armor
                            }
                            else if (gold > 0)
                            {
                                builder = new ColorStringBuilder();

                                builder.AddText("You find ", XleColor.White);
                                builder.AddText(gold.ToString(), XleColor.Yellow);
                                builder.AddText(" gold.", XleColor.White);

                                g.AddBottom(builder);
                                player.Gold += gold;


                            }

                            XleCore.wait(400 + 100 * player.Gamespeed);

                        }

                    }

                }
                else if (EncounterState > 0)
                {
                    g.AddBottom("The unknown creature is not ");
                    g.AddBottom("within range.");

                    XleCore.wait(300 + 100 * player.Gamespeed);
                }
                else
                {
                    return false;
                }

                return true;
            }
            public override void PlayerCursorMovement(Player player, Direction dir)
            {
                string command;
                Point stepDirection;

                _Move2D(player, dir, "Move", out command, out stepDirection);

                Commands.UpdateCommand(command);

                if (player.Move(stepDirection) == false)
                {
                    TerrainType terrain = Terrain(player.X + stepDirection.X, player.Y + stepDirection.Y);

                    if (InEncounter)
                    {
                        SoundMan.PlaySound(LotaSound.Bump);

                        g.AddBottom("");
                        g.AddBottom("Attempt to disengage");
                        g.AddBottom("is blocked.");

                        XleCore.wait(500);
                    }
                    if (player.IsOnRaft)
                    {
                        SoundMan.PlaySound(LotaSound.Bump);

                        g.AddBottom("");
                        g.AddBottom("The raft must stay in the water.", XleColor.Cyan);
                    }
                    else if (terrain == TerrainType.Water)
                    {
                        SoundMan.PlaySound(LotaSound.Bump);

                        g.AddBottom("");
                        g.AddBottom("There is too much water for travel.", XleColor.Cyan);
                    }
                    else if (terrain == TerrainType.Mountain)
                    {
                        SoundMan.PlaySound(LotaSound.Bump);

                        g.AddBottom("");
                        g.AddBottom("You are not equipped to");
                        g.AddBottom("cross the mountains.");
                    }
                    else
                    {
                        SoundMan.PlaySound(LotaSound.Invalid);

                        g.UpdateBottom("Enter command: Move nowhere");
                        g.waitCommand = 75;
                    }
                }
                else
                {
                    if (InEncounter)
                    {
                        g.AddBottom("");
                        g.AddBottom("Attempt to disengage");
                        g.AddBottom("is successful.");


                        XleCore.wait(500);
                    }


                    if (player.IsOnRaft == false)
                    {

                        switch (player.Terrain)
                        {
                            case TerrainType.Swamp:
                                SoundMan.PlaySound(LotaSound.Swamp);
                                break;

                            case TerrainType.Desert:
                                SoundMan.PlaySound(LotaSound.Desert);
                                break;

                            case TerrainType.Grass:
                            case TerrainType.Forest:
                            case TerrainType.Mixed:
                            default:
                                SoundMan.PlaySound(LotaSound.WalkOutside);
                                break;
                        }
                    }
                    else if (CheckStormy(player))
                    {
                        // check stormy returns true if the player died.
                        return;
                    }

                    switch (player.Terrain)
                    {
                        case TerrainType.Water:
                        case TerrainType.Grass:
                        case TerrainType.Forest:
                            player.TimeDays += .25;
                            break;
                        case TerrainType.Swamp:
                            player.TimeDays += .5;
                            break;
                        case TerrainType.Mountain:
                            player.TimeDays += 1;
                            break;
                        case TerrainType.Desert:
                            player.TimeDays += 1;
                            break;
                        case TerrainType.Mixed:
                            player.TimeDays += 0.5;
                            break;
                    }
                    /*
                        if (Lota.Map.CheckSpecial() && g.allowEnter)
                        {

                            SpecialEvent dave = Lota.Map.GetSpecial();

                            switch (dave.type)
                            {
                                case 1:
                                
                                    break;

                            }
                     * */
                }
            }
            protected override bool PlayerSpeakImpl(Player player)
            {

                if (EncounterState == 0)
                {
                }
                else if (EncounterState < 10)
                {
                }
                else if (EncounterState == 10)
                {
                    SpeakToMonster(player);

                    return true;
                }
                return false;

            }

            private int EncounterState
            {
                get { return mEncounterState; }
                set { mEncounterState = value; }
            }

            private bool InEncounter
            {
                get { return EncounterState == -1; }
            }

            string MonstName()
            {
                return currentMonst[0].mName;

            }

            int attack(Player player)
            {
                int damage = player.Hit(currentMonst[monstCount - 1].mDefense);

                if (currentMonst[monstCount - 1].mWeapon > 0)
                {
                    if (player.WeaponType(player.CurrentWeapon) == currentMonst[monstCount - 1].mWeapon)
                    {
                        damage += XleCore.random.Next(11) + 20;
                    }
                    else
                    {
                        damage = 1 + XleCore.random.Next((damage < 10) ? damage : 10);
                    }
                }

                currentMonst[monstCount - 1].mHP -= damage;
                friendly = false;

                return damage;
            }

            bool KilledOne()
            {
                if (currentMonst[monstCount - 1].mHP <= 0)
                {
                    monstCount--;

                    return true;

                }

                return false;
            }

            void MonstFight()
            {

            }

            bool FinishedCombat(out int gold, out int food)
            {
                bool finished = false;

                gold = 0;
                food = 0;

                if (monstCount == 0)
                {
                    finished = true;


                    for (int i = 0; i < initMonstCount; i++)
                    {
                        gold += currentMonst[i].mGold;
                        food += currentMonst[i].mFood;

                    }

                    gold = (int)(gold * XleCore.random.NextDouble() + 0.5);
                    food = (int)(food * XleCore.random.NextDouble() + 0.5);

                    if (XleCore.random.Next(100) < 50)
                        food = 0;

                    EncounterState = 0;
                    displayMonst = -1;
                }

                return finished;

            }


            void SpeakToMonster(Player player)
            {

                if (!friendly)
                {
                    g.AddBottom("");
                    g.AddBottom("The " + MonstName() + " does not reply.");

                    XleCore.wait(250);

                    return;
                }

                const int talkTypes = 5;
                int type;
                int qual = XleCore.random.Next(5);
                int cost = 0;
                int item = 0;
                MenuItemList menu = new MenuItemList("Yes", "No");

                Color qcolor = XleColor.White;
                ColorStringBuilder builder;

                string[] quality = new string[5] { " Well Crafted", " Slightly Used", " Sparkling New", " Wonderful", " Awesome" };

                do
                {
                    type = XleCore.random.Next(talkTypes) + 1;
                } while (player.MaxHP == player.HP && type == 4);

                switch (type)
                {
                    case 1:			// buy armor
                    case 2:         // buy weapon

                        builder = new ColorStringBuilder();
                        builder.AddText("Do you want to buy a", XleColor.Cyan);
                        if (qual == 4)
                            builder.AddText("n", XleColor.Cyan);

                        builder.AddText(quality[qual], XleColor.White);

                        g.AddBottom(builder);
                        builder.Clear();

                        if (type == 1)
                        {
                            item = XleCore.random.Next(4) + 1;
                            cost = (int)(g.ArmorCost(item, qual) * XleCore.random.NextDouble() * 0.6 + 0.6);

                            builder.AddText(g.ArmorName(item), XleColor.White);
                            builder.AddText(" for ", XleColor.Cyan);
                            builder.AddText(cost.ToString(), XleColor.White);
                            builder.AddText(" Gold?", XleColor.Cyan);
                        }
                        else if (type == 2)
                        {
                            item = XleCore.random.Next(7) + 1;
                            cost = (int)(g.WeaponCost(item, qual) * XleCore.random.NextDouble() * 0.6 + 0.6);

                            builder.AddText(g.WeaponName(item), XleColor.White);
                            builder.AddText(" for ", XleColor.Cyan);
                            builder.AddText(cost.ToString(), XleColor.White);
                            builder.AddText(" Gold?", XleColor.Cyan);

                        }

                        qcolor = XleColor.Cyan;

                        break;
                    case 3:			// buy food

                        item = XleCore.random.Next(21) + 20;
                        cost = (int)(item * XleCore.random.NextDouble() * 0.4 + 0.8);

                        builder = new ColorStringBuilder();
                        builder.AddText("Do you want to buy ", XleColor.Green);
                        builder.AddText(item.ToString(), XleColor.Yellow);

                        g.AddBottom(builder);
                        builder.Clear();

                        // line 2
                        builder.AddText("Days of food for ", XleColor.Green);
                        builder.AddText(cost.ToString(), XleColor.Yellow);
                        builder.AddText(" gold?", XleColor.Green);

                        qcolor = XleColor.Green;

                        break;
                    case 4:			// buy hp


                        item = XleCore.random.Next(player.MaxHP / 4) + 20;

                        if (item > (player.MaxHP - player.HP))
                            item = (player.MaxHP - player.HP);

                        cost = (int)(item * XleCore.random.NextDouble() * 0.15 + 0.75);

                        builder = new ColorStringBuilder();
                        builder.AddText("Do you want to buy a potion worth ", XleColor.Green);

                        g.AddBottom(builder);
                        builder.Clear();

                        // line 2
                        builder.AddText(item.ToString(), XleColor.Yellow);
                        builder.AddText(" Hit Points for ", XleColor.Green);
                        builder.AddText(cost.ToString(), XleColor.Yellow);
                        builder.AddText(" gold?", XleColor.Green);

                        qcolor = XleColor.Green;

                        break;
                    case 5:			// buy museum coin
                        //StoreMuseumCoin();

                        break;

                }

                if (type != 5)
                {
                    g.AddBottom("");

                    int choice = XleCore.QuickMenu(menu, 3, 0, qcolor);

                    if (choice == 0)
                    {
                        if (player.Spend(cost))
                        {
                            SoundMan.PlaySound(LotaSound.Sale);

                            g.AddBottom("");
                            g.AddBottom("Purchase Completed.");

                            bool flash = false;
                            Color clr1 = XleColor.White;
                            Color clr2 = XleColor.White;

                            switch (type)
                            {
                                case 1:
                                    player.AddArmor(item, qual);

                                    break;
                                case 2:
                                    player.AddWeapon(item, qual);

                                    break;
                                case 3:
                                    player.Food += item;
                                    clr2 = XleColor.Green;

                                    break;
                                case 4:
                                    player.HP += item;
                                    clr2 = XleColor.Green;

                                    break;
                                case 5:
                                    break;
                            }

                            Color lastColor = clr1;
                            while (SoundMan.IsPlaying(LotaSound.Sale))
                            {
                                if (lastColor == clr2)
                                    lastColor = clr1;
                                else
                                    lastColor = clr2;

                                g.HPColor = lastColor;

                                XleCore.wait(40);
                            }

                            g.HPColor = XleColor.White;
                        }
                        else
                        {

                            SoundMan.PlaySound(LotaSound.Medium);

                            g.AddBottom("");
                            g.AddBottom("You don't have enough gold...");
                        }

                    }
                    else
                    {
                        SoundMan.PlaySound(LotaSound.Medium);

                        g.AddBottom("");

                        if (1 + XleCore.random.Next(2) == 1)
                            g.AddBottom("Maybe Later...");
                        else
                            g.AddBottom("You passed up a good deal!");

                    }
                }

                EncounterState = 0;
                displayMonst = -1;
            }


            /// <summary>
            /// 		// sets or returns whether or not the player is in stormy water
            /// </summary>
            /// <returns></returns>
            public int Stormy
            {
                get { return stormy; }
                set
                {
                    System.Diagnostics.Debug.Assert(value >= 0);

                    stormy = value;

                }
            }

            /// <summary>
            /// Returns true if the player drowns.
            /// </summary>
            /// <param name="player"></param>
            /// <returns></returns>
            private bool CheckStormy(Player player)
            {
                int wasStormy = stormy;


                if (player.X < -45 || player.X > XleCore.Map.Width + 45 ||
                    player.Y < -45 || player.Y > XleCore.Map.Height + 45)
                {
                    Stormy = 3;
                }
                else if (player.X < -30 || player.X > XleCore.Map.Width + 30 ||
                    player.Y < -30 || player.Y > XleCore.Map.Height + 30)
                {
                    Stormy = 2;
                }
                else if (player.X < -15 || player.X > XleCore.Map.Width + 15 ||
                    player.Y < -15 || player.Y > XleCore.Map.Height + 15)
                {
                    Stormy = 1;
                }
                else
                {
                    Stormy = 0;
                }

                if (Stormy != wasStormy || Stormy >= 2)
                {
                    if (Stormy == 1 && wasStormy == 0)
                    {
                        g.AddBottom("");
                        g.AddBottom("You are sailing into stormy water.", XleColor.Yellow);
                    }
                    else if (Stormy == 2 || Stormy == 3)
                    {
                        g.AddBottom("");
                        g.AddBottom("The water is now very rough.", XleColor.White);
                        g.AddBottom("It will soon swamp your raft.", XleColor.Yellow);
                    }
                    else if (Stormy == 1 && wasStormy == 2)
                    {
                        g.AddBottom("");
                        g.AddBottom("You are out of immediate danger.", XleColor.Yellow);
                    }
                    else if (Stormy == 0 && wasStormy == 1)
                    {
                        g.AddBottom("");
                        g.AddBottom("You leave the storm behind.", XleColor.Cyan);
                    }

                    if (Stormy == 3)
                    {
                        g.AddBottom("");
                        g.AddBottom("Your raft sinks.", XleColor.Yellow);
                        g.AddBottom("");
                    }

                    XleCore.wait(1000);

                    if (Stormy == 3)
                    {
                        player.Dead();
                        return true;
                    }

                }
                return false;
            }

            public override void UpdateAnim()
            {
                /*
                 if ((setLastTime || g.waterReset == true))
                 {
                     if (tile == 0 && Lota.random.Next(0, 1000) < 10 * (Stormy + 1)
                         && g.waterReset == false)
                     {
                         tile = 1;
                         this[i, j] = tile;

                     }
                     else if ((tile == 1 && Lota.random.Next(0, 1000) < 50) ||
                         (g.waterReset == true && tile == 1))
                     {
                         tile = 0;
                         this[i, j] = tile;
                     }

                 }*/
            }
            public override void AfterExecuteCommand(Player player, KeyCode cmd)
            {
                

            }
            protected override void DrawImpl(int x, int y, Rectangle inRect)
            {
                Draw2D(x, y, inRect);


                if (displayMonst > -1)
                {
                    XleCore.DrawMonster(mDrawMonst.X, mDrawMonst.Y, displayMonst);
                }

            }

            public void TestEncounter(Player player, KeyCode cursorKeys)
            {
                int waitAtEnd = 0;
                bool keyBreak = false;
                bool firstTime = false;

                string dirName;

                if (mMonst.Count == 0)
                    return;

                if (g.disableEncounters)
                    return;

                if (EncounterState == 0 && stepCount <= 0)
                {
                    stepCount = XleCore.random.Next(1, 16);
                    int type = XleCore.random.Next(0, 21);

                    if (cursorKeys != KeyCode.Left && cursorKeys != KeyCode.Up &&
                        cursorKeys != KeyCode.Right && cursorKeys != KeyCode.Down)
                        type = 99;

                    friendly = false;
                    monstDir = (Direction)XleCore.random.Next((int)Direction.East, (int)Direction.South + 1);


                    if (type < 10)
                    {
                        mDrawMonst.X = player.X - 1;
                        mDrawMonst.Y = player.Y - 1;

                        switch (monstDir)
                        {

                            case Direction.East: dirName = "East"; mDrawMonst.X += 2; break;
                            case Direction.North: dirName = "North"; mDrawMonst.Y -= 2; break;
                            case Direction.West: dirName = "West"; mDrawMonst.X -= 2; break;
                            case Direction.South: dirName = "South"; mDrawMonst.Y += 2; break;
                            default: dirName = "Unknown"; break;
                        }

                        EncounterState = 1;  // unknown creature
                        SoundMan.PlaySound(LotaSound.Encounter);


                        g.AddBottom("");
                        g.AddBottom("An unknown creature is approaching ", XleColor.Cyan);
                        g.AddBottom("from the " + dirName + ".", XleColor.Cyan);

                        waitAtEnd = 1000;

                    }
                    else if (type < 15)
                    {
                        EncounterState = 2;	// creature is appearing

                        waitAtEnd = 1000;
                    }

                }
                else if (EncounterState == 0 && stepCount > 0 && cursorKeys > 0)
                {
                    stepCount--;
                }
                else if (EncounterState == 1)
                {
                    EncounterState = 2;

                    waitAtEnd = 0;
                }

                if (EncounterState == 2)
                {
                    if (XleCore.random.Next(100) < 55)
                        EncounterState = 5;		// monster has appeared
                    else
                        EncounterState = 10;	// monster is ready

                    int mCount = 0;
                    int val, sel = -1;
                    firstTime = true;

                    SoundMan.PlaySound(LotaSound.Encounter);

                    for (int i = 0; i < mMonst.Count; i++)
                    {
                        if (mMonst[i].mTerrain == TerrainType.All && Terrain(player.X, player.Y) != 0)
                            mCount++;

                        if (mMonst[i].mTerrain == Terrain(player.X, player.Y))
                            mCount += 3;

                        if (Terrain(player.X, player.Y) == TerrainType.Foothills && mMonst[i].mTerrain == TerrainType.Mountain)
                            mCount += 3;
                    }

                    val = 1 + XleCore.random.Next(mCount);

                    for (int i = 0; i < mMonst.Count; i++)
                    {
                        if (mMonst[i].mTerrain == TerrainType.All && Terrain(player.X, player.Y) != 0)
                            val--;

                        if (mMonst[i].mTerrain == Terrain(player.X, player.Y))
                            val -= 3;

                        if (Terrain(player.X, player.Y) == TerrainType.Foothills && mMonst[i].mTerrain == TerrainType.Mountain)
                            val -= 3;

                        if (val == 0 || val == -1 || val == -2)
                        {
                            sel = i;
                            break;
                        }
                    }

                    System.Diagnostics.Debug.Assert(sel > -1);

                    displayMonst = sel;

                    mDrawMonst.X = player.X - 1;
                    mDrawMonst.Y = player.Y - 1;

                    switch (monstDir)
                    {

                        case Direction.East: dirName = "East"; mDrawMonst.X += 2; break;
                        case Direction.North: dirName = "North"; mDrawMonst.Y -= 2; break;
                        case Direction.West: dirName = "West"; mDrawMonst.X -= 2; break;
                        case Direction.South: dirName = "South"; mDrawMonst.Y += 2; break;

                    }


                    int max = 1;
                    initMonstCount = monstCount = 1 + XleCore.random.Next(max);

                    for (int i = 0; i < monstCount; i++)
                    {
                        currentMonst[i] = mMonst[displayMonst];

                        currentMonst[i].mHP = (int)((XleCore.random.NextDouble() * 0.4 + .8) * currentMonst[i].mHP);
                    }

                    if (XleCore.random.Next(256) <= currentMonst[0].mFriendly)
                        friendly = true;
                    else
                        friendly = false;

                    waitAtEnd = 2000;


                }

                if (EncounterState == 3)
                {
                    g.AddBottom("");
                    g.AddBottom("You avoid the unknown creature.");
                    waitAtEnd = player.Gamespeed * 150 + 50;

                    EncounterState = 0;

                }
                else if (EncounterState == 5)
                {
                    Color[] colors = new Color[40];
                    string s = (monstCount > 1) ? "s" : "";

                    for (int i = 0; i < 40; i++)
                        colors[i] = XleColor.Cyan;

                    colors[0] = XleColor.White;

                    EncounterState = 10;			// appeared and ready

                    g.AddBottom("");
                    g.AddBottom(monstCount.ToString() + " " + currentMonst[0].mName + s, colors);

                    colors[0] = XleColor.Cyan;
                    g.AddBottom("is approaching.", colors);

                    waitAtEnd = 2000;

                }
                else if (EncounterState == 10)
                {
                    if (friendly)
                    {
                        Color[] colors = new Color[40];

                        for (int i = 0; i < 40; i++)
                            colors[i] = XleColor.Cyan;
                        colors[0] = XleColor.White;

                        g.AddBottom("");
                        g.AddBottom(monstCount.ToString() + " " + currentMonst[0].mName, colors);
                        g.AddBottom("Stands before you.");

                        if (waitAtEnd == 0)
                            waitAtEnd = 1500;

                    }
                    else
                    {
                        ColorStringBuilder builder = new ColorStringBuilder();
                        builder.AddText("Attacked by ", XleColor.White);
                        builder.AddText(monstCount.ToString(), XleColor.Yellow);
                        builder.AddText(" " + currentMonst[0].mName, XleColor.Cyan);

                        g.AddBottom("");
                        g.AddBottom(builder);

                        int dam = 0;
                        int hits = 0;

                        for (int i = 0; i < monstCount; i++)
                        {
                            int t = player.Damage(currentMonst[i].mAttack);

                            if (t > 0)
                            {
                                dam += t;
                                hits++;
                            }
                        }

                        builder.Clear();
                        builder.AddText("Hits:  ", XleColor.White);
                        builder.AddText(hits.ToString(), XleColor.Yellow);
                        builder.AddText("   Damage:  ", XleColor.White);
                        builder.AddText(dam.ToString(), XleColor.Yellow);

                        g.AddBottom(builder);

                        if (dam > 0)
                        {
                            SoundMan.PlaySound(LotaSound.EnemyHit);
                        }
                        else
                        {
                            SoundMan.PlaySound(LotaSound.EnemyMiss);
                        }



                    }

                    waitAtEnd = 250;
                    g.waitCommand = 1;

                    if (!firstTime)
                        keyBreak = true;

                }


                if (waitAtEnd > 0)
                {

                    XleCore.wait(XleCore.Redraw, waitAtEnd, keyBreak);

                }
            }

            protected override bool CheckMovementImpl(Player player, int dx, int dy)
            {
                if (EncounterState == 1)
                {
                    bool moveTowards = false;

                    switch (monstDir)
                    {

                        case Direction.East: if (dx > 0) moveTowards = true; break;
                        case Direction.North: if (dy < 0) moveTowards = true; break;
                        case Direction.West: if (dx < 0) moveTowards = true; break;
                        case Direction.South: if (dy > 0) moveTowards = true; break;

                    }

                    if (!moveTowards)
                    {
                        if (XleCore.random.Next(100) < 50)
                        {

                            EncounterState = 3;			// avoided;
                        }
                    }

                }
                else if (EncounterState == 10)
                {
                    if (XleCore.random.Next(100) < 40 && !friendly)
                    {
                        return false;
                    }
                    else
                    {
                        EncounterState = -1;
                        displayMonst = -1;
                    }

                }

                return true;
            }
            public override string[] MapMenu()
            {
                List<string> retval = new List<string>();

                retval.Add("Armor");
                retval.Add("Disembark");
                retval.Add("End");
                retval.Add("Fight");
                retval.Add("Gamespeed");
                retval.Add("Hold");
                retval.Add("Inventory");
                retval.Add("Magic");
                retval.Add("Pass");
                retval.Add("Speak");
                retval.Add("Use");
                retval.Add("Weapon");
                retval.Add("Xamine");

                return retval.ToArray();
            }

            public override void GetBoxColors(out Color boxColor, out Color innerColor, out Color fontColor, out int vertLine)
            {
                fontColor = XleColor.White;


                boxColor = XleColor.Brown;
                innerColor = XleColor.Yellow;
                vertLine = 15 * 16;

            }

            public override bool CanPlayerStepInto(Player player, int xx, int yy)
            {
                int t = 0;
                TerrainType terrain = Terrain(xx, yy);
                int test = (int)terrain;

                if (player.IsOnRaft)
                {
                    if (terrain == TerrainType.Water)
                        return true;
                    else
                        return false;
                }

                if (terrain == TerrainType.Water)
                {
                    for (int i = 1; i < player.Rafts.Count; i++)
                    {
                        if (Math.Abs(player.Rafts[i].X - xx) < 2 && Math.Abs(player.Rafts[i].Y - yy) < 2)
                        {
                            return true;
                        }
                    }

                    return false;
                }

                if (terrain == TerrainType.Mountain && player.Hold !=  2)
                {
                    return false;
                }
                
                return true;

            }

        }
        [Serializable]
        public class Town : XleMap, ERY.Xle.IHasGuards, ERY.Xle.IHasRoofs, ISerializable 
        {
            int mWidth;
            int mHeight;
            int[] mData;
            int mOutsideTile = 0;

            List<Roof> mRoofs = new List<Roof>();
            List<Guard> mGuards = new List<Guard>();

            List<int> mail = new List<int>();				// towns to carry mail to



            #region --- Construction and Serialization ---

            public Town()
            { }

            protected Town(SerializationInfo info, StreamingContext context)
            {
                MapName = info.GetString("XleMap+mMapName");
                MapID = info.GetInt32("XleMap+mMapID");
                TileSet = info.GetString("XleMap+mTileSet");
                Events = (List<XleEvent>)info.GetValue("XleMap+mEvents", typeof(List<XleEvent>));
                mRoofs = (List<Roof>)info.GetValue("Town+mRoofs", typeof(List<Roof>));
                mGuards = (List<Guard>)info.GetValue("Town+mGuards", typeof(List<Guard>));
                int[,] data = (int[,])info.GetValue("Town+mData", typeof(int[,]));
                mOutsideTile = info.GetInt32("Town+mOutsideTile");

                mWidth = data.GetUpperBound(1) + 1;
                mHeight = data.GetUpperBound(0) + 1;
                mData = new int[mWidth * mHeight];

                for (int j = 0; j < mHeight; j++)
                {
                    for (int i = 0; i < mWidth; i++)
                    {
                        mData[i + j * mWidth] = data[j, i];
                    }
                }


                SaveMap(this, "test.xmf");

            }

            void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
            {
                throw new NotImplementedException();
            }

            protected override void WriteData(XleSerializationInfo info)
            {
                info.Write("Width", mWidth);
                info.Write("Height", mHeight);
                info.Write("OutsideTile", mOutsideTile);
                info.Write("MapData", mData);
            }
            protected override void ReadData(XleSerializationInfo info)
            {
                mWidth = info.ReadInt32("Width");
                mHeight = info.ReadInt32("Height");
                mOutsideTile = info.ReadInt32("OutsideTile");
                mData = info.ReadInt32Array("MapData");
            }

            #endregion

            public override IEnumerable<string> AvailableTilesets
            {
                get
                {
                    yield return "towntiles.png";
                }
            }

            double lastGuardAnim = 0;

            public List<Roof> Roofs
            {
                get { return mRoofs; }
                set { mRoofs = value; }
            }

            bool mIsAngry = false;					// whether or not the guards are chasing the player

            int guardAnim;


            public List<Guard> Guards
            {
                get { return mGuards; }
            }
            public bool GuardInSpot(int x, int y)
            {
                for (int i = 0; i < Guards.Count; i++)
                {
                    Guard g = Guards[i];

                    if (g.X != 0 && g.Y != 0)
                    {
                        if ((g.X == x - 1 || g.X == x || g.X == x + 1) &&
                            (g.Y == y - 1 || g.Y == y || g.Y == y + 1))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            public bool IsAngry
            {
                get { return mIsAngry; }
                set
                {
                    mIsAngry = value;

                    if (mIsAngry)
                    {
                        g.invisible = false;
                        g.guard = false;
                    }
                }
            }

            public override void AfterExecuteCommand(Player player, KeyCode cmd)
            {
                UpdateGuards(player);
            }
            public void UpdateGuards(Player player)
            {
                if (IsAngry)
                {
                    int i, j;
                    double dist;
                    int xdist;
                    int ydist;
                    int dx;
                    int dy;
                    Point newPt;
                    bool badPt = false;
                    Color[] color = new Color[40];
                    string tempString;
                    int dam;

                    for (i = 0; i < Guards.Count; i++)
                    {
                        Guard guard = Guards[i];

                        if (PointInRoof(guard.X, guard.Y) == -1)
                        {
                            badPt = false;

                            newPt = guard.Location;

                            xdist = player.X - guard.X;
                            ydist = player.Y - guard.Y;

                            if (xdist != 0)
                                dx = xdist / Math.Abs(xdist);
                            else dx = 0;
                            if (ydist != 0)
                                dy = ydist / Math.Abs(ydist);
                            else dy = 0;

                            dist = Math.Sqrt(Math.Pow(xdist, 2) + Math.Pow(ydist, 2));

                            if (Math.Abs(xdist) <= 2 && Math.Abs(ydist) <= 2)
                            {

                                g.AddBottom("");

                                tempString = "Attacked by guard! -- ";
                                dam = player.Damage(guard.Attack);

                                for (j = 0; j < tempString.Length; j++)
                                {
                                    color[j] = XleColor.White;
                                }

                                if (dam > 0)
                                {
                                    tempString += "Blow ";

                                    for (; j < tempString.Length; j++)
                                    {
                                        color[j] = XleColor.Yellow;
                                    }
                                    tempString += dam;
                                    tempString += " H.P.";

                                    for (; j < tempString.Length; j++)
                                    {
                                        color[j] = XleColor.White;
                                    }

                                    SoundMan.PlaySound(LotaSound.EnemyHit);

                                }
                                else
                                {
                                    tempString += "Missed";

                                    for (; j < tempString.Length; j++)
                                    {
                                        color[j] = XleColor.Cyan;
                                    }
                                    SoundMan.PlaySound(LotaSound.EnemyMiss);
                                }


                                g.AddBottom(tempString, color);


                                XleCore.wait(100 * player.Gamespeed);


                            }
                            else if (dist < 25)
                            {
                                if (Math.Abs(xdist) > Math.Abs(ydist))
                                {
                                    newPt.X += dx;
                                }
                                else
                                {
                                    newPt.Y += dy;
                                }

                                badPt = !CheckGuard(newPt, i);

                                if (badPt == true)
                                {
                                    newPt = guard.Location;

                                    if (Math.Abs(xdist) > Math.Abs(ydist))
                                    {
                                        xdist = 0;

                                        if (ydist == 0)
                                        {
                                            dy = XleCore.random.Next(2) * 2 - 1;
                                        }

                                        dx = 0;

                                        newPt.Y += dy;
                                    }
                                    else
                                    {
                                        ydist = 0;

                                        if (xdist == 0)
                                        {
                                            dx = XleCore.random.Next(2) * 2 - 1;
                                        }

                                        dy = 0;

                                        newPt.X += dx;
                                    }
                                    badPt = !CheckGuard(newPt, i);

                                    if (badPt == true)
                                        newPt = guard.Location;

                                }

                                guard.Location = newPt;

                                if (Math.Abs(xdist) > Math.Abs(ydist))
                                {
                                    if (dx < 0)
                                    {
                                        guard.Facing = Direction.West;
                                    }
                                    else
                                    {
                                        guard.Facing = Direction.East;
                                    }
                                }
                                else
                                {
                                    if (dy < 0)
                                    {
                                        guard.Facing = Direction.North;
                                    }
                                    else
                                    {
                                        guard.Facing = Direction.South;
                                    }
                                }

                            }
                        }			// guard(x,y) != (0,0)
                    }

                }
            }

            bool CheckGuard(Point pt, int grd)
            {
                int i, j, k;
                Size guardSize = new Size(2, 2);

                for (j = 0; j < 2; j++)
                {
                    for (i = 0; i < 2; i++)
                    {
                        if (this[pt.X + i, pt.Y + j] >= 128 || (this[pt.X + i, pt.Y + j] % 16) >= 7)
                        {
                            return false;
                        }

                        // check for guard-guard collisions
                        Rectangle grdRect = new Rectangle(pt, guardSize);

                        for (k = 0; k < Guards.Count; k++)
                        {
                            if (k == grd)
                                continue;

                            Guard guard = Guards[k];
                            Rectangle guardRect = new Rectangle(guard.Location, guardSize);

                            if (grdRect.IntersectsWith(guardRect))
                                return false;

                            /*
                                if ((guard.X == pt.X - 1 || guard[k].X == pt.X || guard[k].X == pt.X + 1) &&
                                    (guard.Y == pt.Y - 1 || guard[k].Y == pt.Y || guard[k].Y == pt.Y + 1))
                                {
                                    return false;
                                }
                             * */

                        }
                    }
                }

                return true;
            }
            int AttackGuard(Player player, int grd)
            {
                int dam = 0;
                ColorStringBuilder builder = new ColorStringBuilder();

                dam = player.Hit(Guards[grd].Defense);

                if (dam > 0)
                {
                    IsAngry = true;
                    //g.player.lastAttacked = MapNumber();

                    builder.AddText("Guard struck  ", XleColor.Yellow);
                    builder.AddText(dam.ToString(), XleColor.White);

                    g.AddBottom(builder);

                    Guards[grd].HP -= dam;

                    SoundMan.PlaySound(LotaSound.PlayerHit);

                    if (Guards[grd].HP <= 0)
                    {
                        g.AddBottom("Guard killed");

                        Guards.RemoveAt(grd);

                        XleCore.wait(100);

                        SoundMan.StopSound(LotaSound.PlayerHit);
                        SoundMan.PlaySound(LotaSound.EnemyDie);

                    }

                }
                else
                {
                    g.AddBottom("Attack on guard missed", XleColor.Purple);
                    SoundMan.PlaySound(LotaSound.PlayerMiss);
                }

                return 0;

            }

            /*
     Point GuardPos(int i)
     {
         return guad
         Point tempPt;

         tempPt.X = guard[i].X;
         tempPt.Y = guard[i].Y;

         return tempPt;

     }
     */
            /*
            int SpecialDataLength()
            {
                switch (MapType())
                {
                    case MapTypes.Outside:
                        return 104;
                        break;
                    case MapTypes.Town:
                    case MapTypes.Castle:
                        return 104;
                        break;
                }

                return 5;
            }
            */



            /*
            void CheckRoof(int ptx, int pty)
            {
                if (mapType != MapTypes.Town && mapType != MapTypes.Castle)
                {
                    return;
                }

                if (!IsAngry)
                {
                    for (int i = 0; i < mRoofs.Count; i++)
                        mRoofs[i].Open = false;
                }

                int roofID = InRoof(ptx, pty);

                if (roofID >= 0)
                {
                    mRoofs[roofID].Open = true;
                }

            }

            */

            /// <summary>
            /// Returns the index of the roof the character standing at the point
            /// ptx, pty.  -1 if no roof.
            /// </summary>
            /// <param name="ptx"></param>
            /// <param name="pty"></param>
            /// <returns></returns>
            int CharInRoof(int ptx, int pty)
            {
                for (int i = 0; i < Roofs.Count; i++)
                {
                    Roof r = Roofs[i];

                    if (r.CharIn(ptx, pty, false))
                        return i;
                }

                return -1;
            }
            int PointInRoof(int ptx, int pty)
            {
                for (int i = 0; i < mRoofs.Count; i++)
                {
                    if (mRoofs[i].PointInRoof(ptx, pty, false) && mRoofs[i].Open == false)
                    {
                        return i;
                    }
                }

                return -1;

                /*
                if (pty >= 0 && pty < Height && ptx >= 0 && ptx < Width)
                {
                    //roof = mRoof[pty, ptx];

                    if (mRoofs[roof].Open)
                    {
                        roof = -1;
                    }

                }
                return roof;
                */

            }
            /*

            Point RoofAnchor(int r)
            {
                return new Point(0, 0);
                //if (r == 39)
                //{
                //    int i = 0;
                //}

                //int off = RoofOffset(r);
                //Point size;

                //
                //size.X = m[off] * 256 + m[off + 1];
                //size.Y = m[off + 2] * 256 + m[off + 3];
                //

                //return size;
            }

            Point RoofAnchorTarget(int r)
            {
                int off = RoofOffset(r);
                Point size;

                size.X = m[off + 4] * 256 + m[off + 5];
                size.Y = m[off + 6] * 256 + m[off + 7];

                return size;
            }

            Point RoofSize(int r)
            {
                int off = RoofOffset(r);
                Point size;

                size.X = m[off + 8] * 256 + m[off + 9];
                size.Y = m[off + 10] * 256 + m[off + 11];

                return size;
            }

            int RoofIntOffset(int r)
            {
                Point lastSize;
                int last;
                int me;

                if (r == 0)
                {
                    return roofOffset;
                }

                last = RoofOffset(r - 1);
                lastSize = RoofSize(r - 1);

                me = last + 12 + lastSize.Y * lastSize.X;

                return me;

            }

            int RoofOffset(int r)
            {
                return eachRoofOffset[r];
            }
            */

            int RoofTile(int xx, int yy)
            {
                for (int i = 0; i < Roofs.Count; i++)
                {
                    Roof r = Roofs[i];
                    Rectangle boundingRect = r.Rectangle;

                    if (r.Open)
                        continue;

                    if (boundingRect.Contains(new Point(xx, yy)))
                    {
                        return r[xx - r.X, yy - r.Y];
                    }
                }

                return 127;
            }


            public List<int> Mail
            {
                get { return mail; }
                set { mail = value; }
            }


            public override void InitializeMap(int width, int height)
            {
                mWidth = width;
                mHeight = height;

                mData = new int[mWidth * mHeight];
            }

            public override int Height
            {
                get { return mHeight; }
            }
            public override int Width
            {
                get { return mWidth; }
            }
            public int OutsideTile
            {
                get { return mOutsideTile; }
                set { mOutsideTile = value; }
            }

            public override int this[int xx, int yy]
            {
                get
                {
                    if (yy < 0 || yy >= Height || xx < 0 || xx >= Width)
                    {
                        return mOutsideTile;
                    }
                    else
                    {
                        return mData[xx + yy * mWidth];
                    }
                }
                set
                {
                    if (yy < 0 || yy >= Height ||
                        xx < 0 || xx >= Width)
                    {
                        return;
                    }
                    else
                    {
                        mData[xx + yy * mWidth] = value; 
                    }
                }
            }

            public override void PlayerCursorMovement(Player player, Direction dir)
            {
                string command;
                Point stepDirection;

                _Move2D(player, dir, "Move", out command, out stepDirection);

                Commands.UpdateCommand(command);

                if (player.Move(stepDirection))
                    SoundMan.PlaySound(LotaSound.TownWalk);
                else
                {
                    SoundMan.PlaySound(LotaSound.Invalid);

                    Commands.UpdateCommand("Move Nowhere");
                }

            }
            public override bool PlayerFight(Player player)
            {
                string tempstring;
                string weaponName;
                Point attackPt = Point.Empty, attackPt2 = Point.Empty;
                int i = 0, j = 0;
                int dx = 0, dy = 0;
                bool attacked = false;
                int maxXdist = 1;
                int maxYdist = 1;
                int tile = 0, tile1;
                int hit = 0;
                Color[] colors = new Color[40];
                ColorStringBuilder builder;

                weaponName = player.CurrentWeaponName;


                if (player.WeaponType(player.CurrentWeapon) == 6 ||
                    player.WeaponType(player.CurrentWeapon) == 8)
                {
                    maxXdist = 12;
                    maxYdist = 8;
                }

                g.AddBottom("");

                tempstring = "";

                //TODO: Loadstring(g.hInstance(), player.WeaponType(player.CurrentWeapon()), tempstring, 25);
                tempstring = tempstring.TrimEnd();

                if (tempstring == "")
                    tempstring = "Bare hands";


                g.AddBottom("Fight with " + tempstring);
                g.AddBottom("Enter direction: ");

                tempstring = "Enter direction: ";

                KeyCode key = XleCore.WaitForKey(KeyCode.Up, KeyCode.Down, KeyCode.Left, KeyCode.Right);


                attackPt.X = player.X;
                attackPt.Y = player.Y;
                attackPt2.X = attackPt.X;
                attackPt2.Y = attackPt.Y;

                switch (key)
                {
                    case KeyCode.Right:
                        tempstring += "east";
                        player.FaceDirection = Direction.East;
                        attackPt.X++;
                        attackPt2.X++;
                        attackPt2.Y++;
                        dx = 1;
                        break;
                    case KeyCode.Up:
                        tempstring += "north";
                        player.FaceDirection = Direction.North;
                        dy = -1;
                        attackPt2.X++;
                        break;
                    case KeyCode.Left:
                        tempstring += "west";
                        player.FaceDirection = Direction.West;
                        dx = -1;
                        attackPt2.Y++;
                        break;
                    case KeyCode.Down:
                        tempstring += "south";
                        player.FaceDirection = Direction.South;
                        dy = 1;
                        attackPt.Y++;
                        attackPt2.X++;
                        attackPt2.Y++;
                        break;
                }

                g.UpdateBottom(tempstring);

                for (i = 1; i <= maxXdist && attacked == false && tile < 128; i++)
                {
                    for (j = 1; j <= maxYdist && attacked == false && tile < 128; j++)
                    {

                        for (int k = 0; k < Guards.Count; k++)
                        {
                            if ((Guards[k].X == attackPt.X + dx * i
                                || Guards[k].X + 1 == attackPt.X + dx * i
                                || Guards[k].X == attackPt2.X + dx * i
                                || Guards[k].X + 1 == attackPt2.X + dx * i
                                )
                                &&
                                (Guards[k].Y == attackPt.Y + dy * j
                                || Guards[k].Y + 1 == attackPt.Y + dy * j
                                || Guards[k].Y == attackPt2.Y + dy * j
                                || Guards[k].Y + 1 == attackPt2.Y + dy * j
                                )
                                &&
                                attacked == false)
                            {
                                AttackGuard(player, k);
                                attacked = true;

                                XleCore.wait(200);
                            }
                        }

                        tile = this[attackPt.X + dx, attackPt.Y + dy];
                        tile1 = this[attackPt2.X + dx, attackPt2.Y + dy];

                        int t = RoofTile(attackPt.X + dx, attackPt.Y + dy);
                        if (t != 127)
                            tile = 128;

                        t = RoofTile(attackPt2.X + dx, attackPt2.Y + dy);
                        if (t != 127)
                            tile1 = 128;


                        if (tile == 222 || tile == 223 || tile == 238 || tile == 239)
                        {
                            hit = 1;
                        }
                        else if (tile1 == 222 || tile1 == 223 || tile1 == 238 || tile1 == 239)
                        {
                            hit = 2;
                        }
                        if (hit > 0)
                        {
                            if (hit == 1)
                            {
                                if (tile == 223)
                                {
                                    attackPt.X--;
                                }
                                else if (tile == 238)
                                {
                                    attackPt.Y--;
                                }
                                else if (tile == 239)
                                {
                                    attackPt.X--;
                                    attackPt.Y--;
                                }
                            }
                            else if (hit == 2)
                            {
                                attackPt = attackPt2;

                                if (tile1 == 223)
                                {
                                    attackPt.X--;
                                }
                                else if (tile1 == 238)
                                {
                                    attackPt.Y--;
                                }
                                else if (tile1 == 239)
                                {
                                    attackPt.X--;
                                    attackPt.Y--;
                                }
                            }

                            int dam = XleCore.random.Next(10) + 30;

                            tempstring = (string)"Merchant killed by blow of " + dam.ToString();

                            g.AddBottom("");
                            g.AddBottom(tempstring);

                            this[attackPt.X + dx, attackPt.Y + dy] = 0x52;
                            this[attackPt.X + dx, attackPt.Y + dy + 1] = 0x52;
                            this[attackPt.X + dx + 1, attackPt.Y + dy + 1] = 0x52;
                            this[attackPt.X + dx + 1, attackPt.Y + dy] = 0x52;

                            IsAngry = true;

                            SoundMan.PlaySound(LotaSound.EnemyDie);

                            attacked = true;


                        }

                        if (tile == 176 || tile1 == 176 || tile == 192 || tile == 192)
                        {
                            g.AddBottom("The prison bars hold.");

                            SoundMan.PlaySound(LotaSound.Bump);

                            attacked = true;
                        }
                    }

                }

                if (attacked == false)
                {
                    g.AddBottom("Nothing hit");
                }

                XleCore.wait(XleCore.Redraw, 200 + 50 * player.Gamespeed, true);

                return true;
            }

            public override bool PlayerLeave(Player player)
            {
                if (IsAngry)
                {
                    g.AddBottom("");
                    g.AddBottom("Walk out yourself.");
                }
                else
                {
                    g.AddBottom("");
                    g.AddBottom("Leave " + MapName);
                    g.AddBottom("");

                    XleCore.wait(200);

                    player.ReturnToOutside();
                }

                return true;
            }
            public override bool PlayerXamine(Player player)
            {
                g.AddBottom("");
                g.AddBottom("You are in " + XleCore.Map.MapName + ".");
                g.AddBottom("Look about to see more.");

                return true;
            }
            protected override bool PlayerSpeakImpl(Player player)
            {

                for (int j = -1; j < 3; j++)
                {
                    for (int i = -1; i < 3; i++)
                    {
                        for (int k = 0; k < Guards.Count; k++)
                        {
                            if ((Guards[k].X == player.X + i ||
                                Guards[k].X + 1 == player.X + i) && (
                                Guards[k].Y == player.Y + j ||
                                Guards[k].Y + 1 == player.Y + j))
                            {
                                SpeakToGuard(player);
                                return true;

                            }
                        }
                    }
                }

                return false;
            }
            
            protected override void PlayerStepImpl(Player player)
            {
                Point pt = new Point(player.X, player.Y);

                for (int i = 0; i < mRoofs.Count; i++)
                {
                    if (Roofs[i].Open == false && Roofs[i].CharIn(pt))
                    {
                        Roofs[i].Open = true;
                        OpenRoof(mRoofs[i]);
                    }
                    else if (Roofs[i].Open == true && IsAngry == false
                        && Roofs[i].CharIn(pt) == false)
                    {
                        Roofs[i].Open = false;
                        CloseRoof(mRoofs[i]);
                    }
                }



                if (player.X < 0 || player.X + 1 >= XleCore.Map.Width ||
                    player.Y < 0 || player.Y + 1 >= XleCore.Map.Height)
                {

                    if (IsAngry && this.GetType().Equals(typeof(Town)))
                    {
                        player.LastAttacked = this.MapID;
                    }

                    g.allowEnter = false;

                    g.AddBottom("");
                    g.AddBottom("Leave " + XleCore.Map.MapName);

                    g.AddBottom("");

                    XleCore.wait(2000);

                    player.ReturnToOutside();

                    g.AddBottom("");

                }

            }

            protected virtual void CloseRoof(Roof roof)
            {
                SoundMan.PlaySound(LotaSound.BuildingClose);
                XleCore.wait(50);

            }
            protected virtual void OpenRoof(Roof roof)
            {
                SoundMan.PlaySound(LotaSound.BuildingOpen);
                XleCore.wait(50);

            }

            protected virtual void SpeakToGuard(Player player)
            {
                g.AddBottom("");
                g.AddBottom("The guard salutes.");
            }

            public override bool CanPlayerStepInto(Player player, int xx, int yy)
            {
                int test = 0;
                int t = 0;

                if (GuardInSpot(xx, yy))
                    t = 3;


                for (int j = 0; j < 2; j++)
                {
                    for (int i = 0; i < 2; i++)
                    {

                        int xLimit = 8;
                        const int yLimit = 8;

                        //test = (int)(Lota.Map.M(yy + j, xx + i) / 16) * 16;
                        test = XleCore.Map[xx + i, yy + j];

                        if (test >= 16 * yLimit || test % 16 >= xLimit)
                        {
                            t = 3;
                        }
                    }
                }

                if (t > 0)
                {
                    return false;
                }

                return true;
            }

            public override int DrawTile(int xx, int yy)
            {
                int tile = this[xx, yy];


                int roof = RoofTile(xx, yy);

                if (roof != 127)
                    return roof;
                else
                    return tile;
            }
            protected override void DrawImpl(int x, int y, Rectangle inRect)
            {
                Draw2D(x, y, inRect);

                DrawGuards(new Point(x, y), inRect);

            }
            public void AnimateGuards()
            {
                int animTime = 1000;

                if (IsAngry)
                    animTime = 150;

                if (lastGuardAnim + animTime <= Timing.TotalMilliseconds)
                {
                    guardAnim++;
                    if (guardAnim > 2)
                        guardAnim = 0;

                    lastGuardAnim = Timing.TotalMilliseconds;
                }
            }
            protected void DrawGuards(Point centerPoint, Rectangle inRect)
            {
                int px = inRect.Left + (inRect.Width / 16) / 2 * 16;
                int py = inRect.Top + inRect.Height / 2;

                for (int i = 0; i < Guards.Count; i++)
                {
                    Guard guard = Guards[i];

                    if (PointInRoof(guard.X, guard.Y) == -1)
                    {
                        int tx = guardAnim * 32 + (g.newGraphics ? 1 : 0) * 96;
                        int ty;

                        if (IsAngry)
                            ty = ((int)guard.Facing + 3) * 32;
                        else
                            ty = 7 * 32;

                        Rectangle charRect = new Rectangle(tx, ty, 32, 32);

                        int rx = px - (centerPoint.X - guard.X) * 16;
                        int ry = py - (centerPoint.Y - guard.Y) * 16;

                        if (rx >= inRect.Left && ry >= inRect.Top && rx <= inRect.Right - 32 && ry < inRect.Bottom)
                        {
                            Rectangle destRect = new Rectangle(rx, ry, 32, 32);
                            g.Character.Draw(charRect, destRect);
                        }
                    }
                }
            }

            public override void UpdateAnim()
            {
                AnimateGuards();
            }
            public override string[] MapMenu()
            {
                List<string> retval = new List<string>();

                retval.Add("Armor");
                retval.Add("Fight");
                retval.Add("Gamespeed");
                retval.Add("Hold");
                retval.Add("Inventory");
                retval.Add("Leave");
                retval.Add("Magic");
                retval.Add("Pass");
                retval.Add("Rob");
                retval.Add("Speak");
                retval.Add("Use");
                retval.Add("Weapon");
                retval.Add("Xamine");

                return retval.ToArray();
            }

            protected override bool CheckMovementImpl(Player player, int dx, int dy)
            {
                return true;
            }

            public override void GetBoxColors(out Color boxColor, out Color innerColor, out Color fontColor, out int vertLine)
            {
                fontColor = XleColor.White;


                boxColor = XleColor.Orange;
                innerColor = XleColor.Yellow;
                vertLine = 13 * 16;
            }


            #region IHasGuards Members

            int attack, defense, hp;
            Color clr;

            public int DefaultAttack
            {
                get { return attack; }
                set { attack = value; }
            }
            public int DefaultDefense
            {
                get                {                    return defense; }
                set { defense = value; }
            }

            public int DefaultHP
            {
                get { return hp; }
                set { hp = value; }
            }

            public new Color DefaultColor
            {
                get { return clr; }
                set { clr = value; }
            }

            #endregion

        }
        [Serializable]
        public class Castle : Town
        {
            public Castle(){}
            Castle(SerializationInfo info, StreamingContext context) :
                base(info, context)
            {
            }

            public override IEnumerable<string> AvailableTilesets
            {
                get
                {
                    yield return "CastleTiles.png";
                }
            }

            public override void AfterExecuteCommand(Player player, KeyCode cmd)
            {
                /*
                if (setLastTime && tile % 16 >= 13 && tile / 16 < 2)
                {
                    tx = Lota.random.Next(0x0D, 0x10);
                    ty = Lota.random.Next(2);

                    tile = ty * 0x10 + tx;

                    if (!((tile & 0x0F) >= 0x0D && (tile & 0x10) >> 4 <= 0x01))
                    {
                        int qweruio = 1;
                        tile = 0x0F;
                    }
                    this[i, j] = tile;
                }
                else if (setLastTime && (tile / 16 == 2 && tile % 16 < 8))
                {
                    tile = cyclesDraw % 8 + 0x20;

                    this[i, j] = tile;
                }
                else if (setLastTime && (tile >= 0x40 && tile < 0x43))
                {
                    //tile = OriginalM(j, i);
                    //tile -= cyclesDraw % 3;
                    tile--;

                    while (tile < 0x40)
                        tile += 3;

                    this[i, j] = tile;
                }
                 * */
            }

            public override bool PlayerOpen(Player player)
            {
                XleEvent evt = this.GetEvent(player, 1);

                if (evt == null)
                    return false ;

                return evt.Open(player);
          
            }

            public override bool PlayerTake(Player player)
            {
                XleEvent evt = this.GetEvent(player, 1);

                if (evt == null)
                    return false;

                return evt.Take(player);
            }

            protected override void SpeakToGuard(Player player)
            {
                g.AddBottom("");

                if (!g.invisible && !g.guard)
                {
                    g.AddBottom("The guard ignores you.");
                }
                else if (g.invisible)
                {
                    if (XleCore.random.Next(1000) < 800)
                        g.AddBottom("The guard looks startled.");
                    else
                    {
                        g.AddBottom("The guard looks startled,");
                        g.AddBottom("and starts popping prozac pills.");
                    }
                }
                else if (g.guard)  // for fortress
                {

                }
            }
            protected override void OpenRoof(Roof roof)
            {
            }
            protected override void CloseRoof(Roof roof)
            {
            }

            public override string[] MapMenu()
            {
                List<string> retval = new List<string>();

                retval.Add("Armor");
                retval.Add("Fight");
                retval.Add("Gamespeed");
                retval.Add("Hold");
                retval.Add("Inventory");
                retval.Add("Magic");
                retval.Add("Open");
                retval.Add("Pass");
                retval.Add("Speak");
                retval.Add("Take");
                retval.Add("Use");
                retval.Add("Weapon");
                retval.Add("Xamine");

                return retval.ToArray();
            }
        }
        [Serializable]
        public class Dungeon : XleMap
        {
            /// <summary>
            /// Dungeon data.  Index order is mData[level, y, x].
            /// </summary>
            int[, ,] mData;
            int mCurrentLevel;

            public override bool IsMultiLevelMap
            {
                get { return true; }
            }
            public override void InitializeMap(int width, int height)
            {
                mData = new int[1, height, width];
            }
            public override void SetLevels(int count)
            {
                int[, ,] newData = new int[count, Height, Width];

                for (int i = 0; i < Levels; i++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        for (int y = 0; y < Height; y++)
                        {
                            newData[i, y, x] = mData[i, y, x];
                        }
                    }
                }
            }
            public override int Height
            {
                get { return mData.GetUpperBound(1) + 1; }
            }
            public override int Width
            {
                get { return mData.GetUpperBound(2) + 1; }
            }
            public override int Levels
            {
                get
                {
                    return mData.GetUpperBound(0) + 1;
                }
            }

            public override Color DefaultColor
            {
                get
                {
                    return XleColor.Cyan;
                }
            }
            public override int this[int xx, int yy]
            {
                get
                {
                    if (yy < 0 || yy >= Height || xx < 0 || xx >= Width)
                    {
                        return 0;
                    }
                    else
                    {
                        return mData[mCurrentLevel, yy, xx];
                    }
                }
                set
                {
                    if (yy < 0 || yy >= Height ||
                        xx < 0 || xx >= Width)
                    {
                        return;
                    }
                    else
                    {
                        mData[mCurrentLevel, yy, xx] = value;
                        //mData[(yy + mapExtend) * (mapWidth + 2 * mapExtend) + (xx + mapExtend)] = (byte)val;
                    }

                }
            }
            public override bool PlayerClimb(Player player)
            {
                switch (XleCore.Map[player.X, player.Y])
                {
                    case 0x0D:
                        player.DungeonLevel--;
                        break;

                    case 0x0A:
                        player.DungeonLevel++;
                        break;

                    default:
                        return false;

                }

                mCurrentLevel = player.DungeonLevel - 1;

                if (player.DungeonLevel == 0)
                {
                    g.AddBottom("");
                    g.AddBottom("You climb out of the dungeon.");

                    // TODO: fix this
                    player.ReturnToOutside();
                }
                else
                {
                    string tempstring = "You are now at level " + player.DungeonLevel.ToString() + ".";

                    g.AddBottom("");
                    g.AddBottom(tempstring, XleColor.White);

                }

                return true;
            }

            public override void PlayerCursorMovement(Player player, Direction dir)
            {
                string command;
                Point stepDirection;

                _MoveDungeon(player, dir, out command, out stepDirection);

                Commands.UpdateCommand(command);

                if (stepDirection.IsEmpty == false)
                {
                    player.Move(stepDirection.X, stepDirection.Y);
                }

                Commands.UpdateCommand(command);
            }


            public override string[] MapMenu()
            {
                List<string> retval = new List<string>();


                retval.Add("Armor");
                retval.Add("Climb");
                retval.Add("End");
                retval.Add("Fight");
                retval.Add("Gamespeed");
                retval.Add("Hold");
                retval.Add("Inventory");
                retval.Add("Magic");
                retval.Add("Open");
                retval.Add("Pass");
                retval.Add("Use");
                retval.Add("Weapon");
                retval.Add("Xamine");

                return retval.ToArray();
            }

            protected override void DrawImpl(int x, int y, Rectangle inRect)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            protected override bool CheckMovementImpl(Player player, int dx, int dy)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public override void GetBoxColors(out Color boxColor, out Color innerColor, out Color fontColor, out int vertLine)
            {
                fontColor = XleColor.White;

                boxColor = XleColor.Gray;
                innerColor = XleColor.LightGreen;
                fontColor = XleColor.Cyan;
                vertLine = 15 * 16;

            }

            public override bool PlayerFight(Player player)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public override bool CanPlayerStepInto(Player player, int xx, int yy)
            {
                int t = 0;

                if (this[xx, yy] >= 0x80)
                {
                    t = 3;
                }
                else
                    t = 0;



                if (t > 0)
                {
                    return false;
                }
                else
                    return true;
            }

        }
        [Serializable]
        public class Museum : XleMap
        {
            int[,] mData;

            public override string[] MapMenu()
            {
                List<string> retval = new List<string>();

                retval.Add("Armor");
                retval.Add("Fight");
                retval.Add("Gamespeed");
                retval.Add("Hold");
                retval.Add("Inventory");
                retval.Add("Pass");
                retval.Add("Rob");
                retval.Add("Speak");
                retval.Add("Take");
                retval.Add("Use");
                retval.Add("Weapon");
                retval.Add("Xamine");

                return retval.ToArray();
            }
            public override void InitializeMap(int width, int height)
            {
                mData = new int[height, width];
            }

            public override int Height
            {
                get { return mData.GetUpperBound(0) + 1; }
            }
            public override int Width
            {
                get { return mData.GetUpperBound(1) + 1; }
            }

            public override int this[int xx, int yy]
            {
                get
                {

                    if (yy < 0 || yy >= Height || xx < 0 || xx >= Width)
                    {
                        return 0;
                    }
                    else
                    {
                        return mData[yy, xx];
                    }
                }
                set
                {
                    if (yy < 0 || yy >= Height ||
                        xx < 0 || xx >= Width)
                    {
                        return;
                    }
                    else
                    {
                        mData[yy, xx] = value;
                    }

                }
            }
            public override void PlayerCursorMovement(Player player, Direction dir)
            {
                string command;
                Point stepDirection;

                _MoveDungeon(player, dir, out command, out stepDirection);

                Commands.UpdateCommand(command);

                if (stepDirection.IsEmpty == false)
                {
                    player.Move(stepDirection.X, stepDirection.Y);
                }

                Commands.UpdateCommand(command);
            }


            protected override void DrawImpl(int x, int y, Rectangle inRect)
            {
                
            }

            protected override bool CheckMovementImpl(Player player, int dx, int dy)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public override void GetBoxColors(out Color boxColor, out Color innerColor, out Color fontColor, out int vertLine)
            {
                fontColor = XleColor.White;


                boxColor = XleColor.Gray;
                innerColor = XleColor.Yellow;
                vertLine = 15 * 16;
            }

            public override bool PlayerFight(Player player)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public override bool CanPlayerStepInto(Player player, int xx, int yy)
            {
                if (XleCore.Map[xx, yy] >= 0x80)
                    return false;
                else
                    return true;
            }
        }
    }
}