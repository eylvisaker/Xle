using Xle.Data;
using Xle.Services;
using Xle.Services.Commands.Implementation;
using Xle.Services.Game;
using Xle.Services.Menus;
using Xle.Services.ScreenModel;
using Xle.Services.XleSystem;
using Microsoft.Xna.Framework;
using System;
using System.Threading.Tasks;

namespace Xle.Maps.Outdoors
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

        public override async Task Execute()
        {
            if (Encounters.EncounterState != EncounterState.MonsterReady)
                await base.Execute();

            await SpeakToMonster();
        }

        private async Task SpeakToMonster()
        {
            await TextArea.PrintLine();

            if (!Encounters.IsMonsterFriendly)
            {
                await TextArea.PrintLine();
                await TextArea.PrintLine("The " + Encounters.MonsterName + " does not reply.");

                await GameControl.WaitAsync(250);

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

                    await TextArea.Print("Do you want to buy ", XleColor.Cyan);
                    await TextArea.Print(quality[qual], XleColor.White);
                    await TextArea.PrintLine();

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

                    await TextArea.Print(name, XleColor.White);
                    await TextArea.Print(" for ", XleColor.Cyan);
                    await TextArea.Print(cost.ToString(), XleColor.White);
                    await TextArea.Print(" Gold?", XleColor.Cyan);
                    await TextArea.PrintLine();

                    qcolor = XleColor.Cyan;

                    break;
                case 3:			// buy food

                    item = Random.Next(21) + 20;
                    cost = (int)(item * (Random.NextDouble() * 0.4 + 0.8));

                    await TextArea.Print("Do you want to buy ", XleColor.Green);
                    await TextArea.Print(item.ToString(), XleColor.Yellow);
                    await TextArea.PrintLine();

                    // line 2
                    await TextArea.Print("Days of food for ", XleColor.Green);
                    await TextArea.Print(cost.ToString(), XleColor.Yellow);
                    await TextArea.Print(" gold?", XleColor.Green);
                    await TextArea.PrintLine();

                    qcolor = XleColor.Green;

                    break;
                case 4:			// buy hp

                    item = Random.Next(Player.MaxHP / 4) + 20;

                    if (item > (Player.MaxHP - Player.HP))
                        item = (Player.MaxHP - Player.HP);

                    cost = (int)(item * (Random.NextDouble() * 0.15 + 0.75));

                    await TextArea.Print("Do you want to buy a potion worth ", XleColor.Green);
                    await TextArea.PrintLine();

                    // line 2
                    await TextArea.Print(item.ToString(), XleColor.Yellow);
                    await TextArea.Print(" Hit Points for ", XleColor.Green);
                    await TextArea.Print(cost.ToString(), XleColor.Yellow);
                    await TextArea.Print(" gold?", XleColor.Green);
                    await TextArea.PrintLine();

                    qcolor = XleColor.Green;

                    break;

                default:
                case 5:			// buy museum coin
                    MuseumCoinSale.ResetCoinOffers();
                    await MuseumCoinSale.OfferMuseumCoin();

                    break;

            }

            if (type != 5)
            {
               await  TextArea.PrintLine();

                int choice = await QuickMenu.QuickMenu(menu, 3, 0, qcolor);

                if (choice == 0)
                {
                    if (Player.Spend(cost))
                    {
                        SoundMan.PlaySound(LotaSound.Sale);

                        await TextArea.PrintLine();
                        await TextArea.PrintLine("Purchase Completed.");

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

                        await TextArea.PrintLine();
                        await TextArea.PrintLine("You don't have enough gold...");
                    }

                }
                else
                {
                    SoundMan.PlaySound(LotaSound.Medium);

                    await TextArea.PrintLine();

                    if (1 + Random.Next(2) == 1)
                        await TextArea.PrintLine("Maybe Later...");
                    else
                        await TextArea.PrintLine("You passed up a good deal!");

                }
            }

            Encounters.CancelEncounter();
        }

    }
}
