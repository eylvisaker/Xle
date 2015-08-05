using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Data;
using ERY.Xle.Services;
using ERY.Xle.Services.Commands.Implementation;
using ERY.Xle.Services.Rendering.Maps;

namespace ERY.Xle.Maps.Outdoors.Commands
{
    [ServiceName("OutsideFight")]
    public class OutsideFight : Fight
    {
        OutsideExtender map { get { return (OutsideExtender)GameState.MapExtender; } }
        OutsideRenderer MapRenderer
        {
            get { return map.MapRenderer; }
        }

        EncounterState EncounterState
        {
            get { return map.EncounterState; }
            set { map.EncounterState = value; }
        }

        bool IsMonsterFriendly
        {
            get { return map.IsMonsterFriendly; }
            set { map.IsMonsterFriendly = value; }
        }

        string MonstName
        {
            get { return map.MonstName; }
        }

        List<Monster> currentMonst
        {
            get { return map.CurrentMonsters; }
        }

        int monstCount
        {
            get { return map.monstCount; }
            set { map.monstCount = value; }
        }
        int initMonstCount { get { return map.initMonstCount; } }

        public override void Execute()
        {
            string weaponName = Player.CurrentWeapon.BaseName(Data);

            TextArea.PrintLine("\n");

            if (EncounterState == EncounterState.MonsterReady)
            {
                int dam = attack();

                TextArea.Print("Attack ", XleColor.White);
                TextArea.Print(MonstName, XleColor.Cyan);
                TextArea.PrintLine();

                TextArea.Print("with ", XleColor.White);
                TextArea.Print(weaponName, XleColor.Cyan);
                TextArea.PrintLine();

                if (dam <= 0)
                {
                    SoundMan.PlaySound(LotaSound.PlayerMiss);

                    TextArea.PrintLine("Your Attack missed.", XleColor.Yellow);

                    return;
                }

                SoundMan.PlaySound(LotaSound.PlayerHit);

                HitMonster(dam);
            }
            else if (EncounterState > 0)
            {
                TextArea.PrintLine("The unknown creature is not ");
                TextArea.PrintLine("within range.");

                GameControl.Wait(300 + 100 * Player.Gamespeed);
            }
            else
            {
                return;
            }

            return;
        }
        private void HitMonster(int dam)
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
                TextArea.PrintLine("the " + MonstName + " dies.");

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
                        TextArea.PrintLine(MonstName + "'s flesh for food?");
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
        bool KilledOne()
        {
            if (currentMonst[monstCount - 1].HP <= 0)
            {
                monstCount--;

                return true;

            }

            return false;
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

    }
}
