using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using AgateLib;
using AgateLib.InputLib;
using AgateLib.Geometry;
using AgateLib.Serialization.Xle;
using ERY.Xle.XleEventTypes.Stores;
using ERY.Xle.Maps.XleMapTypes.Extenders;


namespace ERY.Xle.Maps.XleMapTypes
{
	public class Outside : XleMap
	{
		int[] mData;
		int mHeight;
		int mWidth;

		int[] waves;
		Rectangle drawRect;

		List<Monster> currentMonst = new List<Monster>();

		int stepCount;
		public int displayMonst = -1;
		Direction monstDir;
		Point mDrawMonst;
		int monstCount, initMonstCount;
		bool isMonsterFriendly;
		int mWaterAnimLevel;

		#region --- Construction and Serialization ---

		public Outside()
		{ }

		protected override void WriteData(XleSerializationInfo info)
		{
			info.Write("Width", Width);
			info.Write("Height", Height);
			info.Write("MapData", mData, NumericEncoding.Csv);
		}
		protected override void ReadData(XleSerializationInfo info)
		{
			mWidth = info.ReadInt32("Width");
			mHeight = info.ReadInt32("Height");

			mData = info.ReadInt32Array("MapData");

		}

		#endregion


		public override void InitializeMap(int width, int height)
		{
			mWidth = width;
			mHeight = height;
			mData = new int[mWidth * mHeight];
		}
		public override IEnumerable<string> AvailableTileImages
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
				int result;

				if (yy < 0 || yy >= Height || xx < 0 || xx >= Width)
					result = 0;
				else
					result = mData[xx + mWidth * yy];

				if (result == 0 && waves != null)
				{
					int index = xx - drawRect.Left + (yy - drawRect.Top) * drawRect.Width;

					if (index >= 0 && index < waves.Length)
						return waves[index];
				}

				return result;
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
			if (player.IsOnRaft)
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

		private TerrainInfo GetTerrainInfo(Player player)
		{
			var terrain = TerrainAt(player.X, player.Y);

			return GetTerrainInfo(terrain);
		}

		TerrainInfo GetTerrainInfo(TerrainType terrain)
		{
			TerrainInfo info = new TerrainInfo();

			switch (terrain)
			{
				case TerrainType.Water:
				case TerrainType.Grass:
				case TerrainType.Forest:
					info.StepTimeDays += .25;
					break;
				case TerrainType.Swamp:
					info.StepTimeDays += .5;
					break;
				case TerrainType.Mountain:
					info.StepTimeDays += 1;
					break;
				case TerrainType.Desert:
					info.StepTimeDays += 1;
					break;
				case TerrainType.Mixed:
					info.StepTimeDays += 0.5;
					break;
			}

			switch (terrain)
			{
				case TerrainType.Swamp:
					info.WalkSound = (LotaSound.WalkSwamp);
					break;

				case TerrainType.Desert:
					info.WalkSound = (LotaSound.WalkDesert);
					break;

				case TerrainType.Grass:
				case TerrainType.Forest:
				case TerrainType.Mixed:
				default:
					info.WalkSound = (LotaSound.WalkOutside);
					break;
			}
			switch (terrain)
			{
				case TerrainType.Grass:
					info.TerrainName = "grasslands";
					info.TravelText = "easy";
					info.FoodUseText = "low";
					break;
				case TerrainType.Water:
					info.TerrainName = "water";
					info.TravelText = "easy";
					info.FoodUseText = "low";
					break;
				case TerrainType.Mountain:
					info.TerrainName = "the mountains";
					info.TravelText = "slow";
					info.FoodUseText = "high";
					break;
				case TerrainType.Forest:
					info.TerrainName = "a forest";
					info.TravelText = "easy";
					info.FoodUseText = "low";
					break;
				case TerrainType.Desert:
					info.TerrainName = "a desert";
					info.TravelText = "slow";
					info.FoodUseText = "high";
					break;
				case TerrainType.Swamp:
					info.TerrainName = "a swamp";
					info.TravelText = "average";
					info.FoodUseText = "medium";
					break;
				case TerrainType.Foothills:
					info.TerrainName = "mountain foothills";
					info.TravelText = "average";
					info.FoodUseText = "medium";
					break;

				default:
				case TerrainType.Mixed:
					info.TerrainName = "mixed terrain";
					info.TravelText = "average";
					info.FoodUseText = "medium";
					break;

			}

			Extender.ModifyTerrainInfo(info, terrain);

			return info;
		}

		public override bool PlayerXamine(Player player)
		{
			TerrainInfo info = GetTerrainInfo(player);

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("You are in " + info.TerrainName + ".");

			XleCore.TextArea.Print("Travel: ", XleColor.White);
			XleCore.TextArea.Print(info.TravelText, XleColor.Green);
			XleCore.TextArea.Print("  -  Food use: ", XleColor.White);
			XleCore.TextArea.Print(info.FoodUseText, XleColor.Green);
			XleCore.TextArea.PrintLine();

			return true;
		}
		public override bool PlayerFight(Player player)
		{
			string weaponName;

			weaponName = player.CurrentWeaponTypeName;

			XleCore.TextArea.PrintLine("\n");

			if (EncounterState == EncounterState.MonsterReady)
			{
				int dam = attack(player);

				XleCore.TextArea.Print("Attack ", XleColor.White);
				XleCore.TextArea.Print(MonstName, XleColor.Cyan);
				XleCore.TextArea.PrintLine();

				XleCore.TextArea.Print("with ", XleColor.White);
				XleCore.TextArea.Print(weaponName, XleColor.Cyan);
				XleCore.TextArea.PrintLine();

				if (dam <= 0)
				{
					SoundMan.PlaySound(LotaSound.PlayerMiss);

					XleCore.TextArea.PrintLine("Your Attack missed.", XleColor.Yellow);

					return true;
				}

				SoundMan.PlaySound(LotaSound.PlayerHit);

				HitMonster(player, dam);
			}
			else if (EncounterState > 0)
			{
				XleCore.TextArea.PrintLine("The unknown creature is not ");
				XleCore.TextArea.PrintLine("within range.");

				XleCore.Wait(300 + 100 * player.Gamespeed);
			}
			else
			{
				return false;
			}

			return true;
		}

		private void HitMonster(Player player, int dam)
		{
			XleCore.TextArea.Print("Enemy hit by blow of ", XleColor.White);
			XleCore.TextArea.Print(dam.ToString(), XleColor.Cyan);
			XleCore.TextArea.Print(".", XleColor.White);
			XleCore.TextArea.PrintLine();

			XleCore.Wait(250 + 100 * player.Gamespeed, true, XleCore.Redraw);

			currentMonst[monstCount - 1].HP -= dam;

			if (KilledOne())
			{
				XleCore.Wait(250);

				SoundMan.PlaySound(LotaSound.EnemyDie);

				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("the " + MonstName + " dies.");

				int gold, food;
				bool finished = FinishedCombat(out gold, out food);

				XleCore.Wait(250 + 150 * player.Gamespeed);

				if (finished)
				{
					XleCore.TextArea.PrintLine();

					if (food > 0)
					{
						MenuItemList menu = new MenuItemList("Yes", "No");
						int choice;

						XleCore.TextArea.PrintLine("Would you like to use the");
						XleCore.TextArea.PrintLine(MonstName + "'s flesh for food?");
						XleCore.TextArea.PrintLine();

						choice = XleCore.QuickMenu(menu, 3, 0);

						if (choice == 1)
							food = 0;
						else
						{
							XleCore.TextArea.Print("You gain ", XleColor.White);
							XleCore.TextArea.Print(food.ToString(), XleColor.Green);
							XleCore.TextArea.Print(" days of food.", XleColor.White);
							XleCore.TextArea.PrintLine();

							player.Food += food;
						}

					}


					if (gold < 0)
					{
						// gain weapon or armor
					}
					else if (gold > 0)
					{
						XleCore.TextArea.Print("You find ", XleColor.White);
						XleCore.TextArea.Print(gold.ToString(), XleColor.Yellow);
						XleCore.TextArea.Print(" gold.", XleColor.White);
						XleCore.TextArea.PrintLine();

						player.Gold += gold;
					}

					XleCore.Wait(400 + 100 * player.Gamespeed);
				}
			}
		}
		public override void PlayerMagic(GameState state)
		{
			var magics = Extender.ValidMagic.Where(x => state.Player.Items[x.ItemID] > 0).ToList();

			MenuItemList menu = new MenuItemList();
			menu.Add("Nothing");
			menu.AddRange(magics.Select(x => x.Name));

			XleCore.TextArea.PrintLine(" - select above", XleColor.Cyan);
			int choice = XleCore.SubMenu("Pick Magic", 0, menu);

			if (choice == 0)
				return;

			var magic = magics[choice-1];

			XleCore.TextArea.PrintLine();

			state.Player.Items[magic.ItemID]--;

			switch (choice)
			{
				case 1:
				case 2:
					if (EncounterState == 0)
					{
						state.Player.Items[magic.ItemID]++;
						XleCore.TextArea.PrintLine("Nothing to fight.");
						return;
					}
					else if (EncounterState != XleMapTypes.EncounterState.MonsterReady)
					{
						state.Player.Items[magic.ItemID]++;
						XleCore.TextArea.PrintLine("The unknown creature is out of range.");
						return;
					}

					XleCore.TextArea.PrintLine("Attack with " + magic.Name + ".");

					var sound = (choice == 1) ? LotaSound.MagicFlame : LotaSound.FireBolt;
					
					if (Extender.RollSpellFizzle(state, magic))
					{
						PlayMagicSound(sound, LotaSound.MagicFizzle, 1);

						XleCore.TextArea.PrintLine("Attack fizzles.", XleColor.Yellow);
						return;
					}
					else 
						PlayMagicSound(sound, LotaSound.MagicHit, 1);

					int damage = Extender.RollSpellDamage(state, magic, 0);

					HitMonster(state.Player, damage);

					break;

				default:

					Extender.CastSpell(state, magic);
					break;
			}
		}

		public override void PlayerCursorMovement(Player player, Direction dir)
		{
			string command;
			Point stepDirection;

			_Move2D(player, dir, "Move", out command, out stepDirection);

			XleCore.TextArea.PrintLine(command);

			if (CanPlayerStep(player, stepDirection) == false)
			{
				TerrainType terrain = TerrainAt(player.X + stepDirection.X, player.Y + stepDirection.Y);

				if (InEncounter)
				{
					SoundMan.PlaySound(LotaSound.Bump);

					XleCore.TextArea.PrintLine();
					XleCore.TextArea.PrintLine("Attempt to disengage");
					XleCore.TextArea.PrintLine("is blocked.");

					XleCore.Wait(500);
				}
				else if (player.IsOnRaft)
				{
					SoundMan.PlaySound(LotaSound.Bump);

					XleCore.TextArea.PrintLine();
					XleCore.TextArea.PrintLine("The raft must stay in the water.", XleColor.Cyan);
				}
				else if (terrain == TerrainType.Water)
				{
					SoundMan.PlaySound(LotaSound.Bump);

					XleCore.TextArea.PrintLine();
					XleCore.TextArea.PrintLine("There is too much water for travel.", XleColor.Cyan);
				}
				else if (terrain == TerrainType.Mountain)
				{
					SoundMan.PlaySound(LotaSound.Bump);

					XleCore.TextArea.PrintLine();
					XleCore.TextArea.PrintLine("You are not equipped to");
					XleCore.TextArea.PrintLine("cross the mountains.");
				}
				else
				{
					throw new NotImplementedException();
					//SoundMan.PlaySound(LotaSound.Invalid);
				}
			}
			else
			{
				BeforeStepOn(player, player.X + stepDirection.X, player.Y + stepDirection.Y);

				MovePlayer(XleCore.GameState, stepDirection);

				if (EncounterState == XleMapTypes.EncounterState.JustDisengaged)
				{
					XleCore.TextArea.PrintLine();
					XleCore.TextArea.PrintLine("Attempt to disengage");
					XleCore.TextArea.PrintLine("is successful.");

					XleCore.Wait(500);

					EncounterState = XleMapTypes.EncounterState.NoEncounter;
				}

				TerrainInfo info = GetTerrainInfo(player);

				if (player.IsOnRaft == false)
				{
					SoundMan.PlaySound(info.WalkSound);
				}

				AfterPlayerStep(player);

				player.TimeDays += info.StepTimeDays;
				player.TimeQuality += 1;
			}
		}
		public override bool PlayerDisembark(GameState state)
		{
			XleCore.TextArea.PrintLine(" raft");

			if (state.Player.IsOnRaft == false)
				return false;

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Disembark in which direction?");

			do
			{
				XleCore.Redraw();

			} while (!(
				Keyboard.Keys[KeyCode.Left] || Keyboard.Keys[KeyCode.Right] ||
				Keyboard.Keys[KeyCode.Up] || Keyboard.Keys[KeyCode.Down]));

			int newx = state.Player.X;
			int newy = state.Player.Y;

			Direction dir = Direction.East;

			if (Keyboard.Keys[KeyCode.Left])
				dir = Direction.West;
			else if (Keyboard.Keys[KeyCode.Up])
				dir = Direction.North;
			else if (Keyboard.Keys[KeyCode.Down])
				dir = Direction.South;
			else if (Keyboard.Keys[KeyCode.Right])
				dir = Direction.East;

			PlayerDisembark(state, dir);

			return true;
		}

		private void PlayerDisembark(GameState state, Direction dir)
		{
			state.Player.BoardedRaft = null;
			PlayerCursorMovement(state.Player, dir);

			SoundMan.StopSound(LotaSound.Raft1);
		}

		protected override bool PlayerSpeakImpl(Player player)
		{
			if (EncounterState != EncounterState.MonsterReady)
			{
				return false;
			}

			SpeakToMonster(player);

			return true;
		}

		public EncounterState EncounterState { get; set; }

		private bool InEncounter
		{
			get
			{
				switch (EncounterState)
				{
					case XleMapTypes.EncounterState.MonsterAppeared:
					case XleMapTypes.EncounterState.MonsterReady:
						return true;
					default:
						return false;
				}
			}
		}

		string MonstName
		{
			get { return currentMonst[0].Name; }
		}

		int attack(Player player)
		{
			int damage = player.Hit(currentMonst[monstCount - 1].Defense);

			if (currentMonst[monstCount - 1].Weapon > 0)
			{
				if (player.WeaponType(player.CurrentWeaponIndex) == currentMonst[monstCount - 1].Weapon)
				{
					damage += XleCore.random.Next(11) + 20;
				}
				else
				{
					damage = 1 + XleCore.random.Next((damage < 10) ? damage : 10);
				}
			}
			isMonsterFriendly = false;

			return damage;
		}

		bool KilledOne()
		{
			if (currentMonst[monstCount - 1].HP <= 0)
			{
				monstCount--;

				return true;

			}

			return false;
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
					gold += currentMonst[i].Gold;
					food += currentMonst[i].Food;

				}

				gold = (int)(gold * (XleCore.random.NextDouble() + 0.5));
				food = (int)(food * (XleCore.random.NextDouble() + 0.5));

				if (XleCore.random.Next(100) < 50)
					food = 0;

				EncounterState = 0;
				displayMonst = -1;
			}

			return finished;
		}

		void SpeakToMonster(Player player)
		{
			XleCore.TextArea.PrintLine();

			if (!isMonsterFriendly)
			{
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("The " + MonstName + " does not reply.");

				XleCore.Wait(250);

				return;
			}

			const int talkTypes = 5;
			int type;
			int qual = XleCore.random.Next(5);
			int cost = 0;
			int item = 0;
			MenuItemList menu = new MenuItemList("Yes", "No");

			Color qcolor = XleColor.White;

			string[] quality = new string[5] { "a Well Crafted", "a Slightly Used", "a Sparkling New", "a Wonderful", "an Awesome" };

			do
			{
				type = XleCore.random.Next(talkTypes) + 1;
			} while (player.MaxHP == player.HP && type == 4);

			string name = "";

			switch (type)
			{
				case 1:			// buy armor
				case 2:         // buy weapon

					XleCore.TextArea.Print("Do you want to buy ", XleColor.Cyan);
					XleCore.TextArea.Print(quality[qual], XleColor.White);
					XleCore.TextArea.PrintLine();

					if (type == 1)
					{
						item = XleCore.random.Next(4) + 1;
						cost = (int)(XleCore.ArmorCost(item, qual) * (XleCore.random.NextDouble() * 0.6 + 0.6));
						name = XleCore.Data.ArmorList[item].Name;
					}
					else if (type == 2)
					{
						item = XleCore.random.Next(7) + 1;
						cost = (int)(XleCore.WeaponCost(item, qual) * (XleCore.random.NextDouble() * 0.6 + 0.6));
						name = XleCore.Data.WeaponList[item].Name;
					}

					XleCore.TextArea.Print(name, XleColor.White);
					XleCore.TextArea.Print(" for ", XleColor.Cyan);
					XleCore.TextArea.Print(cost.ToString(), XleColor.White);
					XleCore.TextArea.Print(" Gold?", XleColor.Cyan);
					XleCore.TextArea.PrintLine();

					qcolor = XleColor.Cyan;

					break;
				case 3:			// buy food

					item = XleCore.random.Next(21) + 20;
					cost = (int)(item * (XleCore.random.NextDouble() * 0.4 + 0.8));

					XleCore.TextArea.Print("Do you want to buy ", XleColor.Green);
					XleCore.TextArea.Print(item.ToString(), XleColor.Yellow);
					XleCore.TextArea.PrintLine();

					// line 2
					XleCore.TextArea.Print("Days of food for ", XleColor.Green);
					XleCore.TextArea.Print(cost.ToString(), XleColor.Yellow);
					XleCore.TextArea.Print(" gold?", XleColor.Green);
					XleCore.TextArea.PrintLine();

					qcolor = XleColor.Green;

					break;
				case 4:			// buy hp

					item = XleCore.random.Next(player.MaxHP / 4) + 20;

					if (item > (player.MaxHP - player.HP))
						item = (player.MaxHP - player.HP);

					cost = (int)(item * (XleCore.random.NextDouble() * 0.15 + 0.75));

					XleCore.TextArea.Print("Do you want to buy a potion worth ", XleColor.Green);
					XleCore.TextArea.PrintLine();

					// line 2
					XleCore.TextArea.Print(item.ToString(), XleColor.Yellow);
					XleCore.TextArea.Print(" Hit Points for ", XleColor.Green);
					XleCore.TextArea.Print(cost.ToString(), XleColor.Yellow);
					XleCore.TextArea.Print(" gold?", XleColor.Green);
					XleCore.TextArea.PrintLine();

					qcolor = XleColor.Green;

					break;
				case 5:			// buy museum coin
					Store.OfferMuseumCoin(player);

					break;

			}

			if (type != 5)
			{
				XleCore.TextArea.PrintLine();

				int choice = XleCore.QuickMenu(menu, 3, 0, qcolor);

				if (choice == 0)
				{
					if (player.Spend(cost))
					{
						SoundMan.PlaySound(LotaSound.Sale);

						XleCore.TextArea.PrintLine();
						XleCore.TextArea.PrintLine("Purchase Completed.");

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

						XleCore.TextArea.PrintLine();
						XleCore.TextArea.PrintLine("You don't have enough gold...");
					}

				}
				else
				{
					SoundMan.PlaySound(LotaSound.Medium);

					XleCore.TextArea.PrintLine();

					if (1 + XleCore.random.Next(2) == 1)
						XleCore.TextArea.PrintLine("Maybe Later...");
					else
						XleCore.TextArea.PrintLine("You passed up a good deal!");

				}
			}

			EncounterState = EncounterState.NoEncounter;
			displayMonst = -1;
		}



		/// <summary>
		/// Gets or sets whether or not the player is in stormy water
		/// </summary>
		/// <returns></returns>
		public int WaterAnimLevel
		{
			get { return mWaterAnimLevel; }
			set
			{
				System.Diagnostics.Debug.Assert(value >= 0);

				mWaterAnimLevel = value;
			}
		}

		int lastAnimate = 0;

		protected override void AnimateTiles(Rectangle rectangle)
		{
			int now = (int)Timing.TotalMilliseconds;

			if (rectangle != drawRect)
			{
				ClearWaves();

				drawRect = rectangle;
			}
			if (lastAnimate + 250 > now)
				return;

			if (waves == null || waves.Length != rectangle.Width * rectangle.Height)
			{
				waves = new int[rectangle.Width * rectangle.Height];
			}

			lastAnimate = now;

			for (int j = 0; j < rectangle.Height; j++)
			{
				for (int i = 0; i < rectangle.Width; i++)
				{
					int x = i + rectangle.Left;
					int y = j + rectangle.Top;
					int index = j * rectangle.Width + i;

					int tile = this[x, y];

					if (tile == 0)
					{
						if (XleCore.random.Next(0, 1000) < 20 * (WaterAnimLevel + 1))
						{
							waves[index] = XleCore.random.Next(1, 3);
						}
					}
					else if (tile == 1 || tile == 2)
					{
						if (XleCore.random.Next(0, 100) < 25)
						{
							waves[index] = 0;
						}
					}
				}
			}
		}

		public void ClearWaves()
		{
			if (waves != null)
				Array.Clear(waves, 0, waves.Length);

			int now = (int)Timing.TotalMilliseconds;

			// force an update.
			lastAnimate = now - 500;
		}

		protected override void DrawImpl(int x, int y, Direction facingDirection, Rectangle inRect)
		{
			Draw2D(x, y, facingDirection, inRect);

			if (displayMonst > -1)
			{
				Point pt = new Point(mDrawMonst.X - x, mDrawMonst.Y - y);
				pt.X *= 16;
				pt.Y *= 16;

				pt.X += XleCore.Renderer.CharRect.X;
				pt.Y += XleCore.Renderer.CharRect.Y;

				XleCore.Renderer.DrawMonster(pt.X, pt.Y, displayMonst);
			}
		}
		protected override void AfterStepImpl(Player player, bool didEvent)
		{
			Extender.AfterPlayerStep(XleCore.GameState);

			UpdateRaftState(player);

			StepEncounter(player, didEvent);
		}

		private void UpdateRaftState(Player player)
		{
			if (player.IsOnRaft == false)
			{
				foreach (var raft in player.Rafts)
				{
					if (raft.MapNumber != MapID)
						continue;

					if (raft.Location.Equals(player.Location))
					{
						player.BoardedRaft = raft;
					}
				}
			}

			if (player.IsOnRaft)
			{
				var raft = player.BoardedRaft;

				raft.X = player.X;
				raft.Y = player.Y;
			}
		}
		private void StepEncounter(Player player, bool didEvent)
		{
			if (XleCore.Data.Database.MonsterList.Count == 0) return;
			if (XleCore.Options.DisableOutsideEncounters) return;

			// bail out if the player entered another map on this step.
			if (XleCore.GameState.Map != this)
				return;

			bool handled = false;
			Extender.UpdateEncounterState(XleCore.GameState, ref handled);

			if (handled)
				return;

			if (EncounterState == EncounterState.NoEncounter && stepCount <= 0)
			{
				SetNextEncounterStepCount();

				StartEncounter(player);
			}
			else if (EncounterState == EncounterState.NoEncounter && stepCount > 0)
			{
				currentMonst.Clear();
				stepCount--;
			}
			else if (EncounterState == EncounterState.UnknownCreatureApproaching)
			{
				currentMonst.Clear();
				EncounterState = EncounterState.CreatureAppearing;
			}

			if (EncounterState == EncounterState.CreatureAppearing)
			{
				MonsterAppearing(player);
			}

		}


		protected override void AfterExecuteCommandImpl(Player player, KeyCode cmd)
		{

			if (EncounterState == EncounterState.MonsterAvoided)
			{
				AvoidMonster(player);
			}
			else if (EncounterState == EncounterState.MonsterAppeared)
			{
				MonsterAppeared();
			}
			else if (EncounterState == EncounterState.MonsterReady)
			{
				MonsterTurn(player, false);
			}
		}

		private void StartEncounter(Player player)
		{
			currentMonst.Clear();
			isMonsterFriendly = false;

			int type = XleCore.random.Next(0, 15);

			if (type < 10)
			{
				string dirName = MonsterDirection(player);

				EncounterState = EncounterState.UnknownCreatureApproaching;
				SoundMan.PlaySound(LotaSound.Encounter);

				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("An unknown creature is approaching ", XleColor.Cyan);
				XleCore.TextArea.PrintLine("from the " + dirName + ".", XleColor.Cyan);

				XleCore.Wait(1000);
			}
			else if (type < 15)
			{
				EncounterState = EncounterState.CreatureAppearing;

				//XleCore.wait(1000);
			}
			else
			{
				// TODO: figure out what I want to do with this, or eliminate this condition.
			}

		}

		private void SetNextEncounterStepCount()
		{
			stepCount = XleCore.random.Next(1, 40);
		}

		private string MonsterAppearing(Player player)
		{
			if (XleCore.random.Next(100) < 55)
				EncounterState = EncounterState.MonsterAppeared;
			else
				EncounterState = EncounterState.MonsterReady;

			SoundMan.PlaySound(LotaSound.Encounter);

			displayMonst = SelectRandomMonster(TerrainAt(player.X, player.Y));

			mDrawMonst.X = player.X - 1;
			mDrawMonst.Y = player.Y - 1;

			string dirName;

			if (monstDir == Direction.None)
				dirName = MonsterDirection(player);
			else
			{
				switch (monstDir)
				{
					case Direction.East: dirName = "East"; mDrawMonst.X += 2; break;
					case Direction.North: dirName = "North"; mDrawMonst.Y -= 2; break;
					case Direction.West: dirName = "West"; mDrawMonst.X -= 2; break;
					case Direction.South: dirName = "South"; mDrawMonst.Y += 2; break;
					default:
						throw new Exception("Invalid Direction");
				}
			}

			int max = 1;
			initMonstCount = monstCount = 1 + XleCore.random.Next(max);

			for (int i = 0; i < monstCount; i++)
			{
				var m = new Monster(XleCore.Data.Database.MonsterList[displayMonst]);

				m.HP = (int)(m.HP * (XleCore.random.NextDouble() * 0.4 + 0.8));

				currentMonst.Add(m);
			}

			if (XleCore.random.Next(256) <= currentMonst[0].Friendly)
				isMonsterFriendly = true;
			else
				isMonsterFriendly = false;

			XleCore.Wait(500);
			return dirName;
		}

		private void AvoidMonster(Player player)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("You avoid the unknown creature.");

			EncounterState = EncounterState.NoEncounter;

			XleCore.Wait(250);
		}

		private void MonsterAppeared()
		{
			XleCore.Wait(500);

			Color[] colors = new Color[40];
			string plural = (monstCount > 1) ? "s" : "";

			for (int i = 0; i < 40; i++)
				colors[i] = XleColor.Cyan;

			colors[0] = XleColor.White;

			EncounterState = EncounterState.MonsterReady;

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine(monstCount.ToString() + " " + currentMonst[0].Name + plural, colors);

			colors[0] = XleColor.Cyan;
			XleCore.TextArea.PrintLine("is approaching.", colors);

			XleCore.Wait(1000);
		}

		private void MonsterTurn(Player player, bool firstTime)
		{
			XleCore.Wait(500);

			if (isMonsterFriendly)
			{
				Color[] colors = new Color[40];

				for (int i = 0; i < 40; i++)
					colors[i] = XleColor.Cyan;
				colors[0] = XleColor.White;

				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine(monstCount.ToString() + " " + currentMonst[0].Name, colors);
				XleCore.TextArea.PrintLine("Stands before you.");

				XleCore.Wait(1500);
			}
			else
			{
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.Print("Attacked by ", XleColor.White);
				XleCore.TextArea.Print(monstCount.ToString(), XleColor.Yellow);
				XleCore.TextArea.Print(" " + currentMonst[0].Name, XleColor.Cyan);
				XleCore.TextArea.PrintLine();

				int dam = 0;
				int hits = 0;

				for (int i = 0; i < monstCount; i++)
				{
					int t = player.Damage(currentMonst[i].Attack);

					if (t > 0)
					{
						dam += t;
						hits++;
					}
				}

				XleCore.TextArea.Print("Hits:  ", XleColor.White);
				XleCore.TextArea.Print(hits.ToString(), XleColor.Yellow);
				XleCore.TextArea.Print("   Damage:  ", XleColor.White);
				XleCore.TextArea.Print(dam.ToString(), XleColor.Yellow);
				XleCore.TextArea.PrintLine();

				if (dam > 0)
				{
					SoundMan.PlaySound(LotaSound.EnemyHit);
				}
				else
				{
					SoundMan.PlaySound(LotaSound.EnemyMiss);
				}
			}

			XleCore.Wait(250, !firstTime, XleCore.Redraw);
		}

		private static int SelectRandomMonster(TerrainType terrain)
		{
			int mCount = 0;
			int val, sel = -1;

			for (int i = 0; i < XleCore.Data.Database.MonsterList.Count; i++)
			{
				if ((TerrainType)XleCore.Data.Database.MonsterList[i].Terrain == TerrainType.All && terrain != 0)
					mCount++;

				if ((TerrainType)XleCore.Data.Database.MonsterList[i].Terrain == terrain)
					mCount += 3;

				if (terrain == TerrainType.Foothills &&
					(TerrainType)XleCore.Data.Database.MonsterList[i].Terrain == TerrainType.Mountain)
					mCount += 3;
			}

			val = 1 + XleCore.random.Next(mCount);

			for (int i = 0; i < XleCore.Data.Database.MonsterList.Count; i++)
			{
				if ((TerrainType)XleCore.Data.Database.MonsterList[i].Terrain == TerrainType.All && terrain != 0)
					val--;

				if ((TerrainType)XleCore.Data.Database.MonsterList[i].Terrain == terrain)
					val -= 3;

				if (terrain == TerrainType.Foothills &&
					(TerrainType)XleCore.Data.Database.MonsterList[i].Terrain == TerrainType.Mountain)
					val -= 3;

				if (val == 0 || val == -1 || val == -2)
				{
					sel = i;
					break;
				}
			}

			System.Diagnostics.Debug.Assert(sel > -1);
			return sel;
		}

		[Obsolete("This function is weird and should be replaced with something else.")]
		public string MonsterDirection(Player player)
		{
			string dirName;
			mDrawMonst.X = player.X - 1;
			mDrawMonst.Y = player.Y - 1;

			monstDir = (Direction)XleCore.random.Next((int)Direction.East, (int)Direction.South + 1);

			switch (monstDir)
			{
				case Direction.East: dirName = "East"; mDrawMonst.X += 2; break;
				case Direction.North: dirName = "North"; mDrawMonst.Y -= 2; break;
				case Direction.West: dirName = "West"; mDrawMonst.X -= 2; break;
				case Direction.South: dirName = "South"; mDrawMonst.Y += 2; break;
				default:
					throw new Exception("Invalid direction.");
			}
			return dirName;
		}

		protected override Extenders.IMapExtender CreateExtenderImpl()
		{
			if (XleCore.Factory == null)
			{
				Extender = new NullOutsideExtender();
			}
			else
			{
				Extender = XleCore.Factory.CreateMapExtender(this);
			}

			return Extender;
		}

		public override bool CanPlayerStepIntoImpl(Player player, int xx, int yy)
		{
			if (EncounterState == EncounterState.UnknownCreatureApproaching)
			{
				bool moveTowards = false;

				int dx = xx - player.X;
				int dy = yy - player.Y;

				switch (monstDir)
				{
					case Direction.East: if (dx > 0) moveTowards = true; break;
					case Direction.North: if (dy < 0) moveTowards = true; break;
					case Direction.West: if (dx < 0) moveTowards = true; break;
					case Direction.South: if (dy > 0) moveTowards = true; break;
				}

				if (moveTowards == false)
				{
					if (XleCore.random.Next(100) < 50)
					{
						EncounterState = EncounterState.MonsterAvoided;
					}
				}
			}
			else if (EncounterState == EncounterState.MonsterReady)
			{
				if (XleCore.random.Next(100) < 50 && isMonsterFriendly == false)
				{
					return false;
				}
				else
				{
					EncounterState = EncounterState.JustDisengaged;
					displayMonst = -1;
				}
			}

			TerrainType terrain = TerrainAt(xx, yy);
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
				for (int i = 0; i < player.Rafts.Count; i++)
				{
					if (Math.Abs(player.Rafts[i].X - xx) < 2 && Math.Abs(player.Rafts[i].Y - yy) < 2)
					{
						return true;
					}
				}

				return false;
			}

			if (terrain == TerrainType.Mountain && player.Hold != XleCore.Factory.ClimbingGearItemID)
			{
				return false;
			}

			return true;

		}

		public override void OnLoad(Player player)
		{
			SetNextEncounterStepCount();

			base.OnLoad(player);
		}

		public override void LeaveMap(Player player)
		{
			throw new InvalidOperationException();
		}
		public override int WaitTimeAfterStep
		{
			get
			{
				return XleCore.GameState.GameSpeed.OutsideStepTime;
			}
		}

		public IOutsideExtender Extender { get; private set; }
	}
}