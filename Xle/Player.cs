using System;
using System.Collections.Generic;
using System.Text;

using AgateLib;
using AgateLib.Geometry;
using AgateLib.Serialization.Xle;
using ERY.Xle.Maps.XleMapTypes;

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

	[Obsolete]
	public class VariableContainer : IDictionary<string, int>, IXleSerializable
	{
		Dictionary<string, int> mValues = new Dictionary<string, int>();

		#region IDictionary<string,int> Members

		public void Add(string key, int value)
		{
			mValues[key] = value;
		}

		public bool ContainsKey(string key)
		{
			return mValues.ContainsKey(key);
		}

		public ICollection<string> Keys
		{
			get { return mValues.Keys; }
		}

		public bool Remove(string key)
		{
			return mValues.Remove(key);
		}

		public bool TryGetValue(string key, out int value)
		{
			return mValues.TryGetValue(key, out value);
		}

		public ICollection<int> Values
		{
			get { return mValues.Values; }
		}

		public int this[string key]
		{
			get
			{
				if (mValues.ContainsKey(key))
					return mValues[key];
				else
					return 0;
			}
			set
			{
				mValues[key] = value;
			}
		}

		#endregion
		#region ICollection<KeyValuePair<string,int>> Members

		void ICollection<KeyValuePair<string, int>>.Add(KeyValuePair<string, int> item)
		{
			((ICollection<KeyValuePair<string, int>>)mValues).Add(item);
		}

		public void Clear()
		{
			((ICollection<KeyValuePair<string, int>>)mValues).Clear();
		}

		bool ICollection<KeyValuePair<string, int>>.Contains(KeyValuePair<string, int> item)
		{
			return ((ICollection<KeyValuePair<string, int>>)mValues).Contains(item);
		}

		void ICollection<KeyValuePair<string, int>>.CopyTo(KeyValuePair<string, int>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<string, int>>)mValues).CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return mValues.Count; }
		}

		bool ICollection<KeyValuePair<string, int>>.IsReadOnly
		{
			get { return false; }
		}

		bool ICollection<KeyValuePair<string, int>>.Remove(KeyValuePair<string, int> item)
		{
			return ((ICollection<KeyValuePair<string, int>>)mValues).Remove(item);
		}

		#endregion
		#region IEnumerable<KeyValuePair<string,int>> Members

		public IEnumerator<KeyValuePair<string, int>> GetEnumerator()
		{
			return mValues.GetEnumerator();
		}

		#endregion
		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
		#region IXleSerializable Members

		void IXleSerializable.WriteData(XleSerializationInfo info)
		{
			info.Write("Values", mValues);
		}

		void IXleSerializable.ReadData(XleSerializationInfo info)
		{
			mValues = info.ReadDictionaryInt32<string>("Values");
		}

		#endregion
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
		Direction returnFacing;
		int x, y;
		int dungeonLevel;
		Direction faceDirection = Direction.East;

		int currentArmor;
		int currentWeapon;
		int[] weapon = new int[6];
		int[] armor = new int[4];
		int[] weaponQuality = new int[6];
		int[] armorQuality = new int[4];
		[Obsolete]
		int[] item { get { return mItems.ItemArray; } set { mItems.ItemArray = value; } }

		ItemContainer mItems = new ItemContainer();

		int hold;
		int lastAttacked;
		int vaultGold;

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

			food = 40;
			gold = 20;

			gamespeed = 3;

			hp = 200;
			level = 1;

			vaultGold = 17;

			ClearRafts();

			SortEquipment();
			DebugSettings();
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

			info.Write("CurrentArmor", currentArmor);
			info.Write("CurrentWeapon", currentWeapon);
			info.Write("Weapon", weapon);
			info.Write("Armor", armor);
			info.Write("WeaponQuality", weaponQuality);
			info.Write("ArmorQuality", armorQuality);
			info.Write("Item", Items);
			info.Write("Hold", hold);

			info.Write("LastAttacked", lastAttacked);
			info.Write("VaultGold", vaultGold);

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
			if (info.ContainsKey("TimeQuality"))
				timequality = info.ReadDouble("TimeQuality");

			onRaft = info.ReadInt32("OnRaft");
			if (info.ContainsKey("Rafts"))
			{
				rafts = info.ReadList<RaftData>("Rafts");
			}
			else
				onRaft = -1;

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

			currentArmor = info.ReadInt32("CurrentArmor");
			currentWeapon = info.ReadInt32("CurrentWeapon");
			weapon = info.ReadInt32Array("Weapon");
			armor = info.ReadInt32Array("Armor");
			weaponQuality = info.ReadInt32Array("WeaponQuality");
			armorQuality = info.ReadInt32Array("ArmorQuality");
			hold = info.ReadInt32("Hold");

			lastAttacked = info.ReadInt32("LastAttacked");
			vaultGold = info.ReadInt32("VaultGold");

			chests = info.ReadInt32Array("Chests");

			loan = info.ReadInt32("Loan");					// loan amount
			dueDate = info.ReadInt32("DueDate");				// time in days that the money is due

			mailTown = info.ReadInt32("MailTown");

			mName = info.ReadString("Name");

			if (info.ContainsKey("StoryData"))
			{
				StoryData = info.ReadObject<IXleSerializable>("StoryData");
			}
			else
			{
				StoryData = XleCore.Factory.CreateStoryData();
			}
			DebugSettings();
		}

		private void DebugSettings()
		{
			if (XleCore.EnableDebugMode)
			{
				Gamespeed = 1;
			}
		}

		#endregion

		/// <summary>
		/// The player has died
		/// </summary>
		public void Dead()
		{
			hp = 0;

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("            You died!");
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();

			SoundMan.PlaySound(LotaSound.VeryBad);

			XleCore.FlashHPWhileSound(XleColor.Red, XleColor.Yellow);

			XleCore.ChangeMap(this, 1, 0);

			TerrainType t;

			do
			{
				x = XleCore.random.Next(XleCore.Map.Width);
				y = XleCore.random.Next(XleCore.Map.Height);

				t = XleCore.GameState.Map.TerrainAt(x, y);

			} while (t != TerrainType.Grass && t != TerrainType.Forest);

			Rafts.Clear();

			hp = MaxHP;
			food = 30 + XleCore.random.Next(10);
			gold = 25 + XleCore.random.Next(30);
			onRaft = -1;

			while (SoundMan.IsPlaying(LotaSound.VeryBad))
				XleCore.Wait(40);

			XleCore.TextArea.PrintLine("The powers of the museum");
			XleCore.TextArea.PrintLine("resurrect you from the grave!");
			XleCore.TextArea.PrintLine();

			SoundMan.PlaySoundSync(LotaSound.VeryGood);
		}

		public string Name
		{
			get
			{
				return mName;
			}
		}

		public void CheckDead()
		{
			if (hp == 0 || food < 0)
				Dead();
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
		/// the current outside terrain the player is standing on
		/// </summary>
		/// <returns></returns>
		public TerrainType Terrain
		{
			get
			{
				return XleCore.Map.TerrainAt(x, y);
			}
		}
		/// <summary>
		/// 					// returns x position
		/// </summary>
		/// <returns></returns>
		public int X
		{
			get { return x; }
			set
			{
				x = value;
				UpdateRaftPosition();
			}
		}
		/// <summary>
		/// 					// returns y position
		/// </summary>
		/// <returns></returns>
		public int Y
		{
			get { return y; }
			set
			{
				y = value;
				UpdateRaftPosition();
			}
		}


		public Point Location
		{
			get { return new Point(x, y); }
			set
			{
				SetPos(value.X, value.Y);
			}
		}


		private void UpdateRaftPosition()
		{
			if (IsOnRaft)
			{
				rafts[onRaft].X = X;
				rafts[onRaft].Y = Y;
			}
		}
		/// <summary>
		///  Sets the positions of the player on the current map.  Returns true
		/// if successful.
		/// </summary>
		/// <param name="xx"></param>
		/// <param name="yy"></param>
		/// <returns></returns>
		public bool SetPos(int xx, int yy)
		{
			if (XleCore.GameState == null || XleCore.GameState.Map == null)
			{
				x = xx;
				y = yy;

				return true;
			}

			if (XleCore.Map.CanPlayerStepInto(this, xx, yy))
			{
				x = xx;
				y = yy;

				for (int i = 0; i < rafts.Count; i++)
				{
					if (Rafts[i].MapNumber != MapID)
						continue;

					if (Rafts[i].Location.Equals(new Point(xx, yy)))
					{
						BoardRaft(i);
					}
				}

				UpdateRaftPosition();

				return true;
			}
			else
			{
				return false;
			}

		}
		/// <summary>
		/// Moves the PC in the specified direction.
		/// Returns true if successful.
		/// </summary>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		/// <returns></returns>
		public bool Move(int dx, int dy)
		{
			if (dx != 0 || dy != 0)
			{
				if (XleCore.GameState.Map.CheckMovement(this, dx, dy))
				{
					XleCore.GameState.Map.BeforeStepOn(this, x + dx, y + dy);

					return SetPos(x + dx, y + dy);
				}
			}

			return false;
		}

		public bool Move(Point stepDirection)
		{
			return Move(stepDirection.X, stepDirection.Y);
		}

		public void NewMap()
		{
			NewMap(0, 0);
		}
		/// <summary>
		/// 	// sets coordinates for a new map & loads coordinates for outside
		/// </summary>
		/// <param name="xx"></param>
		/// <param name="yy"></param>
		public void NewMap(int xx, int yy)
		{
			SoundMan.StopAllSounds();

			map = XleCore.Map.MapID;
			/*
			if (Lota.Map.MapType == MapTypes.Outside && outmap == map)
			{
				x = outx;
				y = outy;

				dungeonLevel = 0;

				Lota.Map.IsAngry = false;
			}
			else
			{
			 * */
			// store outside map coordinate
			returnX = x;
			returnY = y;

			// set new coordinate
			x = xx;
			y = yy;

			if (map == lastAttacked)
			{
				(XleCore.Map as Town).IsAngry = true;

				g.ClearBottom();
				XleCore.TextArea.PrintLine("We remember you - slime!");
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine();

				XleCore.Wait(2000);

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
				System.Diagnostics.Debug.Assert(
					value == Direction.East ||
					value == Direction.West ||
					value == Direction.North ||
					value == Direction.South);

				faceDirection = value;

			}

		}
		/// <summary>
		/// Stores the town number that was last attacked
		/// </summary>
		/// <param name="newAtt"></param>
		/// <returns></returns>
		public int LastAttacked
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

		[Obsolete]
		public void SetMap(int newMap, int newX, int newY)
		{
			XleCore.ChangeMap(this, newMap, -1, newX, newY);
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

		public int VaultGold
		{
			get { return vaultGold; }
			set
			{
				System.Diagnostics.Debug.Assert(value > 0 && value < 20);
				vaultGold = value;
			}
		}

		public bool IsOnRaft
		{
			get
			{
				return onRaft >= 0;
			}
		}

		/// <summary>
		/// Returns the raft that the player is on (or -1 if not)
		/// </summary>
		public int OnRaft
		{
			get
			{
				return onRaft;
			}
			set
			{
				if (value < -1 || value >= rafts.Count)
					throw new ArgumentOutOfRangeException();

				onRaft = value;
			}
		}
		/// <summary>
		/// Boards a raft
		/// </summary>
		/// <param name="?"></param>
		/// <returns></returns>    
		[Obsolete("Majorly.")]
		public void BoardRaft(int raftNum)
		{
			if (!(raftNum < 0 || (Rafts[raftNum].X == -10 && Rafts[raftNum].Y == -10)))
			{
				onRaft = raftNum;
			}
		}
		public void BoardRaft(RaftData raft)
		{
			onRaft = rafts.IndexOf(raft);
		}

		/// <summary>
		/// Disembarks a raft
		/// </summary>
		/// <param name="dir"></param>
		public void Disembark(Direction dir)
		{
			onRaft = -1;

			FaceDirection = dir;

			switch (dir)
			{
				case Direction.East:
					Move(1, 0);
					break;
				case Direction.North:
					Move(0, -1);
					break;
				case Direction.West:
					Move(-1, 0);
					break;
				case Direction.South:
					Move(0, 1);
					break;
			}

		}


		/// <summary>
		/// adds a raft to the map
		/// </summary>
		/// <param name="map"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void AddRaft(int map, int x, int y)
		{
			Rafts.Add(new RaftData(x, y, map));
		}
		/// <summary>
		/// Clears all rafts from all maps
		/// </summary>
		public void ClearRafts()
		{
			Rafts.Clear();
			//rafts.Add(new RaftData(0, 0, 0)); // add a dummy raft, because the list is zero based.
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
					if (item[value] > 0)
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
					if (item[i] == 0)
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
		/// <summary>
		/// Returns the armor that the player is carrying in the specified slot
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public int ArmorType(int index)
		{
			if (index <= 0 || index > 3)
				return 0;

			return armor[index];

		}
		/// <summary>
		/// returns the weapon that the player is carrying in the specified slot
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public int WeaponType(int index)
		{
			if (index <= 0 || index > 5)
				return 0;

			return weapon[index];

		}
		/// <summary>
		/// returns the armor quality
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public int ArmorQuality(int index)
		{
			if (index <= 0 || index > 3)
				return 0;

			return armorQuality[index];

		}
		/// <summary>
		/// returns the weapon quality
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public int WeaponQuality(int index)
		{
			if (index <= 0 || index > 5)
				return 0;

			return weaponQuality[index];
		}

		/// sets or returns the armor currently worn
		public int CurrentArmorIndex
		{
			get
			{
				return currentArmor;
			}
			set
			{
				if (value == 0)
				{
					currentArmor = value;
				}
				else
				{
					for (int j = 1; j <= value; j++)
					{
						if (armor[j] == 0)
							value++;
					}

					currentArmor = value;
				}
			}
		}
		/// sets or returns the weapon currently equiped
		public int CurrentWeaponIndex
		{
			get { return currentWeapon; }
			set
			{
				if (value == 0)
				{
					currentWeapon = value;
				}
				else
				{
					currentWeapon = 0;

					for (int j = 1; j <= value; j++)
					{
						if (weapon[j] == 0)
							value++;
					}

					currentWeapon = value;
				}
			}
		}

		public int CurrentArmorType
		{
			get
			{
				if (currentArmor == 0)
					return 0;

				return armor[currentArmor];
			}
		}
		public double CurrentArmorQuality
		{
			get
			{
				if (currentArmor == 0)
					return 0;

				return armorQuality[currentArmor];
			}
		}

		public int CurrentWeaponType
		{
			get
			{
				if (currentWeapon == 0)
					return 0;

				return weapon[currentWeapon];
			}
		}

		public double CurrentWeaponQuality
		{
			get
			{
				if (currentWeapon == 0)
					return 0;

				return weaponQuality[currentWeapon];
			}
		}

		public string CurrentArmorName
		{
			get
			{
				if (currentArmor == 0)
					return "Nothing";

				return XleCore.ArmorList[armor[currentArmor]].Name;
			}
		}

		/// <summary>
		/// Gives the name of the current weapon being used. Does not include the
		/// quality information (shoddy, good, etc.)
		/// </summary>
		public string CurrentWeaponTypeName
		{
			get
			{
				if (currentWeapon == 0)
					return "Bare Hands";

				return XleCore.WeaponList[weapon[currentWeapon]].Name;
			}
		}

		public ItemContainer Items { get { return mItems; } }

		/// <summary>
		/// Returns the number of the specified items
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		[Obsolete("Use Items property instead.")]
		public int Item(int index)
		{
			return item[index];
		}
		/// <summary>
		/// Adjusts the number of items the player has
		/// </summary>
		/// <param name="itm"></param>
		/// <param name="inc"></param>
		/// <returns></returns>
		[Obsolete("Use Items property instead.")]
		public int ItemCount(int itm, int inc)
		{
			item[itm] += inc;

			if (item[itm] <= 0)
			{
				item[itm] = 0;

				if (hold == itm)
				{
					hold = 0;
				}
			}

			return item[itm];
		}
		/// <summary>
		/// Adds a weapon to inventory
		/// </summary>
		/// <param name="w"></param>
		/// <param name="q"></param>
		/// <returns></returns>
		public bool AddWeapon(int w, int q)
		{
			int i;

			for (i = 1; i <= 5; i++)
			{
				if (weapon[i] == 0)
				{
					weapon[i] = w;
					weaponQuality[i] = q;

					SortEquipment();

					return true;
				}
			}

			return false;
		}
		///
		public bool AddArmor(int a, int q)
		{
			int i;

			for (i = 1; i <= 3; i++)
			{
				if (ArmorType(i) == 0)
				{
					armor[i] = a;
					armorQuality[i] = q;

					SortEquipment();

					return true;
				}
			}

			return false;
		}

		public bool RemoveWeapon(int index)
		{
			if (index < 1 || index > 5) throw new ArgumentOutOfRangeException();

			if (weapon[index] == 0)
				return false;

			for (int i = index; i < 5; i++)
			{
				weapon[i] = weapon[i + 1];
				weaponQuality[i] = weaponQuality[i + 1];
			}

			weapon[5] = 0;
			weaponQuality[5] = 0;

			return true;
		}

		public bool RemoveArmor(int index)
		{
			if (index < 1 || index > 3) throw new ArgumentOutOfRangeException();

			if (armor[index] == 0)
				return false;

			for (int i = index; i < 3; i++)
			{
				armor[i] = armor[i + 1];
				armorQuality[i] = armorQuality[i + 1];
			}

			armor[3] = 0;
			armorQuality[3] = 0;

			return true;
		}
		/// <summary>
		/// Sorts weapons and armor
		/// </summary>	
		public void SortEquipment()
		{
			int i = 0;
			int tempItem;
			int tempQuality;

			do
			{

				i++;

				if (weapon[i + 1] < weapon[i])
				{
					tempItem = weapon[i];
					tempQuality = weaponQuality[i];

					weapon[i] = weapon[i + 1];
					weaponQuality[i] = weaponQuality[i + 1];

					weapon[i + 1] = tempItem;
					weaponQuality[i + 1] = tempQuality;


					if (currentWeapon == i)
					{
						currentWeapon = i + 1;
					}
					else if (currentWeapon == i + 1)
					{
						currentWeapon = i;
					}

					i = 0;

				}
				else if (weapon[i + 1] == weapon[i] && weaponQuality[i + 1] < weaponQuality[i])
				{

					tempItem = weapon[i];
					tempQuality = weaponQuality[i];

					weapon[i] = weapon[i + 1];
					weaponQuality[i] = weaponQuality[i + 1];

					weapon[i + 1] = tempItem;
					weaponQuality[i + 1] = tempQuality;


					if (currentWeapon == i)
					{
						currentWeapon = i + 1;
					}
					else if (currentWeapon == i + 1)
					{
						currentWeapon = i;
					}

					i = 0;
				}

			} while (i < 4);

			i = 0;

			do
			{

				i++;

				if (armor[i + 1] < armor[i])
				{
					tempItem = armor[i];
					tempQuality = armorQuality[i];

					armor[i] = armor[i + 1];
					armorQuality[i] = armorQuality[i + 1];

					armor[i + 1] = tempItem;
					armorQuality[i + 1] = tempQuality;


					if (currentArmor == i)
					{
						currentArmor = i + 1;
					}
					else if (currentArmor == i + 1)
					{
						currentArmor = i;
					}

					i = 0;

				}
				else if (armor[i + 1] == armor[i] && armorQuality[i + 1] < armorQuality[i])
				{
					tempItem = armor[i];
					tempQuality = armorQuality[i];

					armor[i] = armor[i + 1];
					armorQuality[i] = armorQuality[i + 1];

					armor[i + 1] = tempItem;
					armorQuality[i + 1] = tempQuality;


					if (currentArmor == i)
					{
						currentArmor = i + 1;
					}
					else if (currentArmor == i + 1)
					{
						currentArmor = i;
					}

					i = 0;
				}

			} while (i < 2);
		}

		/// <summary>
		/// Player damages a creature. Returns the amount of damage the player did,
		/// or zero if the player missed.
		/// </summary>
		/// <param name="defense"></param>
		/// <returns></returns>
		public int Hit(int defense)
		{
			int wt = WeaponType(CurrentWeaponIndex);
			int qt = WeaponQuality(CurrentWeaponIndex);

			int dam = Attribute[Attributes.strength] - 12;
			dam += (int)(wt * (qt + 2)) / 2;

			dam = (int)(dam * XleCore.random.Next(30, 150) / 100.0 + 0.5);
			dam += XleCore.random.Next(-2, 3);

			if (dam < 3)
				dam = 1 + XleCore.random.Next(3);

			int hit = Attribute[Attributes.dexterity] * 8 + 15 * qt;

			System.Diagnostics.Debug.WriteLine("Hit: " + hit.ToString() + " Dam: " + dam.ToString());

			hit -= XleCore.random.Next(400);

			if (hit < 0)
				dam = 0;

			//return 100;
			return dam;
		}
		/// <summary>
		/// Called when the player gets hit. Returns the damage done to the player and
		/// subtracts that value from HP.
		/// </summary>
		/// <param name="attack"></param>
		/// <returns></returns>	
		public int Damage(int attack)
		{
			int dam = (int)(attack - (Attribute[Attributes.endurance]
				+ ArmorType(CurrentArmorIndex) * 4) * 0.8);

			dam += (int)(dam * XleCore.random.Next(-50, 100) / 100 + 0.5);

			if (dam < 0 || 1 + XleCore.random.Next(60) + attack / 15
							< Attribute[Attributes.dexterity] + ArmorQuality(CurrentArmorIndex))
			{
				dam = 0;
			}

			HP -= dam;

			return dam;
		}


		public void ReturnToPreviousMap()
		{
			SetMap(returnMap, returnX, returnY);
			faceDirection = returnFacing;
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


		public int WeaponEnchantTurnsLeft { get; set; }

	}
}