using AgateLib.Geometry;
using AgateLib.Serialization.Xle;
using ERY.Xle.Maps.XleMapTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Stores
{

	public class StoreRaft : Store
	{
		// map and coords that mark where a purchased raft shows up
		int mBuyRaftMap;
		Point mBuyRaftPt;

		protected override void WriteData(XleSerializationInfo info)
		{
			base.WriteData(info);

			info.Write("BuyRaftMap", mBuyRaftMap);
			info.Write("BuyRaftX", mBuyRaftPt.X);
			info.Write("BuyRaftY", mBuyRaftPt.Y);
		}
		protected override void ReadData(XleSerializationInfo info)
		{
			base.ReadData(info);

			mBuyRaftMap = info.ReadInt32("BuyRaftMap", 0);
			mBuyRaftPt.X = info.ReadInt32("BuyRaftX", 0);
			mBuyRaftPt.Y = info.ReadInt32("BuyRaftY", 0);
		}

		public int BuyRaftMap
		{
			get { return mBuyRaftMap; }
			set { mBuyRaftMap = value; }
		}
		public Point BuyRaftPt
		{
			get { return mBuyRaftPt; }
			set { mBuyRaftPt = value; }
		}

		public override bool AllowedOnMapType(XleMap type)
		{
			if (type.Equals(typeof(Town)))
				return true;
			else
				return false;
		}
		public override bool Speak(GameState state)
		{
			var player = state.Player;

			int choice = 0;
			int raftCost = (int)(400 * this.CostFactor);
			int gearCost = (int)(50 * this.CostFactor);
			MenuItemList theList = new MenuItemList("Yes", "No");
			bool skipRaft = false;
			bool offerCoin = false;

			if (IsLoanOverdue(state, true))
				return true;

			// check to see if there are any rafts near the raft drop point
			skipRaft = CheckForNearbyRaft(player, skipRaft);

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("** " + this.ShopName + " **", XleColor.Yellow);
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
						player.Rafts.Add(new RaftData(BuyRaftPt.X, BuyRaftPt.Y, BuyRaftMap));

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
				if (player.Rafts[i].MapNumber != BuyRaftMap)
					continue;

				int dist = Math.Abs(player.Rafts[i].X - BuyRaftPt.X) +
					Math.Abs(player.Rafts[i].Y - BuyRaftPt.Y);

				if (dist > 10)
					continue;

				skipRaft = true;
				break;
			}
			return skipRaft;
		}

	}
}
