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

		int[] waves;
		Rectangle drawRect;

		List<Monster> currentMonst = new List<Monster>();

		int stepCount;
		int displayMonst = -1;
		Direction monstDir;
		Point mDrawMonst;
		int monstCount, initMonstCount;
		bool isMonsterFriendly;
		int banditAmbush;

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

		[NonSerialized]
		int stormy;

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
			ColorStringBuilder builder;

			weaponName = player.CurrentWeaponTypeName;

			g.AddBottom("");

			if (EncounterState == EncounterState.MonsterReady)
			{
				int dam = attack(player);

				builder = new ColorStringBuilder();
				builder.AddText("Attack ", XleColor.White);
				builder.AddText(MonstName, XleColor.Cyan);

				g.AddBottom(builder);
				builder.Clear();

				builder.AddText("with ", XleColor.White);
				builder.AddText(weaponName, XleColor.Cyan);
				g.AddBottom(builder);

				builder.Clear();

				if (dam > 0)
				{
					SoundMan.PlaySound(LotaSound.PlayerHit);

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

				XleCore.Wait(250 + 100 * player.Gamespeed, true, XleCore.Redraw);

				if (KilledOne())
				{
					XleCore.Wait(250);

					SoundMan.PlaySound(LotaSound.EnemyDie);

					g.AddBottom("");
					g.AddBottom("the " + MonstName + " dies.");

					int gold, food;
					bool finished = FinishedCombat(out gold, out food);

					XleCore.Wait(250 + 150 * player.Gamespeed);

					if (finished)
					{
						g.AddBottom("");

						if (food > 0)
						{
							MenuItemList menu = new MenuItemList("Yes", "No");
							int choice;

							g.AddBottom("Would you like to use the");
							g.AddBottom(MonstName + "'s flesh for food?");
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
								g.AddBottom(builder);

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

						XleCore.Wait(400 + 100 * player.Gamespeed);
					}
				}
			}
			else if (EncounterState > 0)
			{
				g.AddBottom("The unknown creature is not ");
				g.AddBottom("within range.");

				XleCore.Wait(300 + 100 * player.Gamespeed);
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

					XleCore.Wait(500);
				}
				else if (player.IsOnRaft)
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
				if (EncounterState == XleMapTypes.EncounterState.JustDisengaged)
				{
					g.AddBottom("");
					g.AddBottom("Attempt to disengage");
					g.AddBottom("is successful.");

					XleCore.Wait(500);

					EncounterState = XleMapTypes.EncounterState.NoEncounter;
				}

				if (player.IsOnRaft == false)
				{
					switch (player.Terrain)
					{
						case TerrainType.Swamp:
							SoundMan.PlaySound(LotaSound.WalkSwamp);
							break;

						case TerrainType.Desert:
							SoundMan.PlaySound(LotaSound.WalkDesert);
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

				player.TimeQuality += 1;
			}
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

		private EncounterState EncounterState { get; set; }

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
				if (player.WeaponType(player.CurrentWeapon) == currentMonst[monstCount - 1].Weapon)
				{
					damage += XleCore.random.Next(11) + 20;
				}
				else
				{
					damage = 1 + XleCore.random.Next((damage < 10) ? damage : 10);
				}
			}

			currentMonst[monstCount - 1].HP -= damage;
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
			if (!isMonsterFriendly)
			{
				g.AddBottom("");
				g.AddBottom("The " + MonstName + " does not reply.");

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
			ColorStringBuilder builder;

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

					builder = new ColorStringBuilder();
					builder.AddText("Do you want to buy ", XleColor.Cyan);
					builder.AddText(quality[qual], XleColor.White);

					g.AddBottom(builder);
					builder.Clear();

					if (type == 1)
					{
						item = XleCore.random.Next(4) + 1;
						cost = (int)(XleCore.ArmorCost(item, qual) * (XleCore.random.NextDouble() * 0.6 + 0.6));
						name = XleCore.ArmorList[item].Name;
					}
					else if (type == 2)
					{
						item = XleCore.random.Next(7) + 1;
						cost = (int)(XleCore.WeaponCost(item, qual) * (XleCore.random.NextDouble() * 0.6 + 0.6));
						name = XleCore.WeaponList[item].Name;
					}

					builder.AddText(name, XleColor.White);
					builder.AddText(" for ", XleColor.Cyan);
					builder.AddText(cost.ToString(), XleColor.White);
					builder.AddText(" Gold?", XleColor.Cyan);

					g.AddBottom(builder);
					qcolor = XleColor.Cyan;

					break;
				case 3:			// buy food

					item = XleCore.random.Next(21) + 20;
					cost = (int)(item * (XleCore.random.NextDouble() * 0.4 + 0.8));

					builder = new ColorStringBuilder();
					builder.AddText("Do you want to buy ", XleColor.Green);
					builder.AddText(item.ToString(), XleColor.Yellow);

					g.AddBottom(builder);
					builder.Clear();

					// line 2
					builder.AddText("Days of food for ", XleColor.Green);
					builder.AddText(cost.ToString(), XleColor.Yellow);
					builder.AddText(" gold?", XleColor.Green);
					g.AddBottom(builder);

					qcolor = XleColor.Green;

					break;
				case 4:			// buy hp

					item = XleCore.random.Next(player.MaxHP / 4) + 20;

					if (item > (player.MaxHP - player.HP))
						item = (player.MaxHP - player.HP);

					cost = (int)(item * (XleCore.random.NextDouble() * 0.15 + 0.75));

					builder = new ColorStringBuilder();
					builder.AddText("Do you want to buy a potion worth ", XleColor.Green);

					g.AddBottom(builder);
					builder.Clear();

					// line 2
					builder.AddText(item.ToString(), XleColor.Yellow);
					builder.AddText(" Hit Points for ", XleColor.Green);
					builder.AddText(cost.ToString(), XleColor.Yellow);
					builder.AddText(" gold?", XleColor.Green);
					g.AddBottom(builder);

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

			EncounterState = EncounterState.NoEncounter;
			displayMonst = -1;
		}



		/// <summary>
		/// Gets or sets whether or not the player is in stormy water
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

				XleCore.Wait(1000);

				if (Stormy == 3)
				{
					player.Dead();
					return true;
				}

			}
			return false;
		}


		int lastAnimate = 0;

		protected override void AnimateTiles(Rectangle rectangle)
		{
			int now = (int)Timing.TotalMilliseconds;

			if (rectangle != drawRect)
			{
				if (waves != null)
					Array.Clear(waves, 0, waves.Length);

				// force an update.
				lastAnimate = now - 500;

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
						if (XleCore.random.Next(0, 1000) < 20 * (Stormy + 1))
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
		protected override void DrawImpl(int x, int y, Direction facingDirection, Rectangle inRect)
		{
			Draw2D(x, y, facingDirection, inRect);

			if (displayMonst > -1)
			{
				Point pt = new Point(mDrawMonst.X - x, mDrawMonst.Y - y);
				pt.X *= 16;
				pt.Y *= 16;

				pt.X += XleCore.CharRect.X;
				pt.Y += XleCore.CharRect.Y;

				XleCore.DrawMonster(pt.X, pt.Y, displayMonst);
			}
		}
		protected override void PlayerStepImpl(Player player, bool didEvent)
		{
			StepEncounter(player, didEvent);
		}
		private void StepEncounter(Player player, bool didEvent)
		{
			if (XleCore.Database.MonsterList.Count == 0) return;
			if (g.disableEncounters) return;

			// bail out if the player entered another map on this step.
			if (XleCore.Map != this)
				return;

			if (EncounterState == EncounterState.NoEncounter && stepCount <= 0)
			{
				SetNextEncounterStepCount();

				if (BanditAmbush(player))
					return;

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


		public override void AfterExecuteCommand(Player player, KeyCode cmd)
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

				g.AddBottom();
				g.AddBottom("An unknown creature is approaching ", XleColor.Cyan);
				g.AddBottom("from the " + dirName + ".", XleColor.Cyan);

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

			displayMonst = SelectRandomMonster(Terrain(player.X, player.Y));

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
				var m = new Monster(XleCore.Database.MonsterList[displayMonst]);

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
			g.AddBottom();
			g.AddBottom("You avoid the unknown creature.");

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

			g.AddBottom();
			g.AddBottom(monstCount.ToString() + " " + currentMonst[0].Name + plural, colors);

			colors[0] = XleColor.Cyan;
			g.AddBottom("is approaching.", colors);

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

				g.AddBottom("");
				g.AddBottom(monstCount.ToString() + " " + currentMonst[0].Name, colors);
				g.AddBottom("Stands before you.");

				XleCore.Wait(1500);
			}
			else
			{
				ColorStringBuilder builder = new ColorStringBuilder();
				builder.AddText("Attacked by ", XleColor.White);
				builder.AddText(monstCount.ToString(), XleColor.Yellow);
				builder.AddText(" " + currentMonst[0].Name, XleColor.Cyan);

				g.AddBottom("");
				g.AddBottom(builder);

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

			XleCore.Wait(250, !firstTime, XleCore.Redraw);
		}

		private static int SelectRandomMonster(TerrainType terrain)
		{
			int mCount = 0;
			int val, sel = -1;

			for (int i = 0; i < XleCore.Database.MonsterList.Count; i++)
			{
				if ((TerrainType)XleCore.Database.MonsterList[i].Terrain == TerrainType.All && terrain != 0)
					mCount++;

				if ((TerrainType)XleCore.Database.MonsterList[i].Terrain == terrain)
					mCount += 3;

				if (terrain == TerrainType.Foothills &&
					(TerrainType)XleCore.Database.MonsterList[i].Terrain == TerrainType.Mountain)
					mCount += 3;
			}

			val = 1 + XleCore.random.Next(mCount);

			for (int i = 0; i < XleCore.Database.MonsterList.Count; i++)
			{
				if ((TerrainType)XleCore.Database.MonsterList[i].Terrain == TerrainType.All && terrain != 0)
					val--;

				if ((TerrainType)XleCore.Database.MonsterList[i].Terrain == terrain)
					val -= 3;

				if (terrain == TerrainType.Foothills &&
					(TerrainType)XleCore.Database.MonsterList[i].Terrain == TerrainType.Mountain)
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

		/// <summary>
		/// Check to see if we should have bandits ambush the player. If so,
		/// sets banditAmbush variable.
		/// </summary>
		/// <param name="player"></param>
		private void SetBanditAmbushTime(Player player)
		{
			// make sure the player has the compendium
			if (player.Item(15) == 0) return;

			// if the player has the guard jewels we bail.
			if (player.Item(14) == 4) return;

			int pastTime = (int)(player.TimeDays - 100);
			if (pastTime < 0) pastTime = 0;

			int min = 40 - (int)(pastTime / 2);
			if (min < 3) min = 3;

			int max = 100 - (int)(pastTime / 5);
			if (max < 12) max = 12;

			int time = XleCore.random.Next(min, max);

			if (time > player.Food - 2)
			{
				time = player.Food - 2;
				if (time < 0)
					time = 1;
			}

			banditAmbush = (int)(player.TimeDays) + time;
		}
		private bool BanditAmbush(Player player)
		{
			// bail if player doesn't have compendium
			if (player.Item(15) == 0) return false;

			// bail if player has compendium and guard jewels.
			if (player.Item(14) == 4) return false;

			if (banditAmbush <= 0)
				SetBanditAmbushTime(player);

			if (player.TimeDays <= banditAmbush)
				return false;

			// set a random position for the appearance of the bandits.
			MonsterDirection(player);

			// bandit icon is number 4.
			displayMonst = 4;

			g.AddBottom();
			g.AddBottom("You are ambushed by bandits!", XleColor.Cyan);

			SoundMan.PlaySound(LotaSound.Encounter);
			XleCore.Wait(500);

			int maxDamage = player.HP / 15;
			int minDamage = Math.Min(5, maxDamage / 2);

			for (int i = 0; i < 8; i++)
			{
				player.HP -= XleCore.random.Next(minDamage, maxDamage + 1);

				SoundMan.PlaySound(LotaSound.EnemyHit);
				XleCore.Wait(250);
			}

			g.AddBottom("You fall unconsious.", XleColor.Yellow);

			XleCore.Wait(1000);
			displayMonst = -1;
			XleCore.Wait(3000, RedrawUnconscious);

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

			XleCore.Wait(3000);
			banditAmbush = 0;

			return true;
		}

		void RedrawUnconscious()
		{
			AgateLib.DisplayLib.Display.BeginFrame();

			AgateLib.DisplayLib.Display.Clear(XleColor.Gray);

			AgateLib.DisplayLib.Display.EndFrame();
		}
		private string MonsterDirection(Player player)
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

		protected override bool CheckMovementImpl(Player player, int dx, int dy)
		{
			if (EncounterState == EncounterState.UnknownCreatureApproaching)
			{
				bool moveTowards = false;

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

		protected override Extenders.IMapExtender CreateExtenderImpl()
		{
			if (XleCore.Factory == null)
				return base.CreateExtenderImpl();

			return XleCore.Factory.CreateMapExtender(this);
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
			SetBanditAmbushTime(player);
			SetNextEncounterStepCount();

			base.OnLoad(player);
		}

		public override void LeaveMap(Player player)
		{
			throw new InvalidOperationException();
		}
	}
}