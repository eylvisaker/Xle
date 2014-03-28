using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Stores.Extenders
{
	class StoreRaftExtender : StoreExtender
	{
		public new StoreRaft TheEvent { get { return (StoreRaft)base.TheEvent; } }

		public override bool Speak(GameState state)
		{
			var player = state.Player;

			int choice = 0;
			int raftCost = (int)(400 * TheEvent.CostFactor);
			int gearCost = (int)(50 * TheEvent.CostFactor);
			MenuItemList theList = new MenuItemList("Yes", "No");
			bool skipRaft = false;
			bool offerCoin = false;

			if (IsLoanOverdue(state, true))
				return true;

			// check to see if there are any rafts near the raft drop point
			skipRaft = CheckForNearbyRaft(player, skipRaft);

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("** " + TheEvent.ShopName + " **", XleColor.Yellow);
			XleCore.TextArea.PrintLine();

			if (skipRaft == false)
			{
				XleCore.TextArea.PrintLine("Want to buy a raft for " + raftCost.ToString() + " gold?");

				choice = XleCore.QuickMenu(theList, 3, 1);

				if (choice == 0)
				{
					// Purchase raft
					if (player.Spend(raftCost))
					{
						player.Rafts.Add(new RaftData(TheEvent.BuyRaftPt.X, TheEvent.BuyRaftPt.Y, TheEvent.BuyRaftMap));

						XleCore.TextArea.PrintLine("Raft purchased.");
						SoundMan.PlaySound(LotaSound.Sale);
						XleCore.Wait(1000);

						XleCore.TextArea.PrintLine("Board raft outside.");

						offerCoin = true;
					}
					else
					{
						XleCore.TextArea.PrintLine("Not enough gold.");
						SoundMan.PlaySound(LotaSound.Medium);
						XleCore.Wait(750);
					}
				}
			}

			if (skipRaft == true || choice == 1)
			{
				XleCore.TextArea.PrintLine("How about some climbing gear");
				XleCore.TextArea.PrintLine("for " + gearCost.ToString() + " gold?");
				XleCore.TextArea.PrintLine();

				choice = XleCore.QuickMenu(theList, 3, 1);

				if (choice == 0)
				{
					if (player.Spend(gearCost))
					{
						XleCore.TextArea.PrintLine("Climbing gear purchased.");

						player.Items[XleCore.Factory.ClimbingGearItemID] += 1;
						offerCoin = true;

						SoundMan.PlaySound(LotaSound.Sale);
					}
					else
					{
						XleCore.TextArea.PrintLine("Not enough gold.");
						SoundMan.PlaySound(LotaSound.Medium);
					}
				}
				else if (choice == 1)
				{
					XleCore.TextArea.PrintLine();
					XleCore.TextArea.PrintLine("Nothing Purchased.");

					SoundMan.PlaySound(LotaSound.Medium);
				}

				XleCore.Wait(750);

			}

			if (offerCoin)
				CheckOfferMuseumCoin(player);

			return true;
		}

		private bool CheckForNearbyRaft(Player player, bool skipRaft)
		{
			for (int i = 0; i < player.Rafts.Count; i++)
			{
				if (player.Rafts[i].MapNumber != TheEvent.BuyRaftMap)
					continue;

				int dist = Math.Abs(player.Rafts[i].X - TheEvent.BuyRaftPt.X) +
					Math.Abs(player.Rafts[i].Y - TheEvent.BuyRaftPt.Y);

				if (dist > 10)
					continue;

				skipRaft = true;
				break;
			}
			return skipRaft;
		}

	}
}
