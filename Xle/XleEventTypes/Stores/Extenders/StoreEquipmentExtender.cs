using AgateLib.Geometry;
using AgateLib.Serialization.Xle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Stores.Extenders
{
	public abstract class StoreEquipmentExtender : StoreFrontExtender
	{
		Player player;
		int LeftOffset;
		
		TextWindow titlePrompt = new TextWindow();
		TextWindow itemsPrompt = new TextWindow();
		TextWindow inventoryDisplay = new TextWindow();

		public new StoreEquipment TheEvent { get { return (StoreEquipment)base.TheEvent; } }

		public StoreEquipmentExtender()
		{
			Windows.Add(titlePrompt);
			Windows.Add(itemsPrompt);
			Windows.Add(inventoryDisplay);
		}

		protected abstract string StoreType { get; }

		protected override bool SpeakImpl(GameState state)
		{
			var player = state.Player;

			if (IsLoanOverdue(state, true))
				return true;

			MenuItemList theList = new MenuItemList("Buy", "Sell", "Neither");

			this.player = player;

			Title = TheEvent.ShopName;

			InitializeWindows();

			int[] itemList = new int[16];
			int[] qualList = new int[16];
			int[] priceList = new int[16];

			FillItems(player.TimeQuality, itemList, qualList, priceList);

			XleCore.TextArea.Clear();
			int choice = QuickMenu(theList, 2, 0);
			Wait(1);

			if (choice == 0)
			{
				itemsPrompt.Text = "Items               Prices";

				StoreSound(LotaSound.Sale);

				int count = 0;

				for (int i = 1; i < 16 && itemList[i] > 0; i++)
				{
					count = i+1;
					var name = ItemName(itemList[i], qualList[i]);
					var price = priceList[i];

					AddItemToDisplay(i, name, price);
					Wait(1);
				}

				MenuItemList theList2 = new MenuItemList();

				for (int k = 0; k < count; k++)
				{
					theList2.Add(k.ToString());
				}

				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("Make choice (hit 0 to cancel)");
				XleCore.TextArea.PrintLine();

				choice = QuickMenu(theList2, 2, 0);

				if (choice == 0)
				{
					XleCore.TextArea.PrintLine();
					XleCore.TextArea.PrintLine("Nothing purchased");
					XleCore.TextArea.PrintLine();

					StoreSound(LotaSound.Medium);
				}
				else if (player.Spend(priceList[choice]))
				{
					// spend the cash, if they have it
					if (AddItem(player, itemList[choice], qualList[choice]))
					{
						XleCore.TextArea.PrintLine(ItemName(itemList[choice], qualList[choice]) + " purchased.");
						XleCore.TextArea.PrintLine();

						StoreSound(LotaSound.Sale);
					}
					else
					{
						player.Gold += priceList[choice];
						XleCore.TextArea.PrintLine();
						XleCore.TextArea.PrintLine();
						XleCore.TextArea.PrintLine();
						XleCore.TextArea.PrintLine("No purchase.  You're");
						XleCore.TextArea.PrintLine("carrying too much.");
					}

				}
				else
				{
					XleCore.TextArea.PrintLine("You're short on gold.");
					StoreSound(LotaSound.Medium);
				}
			}
			else if (choice == 1)		// sell item
			{

			}

			return true;
		}

		private void AddItemToDisplay(int index, string name, int price)
		{
			inventoryDisplay.WriteLine(string.Format(
				"{0}. {1}{2}{3}", index, name, new string(' ', 22 - name.Length), price));
		}

		private void InitializeWindows()
		{
			titlePrompt.Clear();
			itemsPrompt.Clear();
			inventoryDisplay.Clear();

			titlePrompt.Location = new Point(17, 2);
			titlePrompt.WriteLine(StoreType);

			itemsPrompt.Location = new Point(7, 4);

			inventoryDisplay.Location = new Point(4, 6);
		}

		protected abstract bool AddItem(Player player, int itemIndex, int qualityIndex);

		public List<int> AllowedItemTypes { get { return TheEvent.AllowedItemTypes; } }
		protected List<int> ItemStockThisTime { get; set; }

		protected List<int> DetermineCurrentStock(List<int> stock)
		{
			var retval = new List<int>();

			foreach (var it in stock)
			{
				if (XleCore.random.Next(256) >= 191 && retval.Count > 0)
				{
					continue;
				}

				retval.Add(it);
			}

			return retval;
		}


		void FillItems(double timeQuality,
			int[] itemList, int[] qualList, int[] priceList)
		{
			if (ItemStockThisTime == null)
			{
				ItemStockThisTime = DetermineCurrentStock(AllowedItemTypes);
			}

			int maxvalue = MaxItem(timeQuality);

			List<int> stock = DetermineStockFromTQ(ItemStockThisTime, maxvalue);
			stock.Reverse();

			for (int i = 1; i <= stock.Count && i <= 8; i++)
			{
				int item = stock[i - 1];
				int itemType = (int)(item / 5);
				int quality = (int)(item - itemType * 5);

				itemList[i] = itemType;
				qualList[i] = quality;

				priceList[i] = ItemCost(itemType, quality);
			}
		}

		protected List<int> DetermineStockFromTQ(List<int> stock, int maxItem)
		{
			List<int> retval = new List<int>();

			foreach (var item in stock)
			{
				if (item > maxItem)
					break;

				retval.Add(item);
			}

			// add the bottom item if there are none.
			if (retval.Count == 0)
				retval.Add(stock.Min());

			return retval;
		}
		protected abstract int MaxItem(double timeQuality);

		protected abstract string ItemName(int itemIndex, int qualityIndex);

		protected abstract int ItemCost(int itemIndex, int quality);
	}

	public class StoreWeapon : StoreEquipmentExtender
	{
		protected override void SetColorScheme(ColorScheme cs)
		{
			cs.BackColor = XleColor.Brown;
			cs.FrameColor = XleColor.Orange;
			cs.FrameHighlightColor = XleColor.Yellow;
			cs.BorderColor = XleColor.Red;
		}

		protected override string StoreType
		{
			get { return "Weapon"; }
		}

		protected override bool AddItem(Player player, int itemIndex, int qualityIndex)
		{
			return player.AddWeapon(itemIndex, qualityIndex);
		}

		protected override int MaxItem(double timeQuality)
		{
			double retval = 10 + timeQuality / 600;

			if (retval > 44) retval = 44;

			return (int)retval;
		}

		protected override string ItemName(int itemIndex, int qualityIndex)
		{
			return XleCore.GetWeaponName(itemIndex, qualityIndex);
		}

		protected override int ItemCost(int itemIndex, int quality)
		{
			return XleCore.WeaponCost(itemIndex, quality);
		}
	}
	public class StoreArmor : StoreEquipmentExtender
	{
		protected override void SetColorScheme(ColorScheme cs)
		{
			cs.BackColor = XleColor.Purple;
			cs.FrameColor = XleColor.Blue;
			cs.FrameHighlightColor = XleColor.Yellow;
			cs.BorderColor = XleColor.LightBlue;
		}

		protected override string StoreType
		{
			get { return "armor"; }
		}

		protected override bool AddItem(Player player, int itemIndex, int qualityIndex)
		{
			return player.AddArmor(itemIndex, qualityIndex);
		}

		protected override int MaxItem(double timeQuality)
		{
			double retval = 6 + timeQuality / 1133;

			if (retval > 24) retval = 24;

			return (int)retval;
		}

		protected override string ItemName(int itemIndex, int qualityIndex)
		{
			return XleCore.GetArmorName(itemIndex, qualityIndex);
		}

		protected override int ItemCost(int itemIndex, int quality)
		{
			return XleCore.ArmorCost(itemIndex, quality);
		}
	}
}
