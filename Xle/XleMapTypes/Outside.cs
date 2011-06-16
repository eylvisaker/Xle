using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using AgateLib;
using AgateLib.InputLib;
using AgateLib.Geometry;
using AgateLib.Serialization.Xle;


namespace ERY.Xle.XleMapTypes
{
	public class Outside : XleMap
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
		int banditAmbush;

		#region --- Construction and Serialization ---

		public Outside()
		{ }

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
			}
		}
		public override int TerrainWaitTime(Player player)
		{
			switch (player.Terrain)
			{
				case TerrainType.Water:
				case TerrainType.Grass:
				case TerrainType.Forest:
					return 0;
				case TerrainType.Swamp:
					return 75;
				case TerrainType.Mountain:
					return 150;
				case TerrainType.Desert:
					return 150;
				case TerrainType.Mixed:
					return 50;
			}

			return base.TerrainWaitTime(player);
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

						builder.AddText(XleCore.ArmorList[item].Name, XleColor.White);
						builder.AddText(" for ", XleColor.Cyan);
						builder.AddText(cost.ToString(), XleColor.White);
						builder.AddText(" Gold?", XleColor.Cyan);
					}
					else if (type == 2)
					{
						item = XleCore.random.Next(7) + 1;
						cost = (int)(g.WeaponCost(item, qual) * XleCore.random.NextDouble() * 0.6 + 0.6);

						builder.AddText(XleCore.WeaponList[item].Name, XleColor.White);
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
					XleEventTypes.Store.OfferMuseumCoin(player);

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

						XleCore.FlashHPWhileSound(clr2);
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
		protected override void DrawImpl(int x, int y, Direction facingDirection, Rectangle inRect)
		{
			Draw2D(x, y,facingDirection, inRect);


			if (displayMonst > -1)
			{
				XleCore.DrawMonster(mDrawMonst.X, mDrawMonst.Y, displayMonst);
			}

		}
		protected override void PlayerStepImpl(Player player)
		{
			TestEncounter(player);
		}
		public void TestEncounter(Player player)
		{
			int waitAtEnd = 0;
			bool keyBreak = false;
			bool firstTime = false;
			Direction dir = player.FaceDirection;

			string dirName;

			if (player.TimeDays >= banditAmbush && banditAmbush > 0 &&
				player.Item(15) > 0)
			{
				// TODO: create image

				g.AddBottom();
				g.AddBottom("You are ambushed by bandits!", XleColor.Cyan);

				for (int i = 0; i < 8; i++)
				{
					SoundMan.PlaySound(LotaSound.EnemyHit);
					XleCore.wait(250);
				}

				g.AddBottom("You fall unconsious.", XleColor.Yellow);

				XleCore.wait(3000);

				g.AddBottom();
				g.AddBottom("You awake.  The compendium is gone.");
				g.AddBottom();

				while (player.Item(15) > 0)
					player.ItemCount(15, -1);

				SoundMan.PlaySoundSync(LotaSound.VeryBad);

				g.AddBottom("You hear a voice...");

				g.AddBottom();
				g.WriteSlow("Do not be discouraged.  It was", 0, XleColor.White);

				g.AddBottom();
				g.WriteSlow("inevitable.  Keep to your quest.", 0, XleColor.White);

				XleCore.wait(3000);
				banditAmbush = 0;
			}

			if (mMonst.Count == 0)
				return;

			if (g.disableEncounters)
				return;

			if (EncounterState == 0 && stepCount <= 0)
			{
				stepCount = XleCore.random.Next(1, 16);
				int type = XleCore.random.Next(0, 21);

				//if (cursorKeys != KeyCode.Left && cursorKeys != KeyCode.Up &&
				//    cursorKeys != KeyCode.Right && cursorKeys != KeyCode.Down)
				//    type = 99;

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
			else if (EncounterState == 0 && stepCount > 0)
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

			if (terrain == TerrainType.Mountain && player.Hold != 2)
			{
				return false;
			}

			return true;

		}

		public override void OnLoad(Player player)
		{
			// check to see if we should have bandits ambush the player.
			if (player.TimeDays > 100 && 
				player.Item(15) > 0 && // make sure the player has the compendium
				player.Item(14) == 0) // make sure the player doesn't have the guard jewels.
			{
				int min = 40 - (int)( player.TimeDays / 4);
				if (min < 3) min = 3;

				int max = 100 - (int)(player.TimeDays / 8);
				if (max < 8) max = 20;

				banditAmbush = (int)(player.TimeDays) +
					XleCore.random.Next(min, max);
			}
			
		}
	}
}
