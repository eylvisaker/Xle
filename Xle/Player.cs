using System;
using System.Collections.Generic;
using System.Text;

using AgateLib;
using AgateLib.Geometry;
using AgateLib.Serialization.Xle;

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
			for (int i = 0; i < 5; i++)
			{
				atr[i] = 15;
			}
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
	}

	public class Player : IXleSerializable
	{
		AttributeContainer mAttributes = new AttributeContainer();
		int food;
		int gold;
		int goldBank;
		double timedays;

		int onRaft;

		int gamespeed;
		int map;
		int lastMap;
		int dungeon;
		int hp;
		int level;
		int outx, outy, outmap;
		int x, y;
		int dungeonLevel;
		Direction faceDirection = Direction.East;

		int currentArmor;
		int currentWeapon;
		int[] weapon = new int[6];
		int[] armor = new int[4];
		int[] weaponQuality = new int[6];
		int[] armorQuality = new int[4];
		int[] item = new int[30];
		int hold;

		int lastAttacked;
		int vaultGold;

		int[] chests = new int[50];

		List<RaftData> rafts = new List<RaftData>();

		public int loan;					// loan amount
		public int dueDate;				// time in days that the money is due

		public int guardian;
		public int ambushed;
		public int wizardOfPotions;
		public int casandra;

		public int[] museum = new int[16];
		public int caretaker;

		public int mailTown;


		string name;

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
			int i;

			name = newName;

			goldBank = 0;
			gamespeed = 3;
			loan = 0;
			dueDate = 0;

			dungeon = 0;
			food = 40;
			gold = 20;

			hp = 200;
			level = 1;

			map = 0;
			x = 3;
			y = 1;

			//  temporary, until the museum gets implemented.
			food = 100;
			gold = 200;
			/////////////////////////

			dungeonLevel = 0;
			faceDirection = Direction.West;
			mailTown = 0;

			currentArmor = 1;
			currentWeapon = 0;

			for (i = 1; i <= 5; i++)
			{
				weapon[i] = 0;
				weaponQuality[i] = 0;
			}

			armor[1] = 1;
			armorQuality[1] = 0;

			for (i = 2; i <= 3; i++)
			{
				armor[i] = 0;
				armorQuality[i] = 0;
			}

			for (i = 0; i < 30; i++)
			{
				item[i] = 0;
			}

			item[1] = 1;			// gold armband
			item[15] = 1;			// compendium
			item[17] = 2;			// jade coins

			for (i = 0; i < museum.Length; i++)
			{
				museum[i] = 0;
			}

			caretaker = 0;
			lastAttacked = 0;
			vaultGold = 17;

			//chests[50] = 0;

			guardian = 0;
			ambushed = 0;
			wizardOfPotions = 0;
			casandra = 0;

			ClearRafts();

			onRaft = 0;

			SortEquipment();
		}


		void IXleSerializable.WriteData(XleSerializationInfo info)
		{
			info.Write("Attributes", mAttributes);
			info.Write("Food", food);
			info.Write("Gold", gold);
			info.Write("GoldInBank", goldBank);
			info.Write("TimeDays", timedays);

			info.Write("OnRaft", onRaft);

			info.Write("GameSpeed", gamespeed);
			info.Write("Map", map);
			info.Write("LastMap", lastMap);
			info.Write("Dungeon", dungeon);
			info.Write("HP", hp);
			info.Write("Level", level);
			info.Write("OutX", outx);
			info.Write("OutY", outy);
			info.Write("OutMap", outmap);
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
			info.Write("Item", item);
			info.Write("Hold", hold);

			info.Write("LastAttacked", lastAttacked);
			info.Write("VaultGold", vaultGold);

			info.Write("Chests", chests);

			info.Write("Rafts", rafts);


			info.Write("Loan", loan);					// loan amount
			info.Write("DueDate", dueDate);				// time in days that the money is due

			info.Write("Guardian", guardian);
			info.Write("Ambushed", ambushed);
			info.Write("Wizard", wizardOfPotions);
			info.Write("Casandra", casandra);

			info.Write("Museum", museum);
			info.Write("Caretaker", caretaker);

			info.Write("MailTown", mailTown);

			info.Write("Name", name);
		}

		void IXleSerializable.ReadData(XleSerializationInfo info)
		{
			mAttributes = (AttributeContainer)info.ReadObject("Attributes");
			food = info.ReadInt32("Food");
			gold = info.ReadInt32("Gold");
			goldBank = info.ReadInt32("GoldInBank");
			timedays = info.ReadDouble("TimeDays");

			onRaft = info.ReadInt32("OnRaft");

			gamespeed = info.ReadInt32("GameSpeed");
			map = info.ReadInt32("Map");
			lastMap = info.ReadInt32("LastMap");
			dungeon = info.ReadInt32("Dungeon");
			hp = info.ReadInt32("HP");
			level = info.ReadInt32("Level");
			outx = info.ReadInt32("OutX");
			outy = info.ReadInt32("OutY");
			outmap = info.ReadInt32("OutMap");
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
			item = info.ReadInt32Array("Item");
			hold = info.ReadInt32("Hold");

			lastAttacked = info.ReadInt32("LastAttacked");
			vaultGold = info.ReadInt32("VaultGold");

			chests = info.ReadInt32Array("Chests");

			rafts.AddRange(info.ReadArray<RaftData>("Rafts"));


			loan = info.ReadInt32("Loan");					// loan amount
			dueDate = info.ReadInt32("DueDate");				// time in days that the money is due

			guardian = info.ReadInt32("Guardian");
			ambushed = info.ReadInt32("Ambushed");
			wizardOfPotions = info.ReadInt32("Wizard");
			casandra = info.ReadInt32("Casandra");

			museum = info.ReadInt32Array("Museum");
			caretaker = info.ReadInt32("Caretaker");

			mailTown = info.ReadInt32("MailTown");

			name = info.ReadString("Name");

			if (museum.Length < 16)
				museum = new int[16];
		}

		#endregion

		/// <summary>
		/// The player has died
		/// </summary>
		public void Dead()
		{
			hp = 0;

			g.AddBottom("");
			g.AddBottom("");
			g.AddBottom("            You died!");
			g.AddBottom("");
			g.AddBottom("");

			SoundMan.PlaySound(LotaSound.VeryBad);

			while (SoundMan.IsPlaying(LotaSound.VeryBad) && !g.Done)
			{
				g.HPColor = XleColor.Red;
				XleCore.wait(50);

				g.HPColor = XleColor.Yellow;
				XleCore.wait(50);
			}

			g.HPColor = XleColor.White;

			SetMap(1, 80, 80);

			TerrainType t;

			do
			{
				x = XleCore.random.Next(XleCore.Map.Width);
				y = XleCore.random.Next(XleCore.Map.Height);

				t = Terrain;

			} while (t != TerrainType.Grass && t != TerrainType.Forest);

			Rafts.Clear();

			hp = MaxHP;
			food = 30 + XleCore.random.Next(10);
			gold = 25 + XleCore.random.Next(30);
			onRaft = 0;

			while (SoundMan.IsPlaying(LotaSound.VeryBad))
				XleCore.wait(40);

			g.AddBottom("The powers of the museum");
			g.AddBottom("resurrect you from the grave!");
			g.AddBottom("");

			SoundMan.PlaySound(LotaSound.VeryGood);

			while (SoundMan.IsPlaying(LotaSound.VeryGood))
				XleCore.wait(50);


		}
		public string Name
		{
			get
			{
				return name;
			}
		}

		public void CheckDead()
		{
			if (hp == 0 || food < -2)
				Dead();
		}

		public void SavePlayer()
		{
			SavePlayer(@"Saved\" + Name + ".chr");
		}
		private void SavePlayer(string filename)
		{
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
		/// gets or sets the character leve.
		/// </summary>
		public int Level
		{
			get { return level; }
			set
			{
				System.Diagnostics.Debug.Assert(value > level);
				level = value;
			}
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
				System.Diagnostics.Debug.Assert(value > 0 && value < 6);

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
			get { return 200 * level; }
		}
		/// <summary>
		/// 	// returns or adjusts food
		/// </summary>
		/// <param name="change"></param>
		/// <returns></returns>
		public int Food
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
		/// 	// gains gold
		/// </summary>
		/// <param name="amount"></param>
		public void GainGold(int amount)
		{
			gold += amount;
		}

		//		Time
		public void OneDay()
		{
			// TODO: get or write OneDay function.
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

				if ((int)value == (int)(timedays + 1))
				{
					food--;

				}

				timedays = value;
			}
		}

		/// <summary>
		/// 				// the current outside terrain the player is standing on
		/// </summary>
		/// <returns></returns>
		public TerrainType Terrain
		{
			get
			{
				return XleCore.Map.Terrain(x, y);
			}
		}
		/// <summary>
		/// 					// returns x position
		/// </summary>
		/// <returns></returns>
		public int X
		{
			get { return x; }
		}
		/// <summary>
		/// 					// returns y position
		/// </summary>
		/// <returns></returns>
		public int Y
		{
			get { return y; }
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
			if (XleCore.Map.CanPlayerStepInto(this, xx, yy))
			{
				x = xx;
				y = yy;

				for (int i = 1; i < rafts.Count; i++)
				{
					if (Rafts[i].MapNumber != Map)
						continue;

					if (Rafts[i].Location.Equals(new Point(xx, yy)))
					{
						BoardRaft(i);
					}
				}

				if (OnRaft > 0)
				{
					rafts[onRaft].Location = new Point(x, y);
				}

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
				if (XleCore.Map.CheckMovement(this, dx, dy))
				{
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
			outx = x;
			outy = y;

			// set new coordinate
			x = xx;
			y = yy;

			if (map == lastAttacked)
			{
				(XleCore.Map as XleMapTypes.Town).IsAngry = true;

				g.ClearBottom();
				g.AddBottom("We remember you - slime!");
				g.AddBottom("");
				g.AddBottom("");
				g.AddBottom("");

				XleCore.wait(2000);

			}

		}
		/// <summary>
		/// 	// sets or returns the current dungeon level for the player
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public int DungeonLevel
		{
			get { return dungeonLevel; }
			set
			{
				if (value >= 0 && value <= 8)
				{
					dungeonLevel = value;
				}

			}
		}
		/// <summary>
		/// 	// sets or returns the direction the player is facing
		/// </summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		public Direction FaceDirection
		{
			get { return faceDirection; }
			set
			{
				System.Diagnostics.Debug.Assert(value >= Direction.East && value <= Direction.South);

				faceDirection = value;

			}

		}
		/// <summary>
		///  // stores the town number that was last attacked
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
			set { rafts = value; }
		}

		/// <summary>
		/// 		// sets or returns the current map
		/// </summary>
		/// <param name="m"></param>
		/// <returns></returns>
		public int Map
		{
			get
			{
				return map;
			}
		}

		public void SetMap(int newMap, int newX, int newY)
		{
			if (XleCore.Map == null)
			{
				map = newMap;
				return;
			}

			if (XleCore.Map.GetType().Equals(typeof(XleMapTypes.Outside)))
			{
				outmap = map;
				outx = x;
				outy = y;
			}

			lastMap = map;

			map = newMap;


			if (MapChangedStorage != null)
			{
				try
				{
					MapChangedStorage(this, EventArgs.Empty);

					x = newX;
					y = newY;

					XleCore.Map.OnLoad(this);

					g.ClearBottom();
				}
				catch (Exception e)
				{
					map = lastMap;
					throw e;
				}
			}
		}

		[NonSerialized]
		private EventHandler MapChangedStorage;

		public event EventHandler MapChanged
		{
			add
			{
				MapChangedStorage += value;
			}
			remove
			{
				MapChangedStorage -= value;
			}
		}

		/// <summary>
		/// 				// returns the last map the player was on
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
				return onRaft > 0;
			}
		}

		/// <summary>
		/// // returns the raft that the player is on (or 0 if not)
		/// </summary>
		public int OnRaft
		{
			get
			{
				return onRaft;
			}
		}
		/// <summary>
		/// // boards a raft
		/// </summary>
		/// <param name="?"></param>
		/// <returns></returns>    			
		public void BoardRaft(int raftNum)
		{
			if (!(raftNum < 1 || (Rafts[raftNum].X == -10 && Rafts[raftNum].Y == -10)))
			{
				onRaft = raftNum;
			}

		}
		/// <summary>
		/// 		// disembarks a raft
		/// </summary>
		/// <param name="dir"></param>
		public void Disembark(Direction dir)
		{
			onRaft = 0;

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
		/// 			// returns the coordinates of a given raft
		/// </summary>
		/// <param name="r"></param>
		/// <returns></returns>
		public RaftData Raft(int r)
		{
			return Rafts[r];
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
		/// 			// Clears all rafts from all maps
		/// </summary>
		public void ClearRafts()
		{
			Rafts.Clear();
			rafts.Add(new RaftData(0, 0, 0)); // add a dummy raft, because the list is zero based.
		}

		//		Items
		/// sets or returns the item that is currently being held
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
		/// 		// sets the item that is currently being held (menu selection)
		/// </summary>
		/// <param name="h"></param>
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
		/// 			// returns the armor that the player is carrying in the specified slot
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public int ArmorType(int index)
		{
			if (index <= 0 || index > 3)
				return 0;

			return armor[index];

		}
		public int WeaponType(int index)
		{
			if (index <= 0 || index > 5)
				return 0;

			return weapon[index];

		}		// returns the weapon that the player is carrying in the specified slot
		public int ArmorQuality(int index)
		{
			if (index <= 0 || index > 3)
				return 0;

			return armorQuality[index];

		}		// returns the armor quality
		public int WeaponQuality(int index)
		{
			if (index <= 0 || index > 5)
				return 0;

			return weaponQuality[index];

		}		// returns the weapon quality





		/// sets or returns the armor currently worn
		public int CurrentArmor
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
		/// sets or returns the armor currently equiped
		public int CurrentWeapon
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

		public string CurrentArmorName
		{
			get { return "Nothing"; }
		}

		public string CurrentWeaponName
		{
			get
			{
				return "Bare Hands";
			}
		}

		/// <summary>
		/// 				// returns the number of the specified items
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public int Item(int index)
		{

			return item[index];

		}
		/// <summary>
		/// 	// Adjusts the number of items the player has
		/// </summary>
		/// <param name="itm"></param>
		/// <param name="inc"></param>
		/// <returns></returns>
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
		/// 	// Adds a weapon to inventory
		/// </summary>
		/// <param name="w"></param>
		/// <param name="q"></param>
		/// <returns></returns>
		public bool AddWeapon(int w, int q)
		{
			int i;

			for (i = 1; i <= 5; i++)
			{
				if (WeaponType(i) == 0)
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

		public bool RemoveWeapon(int w)
		{
			return false;
		}

		public bool RemoveArmor(int a)
		{
			return false;
		}
		/// <summary>
		/// 			// sorts weapons and armor
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

		//		Combat
		/// <summary>
		/// 		// player damages a creature returns damage
		/// </summary>
		/// <param name="defense"></param>
		/// <returns></returns>
		public int Hit(int defense)
		{
			int wt = WeaponType(CurrentWeapon);
			int qt = WeaponQuality(CurrentWeapon);
			int hit;

			int dam = Attribute[Attributes.strength] - 12;
			dam += (int)(wt * (qt + 2)) / 2;

			dam = (int)(dam * XleCore.random.Next(30, 150) / 100.0 + 0.5);
			dam += XleCore.random.Next(-2, 3);

			if (dam < 3)
				dam = 1 + XleCore.random.Next(3);

			hit = Attribute[Attributes.dexterity] - (int)(defense * 0.3);
			hit += qt;

			if (hit > 24)
				hit = 24;
			else if (hit < 4)
				hit = 4;

			hit -= XleCore.random.Next(1 + 25);

			if (XleCore.random.Next(100) < 4)
				hit -= 10000;

			if (hit < 0 || XleCore.random.Next(100) < 2)
			{
				dam = 0;
			}

			System.Diagnostics.Debug.WriteLine("Hit: " + hit.ToString() + " Dam: " + dam.ToString());

			return 100;
			return dam;
		}
		/// <summary>
		/// 		// player gets hit
		/// </summary>
		/// <param name="attack"></param>
		/// <returns></returns>	
		public int Damage(int attack)
		{
			int dam = (int)(attack - (Attribute[Attributes.endurance]
				+ ArmorType(CurrentArmor) * 4) * 0.8);

			dam += (int)(dam * XleCore.random.Next(-50, 100) / 100 + 0.5);

			if (dam < 0 || 1 + XleCore.random.Next(60) + attack / 15
							< Attribute[Attributes.dexterity] + ArmorQuality(CurrentArmor))
			{
				dam = 0;
			}

			HP -= dam;

			return dam;

		}


		public void ReturnToOutside()
		{
			SetMap(outmap, outx, outy);
		}

		public void SetOutsideLocation(int outmap, int outx, int outy)
		{
			this.outmap = outmap;
			this.outx = outx;
			this.outy = outy;
		}
	}


}