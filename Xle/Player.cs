using System;
using System.Collections.Generic;
using System.Text;

using AgateLib;
using AgateLib.Geometry;
using AgateLib.Serialization.Xle;
using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Services.Implementation;

namespace ERY.Xle
{
    public enum Attributes
    {
        dexterity = 0,
        strength,
        charm,
        endurance,
        intelligence
    };

    public class AttributeContainer : IXleSerializable
    {
        int[] atr = new int[5];

        public AttributeContainer()
        {
            Reset();
        }

        public int this[Attributes index]
        {
            get { return atr[(int)index]; }
            set
            {
                atr[(int)index] = value;
            }
        }

        #region IXleSerializable Members

        void IXleSerializable.WriteData(XleSerializationInfo info)
        {
            info.Write("Attributes", atr);
        }
        void IXleSerializable.ReadData(XleSerializationInfo info)
        {
            atr = info.ReadInt32Array("Attributes");
        }

        #endregion

        public void Reset()
        {
            for (int i = 0; i < 5; i++)
            {
                atr[i] = 15;
            }
        }
    }

    public class RaftData : IXleSerializable
    {
        public Point Location;
        public int MapNumber;

        public RaftData(int x, int y, int map)
        {
            Location = new Point(x, y);
            MapNumber = map;
        }
        private RaftData()
        { }

        public int X
        {
            get { return Location.X; }
            set { Location.X = value; }
        }
        public int Y
        {
            get { return Location.Y; }
            set { Location.Y = value; }
        }

        #region IXleSerializable Members

        void IXleSerializable.WriteData(XleSerializationInfo info)
        {
            info.Write("X", Location.X);
            info.Write("Y", Location.Y);
            info.Write("MapID", MapNumber);
        }
        void IXleSerializable.ReadData(XleSerializationInfo info)
        {
            MapNumber = info.ReadInt32("MapID");
            Location.X = info.ReadInt32("X");
            Location.Y = info.ReadInt32("Y");
        }

        #endregion

        public int RaftImage { get; set; }
    }

    public class Player : IXleSerializable
    {
        AttributeContainer mAttributes = new AttributeContainer();
        double food;
        int gold;
        int goldBank;
        double timedays;
        double timequality;

        int onRaft = -1;

        int gamespeed;
        int map;
        int lastMap;
        int dungeon;
        int hp;
        int level;
        public int returnX, returnY, returnMap;
        public Direction returnFacing = Direction.East;
        int x, y;
        int dungeonLevel;
        Direction faceDirection = Direction.East;

        int currentArmorIndex = -1;
        int currentWeaponIndex = -1;

        WeaponItem bareHands = new WeaponItem();
        ArmorItem bareArmor = new ArmorItem();

        List<WeaponItem> weapons = new List<WeaponItem>();
        List<ArmorItem> armor = new List<ArmorItem>();

        ItemContainer mItems = new ItemContainer();

        int hold;
        int lastAttacked;

        int[] chests = new int[50];

        List<RaftData> rafts = new List<RaftData>();

        public int loan;					// loan amount
        public int dueDate;				// time in days that the money is due

        public int mailTown;

        public IXleSerializable StoryData { get; set; }

        string mName;

        #region --- Construction and Serialization ---

        public Player()
        {
            NewPlayer("New Player");
        }
        public Player(string newName)
        {
            NewPlayer(newName);
        }

        /// <summary>
        /// reinitialize all the variables
        /// </summary>
        /// <param name="newName"></param>
        private void NewPlayer(string newName)
        {
            mName = newName;

            RaftFaceDirection = Direction.West;
            faceDirection = Direction.East;
            RenderColor = XleColor.White;

            food = 40;
            gold = 20;

            gamespeed = 3;

            hp = 200;
            level = 1;

            ClearRafts();

            SortEquipment();
        }

        void IXleSerializable.WriteData(XleSerializationInfo info)
        {
            info.Write("Attributes", mAttributes);
            info.Write("Food", food);
            info.Write("Gold", gold);
            info.Write("GoldInBank", goldBank);
            info.Write("TimeDays", timedays);
            info.Write("TimeQuality", timequality);

            info.Write("OnRaft", onRaft);
            info.Write("Rafts", rafts);

            info.Write("GameSpeed", gamespeed);
            info.Write("Map", map);
            info.Write("LastMap", lastMap);
            info.Write("Dungeon", dungeon);
            info.Write("HP", hp);
            info.Write("Level", level);
            info.Write("ReturnMap", returnMap);
            info.Write("ReturnX", returnX);
            info.Write("ReturnY", returnY);
            info.WriteEnum<Direction>("ReturnFacing", returnFacing, false);
            info.Write("X", x);
            info.Write("Y", y);
            info.Write("DungeonLevel", dungeonLevel);
            info.Write("Facing", (int)faceDirection);

            info.Write("CurrentArmorIndex", currentArmorIndex);
            info.Write("CurrentWeaponIndex", currentWeaponIndex);
            info.Write("Weapons", Weapons);
            info.Write("Armor", Armor);

            info.Write("Item", Items);
            info.Write("Hold", hold);

            info.Write("LastAttacked", lastAttacked);
            info.Write("VaultGold", VaultGold);

            info.Write("Chests", chests);

            info.Write("Loan", loan);					// loan amount
            info.Write("DueDate", dueDate);				// time in days that the money is due

            info.Write("MailTown", mailTown);

            info.Write("Name", mName);
            info.Write("StoryData", StoryData);
        }
        void IXleSerializable.ReadData(XleSerializationInfo info)
        {
            mAttributes = (AttributeContainer)info.ReadObject("Attributes");
            food = info.ReadDouble("Food");
            gold = info.ReadInt32("Gold");
            goldBank = info.ReadInt32("GoldInBank");
            timedays = info.ReadDouble("TimeDays");
            timequality = info.ReadDouble("TimeQuality");

            onRaft = info.ReadInt32("OnRaft");
            rafts = info.ReadList<RaftData>("Rafts");

            gamespeed = info.ReadInt32("GameSpeed");
            map = info.ReadInt32("Map");
            lastMap = info.ReadInt32("LastMap");
            dungeon = info.ReadInt32("Dungeon");
            hp = info.ReadInt32("HP");
            level = info.ReadInt32("Level");
            returnMap = info.ReadInt32("ReturnMap");
            returnX = info.ReadInt32("ReturnX");
            returnY = info.ReadInt32("ReturnY");
            returnFacing = info.ReadEnum<Direction>("ReturnFacing");
            x = info.ReadInt32("X");
            y = info.ReadInt32("Y");
            dungeonLevel = info.ReadInt32("DungeonLevel");
            faceDirection = (Direction)info.ReadInt32("Facing");

            weapons = info.ReadList<WeaponItem>("Weapons");
            armor = info.ReadList<ArmorItem>("Armor");

            currentArmorIndex = info.ReadInt32("CurrentArmorIndex");
            currentWeaponIndex = info.ReadInt32("CurrentWeaponIndex");

            mItems = info.ReadObject<ItemContainer>("Item");
            hold = info.ReadInt32("Hold");

            lastAttacked = info.ReadInt32("LastAttacked");
            VaultGold = info.ReadInt32("VaultGold");

            chests = info.ReadArray<int>("Chests");

            loan = info.ReadInt32("Loan");					// loan amount
            dueDate = info.ReadInt32("DueDate");				// time in days that the money is due

            mailTown = info.ReadInt32("MailTown");

            mName = info.ReadString("Name");

            StoryData = info.ReadObject<IXleSerializable>("StoryData");

            RenderColor = XleColor.White;

        }

        #endregion

        public string Name
        {
            get
            {
                return mName;
            }
        }

        public void SavePlayer()
        {
            SavePlayer(@"Saved/" + Name + ".chr");
        }
        private void SavePlayer(string filename)
        {
            if (StoryData == null)
                throw new NullReferenceException("StoryData cannot be null!");

            XleSerializer ser = new XleSerializer(typeof(Player));

            using (System.IO.Stream ff = System.IO.File.Open
                (filename, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                ser.Serialize(ff, this);
            }
        }
        public static Player LoadPlayer(string filename)
        {
            XleSerializer ser = new XleSerializer(typeof(Player));

            using (System.IO.Stream ff = System.IO.File.OpenRead(filename))
            {
                Player p = (Player)ser.Deserialize(ff);

                return p;
            }
        }

        /// <summary>
        /// Gets or sets the character level.
        /// </summary>
        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        /// <summary>
        /// Returns or changes the specified attribute
        /// </summary>
        /// <param name="atr"></param>
        /// <param name="change"></param>
        /// <returns></returns>
        public AttributeContainer Attribute
        {
            get
            {
                return mAttributes;
            }
        }

        public int Gamespeed
        {
            get { return gamespeed; }
            set
            {
                System.Diagnostics.Debug.Assert(value >= 1 && value <= 5);

                gamespeed = value;

            }
        }

        /// <summary>
        /// Returns or adjusts current HP
        /// </summary>
        /// <param name="change"></param>
        /// <returns></returns>
        public int HP
        {
            get { return hp; }
            set
            {
                hp = value;

                if (hp > MaxHP)
                    hp = MaxHP;

                if (hp <= 0)
                {
                    hp = 0;
                }

            }
        }

        /// <summary>
        /// 			// calculates max hp
        /// </summary>	
        public int MaxHP
        {
            get
            {
                switch (Level)
                {
                    case 1: return 200;
                    case 2: return 300;
                    case 3: return 500;
                    case 4: return 800;
                    case 5: return 1200;
                    case 6: return 1600;
                    case 7: return 2200;
                    case 10: return 4600;

                    default:
                        throw new ArgumentException("Level is wrong!");
                }
            }
        }
        /// <summary>
        /// Gets or sets food.
        /// </summary>
        /// <param name="change"></param>
        /// <returns></returns>
        public double Food
        {
            get
            {
                if (food < 0)
                    return 0;
                else
                    return food;
            }
            set
            {
                food = value;
            }
        }
        public int Gold
        {
            get { return gold; }
            set
            {
                if (gold < 0)
                {
                    throw new InvalidOperationException("Cannot set gold to less than zero.");
                }

                gold = value;
            }
        }
        /// <summary>
        /// Adjusts current gold if there's enough available and returns true if there is
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool Spend(int amount)
        {
            if (amount <= gold)
            {
                gold -= amount;
                return true;
            }

            return false;
        }
        /// <summary>
        /// Increases or decreases gold in bank or returns current value
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public int GoldInBank
        {
            get { return goldBank; }
            set
            {
                goldBank = value;

                if (goldBank < 0)
                    goldBank = 0;

            }
        }

        /// <summary>
        /// Returns the time in days or increments them.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public double TimeDays
        {
            get { return timedays; }
            set
            {
                System.Diagnostics.Debug.Assert(value >= timedays);

                double diff = value - timedays;

                food -= diff;

                timedays = value;
            }
        }
        public double TimeQuality
        {
            get { return timequality; }
            set { timequality = value; }
        }

        /// <summary>
        /// 					// returns x position
        /// </summary>
        /// <returns></returns>
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        /// <summary>
        /// 					// returns y position
        /// </summary>
        /// <returns></returns>
        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public Point Location
        {
            get { return new Point(x, y); }
            set
            {
                x = value.X;
                y = value.Y;
            }
        }

        /// <summary>
        /// Sets or returns the current dungeon level for the player
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public int DungeonLevel
        {
            get { return dungeonLevel; }
            set
            {
                dungeonLevel = value;
            }
        }
        /// <summary>
        /// Sets or returns the direction the player is facing
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public Direction FaceDirection
        {
            get { return faceDirection; }
            set
            {
                if (value == Direction.None)
                    throw new ArgumentException();

                System.Diagnostics.Debug.Assert(
                    value == Direction.East ||
                    value == Direction.West ||
                    value == Direction.North ||
                    value == Direction.South);

                faceDirection = value;

                if (value == Direction.East || value == Direction.West)
                    RaftFaceDirection = value;
            }

        }
        /// <summary>
        /// Stores the town number that was last attacked
        /// </summary>
        /// <param name="newAtt"></param>
        /// <returns></returns>
        public int LastAttackedMapID
        {
            get { return lastAttacked; }
            set
            {
                lastAttacked = value;
            }
        }

        public List<RaftData> Rafts
        {
            get { return rafts; }
        }

        /// <summary>
        /// Gets the current map ID. This is used for serialization.
        /// </summary>
        public int MapID
        {
            get { return map; }
            set { map = value; }
        }

        /// <summary>
        /// Returns the last map the player was on
        /// </summary>
        /// <returns></returns>
        private int LastMap
        {
            get
            {
                return lastMap;
            }
        }

        public int VaultGold { get; set; }

        /// <summary>
        /// Gets whether or not the player is on a raft. If you want to have the player
        /// disembark, set the OnRaft property to -1.
        /// </summary>
        public bool IsOnRaft
        {
            get
            {
                return onRaft >= 0;
            }
        }

        public RaftData BoardedRaft
        {
            get
            {
                if (onRaft == -1)
                    return null;

                return Rafts[onRaft];
            }
            set { onRaft = Rafts.IndexOf(value); }
        }

        /// <summary>
        /// Clears all rafts from all maps
        /// </summary>
        public void ClearRafts()
        {
            Rafts.Clear();
        }

        /// <summary>
        /// sets or returns the item that is currently being held
        /// </summary>
        public int Hold
        {
            get { return hold; }
            set
            {
                if (value > 0 && value < 30)
                {
                    if (Items[value] > 0)
                    {
                        hold = value;
                    }
                }

                if (value == 0)
                {
                    hold = value;
                }
            }
        }
        /// <summary>
        /// Sets the item that is currently being held.
        /// </summary>
        /// <param name="h">The value of the menu selection chosen.</param>
        /// <returns></returns>
        public int HoldMenu(int h)
        {
            int i;

            if (h != 0)
            {
                for (i = 1; i <= h && h < 30; i++)
                {
                    if (Items[i] == 0)
                    {
                        h++;
                    }

                }
            }

            if (h < 30)
            {
                hold = h;
            }

            return hold;

        }

        public WeaponItem CurrentWeapon
        {
            get
            {
                if (currentWeaponIndex == -1)
                    return bareHands;

                return weapons[currentWeaponIndex];
            }
            set
            {
                if (value == null)
                    currentWeaponIndex = -1;
                else
                {
                    int index = weapons.IndexOf(value);

                    if (index == -1)
                        throw new InvalidOperationException("Weapon not found.");

                    currentWeaponIndex = weapons.IndexOf(value);
                }
            }
        }
        public ArmorItem CurrentArmor
        {
            get
            {
                if (currentArmorIndex == -1)
                    return bareArmor;

                return armor[currentArmorIndex];
            }
            set
            {
                currentArmorIndex = armor.IndexOf(value);
            }
        }

        public List<ArmorItem> Armor { get { return armor; } }
        public List<WeaponItem> Weapons { get { return weapons; } }

        public ItemContainer Items { get { return mItems; } }

        /// <summary>
        /// Adds a weapon to inventory
        /// </summary>
        /// <param name="w"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public bool AddWeapon(int w, int q)
        {
            if (Weapons.Count >= 5)
                return false;

            Weapons.Add(new WeaponItem { ID = w, Quality = q });
            SortEquipment();

            return true;
        }
        public bool AddArmor(int a, int q)
        {
            if (Armor.Count >= 3)
                return false;

            Armor.Add(new ArmorItem { ID = a, Quality = q });
            SortEquipment();

            return true;
        }

        public void RemoveWeapon(int index)
        {
            Weapons.RemoveAt(index);
        }
        public void RemoveArmor(int index)
        {
            Armor.RemoveAt(index);
        }
        /// <summary>
        /// Sorts weapons and armor
        /// </summary>	
        public void SortEquipment()
        {
            Weapons.Sort(Equipment.Sorter);
            Armor.Sort(Equipment.Sorter);

            while (currentWeaponIndex >= weapons.Count)
                currentWeaponIndex--;
            while (currentArmorIndex >= armor.Count)
                currentArmorIndex--;
        }
        
        public void SetReturnLocation(int map, int x, int y)
        {
            SetReturnLocation(map, x, y, Direction.South);
        }
        public void SetReturnLocation(int map, int x, int y, Direction facing)
        {
            this.returnMap = map;
            this.returnX = x;
            this.returnY = y;
            this.returnFacing = facing;
        }

        public int WeaponEnchantTurnsRemaining { get; set; }
        public int ArmorEnchantTurnsRemaining { get; set; }

        public Direction RaftFaceDirection { get; private set; }

        public void RemoveEquipment(Equipment item)
        {
            Weapons.Remove(item as WeaponItem);
            Armor.Remove(item as ArmorItem);
            SortEquipment();
        }

        public Color RenderColor { get; set; }
    }
}