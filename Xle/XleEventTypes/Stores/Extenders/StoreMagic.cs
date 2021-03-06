﻿using AgateLib;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xle.Data;

namespace Xle.XleEventTypes.Stores.Extenders
{
    [Transient("StoreMagic")]
    public class StoreMagic : StoreFront
    {
        public XleData Data { get; set; }
        public XleOptions Options { get; set; }

        public virtual int GetItemValue(int choice)
        {
            throw new NotImplementedException();
        }

        public virtual int GetMaxCarry(int choice)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<TextWindow> CreateStoreWindows()
        {
            yield return CreateWindow();
        }

        private TextWindow CreateWindow()
        {
            TextWindow window = new TextWindow();

            window.Location = new Point(8, 2);

            window.WriteLine("General Purpose      Prices", XleColor.Blue);
            window.WriteLine("");
            window.WriteLine("1. Magic flame        " + MagicPrice(1));
            window.WriteLine("2. Firebolt           " + MagicPrice(2));
            window.WriteLine("");
            window.WriteLine("Dungeon use only     Prices", XleColor.Blue);
            window.WriteLine("");
            window.WriteLine("3. Befuddle spell     " + MagicPrice(3));
            window.WriteLine("4. Psycho strength    " + MagicPrice(4));
            window.WriteLine("5. Kill Flash         " + MagicPrice(5));
            window.WriteLine("");
            window.WriteLine("Outside use only     Prices", XleColor.Blue);
            window.WriteLine("");
            window.WriteLine("6. Seek spell         " + MagicPrice(6));
            return window;
        }

        public virtual IEnumerable<MagicSpell> AvailableSpells
        {
            get { return Data.MagicSpells.Values; }
        }
        protected override void InitializeColorScheme(ColorScheme cs)
        {
            cs.BorderColor = XleColor.Purple;
            cs.BackColor = XleColor.LightBlue;
            cs.FrameColor = XleColor.Cyan;
            cs.FrameHighlightColor = XleColor.Yellow;
            cs.TextColor = XleColor.Cyan;
            cs.TitleColor = XleColor.White;
            cs.TextAreaBackColor = XleColor.Blue;
        }

        protected override async Task<bool> SpeakImplAsync()
        {
            Screen.ClearWindows();

            foreach(var window in CreateStoreWindows())
            {
                Screen.AddWindow(window);
            }

            Title = TheEvent.ShopName;

            TextArea.Clear();
            await TextArea.PrintLine("Make choice (hit 0 to cancel)");
            await TextArea.PrintLine();

            IEnumerable<MagicSpell> magicSpells = AvailableSpells;

            int choice = await QuickMenu(MenuItemList.Numbers(0, magicSpells.Count()), 2);

            if (choice == 0)
            {
                await NothingPurchased("Nothing purchased.");
                return true;
            }

            var item = magicSpells.ToArray()[choice - 1];

            int maxCarry = item.MaxCarry - Player.Items[item.ItemID];
            int maxAfford = Player.Gold / MagicPrice(item);
            int maxPurchase = Math.Min(maxCarry, maxAfford);

            if (maxAfford <= 0)
            {
                await NothingPurchased("You can't afford any " + item.PluralName + ".");
                return true;
            }

            if (Options.EnhancedUserInterface)
            {
                if (maxCarry == 0)
                {
                    await NothingPurchased("You can't buy any more " + item.PluralName + ".");
                    return true;
                }
            }
            else
                maxPurchase = maxAfford;

     await        TextArea.PrintLine();
            await TextArea.PrintLine("Purchase how many " + item.PluralName + "?");

            int purchaseCount = await ChooseNumber(maxPurchase);

            if (purchaseCount == 0)
            {
                await NothingPurchased("Nothing purchased.");
                return true;
            }

            if (Player.Items[item.ItemID] + purchaseCount > item.MaxCarry)
            {
                await NothingPurchased("You can't buy this many.");
                return true;
            }

            int cost = purchaseCount * MagicPrice(choice);

            if (cost > Player.Gold)
            {
                await NothingPurchased("You're short on gold.");
                return true;
            }

            Player.Items[item.ItemID] += purchaseCount;
            Player.Gold -= purchaseCount * MagicPrice(choice);

            TextArea.Clear();
            await TextArea.PrintLine(" " + purchaseCount.ToString() + " " +
                ((purchaseCount != 1) ? item.PluralName : item.Name) + " purchased.");
            await TextArea.PrintLine();

            await StoreSound(LotaSound.Sale);

            return true;
        }

        private async Task NothingPurchased(string message)
        {
            TextArea.Clear();
            await TextArea.PrintLine(message);

            await StoreSound(LotaSound.Medium);
        }

        private int MagicPrice(int id)
        {
            return MagicPrice(Data.MagicSpells[id]);
        }

        private int MagicPrice(MagicSpell magicSpell)
        {
            return (int)(magicSpell.BasePrice * TheEvent.CostFactor);
        }
    }
}
