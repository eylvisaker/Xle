using AgateLib.Geometry;
using AgateLib.InputLib;
using ERY.Xle.Maps.Renderers;
using ERY.Xle.XleEventTypes;
using ERY.Xle.XleEventTypes.Extenders;
using ERY.Xle.XleEventTypes.Extenders.Common;
using ERY.Xle.XleEventTypes.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps.XleMapTypes.Extenders
{
	public class OutsideExtender : Map2DExtender
	{
		List<Monster> currentMonst = new List<Monster>();

		int stepCountToEncounter;
		Direction monstDir;
		int monstCount, initMonstCount;
		bool isMonsterFriendly;

		public EncounterState EncounterState { get; set; }

		public new Outside TheMap { get { return (Outside)base.TheMap; } }
		public new OutsideRenderer MapRenderer
		{
			get { return (OutsideRenderer)base.MapRenderer; }
		}

		protected override XleMapRenderer CreateMapRenderer()
		{
			return new OutsideRenderer();
		}

		public override void CheckSounds(GameState state)
		{
			var player = state.Player;

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
							if (TheMap[player.X + i, player.Y + j] < 16)
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

		public override int GetOutsideTile(AgateLib.Geometry.Point playerPoint, int x, int y)
		{
			return 0;
		}

		public override bool UseFancyMagicPrompt
		{
			get { return false; }
		}

		public override IEventExtender CreateEventExtender(XleEvent evt, Type defaultExtender)
		{
			if (evt is ChangeMapEvent)
				return new ChangeMapQuestion();
			else
				return base.CreateEventExtender(evt, defaultExtender);
		}

		public override void SetColorScheme(ColorScheme scheme)
		{
			scheme.TextColor = XleColor.White;

			scheme.FrameColor = XleColor.Brown;
			scheme.FrameHighlightColor = XleColor.Yellow;

			scheme.MapAreaWidth = 25;
		}

		public virtual void ModifyTerrainInfo(TerrainInfo info, TerrainType terrain)
		{
		}

		protected override bool PlayerSpeakImpl(GameState state)
		{
			if (EncounterState != EncounterState.MonsterReady)
			{
				return false;
			}

			SpeakToMonster(state.Player);

			return true;
		}

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
				MapRenderer.DisplayMonsterID = -1;
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
			MapRenderer.DisplayMonsterID = -1;
		}


		private void StartEncounter(Player player)
		{
			currentMonst.Clear();
			isMonsterFriendly = false;

			int type = XleCore.random.Next(0, 15);

			if (type < 10)
			{
				SetMonsterImagePosition(player);

				EncounterState = EncounterState.UnknownCreatureApproaching;
				SoundMan.PlaySound(LotaSound.Encounter);

				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("An unknown creature is approaching ", XleColor.Cyan);
				XleCore.TextArea.PrintLine("from the " + monstDir.ToString() + ".", XleColor.Cyan);

				XleCore.Wait(1000);
			}
			else if (type < 15)
			{
				EncounterState = EncounterState.CreatureAppearing;

				//XleCore.wait(1000);
			}
		}

		private void SetNextEncounterStepCount()
		{
			stepCountToEncounter = XleCore.random.Next(1, 40);
		}

		private void MonsterAppearing(Player player)
		{
			if (XleCore.random.Next(100) < 55)
				EncounterState = EncounterState.MonsterAppeared;
			else
				EncounterState = EncounterState.MonsterReady;

			SoundMan.PlaySound(LotaSound.Encounter);

			MapRenderer.DisplayMonsterID = SelectRandomMonster(TerrainAt(player.X, player.Y));

			if (monstDir == Direction.None)
				SetMonsterImagePosition(player);

			int max = 1;
			initMonstCount = monstCount = 1 + XleCore.random.Next(max);

			for (int i = 0; i < monstCount; i++)
			{
				var m = new Monster(XleCore.Data.Database.MonsterList[MapRenderer.DisplayMonsterID]);

				m.HP = (int)(m.HP * (XleCore.random.NextDouble() * 0.4 + 0.8));

				currentMonst.Add(m);
			}

			if (XleCore.random.Next(256) <= currentMonst[0].Friendly)
				isMonsterFriendly = true;
			else
				isMonsterFriendly = false;

			XleCore.Wait(500);
		}

		private void AvoidMonster(GameState state)
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

		private void MonsterTurn(GameState state, bool firstTime)
		{
			var player = state.Player;
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


		public virtual void UpdateEncounterState(GameState state, ref bool handled)
		{
		}

		public override void PlayerCursorMovement(GameState state, Direction dir)
		{
			var player = state.Player;
			string command;
			Point stepDirection;

			_Move2D(player, dir, "Move", out command, out stepDirection);

			XleCore.TextArea.PrintLine(command);

			if (CanPlayerStep(state, stepDirection) == false)
			{
				TerrainType terrain = TheMap.TerrainAt(player.X + stepDirection.X, player.Y + stepDirection.Y);

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
				BeforeStepOn(state, player.X + stepDirection.X, player.Y + stepDirection.Y);

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
			PlayerCursorMovement(state, dir);

			SoundMan.StopSound(LotaSound.Raft1);
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

		public override bool PlayerFight(GameState state)
		{
			string weaponName;
			var player = state.Player;

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

		public override bool PlayerXamine(GameState state)
		{
			TerrainInfo info = GetTerrainInfo(state.Player);

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

			ModifyTerrainInfo(info, terrain);

			return info;
		}

		public TerrainType TerrainAt(int xx, int yy)
		{
			int[,] t = new int[2, 2] { { 0, 0 }, { 0, 0 } };
			int[] tc = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };

			for (int j = 0; j < 2; j++)
			{
				for (int i = 0; i < 2; i++)
				{
					t[j, i] = TheMap[xx + i, yy + j];
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

		private void UpdateRaftState(Player player)
		{
			if (player.IsOnRaft == false)
			{
				foreach (var raft in player.Rafts.Where(r => r.MapNumber == TheMap.MapID))
				{
					if (raft.Location.Equals(player.Location))
					{
						player.BoardedRaft = raft;
						break;
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
		private void StepEncounter(Player player)
		{
			if (XleCore.Data.Database.MonsterList.Count == 0) return;
			if (XleCore.Options.DisableOutsideEncounters) return;

			// bail out if the player entered another map on this step.
			if (XleCore.GameState.Map != TheMap)
				return;

			bool handled = false;
			UpdateEncounterState(XleCore.GameState, ref handled);

			if (handled)
				return;

			if (EncounterState == EncounterState.NoEncounter && stepCountToEncounter <= 0)
			{
				SetNextEncounterStepCount();

				StartEncounter(player);
			}
			else if (EncounterState == EncounterState.NoEncounter && stepCountToEncounter > 0)
			{
				currentMonst.Clear();
				stepCountToEncounter--;
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

		public override void AfterExecuteCommand(GameState state, KeyCode cmd)
		{
			if (EncounterState == EncounterState.MonsterAvoided)
			{
				AvoidMonster(state);
			}
			else if (EncounterState == EncounterState.MonsterAppeared)
			{
				MonsterAppeared();
			}
			else if (EncounterState == EncounterState.MonsterReady)
			{
				MonsterTurn(state, false);
			}
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
					MapRenderer.DisplayMonsterID = -1;
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

		public override void AfterPlayerStep(GameState state)
		{
			base.AfterPlayerStep(state);

			UpdateRaftState(state.Player);

			StepEncounter(state.Player);
		}

		public override void LeaveMap(Player player)
		{
			throw new InvalidOperationException();
		}

		public override void OnLoad(GameState state)
		{
			SetNextEncounterStepCount();
			base.OnLoad(state);
		}

		[Obsolete("Call SetMonsterImagePosition first, then use monstDir.ToString for the return value.", true)]
		public string MonsterDirection(Player player)
		{
			SetMonsterImagePosition(player);

			return monstDir.ToString();
		}

		protected void SetMonsterImagePosition(Player player)
		{
			MapRenderer.MonsterDrawPoint.X = player.X - 1;
			MapRenderer.MonsterDrawPoint.Y = player.Y - 1;

			monstDir = (Direction)XleCore.random.Next((int)Direction.East, (int)Direction.South + 1);

			switch (monstDir)
			{
				case Direction.East: MapRenderer.MonsterDrawPoint.X += 2; break;
				case Direction.North: MapRenderer.MonsterDrawPoint.Y -= 2; break;
				case Direction.West: MapRenderer.MonsterDrawPoint.X -= 2; break;
				case Direction.South: MapRenderer.MonsterDrawPoint.Y += 2; break;
				default:
					throw new Exception("Invalid direction.");
			}
		}


		/*
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

			
		}
		*/
		protected override void PlayerMagicImpl(GameState state, MagicSpell magic)
		{
			switch (magic.ID)
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

					var sound = (magic.ID == 1) ?
						LotaSound.MagicFlame : LotaSound.MagicBolt;

					if (RollSpellFizzle(state, magic))
					{
						SoundMan.PlayMagicSound(sound, LotaSound.MagicFizzle, 1);

						XleCore.TextArea.PrintLine("Attack fizzles.", XleColor.Yellow);
						return;
					}
					else
						SoundMan.PlayMagicSound(sound, LotaSound.MagicFlameHit, 1);

					int damage = RollSpellDamage(state, magic, 0);

					HitMonster(state.Player, damage);

					break;

				default:
					CastSpell(state, magic);
					break;
			}
		}
	}
}
