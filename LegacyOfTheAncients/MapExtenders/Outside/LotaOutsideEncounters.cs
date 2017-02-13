using AgateLib.Mathematics.Geometry;
using ERY.Xle.Data;
using ERY.Xle.Maps.Outdoors;
using ERY.Xle.Services;
using ERY.Xle.Services.Game;
using ERY.Xle.Services.Menus;
using ERY.Xle.Services.Rendering.Maps;
using ERY.Xle.Services.ScreenModel;
using ERY.Xle.Services.XleSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;

namespace ERY.Xle.LotA.MapExtenders.Outside
{
	public class OutsideEncounters : IOutsideEncounters
	{
		int stepCountToEncounter;
		Direction monstDir;

		int monstCount;
		int initMonstCount;

		List<Monster> currentMonst = new List<Monster>();

		public XleData Data { get; set; }
		public XleOptions Options { get; set; }
		public GameState GameState { get; set; }
		public Random Random { get; set; }
		public ISoundMan SoundMan { get; set; }
		public IXleGameControl GameControl { get; set; }
		public ITextArea TextArea { get; set; }
		public IOutsideEncounterRenderer MapRenderer { get; set; }
		public ITerrainMeasurement TerrainMeasurement { get; set; }
		public IQuickMenu QuickMenu { get; set; }

		Player Player { get { return GameState.Player; } }

		public EncounterState EncounterState { get; set; }

		public bool IsMonsterFriendly { get; set; }

		public IReadOnlyList<Monster> CurrentMonsters
		{
			get { return currentMonst; }
		}

		public string MonsterName
		{
			get { return currentMonst[0].Name; }
		}

		public bool InEncounter
		{
			get
			{
				switch (EncounterState)
				{
					case EncounterState.MonsterAppeared:
					case EncounterState.MonsterReady:
						return true;
					default:
						return false;
				}
			}
		}

		double TerrainEncounterChance()
		{
			switch (TerrainMeasurement.TerrainAtPlayer())
			{
				case TerrainType.Grass:
					return 0.51;
				case TerrainType.Mixed:
				case TerrainType.Forest:
					return 0.67;
				case TerrainType.Swamp:
				case TerrainType.Foothills:
					return 0.9;
				case TerrainType.Desert:
				case TerrainType.Mountain:
					return 1.25;
				case TerrainType.Water:
				default:
					return 0.4;
			}
		}

		public void OnLoad()
		{
			SetNextEncounterStepCount();
		}

		public void Step()
		{
			if (Data.MonsterInfoList.Count == 0) return;
			if (Options.DisableOutsideEncounters) return;

			bool handled = false;
			UpdateEncounterState(ref handled);

			if (handled)
				return;

			if (EncounterState == EncounterState.NoEncounter)
			{
				if (Random.Next(Player.Level + 8) > TerrainEncounterChance())
				{
					currentMonst.Clear();
				}
				else
				{
					StartEncounter();
				}
			}
			else if (EncounterState == EncounterState.UnknownCreatureApproaching)
			{
				currentMonst.Clear();
				EncounterState = EncounterState.CreatureAppearing;
			}

			if (EncounterState == EncounterState.CreatureAppearing)
			{
				MonsterAppearing();
			}

		}

		private void StartEncounter()
		{
			currentMonst.Clear();
			IsMonsterFriendly = false;

			int type = Random.Next(0, 15);

			if (type < 10)
			{
				SetMonsterImagePosition();

				EncounterState = EncounterState.UnknownCreatureApproaching;
				SoundMan.PlaySound(LotaSound.Encounter);

				TextArea.PrintLine();
				TextArea.PrintLine("An unknown creature is approaching ", XleColor.Cyan);
				TextArea.PrintLine("from the " + monstDir.ToString() + ".", XleColor.Cyan);

				GameControl.Wait(1000);
			}
			else if (type < 15)
			{
				EncounterState = EncounterState.CreatureAppearing;

				//GameControl.Wait(1000);
			}
		}

		protected void SetMonsterImagePosition()
		{
			monstDir = (Direction)Random.Next((int)Direction.East, (int)Direction.South + 1);
			MapRenderer.MonsterDrawDirection = monstDir;
		}

		private void MonsterAppearing()
		{
			if (Random.Next(100) < 55)
				EncounterState = EncounterState.MonsterAppeared;
			else
				EncounterState = EncounterState.MonsterReady;

			SoundMan.PlaySound(LotaSound.Encounter);

			InitiateEncounter();

			GameControl.Wait(500);
		}

		public void InitiateEncounter()
		{
			var monsterId = SelectRandomMonster(TerrainMeasurement.TerrainAtPlayer());

			MapRenderer.DisplayMonsterID = monsterId;

			if (monstDir == Direction.None)
			{
				SetMonsterImagePosition();
			}

			for (int i = 0; i < monstCount; i++)
			{
				var m = new Monster(Data.MonsterInfoList.First(x => x.ID == monsterId));

				m.HP = (int)(m.HP * (Random.NextDouble() * 0.4 + 0.8));

				currentMonst.Add(m);
			}
		}

		private int SelectRandomMonster(TerrainType terrain)
		{
			int friendlyChance = 0;
			int allTerrainChance = 0;

			switch (terrain)
			{
				case TerrainType.Swamp:
				case TerrainType.Foothills:
					allTerrainChance = 35;
					friendlyChance = 55;
					break;
				case TerrainType.Desert:
				case TerrainType.Mountain:
					allTerrainChance = 40;
					friendlyChance = 60;
					break;
				case TerrainType.Mixed:
				case TerrainType.Forest:
					allTerrainChance = 25;
					friendlyChance = 50;
					break;
				case TerrainType.Grass:
					allTerrainChance = 22;
					friendlyChance = 40;
					break;
				case TerrainType.Water:
				default:
					break;
			}

			IEnumerable<MonsterInfo> monsters;

			IsMonsterFriendly = false;

			if (Random.Next(100) < allTerrainChance)
			{
				int skip = 3;
				int count = 4;

				if (Random.Next(100) < friendlyChance)
				{
					IsMonsterFriendly = true;

					if (Random.Next(2) < 1)
					{
						skip = 0;
						count = 3;
					}
				}

				monsters = Data.MonsterInfoList
					.Where(x => x.Terrain == TerrainType.All)
					.Skip(skip)
					.Take(count);
			}
			else if (terrain == TerrainType.Mixed)
			{
				monsters = Data.MonsterInfoList.Where(x => x.Terrain == TerrainType.Forest);
			}
			else
			{
				monsters = Data.MonsterInfoList.Where(x => x.Terrain == terrain);
			}

			int sp = (int)(Math.Min(Player.TimeQuality / 2500.0 + 1, 7.0) + 0.5);
			double rnd = Random.NextDouble();

			initMonstCount = (int)
				(Math.Pow(rnd, 3.2 * rnd + 0.83)) * sp + 1;
			monstCount = initMonstCount;

			int index = Random.Next(monsters.Count());
			return (monsters.Skip(index).First()).ID;
		}

		public virtual void UpdateEncounterState(ref bool handled)
		{
		}

		private void SetNextEncounterStepCount()
		{
			stepCountToEncounter = Random.Next(1, 40);
		}

		int attack()
		{
			int damage = PlayerHit(currentMonst[monstCount - 1].Defense);

			if (currentMonst[monstCount - 1].Vulnerability > 0)
			{
				if (Player.CurrentWeapon.ID == currentMonst[monstCount - 1].Vulnerability)
				{
					damage += Random.Next(11) + 20;
				}
				else
				{
					damage = 1 + Random.Next((damage < 10) ? damage : 10);
				}
			}
			IsMonsterFriendly = false;

			return damage;
		}

		/// <summary>
		/// Player damages a creature. Returns the amount of damage the player did,
		/// or zero if the player missed.
		/// </summary>
		/// <param name="defense"></param>
		/// <returns></returns>
		private int PlayerHit(int defense)
		{
			int wt = Player.CurrentWeapon.ID;
			int qt = Player.CurrentWeapon.Quality;

			int dam = Player.Attribute[Attributes.strength] - 12;
			dam += (int)(wt * (qt + 2)) / 2;

			dam = (int)(dam * Random.Next(30, 150) / 100.0 + 0.5);
			dam += Random.Next(-2, 3);

			if (dam < 3)
				dam = 1 + Random.Next(3);

			int hit = Player.Attribute[Attributes.dexterity] * 8 + 15 * qt;

			System.Diagnostics.Debug.WriteLine("Hit: " + hit.ToString() + " Dam: " + dam.ToString());

			hit -= Random.Next(400);

			if (hit < 0)
				dam = 0;

			//return 100;
			return dam;
		}

		public void HitMonster(int dam)
		{
			TextArea.Print("Enemy hit by blow of ", XleColor.White);
			TextArea.Print(dam.ToString(), XleColor.Cyan);
			TextArea.Print(".", XleColor.White);
			TextArea.PrintLine();

			GameControl.Wait(250 + 100 * Player.Gamespeed, keyBreak: true);

			currentMonst[monstCount - 1].HP -= dam;

			if (KilledOne())
			{
				GameControl.Wait(250);

				SoundMan.PlaySound(LotaSound.EnemyDie);

				TextArea.PrintLine();
				TextArea.PrintLine("the " + MonsterName + " dies.");

				int gold, food;
				bool finished = FinishedCombat(out gold, out food);

				GameControl.Wait(250 + 150 * Player.Gamespeed);

				if (finished)
				{
					TextArea.PrintLine();

					if (food > 0)
					{
						MenuItemList menu = new MenuItemList("Yes", "No");
						int choice;

						TextArea.PrintLine("Would you like to use the");
						TextArea.PrintLine(MonsterName + "'s flesh for food?");
						TextArea.PrintLine();

						choice = QuickMenu.QuickMenu(menu, 3, 0);

						if (choice == 1)
							food = 0;
						else
						{
							TextArea.Print("You gain ", XleColor.White);
							TextArea.Print(food.ToString(), XleColor.Green);
							TextArea.Print(" days of food.", XleColor.White);
							TextArea.PrintLine();

							Player.Food += food;
						}

					}


					if (gold < 0)
					{
						// gain weapon or armor
					}
					else if (gold > 0)
					{
						TextArea.Print("You find ", XleColor.White);
						TextArea.Print(gold.ToString(), XleColor.Yellow);
						TextArea.Print(" gold.", XleColor.White);
						TextArea.PrintLine();

						Player.Gold += gold;
					}

					GameControl.Wait(400 + 100 * Player.Gamespeed);
				}
			}
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

				gold = (int)(gold * (Random.NextDouble() + 0.5));
				food = (int)(food * (Random.NextDouble() + 0.5));

				if (Random.Next(100) < 50)
					food = 0;

				EncounterState = 0;
				MapRenderer.DisplayMonsterID = -1;
			}

			return finished;
		}

		public void AfterPlayerAction()
		{
			if (EncounterState == EncounterState.MonsterAvoided)
			{
				AvoidMonster();
			}
			else if (EncounterState == EncounterState.MonsterAppeared)
			{
				MonsterAppeared();
			}
			else if (EncounterState == EncounterState.MonsterReady)
			{
				MonsterTurn(false);
			}
		}

		private void AvoidMonster()
		{
			TextArea.PrintLine();
			TextArea.PrintLine("You avoid the unknown creature.");

			EncounterState = EncounterState.NoEncounter;

			GameControl.Wait(250);
		}

		private void MonsterAppeared()
		{
			GameControl.Wait(500);

			Color[] colors = new Color[40];
			string plural = (monstCount > 1) ? "s" : "";

			for (int i = 0; i < 40; i++)
				colors[i] = XleColor.Cyan;

			colors[0] = XleColor.White;

			EncounterState = EncounterState.MonsterReady;

			TextArea.PrintLine();
			TextArea.PrintLine(monstCount.ToString() + " " + currentMonst[0].Name + plural, colors);

			colors[0] = XleColor.Cyan;
			TextArea.PrintLine("is approaching.", colors);

			GameControl.Wait(1000);
		}

		private void MonsterTurn(bool firstTime)
		{
			GameControl.Wait(500);

			if (IsMonsterFriendly)
			{
				Color[] colors = new Color[40];

				for (int i = 0; i < 40; i++)
					colors[i] = XleColor.Cyan;
				colors[0] = XleColor.White;

				TextArea.PrintLine();
				TextArea.PrintLine(monstCount.ToString() + " " + currentMonst[0].Name, colors);
				TextArea.PrintLine("Stands before you.");

				GameControl.Wait(1500);
			}
			else
			{
				TextArea.PrintLine();
				TextArea.Print("Attacked by ", XleColor.White);
				TextArea.Print(monstCount.ToString(), XleColor.Yellow);
				TextArea.Print(" " + currentMonst[0].Name, XleColor.Cyan);
				TextArea.PrintLine();

				int dam = 0;
				int hits = 0;

				for (int i = 0; i < monstCount; i++)
				{
					int t = DamagePlayer(currentMonst[i].Attack);

					if (t > 0)
					{
						dam += t;
						hits++;
					}
				}

				TextArea.Print("Hits:  ", XleColor.White);
				TextArea.Print(hits.ToString(), XleColor.Yellow);
				TextArea.Print("   Damage:  ", XleColor.White);
				TextArea.Print(dam.ToString(), XleColor.Yellow);
				TextArea.PrintLine();

				if (dam > 0)
				{
					SoundMan.PlaySound(LotaSound.EnemyHit);
				}
				else
				{
					SoundMan.PlaySound(LotaSound.EnemyMiss);
				}
			}

			GameControl.Wait(250, keyBreak: !firstTime);
		}

		/// <summary>
		/// Called when the player gets hit. Returns the damage done to the player and
		/// subtracts that value from HP.
		/// </summary>
		/// <param name="attack"></param>
		/// <returns></returns>	
		private int DamagePlayer(int attack)
		{
			int dam = (int)(attack - (Player.Attribute[Attributes.endurance] + Player.CurrentArmor.ID) * 0.8);

			dam += (int)(dam * Random.Next(-50, 100) / 100 + 0.5);

			if (dam < 0 || 1 + Random.Next(60) + attack / 15
							< Player.Attribute[Attributes.dexterity] + Player.CurrentArmor.Quality)
			{
				dam = 0;
			}

			Player.HP -= dam;

			return dam;
		}

		public void CancelEncounter()
		{
			EncounterState = EncounterState.NoEncounter;
			MapRenderer.DisplayMonsterID = -1;
		}

		public bool AttemptMovement(int dx, int dy)
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

				if (moveTowards == false && Random.Next(100) < 50)
				{
					EncounterState = EncounterState.MonsterAvoided;
				}
			}
			else if (EncounterState == EncounterState.MonsterReady)
			{
				if (Random.Next(100) < 50 && IsMonsterFriendly == false)
				{
					return false;
				}
				else
				{
					EncounterState = EncounterState.JustDisengaged;
					MapRenderer.DisplayMonsterID = -1;
				}
			}

			return true;
		}
	}
}
