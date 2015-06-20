﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services.Implementation;
using ERY.Xle.Services;

namespace ERY.Xle.XleEventTypes.Stores.Extenders
{
    public class StoreRaftExtender : StoreExtender
    {
        public IQuickMenu QuickMenu { get; set; }
        public XleSystemState systemState { get; set; }

        public new StoreRaft TheEvent { get { return (StoreRaft)base.TheEvent; } }

        protected override bool SpeakImpl(GameState state)
        {
            int choice = 0;
            int raftCost = (int)(400 * TheEvent.CostFactor);
            int gearCost = (int)(50 * TheEvent.CostFactor);
            MenuItemList theList = new MenuItemList("Yes", "No");
            bool skipRaft = false;
            bool offerCoin = false;

            if (IsLoanOverdue())
            {
                StoreDeclinePlayer();
                return true;
            }
            // check to see if there are any rafts near the raft drop point
            skipRaft = CheckForNearbyRaft(skipRaft);

            TextArea.PrintLine();
            TextArea.PrintLine("** " + TheEvent.ShopName + " **", XleColor.Yellow);
            TextArea.PrintLine();

            if (skipRaft == false)
            {
                TextArea.PrintLine("Want to buy a raft for " + raftCost.ToString() + " gold?");

                choice = QuickMenu.QuickMenu(theList, 3, 1);

                if (choice == 0)
                {
                    // Purchase raft
                    if (Player.Spend(raftCost))
                    {
                        Player.Rafts.Add(new RaftData(TheEvent.BuyRaftPt.X, TheEvent.BuyRaftPt.Y, TheEvent.BuyRaftMap));

                        TextArea.PrintLine("Raft purchased.");
                        SoundMan.PlaySound(LotaSound.Sale);
                        GameControl.Wait(1000);

                        TextArea.PrintLine("Board raft outside.");

                        offerCoin = true;
                    }
                    else
                    {
                        TextArea.PrintLine("Not enough gold.");
                        SoundMan.PlaySound(LotaSound.Medium);
                        GameControl.Wait(750);
                    }
                }
            }

            if (skipRaft == true || choice == 1)
            {
                TextArea.PrintLine("How about some climbing gear");
                TextArea.PrintLine("for " + gearCost.ToString() + " gold?");
                TextArea.PrintLine();

                choice = QuickMenu.QuickMenu(theList, 3, 1);

                if (choice == 0)
                {
                    if (Player.Spend(gearCost))
                    {
                        TextArea.PrintLine("Climbing gear purchased.");

                        Player.Items[ClimbingGearItemId] += 1;
                        offerCoin = true;

                        SoundMan.PlaySound(LotaSound.Sale);
                    }
                    else
                    {
                        TextArea.PrintLine("Not enough gold.");
                        SoundMan.PlaySound(LotaSound.Medium);
                    }
                }
                else if (choice == 1)
                {
                    TextArea.PrintLine();
                    TextArea.PrintLine("Nothing Purchased.");

                    SoundMan.PlaySound(LotaSound.Medium);
                }

                XleCore.Wait(750);

            }

            if (offerCoin)
                CheckOfferMuseumCoin(Player);

            return true;
        }

        private int ClimbingGearItemId
        {
            get { return systemState.Factory.ClimbingGearItemID; }
        }

        private bool CheckForNearbyRaft(bool skipRaft)
        {
            for (int i = 0; i < Player.Rafts.Count; i++)
            {
                if (Player.Rafts[i].MapNumber != TheEvent.BuyRaftMap)
                    continue;

                int dist = Math.Abs(Player.Rafts[i].X - TheEvent.BuyRaftPt.X) +
                    Math.Abs(Player.Rafts[i].Y - TheEvent.BuyRaftPt.Y);

                if (dist > 10)
                    continue;

                skipRaft = true;
                break;
            }
            return skipRaft;
        }

    }
}
