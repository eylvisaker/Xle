using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgateLib.Geometry;

using ERY.Xle.Data;
using ERY.Xle.Services;
using ERY.Xle.Services.Commands.Implementation;
using ERY.Xle.Services.Game;
using ERY.Xle.Services.Menus;
using ERY.Xle.Services.Rendering.Maps;
using ERY.Xle.Services.ScreenModel;
using ERY.Xle.Services.XleSystem;

namespace ERY.Xle.Maps.Outdoors
{
    [ServiceName("OutsideSpeak")]
    public class OutsideSpeak : Speak
    {
        public IOutsideEncounters Encounters { get; set; }
        public IStatsDisplay StatsDisplay { get; set; }
        public IXleGameControl GameControl { get; set; }
        public Random Random { get; set; }
        public XleData Data { get; set; }
        public IMuseumCoinSale MuseumCoinSale { get; set; }
        public IQuickMenu QuickMenu { get; set; }
        public ISoundMan SoundMan { get; set; }

        public override void Execute()
        {
            if (Encounters.EncounterState != EncounterState.MonsterReady)
                base.Execute();

            SpeakToMonster();
        }

        void SpeakToMonster()
        {
            TextArea.PrintLine();

            if (!Encounters.IsMonsterFriendly)
            {
                TextArea.PrintLine();
                TextArea.PrintLine("The " + Encounters.MonsterName + " does not reply.");

                GameControl.Wait(250);

                return;
            }

            const int talkTypes = 5;
            int type;
            int qual = Random.Next(5);
            int cost = 0;
            int item = 0;
            MenuItemList menu = new MenuItemList("Yes", "No");

            Color qcolor = XleColor.White;

            string[] quality = new string[5] { "a Well Crafted", "a Slightly Used", "a Sparkling New", "a Wonderful", "an Awesome" };

            do
            {
                type = Random.Next(talkTypes) + 1;
            } while (Player.MaxHP == Player.HP && type == 4);

            string name = "";

            switch (type)
            {
                case 1:			// buy armor
                case 2:         // buy weapon

                    TextArea.Print("Do you want to buy ", XleColor.Cyan);
                    TextArea.Print(quality[qual], XleColor.White);
                    TextArea.PrintLine();

                    if (type == 1)
                    {
                        item = Random.Next(4) + 1;
                        cost = (int)(Data.ArmorCost(item, qual) * (Random.NextDouble() * 0.6 + 0.6));
                        name = Data.ArmorList[item].Name;
                    }
                    else if (type == 2)
                    {
                        item = Random.Next(7) + 1;
                        cost = (int)(Data.WeaponCost(item, qual) * (Random.NextDouble() * 0.6 + 0.6));
                        name = Data.WeaponList[item].Name;
                    }

                    TextArea.Print(name, XleColor.White);
                    TextArea.Print(" for ", XleColor.Cyan);
                    TextArea.Print(cost.ToString(), XleColor.White);
                    TextArea.Print(" Gold?", XleColor.Cyan);
                    TextArea.PrintLine();

                    qcolor = XleColor.Cyan;

                    break;
                case 3:			// buy food

                    item = Random.Next(21) + 20;
                    cost = (int)(item * (Random.NextDouble() * 0.4 + 0.8));

                    TextArea.Print("Do you want to buy ", XleColor.Green);
                    TextArea.Print(item.ToString(), XleColor.Yellow);
                    TextArea.PrintLine();

                    // line 2
                    TextArea.Print("Days of food for ", XleColor.Green);
                    TextArea.Print(cost.ToString(), XleColor.Yellow);
                    TextArea.Print(" gold?", XleColor.Green);
                    TextArea.PrintLine();

                    qcolor = XleColor.Green;

                    break;
                case 4:			// buy hp

                    item = Random.Next(Player.MaxHP / 4) + 20;

                    if (item > (Player.MaxHP - Player.HP))
                        item = (Player.MaxHP - Player.HP);

                    cost = (int)(item * (Random.NextDouble() * 0.15 + 0.75));

                    TextArea.Print("Do you want to buy a potion worth ", XleColor.Green);
                    TextArea.PrintLine();

                    // line 2
                    TextArea.Print(item.ToString(), XleColor.Yellow);
                    TextArea.Print(" Hit Points for ", XleColor.Green);
                    TextArea.Print(cost.ToString(), XleColor.Yellow);
                    TextArea.Print(" gold?", XleColor.Green);
                    TextArea.PrintLine();

                    qcolor = XleColor.Green;

                    break;

                default:
                case 5:			// buy museum coin
                    MuseumCoinSale.OfferMuseumCoin();

                    break;

            }

            if (type != 5)
            {
                TextArea.PrintLine();

                int choice = QuickMenu.QuickMenu(menu, 3, 0, qcolor);

                if (choice == 0)
                {
                    if (Player.Spend(cost))
                    {
                        SoundMan.PlaySound(LotaSound.Sale);

                        TextArea.PrintLine();
                        TextArea.PrintLine("Purchase Completed.");

                        Color clr2 = XleColor.White;

                        switch (type)
                        {
                            case 1:
                                Player.AddArmor(item, qual);

                                break;
                            case 2:
                                Player.AddWeapon(item, qual);

                                break;
                            case 3:
                                Player.Food += item;
                                clr2 = XleColor.Green;

                                break;
                            case 4:
                                Player.HP += item;
                                clr2 = XleColor.Green;

                                break;
                            case 5:
                                break;
                        }

                        StatsDisplay.FlashHPWhileSound(clr2);
                    }
                    else
                    {
                        SoundMan.PlaySound(LotaSound.Medium);

                        TextArea.PrintLine();
                        TextArea.PrintLine("You don't have enough gold...");
                    }

                }
                else
                {
                    SoundMan.PlaySound(LotaSound.Medium);

                    TextArea.PrintLine();

                    if (1 + Random.Next(2) == 1)
                        TextArea.PrintLine("Maybe Later...");
                    else
                        TextArea.PrintLine("You passed up a good deal!");

                }
            }

            Encounters.CancelEncounter();
        }

    }
}
