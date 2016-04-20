using AgateLib.Geometry;
using AgateLib.Serialization.Xle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services;
using ERY.Xle.Data;
using ERY.Xle.Services.Menus;

namespace ERY.Xle.XleEventTypes.Stores.Extenders
{
    public abstract class StoreEquipmentExtender : StoreFront
    {
        int LeftOffset;

        TextWindow titlePrompt = new TextWindow();
        TextWindow itemsPrompt = new TextWindow();
        TextWindow inventoryDisplay = new TextWindow();

        public IEquipmentPicker EquipmentPicker { get; set; }
        public XleData Data { get; set; }

        public new StoreEquipment TheEvent { get { return (StoreEquipment)base.TheEvent; } }

        public StoreEquipmentExtender()
        {
            Windows.Add(titlePrompt);
            Windows.Add(itemsPrompt);
            Windows.Add(inventoryDisplay);
        }

        protected abstract string StoreType { get; }

        protected override bool SpeakImpl()
        {
            MenuItemList theList = new MenuItemList("Buy", "Sell", "Neither");

            Title = TheEvent.ShopName;

            InitializeWindows();

            TextArea.Clear();
            int choice = QuickMenu(theList, 2, 0);
            Wait(1);

            if (choice == 0)
            {
                BuyItem();
            }
            else if (choice == 1)		// sell item
            {
                SellItem();
            }

            return true;
        }

        private void SellItem()
        {
            titlePrompt.Visible = false;

            TextArea.Clear();
            TextArea.PrintLine("      what will you sell me?");
            TextArea.PrintLine();

	        Equipment eq = PickItemToSell();

            if (eq == null)
            {
                NoTransactionMessage();
                return;
            }

            int sellPrice = ComputeSellPrice(eq.Price(Data));
            TextArea.PrintLine("I'll pay you exactly " + sellPrice + " gold");
            TextArea.PrintLine("for your " + eq.NameWithQuality(Data) + ".");
            TextArea.PrintLine();

            var choice = QuickMenuService.QuickMenuYesNo(true);

            if (choice == 1)
            {
                NoTransactionMessage();
                return;
            }

            TextArea.Clear();
            TextArea.PrintLine("It's a deal!");
            TextArea.PrintLine(eq.BaseName(Data) + " sold for " + sellPrice + " gold.");

            Player.RemoveEquipment(eq);

            Player.Gold += sellPrice;

            StoreSound(LotaSound.Sale);
            TextArea.PrintLine();
        }

        protected abstract Equipment PickItemToSell();

        private void NoTransactionMessage()
        {
            TextArea.Clear();
            TextArea.PrintLine("           no transaction.\n");
            StoreSound(LotaSound.Medium);
        }

        private int ComputeSellPrice(int retailPrice)
        {
            double factor = Math.Pow(Player.Attribute[Attributes.charm], .7) / 11;
            if (factor > 1) factor = 1;

            int result = (int)(retailPrice * factor);
            result = (int)(.8 * result);

            return result;
        }

        private void BuyItem()
        {
            itemsPrompt.Text = "Items               Prices";

            int[] itemList = new int[16];
            int[] qualList = new int[16];
            int[] priceList = new int[16];

            FillItems(Player.TimeQuality, itemList, qualList, priceList);

            StoreSound(LotaSound.Sale);

            int count = 0;

            for (int i = 1; i < 16 && itemList[i] > 0; i++)
            {
                count = i + 1;
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

            TextArea.PrintLine();
            TextArea.PrintLine("Make choice (hit 0 to cancel)");
            TextArea.PrintLine();

            int choice = QuickMenu(theList2, 2, 0);

            if (choice == 0)
            {
                TextArea.PrintLine();
                TextArea.PrintLine("Nothing purchased");
                TextArea.PrintLine();

                StoreSound(LotaSound.Medium);
            }
            else if (Player.Spend(priceList[choice]))
            {
                // spend the cash, if they have it
                if (AddItem(Player, itemList[choice], qualList[choice]))
                {
                    TextArea.PrintLine(ItemName(itemList[choice], qualList[choice]) + " purchased.");
                    TextArea.PrintLine();

                    StoreSound(LotaSound.Sale);
                }
                else
                {
                    Player.Gold += priceList[choice];
                    TextArea.PrintLine();
                    TextArea.PrintLine();
                    TextArea.PrintLine();
                    TextArea.PrintLine("No purchase.  You're");
                    TextArea.PrintLine("carrying too much.");
                }

            }
            else
            {
                TextArea.PrintLine("You're short on gold.");
                StoreSound(LotaSound.Medium);
            }
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
            var result = new List<int>();

            foreach (var it in stock)
            {
                if (Random.Next(256) >= 191 && result.Count > 0)
                {
                    continue;
                }

                result.Add(it);
            }

            return result;
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
            List<int> result = new List<int>();

            foreach (var item in stock)
            {
                if (item > maxItem)
                    break;

                result.Add(item);
            }

            // add the bottom item if there are none.
            if (result.Count == 0)
                result.Add(stock.Min());

            return result;
        }
        protected abstract int MaxItem(double timeQuality);

        protected abstract string ItemName(int itemIndex, int qualityIndex);

        protected abstract int ItemCost(int itemIndex, int quality);
    }

    public class StoreWeapon : StoreEquipmentExtender
    {
        protected override void InitializeColorScheme(ColorScheme cs)
        {
            cs.BackColor = XleColor.Brown;
            cs.FrameColor = XleColor.Orange;
            cs.FrameHighlightColor = XleColor.Yellow;
            cs.BorderColor = XleColor.Red;
        }

        protected override Equipment PickItemToSell()
        {
            return EquipmentPicker.PickWeapon(GameState, null, Color.FromArgb(0));
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
            double result = 10 + timeQuality / 600;

            if (result > 44) result = 44;

            return (int)result;
        }

        protected override string ItemName(int itemIndex, int qualityIndex)
        {
            return Data.GetWeaponName(itemIndex, qualityIndex);
        }

        protected override int ItemCost(int itemIndex, int quality)
        {
            return Data.WeaponCost(itemIndex, quality);
        }
    }
    public class StoreArmor : StoreEquipmentExtender
    {
        protected override void InitializeColorScheme(ColorScheme cs)
        {
            cs.BackColor = XleColor.Purple;
            cs.FrameColor = XleColor.Blue;
            cs.FrameHighlightColor = XleColor.Yellow;
            cs.BorderColor = XleColor.LightBlue;
        }

        protected override Equipment PickItemToSell()
        {
            return EquipmentPicker.PickArmor(GameState, null, Color.FromArgb(0));
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
            double result = 6 + timeQuality / 1133;

            if (result > 24) result = 24;

            return (int)result;
        }

        protected override string ItemName(int itemIndex, int qualityIndex)
        {
            return Data.GetArmorName(itemIndex, qualityIndex);
        }

        protected override int ItemCost(int itemIndex, int quality)
        {
            return Data.ArmorCost(itemIndex, quality);
        }
    }
}
