using AgateLib;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xle.Data;
using Xle.Services.Menus;

namespace Xle.XleEventTypes.Stores.Extenders
{
    public abstract class StoreEquipmentExtender : StoreFront
    {
        private int LeftOffset;
        private TextWindow titlePrompt = new TextWindow();
        private TextWindow itemsPrompt = new TextWindow();
        private TextWindow inventoryDisplay = new TextWindow();

        public IEquipmentPicker EquipmentPicker { get; set; }
        public XleData Data { get; set; }

        public new StoreEquipment TheEvent { get { return (StoreEquipment)base.TheEvent; } }

        public StoreEquipmentExtender()
        {
            Screen.AddWindow(titlePrompt);
            Screen.AddWindow(itemsPrompt);
            Screen.AddWindow(inventoryDisplay);
        }

        protected abstract string StoreType { get; }

        protected override async Task<bool> SpeakImplAsync()
        {
            MenuItemList theList = new MenuItemList("Buy", "Sell", "Neither");

            Title = TheEvent.ShopName;

            InitializeWindows();

            TextArea.Clear();
            int choice = await QuickMenu(theList, 2, 0);
            await Wait(1);

            if (choice == 0)
            {
                await BuyItem();
            }
            else if (choice == 1)		// sell item
            {
                await SellItem();
            }

            return true;
        }

        private async Task SellItem()
        {
            titlePrompt.Visible = false;

            TextArea.Clear();
            await TextArea.PrintLine("      what will you sell me?");
            await TextArea.PrintLine();

            Equipment eq = await PickItemToSell();

            if (eq == null)
            {
                await NoTransactionMessage();
                return;
            }

            int sellPrice = ComputeSellPrice(eq.Price(Data));
            await TextArea.PrintLine("I'll pay you exactly " + sellPrice + " gold");
            await TextArea.PrintLine("for your " + eq.NameWithQuality(Data) + ".");
            await TextArea.PrintLine();

            var choice = await QuickMenuService.QuickMenuYesNo(true);

            if (choice == 1)
            {
                await NoTransactionMessage();
                return;
            }

            TextArea.Clear();
            await TextArea.PrintLine("It's a deal!");
            await TextArea.PrintLine(eq.BaseName(Data) + " sold for " + sellPrice + " gold.");

            Player.RemoveEquipment(eq);

            Player.Gold += sellPrice;

            await StoreSound(LotaSound.Sale);
            await TextArea.PrintLine();
        }

        protected abstract Task<Equipment> PickItemToSell();

        private async Task NoTransactionMessage()
        {
            TextArea.Clear();
            await TextArea.PrintLine("           no transaction.\n");
            await StoreSound(LotaSound.Medium);
        }

        private int ComputeSellPrice(int retailPrice)
        {
            double factor = Math.Pow(Player.Attribute[Attributes.charm], .7) / 11;
            if (factor > 1) factor = 1;

            int result = (int)(retailPrice * factor);
            result = (int)(.8 * result);

            return result;
        }

        private async Task BuyItem()
        {
            itemsPrompt.Text = "Items               Prices";

            int[] itemList = new int[16];
            int[] qualList = new int[16];
            int[] priceList = new int[16];

            FillItems(Player.TimeQuality, itemList, qualList, priceList);

            await StoreSound(LotaSound.Sale);

            int count = 0;

            for (int i = 1; i < 16 && itemList[i] > 0; i++)
            {
                count = i + 1;
                var name = ItemName(itemList[i], qualList[i]);
                var price = priceList[i];

                AddItemToDisplay(i, name, price);
                await Wait(1);
            }

            MenuItemList theList2 = new MenuItemList();

            for (int k = 0; k < count; k++)
            {
                theList2.Add(k.ToString());
            }

            await TextArea.PrintLine();
            await TextArea.PrintLine("Make choice (hit 0 to cancel)");
            await TextArea.PrintLine();

            int choice = await QuickMenu(theList2, 2, 0);

            if (choice == 0)
            {
                await TextArea.PrintLine();
                await TextArea.PrintLine("Nothing purchased");
                await TextArea.PrintLine();

                await StoreSound(LotaSound.Medium);
            }
            else if (Player.Spend(priceList[choice]))
            {
                // spend the cash, if they have it
                if (AddItem(Player, itemList[choice], qualList[choice]))
                {
                    await TextArea.PrintLine(ItemName(itemList[choice], qualList[choice]) + " purchased.");
                    await TextArea.PrintLine();

                    await StoreSound(LotaSound.Sale);
                }
                else
                {
                    Player.Gold += priceList[choice];
                    await TextArea.PrintLine();
                    await TextArea.PrintLine();
                    await TextArea.PrintLine();
                    await TextArea.PrintLine("No purchase.  You're");
                    await TextArea.PrintLine("carrying too much.");
                }

            }
            else
            {
                await TextArea.PrintLine("You're short on gold.");
                await StoreSound(LotaSound.Medium);
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

        private void FillItems(double timeQuality,
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
                int itemType = item / 5;
                int quality = item - itemType * 5;

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

    [Transient("StoreWeapon")]
    public class StoreWeapon : StoreEquipmentExtender
    {
        protected override void InitializeColorScheme(ColorScheme cs)
        {
            cs.BackColor = XleColor.Brown;
            cs.FrameColor = XleColor.Orange;
            cs.FrameHighlightColor = XleColor.Yellow;
            cs.BorderColor = XleColor.Red;
        }

        protected override async Task<Equipment> PickItemToSell()
        {
            return await EquipmentPicker.PickWeapon(GameState, null, XleColor.Black);
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

    [Transient("StoreArmor")]
    public class StoreArmor : StoreEquipmentExtender
    {
        protected override void InitializeColorScheme(ColorScheme cs)
        {
            cs.BackColor = XleColor.Purple;
            cs.FrameColor = XleColor.Blue;
            cs.FrameHighlightColor = XleColor.Yellow;
            cs.BorderColor = XleColor.LightBlue;
        }

        protected override async Task<Equipment> PickItemToSell()
        {
            return await EquipmentPicker.PickArmor(GameState, null, XleColor.Black);
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
