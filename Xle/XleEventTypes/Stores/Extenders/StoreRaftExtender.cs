using AgateLib;
using System;
using System.Threading.Tasks;
using Xle.Services.Menus;
using Xle.Services.XleSystem;

namespace Xle.XleEventTypes.Stores.Extenders
{
    [Transient("StoreRaft")]
    public class StoreRaftExtender : StoreExtender
    {
        public IQuickMenu QuickMenu { get; set; }
        public XleSystemState systemState { get; set; }

        public new StoreRaft TheEvent { get { return (StoreRaft)base.TheEvent; } }

        protected override async Task<bool> SpeakImplAsync()
        {
            int choice = 0;
            int raftCost = (int)(400 * TheEvent.CostFactor);
            int gearCost = (int)(50 * TheEvent.CostFactor);
            MenuItemList theList = new MenuItemList("Yes", "No");
            bool skipRaft = false;
            bool offerCoin = false;

            if (IsLoanOverdue())
            {
                await StoreDeclinePlayer();
                return true;
            }
            // check to see if there are any rafts near the raft drop point
            skipRaft = CheckForNearbyRaft(skipRaft);

            await TextArea.PrintLine();
            await TextArea.PrintLine("** " + TheEvent.ShopName + " **", XleColor.Yellow);
            await TextArea.PrintLine();

            if (skipRaft == false)
            {
                await TextArea.PrintLine("Want to buy a raft for " + raftCost.ToString() + " gold?");

                choice = await QuickMenu.QuickMenu(theList, 3, 1);

                if (choice == 0)
                {
                    // Purchase raft
                    if (Player.Spend(raftCost))
                    {
                        Player.Rafts.Add(new RaftData(TheEvent.BuyRaftPt.X, TheEvent.BuyRaftPt.Y, TheEvent.BuyRaftMap));

                        await TextArea.PrintLine("Raft purchased.");
                        SoundMan.PlaySound(LotaSound.Sale);
                        await GameControl.WaitAsync(1000);

                        await TextArea.PrintLine("Board raft outside.");

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
                await TextArea.PrintLine("How about some climbing gear");
                await TextArea.PrintLine("for " + gearCost.ToString() + " gold?");
                await TextArea.PrintLine();

                choice = await QuickMenu.QuickMenu(theList, 3, 1);

                if (choice == 0)
                {
                    if (Player.Spend(gearCost))
                    {
                        await TextArea.PrintLine("Climbing gear purchased.");

                        Player.Items[ClimbingGearItemId] += 1;
                        offerCoin = true;

                        SoundMan.PlaySound(LotaSound.Sale);
                    }
                    else
                    {
                        await TextArea.PrintLine("Not enough gold.");
                        SoundMan.PlaySound(LotaSound.Medium);
                    }
                }
                else if (choice == 1)
                {
                    await TextArea.PrintLine();
                    await TextArea.PrintLine("Nothing Purchased.");

                    SoundMan.PlaySound(LotaSound.Medium);
                }

                await GameControl.WaitAsync(750);

            }

            if (offerCoin)
                await CheckOfferMuseumCoin(Player);

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
