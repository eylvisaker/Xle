﻿using AgateLib;
using Microsoft.Xna.Framework;
using System;
using System.Threading.Tasks;
using Xle.Data;
using Xle.Maps.XleMapTypes;
using Xle.XleSystem;

namespace Xle.XleEventTypes.Stores.Extenders
{
    [Transient("StoreFood")]
    public class StoreFood : StoreFront
    {
        private bool skipMailOffer = false;

        public XleData Data { get; set; }
        public XleSystemState systemState { get; set; }

        protected override void InitializeColorScheme(ColorScheme cs)
        {
            cs.BackColor = XleColor.DarkGray;
            cs.FrameColor = XleColor.Green;
            cs.FrameHighlightColor = XleColor.Yellow;
            cs.TextColor = XleColor.Yellow;
        }

        protected override async Task<bool> SpeakImplAsync()
        {
            string tempString;
            double cost = ((int)(13 - Player.Attribute[Attributes.charm] / 7.0)) / 10.0;

            if (cost < 0.1)
                cost = 0.1;

            int max = (int)(Player.Gold / cost);
            if (max > 5000) max = 5000;

            SetTitle();

            this.robbing = false;

            await Wait(1);

            await TextArea.PrintLine();

            int choice;

            if (Player.mailTown == Map.MapID)
            {
                await PayForMail();
                skipMailOffer = true;
            }
            else
            {
                SetWindow(cost);

                tempString = "      Maximum purchase:  ";
                tempString += max;
                tempString += " days";

                await TextArea.PrintLine();
                await TextArea.PrintLine(tempString, XleColor.Cyan);

                choice = await ChooseNumber(max);

                if (choice > 0)
                {
                    Player.Spend((int)(choice * cost));
                    Player.Food += choice;

                    await TextArea.PrintLine();
                    await TextArea.PrintLine(" " + choice + " days of food bought.");

                    await StoreSound(LotaSound.Sale);

                    if (skipMailOffer == false)
                        await OfferMail();

                    return true;
                }
                else
                {
                    await TextArea.PrintLine();
                    await TextArea.PrintLine("Nothing Purchased");

                    await StoreSound(LotaSound.Medium);
                }
            }

            await CheckOfferMuseumCoin(Player);

            return true;

        }

        private async Task OfferMail()
        {
            Town twn = GameState.Map as Town;

            if (Player.Items[MailItemId] > 0) return;
            if (twn == null) return;
            if (twn.Mail.Count == 0) return;

            int target = SelectDeliveryTarget(twn);

            if (target < 0)
                return;

            SoundMan.PlaySound(LotaSound.Question);

            await TextArea.PrintLine();
            await TextArea.PrintLine("Would you like to earn some gold?");

            MenuItemList menu = new MenuItemList("Yes", "No");

            int choice = await QuickMenu(menu, 2);

            if (choice == 0)
            {
                Player.Items[MailItemId] = 1;
                Player.mailTown = target;

                await TextArea.PrintLine();
                await TextArea.PrintLine("Here's some mail to");
                await TextArea.PrintLine("deliver to " + Data.MapList[target].Name + ".");
                await TextArea.PrintLine();
                await TextArea.PrintLine("        Press Key to Continue");

                await WaitForKey();
            }
        }

        private int MailItemId
        {
            get { return systemState.Factory.MailItemID; }
        }

        private int SelectDeliveryTarget(Town twn)
        {
            int target;
            int mMap = Random.Next(twn.Mail.Count);

            int count = 0;
            bool valid = false;

            // search for a valid map
            do
            {
                target = twn.Mail[mMap];

                if (Data.MapList.ContainsKey(target) &&
                    Data.MapList[target].Name != "")
                {
                    valid = true;
                }
                else
                {
                    mMap++;
                    if (mMap == twn.Mail.Count) mMap = 0;
                }

                count++;

            } while (count < 6 && valid == false);

            if (valid == false)
                return -1;

            return target;
        }
        private void SetWindow(double cost)
        {
            Screen.ClearWindows();

            var promptWindow = new TextWindow();
            promptWindow.Location = new Point(9, 4);

            promptWindow.WriteLine("    Food & water");
            promptWindow.WriteLine();
            promptWindow.WriteLine();
            promptWindow.WriteLine("We sell food for travel.");
            promptWindow.WriteLine("Each 'day' of food will ");
            promptWindow.WriteLine("keep you fed for one day");
            promptWindow.WriteLine("of travel (on foot).    ");
            promptWindow.WriteLine();
            promptWindow.WriteLine();

            if (robbing == false)
            {
                promptWindow.Write("Cost is ");
                promptWindow.Write(cost.ToString());
                promptWindow.WriteLine(" gold per 'day'");
            }
            else
                promptWindow.WriteLine("Robbery in progress");

            promptWindow.SetColor(XleColor.Yellow);

            Screen.AddWindow(promptWindow);
        }

        private void SetTitle()
        {
            Title = TheEvent.ShopName;
        }

        private async Task PayForMail()
        {
            int gold = Random.Next(1, 4);

            switch (gold)
            {
                case 1: gold = 95; break;
                case 2: gold = 110; break;
                case 3: gold = 125; break;
            }

            await TextArea.PrintLine();
            await TextArea.PrintLine("Thanks for the delivery. ");
            await TextArea.PrintLine("Here's " + gold.ToString() + " gold.");
            await TextArea.PrintLine();
            await TextArea.PrintLine();

            await StoreSound(LotaSound.Good);
            await TextArea.RewriteLine(4, "        Press Key to Continue");
            await WaitForKey();

            Player.Gold += gold;
            Player.Items[MailItemId] = 0;
            Player.mailTown = 0;
        }

        private int robCount;

        protected override async Task<bool> RobCore()
        {
            SetTitle();
            await Wait(1);
            SetWindow(0);

            TextArea.Clear();

            if (robCount < 4)
            {
                robCount++;
                robbing = true;

                int choice = Random.Next(1, 16) + Random.Next(20, 36);

                await TextArea.PrintLine();
                await TextArea.PrintLine(string.Format("Stole {0} days of food.", choice), XleColor.Yellow);

                Player.Food += choice;
                SoundMan.PlaySound(LotaSound.Sale);

                if (Random.NextDouble() < 0.25)
                    robCount = 4;

            }
            else
            {
                await TextArea.PrintLine();
                await TextArea.PrintLine("No items within reach now.", XleColor.Yellow);

                SoundMan.PlaySound(LotaSound.Medium);
            }

            await TextArea.PrintLine();
            await Wait(2000);

            return true;
        }
    }
}
