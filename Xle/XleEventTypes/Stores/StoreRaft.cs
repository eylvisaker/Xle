using AgateLib.Geometry;
using AgateLib.Serialization.Xle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes
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
			if (type.Equals(typeof(XleMapTypes.Town)))
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

			if (CheckLoan(player, true))
				return true;

			// check to see if there are any rafts near the raft drop point
			skipRaft = CheckForNearbyRaft(player, skipRaft);

			g.AddBottom("** " + this.ShopName + " **", XleColor.Yellow);
			g.AddBottom("");

			if (skipRaft == false)
			{
				g.AddBottom("Want to buy a raft for " + raftCost.ToString() + " gold?");

				choice = XleCore.QuickMenu(theList, 3, 1);

				if (choice == 0)
				{
					// Purchase raft
					if (player.Spend(raftCost))
					{
						player.AddRaft(BuyRaftMap, BuyRaftPt.X, BuyRaftPt.Y);

						g.AddBottom("Raft purchased.");
						SoundMan.PlaySound(LotaSound.Sale);
						XleCore.Wait(1000);

						g.AddBottom("Board raft outside.");

						offerCoin = true;
					}
					else
					{
						g.AddBottom("Not enough gold.");
						SoundMan.PlaySound(LotaSound.Medium);
						XleCore.Wait(750);
					}
				}
			}

			if (skipRaft == true || choice == 1)
			{
				g.AddBottom("How about some climbing gear");
				g.AddBottom("for " + gearCost.ToString() + " gold?");
				g.AddBottom("");

				choice = XleCore.QuickMenu(theList, 3, 1);

				if (choice == 0)
				{
					if (player.Spend(gearCost))
					{
						g.AddBottom("Climbing gear purchased.");

						player.ItemCount(2, 1);
						offerCoin = true;

						SoundMan.PlaySound(LotaSound.Sale);
					}
					else
					{
						g.AddBottom("Not enough gold.");
						SoundMan.PlaySound(LotaSound.Medium);
					}
				}
				else if (choice == 1)
				{
					g.AddBottom("");
					g.AddBottom("Nothing Purchased.");

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
