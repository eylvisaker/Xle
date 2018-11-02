using AgateLib;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xle.Data;
using Xle.Maps.Outdoors;
using Xle.Services.Game;
using Xle.Services.Menus;
using Xle.Services.Rendering.Maps;
using Xle.Services.ScreenModel;
using Xle.Services.XleSystem;

namespace Xle.Ancients.MapExtenders.Outside
{
    [Singleton, InjectProperties]
    public class OutsideEncounters : IOutsideEncounters
    {
        private int stepCountToEncounter;
        private Direction monstDir;
        private int monstCount;
        private int initMonstCount;
        private List<Monster> currentMonst = new List<Monster>();

        public XleData Data { get; set; }
        public XleOptions Options { get; set; }
        public GameState GameState { get; set; }
        public Random Random { get; set; }
        public ISoundMan SoundMan { get; set; }
        public IXleGameControl GameControl { get; set; }
        public ITextArea TextArea { get; set; }
        public OutsideRenderState RenderState { get; set; }
        public ITerrainMeasurement TerrainMeasurement { get; set; }
        public IQuickMenu QuickMenu { get; set; }

        private Player Player { get { return GameState.Player; } }

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

        private double TerrainEncounterChance()
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

        public async Task Step()
        {
            if (Data.MonsterInfo.Count == 0) return;
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
                   await StartEncounter();
                }
            }
            else if (EncounterState == EncounterState.UnknownCreatureApproaching)
            {
                currentMonst.Clear();
                EncounterState = EncounterState.CreatureAppearing;
            }

            if (EncounterState == EncounterState.CreatureAppearing)
            {
                await MonsterAppearing();
            }

        }

        private async Task StartEncounter()
        {
            currentMonst.Clear();
            IsMonsterFriendly = false;

            int type = Random.Next(0, 15);

            if (type < 10)
            {
                SetMonsterImagePosition();

                EncounterState = EncounterState.UnknownCreatureApproaching;
                SoundMan.PlaySound(LotaSound.Encounter);

                await TextArea.PrintLine();
                await TextArea.PrintLine("An unknown creature is approaching ", XleColor.Cyan);
                await TextArea.PrintLine("from the " + monstDir.ToString() + ".", XleColor.Cyan);

                await GameControl.WaitAsync(1000);
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
            RenderState.MonsterDrawDirection = monstDir;
        }

        private async Task MonsterAppearing()
        {
            if (Random.Next(100) < 55)
                EncounterState = EncounterState.MonsterAppeared;
            else
                EncounterState = EncounterState.MonsterReady;

            SoundMan.PlaySound(LotaSound.Encounter);

            InitiateEncounter();

            await GameControl.WaitAsync(500);
        }

        public void InitiateEncounter()
        {
            var monsterId = SelectRandomMonster(TerrainMeasurement.TerrainAtPlayer());

            RenderState.DisplayMonsterID = monsterId;

            if (monstDir == Direction.None)
            {
                SetMonsterImagePosition();
            }

            for (int i = 0; i < monstCount; i++)
            {
                var m = new Monster(Data.MonsterInfo.First(x => x.ID == monsterId));

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

                monsters = Data.MonsterInfo
                    .Where(x => x.Terrain == TerrainType.All)
                    .Skip(skip)
                    .Take(count);
            }
            else if (terrain == TerrainType.Mixed)
            {
                monsters = Data.MonsterInfo.Where(x => x.Terrain == TerrainType.Forest);
            }
            else
            {
                monsters = Data.MonsterInfo.Where(x => x.Terrain == terrain);
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

        private int attack()
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
            dam += wt * (qt + 2) / 2;

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

        public async Task HitMonster(int dam)
        {
            await TextArea.Print("Enemy hit by blow of ", XleColor.White);
            await TextArea.Print(dam.ToString(), XleColor.Cyan);
            await TextArea.Print(".", XleColor.White);
            await TextArea.PrintLine();

            await GameControl.WaitAsync(250 + 100 * Player.Gamespeed, keyBreak: true);

            currentMonst[monstCount - 1].HP -= dam;

            if (KilledOne())
            {
                await GameControl.WaitAsync(250);

                SoundMan.PlaySound(LotaSound.EnemyDie);

                await TextArea.PrintLine();
                await TextArea.PrintLine("the " + MonsterName + " dies.");

                int gold, food;
                bool finished = FinishedCombat(out gold, out food);

                await GameControl.WaitAsync(250 + 150 * Player.Gamespeed);

                if (finished)
                {
                    await TextArea.PrintLine();

                    if (food > 0)
                    {
                        MenuItemList menu = new MenuItemList("Yes", "No");
                        int choice;

                        await TextArea.PrintLine("Would you like to use the");
                        await TextArea.PrintLine(MonsterName + "'s flesh for food?");
                        await TextArea.PrintLine();

                        choice = await QuickMenu.QuickMenu(menu, 3, 0);

                        if (choice == 1)
                            food = 0;
                        else
                        {
                            await TextArea.Print("You gain ", XleColor.White);
                            await TextArea.Print(food.ToString(), XleColor.Green);
                            await TextArea.Print(" days of food.", XleColor.White);
                            await TextArea.PrintLine();

                            Player.Food += food;
                        }

                    }


                    if (gold < 0)
                    {
                        // gain weapon or armor
                    }
                    else if (gold > 0)
                    {
                        await TextArea.Print("You find ", XleColor.White);
                        await TextArea.Print(gold.ToString(), XleColor.Yellow);
                        await TextArea.Print(" gold.", XleColor.White);
                        await TextArea.PrintLine();

                        Player.Gold += gold;
                    }

                    await GameControl.WaitAsync(400 + 100 * Player.Gamespeed);
                }
            }
        }

        private bool KilledOne()
        {
            if (currentMonst[monstCount - 1].HP <= 0)
            {
                monstCount--;

                return true;

            }

            return false;
        }

        private bool FinishedCombat(out int gold, out int food)
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
                RenderState.DisplayMonsterID = -1;
            }

            return finished;
        }

        public async Task AfterPlayerAction()
        {
            if (EncounterState == EncounterState.MonsterAvoided)
            {
                await AvoidMonster();
            }
            else if (EncounterState == EncounterState.MonsterAppeared)
            {
                await MonsterAppeared();
            }
            else if (EncounterState == EncounterState.MonsterReady)
            {
                await MonsterTurn(false);
            }
        }

        private async Task AvoidMonster()
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine("You avoid the unknown creature.");

            EncounterState = EncounterState.NoEncounter;

            await GameControl.WaitAsync(250);
        }

        private async Task MonsterAppeared()
        {
            await GameControl.WaitAsync(500);

            Color[] colors = new Color[40];
            string plural = (monstCount > 1) ? "s" : "";

            for (int i = 0; i < 40; i++)
                colors[i] = XleColor.Cyan;

            colors[0] = XleColor.White;

            EncounterState = EncounterState.MonsterReady;

            await TextArea.PrintLine();
            await TextArea.PrintLine(monstCount.ToString() + " " + currentMonst[0].Name + plural, colors);

            colors[0] = XleColor.Cyan;
            await TextArea.PrintLine("is approaching.", colors);

            await GameControl.WaitAsync(1000);
        }

        private async Task MonsterTurn(bool firstTime)
        {
            await GameControl.WaitAsync(500);

            if (IsMonsterFriendly)
            {
                Color[] colors = new Color[40];

                for (int i = 0; i < 40; i++)
                    colors[i] = XleColor.Cyan;
                colors[0] = XleColor.White;

               await TextArea.PrintLine();
               await TextArea.PrintLine(monstCount.ToString() + " " + currentMonst[0].Name, colors);
               await TextArea.PrintLine("Stands before you.");

                await GameControl.WaitAsync(1500);
            }
            else
            {
                await TextArea.PrintLine();
                await TextArea.Print("Attacked by ", XleColor.White);
                await TextArea.Print(monstCount.ToString(), XleColor.Yellow);
                await TextArea.Print(" " + currentMonst[0].Name, XleColor.Cyan);
                await TextArea.PrintLine();

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

             await   TextArea.Print("Hits:  ", XleColor.White);
             await   TextArea.Print(hits.ToString(), XleColor.Yellow);
             await   TextArea.Print("   Damage:  ", XleColor.White);
             await   TextArea.Print(dam.ToString(), XleColor.Yellow);
             await   TextArea.PrintLine();

                if (dam > 0)
                {
                    SoundMan.PlaySound(LotaSound.EnemyHit);
                }
                else
                {
                    SoundMan.PlaySound(LotaSound.EnemyMiss);
                }
            }

          await  GameControl.WaitAsync(250, keyBreak: !firstTime);
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
            RenderState.DisplayMonsterID = -1;
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
                    RenderState.DisplayMonsterID = -1;
                }
            }

            return true;
        }
    }
}
