using AgateLib.Geometry;
using ERY.Xle.Maps;
using ERY.Xle.XleEventTypes.Stores.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Stores
{
	public class StoreMagic : StoreFront
	{
		protected override void AfterReadData()
		{
			ExtenderName = "StoreMagic";
		}

		protected new StoreMagicExtender Extender { get; set; }

		protected override XleEventTypes.Extenders.EventExtender CreateExtenderImpl(XleMap map)
		{
			Extender = map.CreateEventExtender<StoreMagicExtender>(this);
			base.Extender = Extender;

			return Extender;
		}
		protected override void SetColorScheme(ColorScheme cs)
		{
			cs.BorderColor = XleColor.Purple;
			cs.BackColor = XleColor.LightBlue;
			cs.FrameColor = XleColor.Cyan;
			cs.FrameHighlightColor = XleColor.Yellow;
			cs.TextColor = XleColor.Cyan;
			cs.TitleColor = XleColor.White;
			cs.TextAreaBackColor = XleColor.Blue;
		}

		protected override bool SpeakImpl(GameState state)
		{
			var player = state.Player;

			if (IsLoanOverdue(state, true))
				return true;

			this.player = player;

			LeftOffset = 7;

			Windows.Clear();
			Windows.AddRange(Extender.CreateStoreWindows());

			Title = ShopName;

			XleCore.TextArea.Clear();
			XleCore.TextArea.PrintLine("Make choice (hit 0 to cancel)");
			XleCore.TextArea.PrintLine();

			IEnumerable<MagicSpell> magicSpells = Extender.AvailableSpells;

			int choice = QuickMenu(MenuItemList.Numbers(0, magicSpells.Count()), 2);

			if (choice == 0)
			{
				NothingPurchased("Nothing purchased.");
				return true;
			}

			var item = magicSpells.ToArray()[choice - 1];

			int maxCarry = item.MaxCarry - player.Items[item.ItemID];
			int maxAfford = player.Gold / MagicPrice(item);
			int maxPurchase = Math.Min(maxCarry, maxAfford);

			if (maxAfford <= 0)
			{
				NothingPurchased("You can't afford any " + item.PluralName + ".");
				return true;
			}

			if (XleCore.Options.EnhancedUserInterface)
			{
				if (maxCarry == 0)
				{
					NothingPurchased("You can't buy any more " + item.PluralName + ".");
					return true;
				}
			}
			else
				maxPurchase = maxAfford;

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Purchase how many " + item.PluralName + "?");

			int purchaseCount = XleCore.ChooseNumber(maxPurchase);

			if (purchaseCount == 0)
			{
				NothingPurchased("Nothing purchased.");
				return true;
			}

			if (player.Items[item.ItemID] + purchaseCount > item.MaxCarry)
			{
				NothingPurchased("You can't buy this many.");
				return true;
			}

			int cost = purchaseCount * Extender.MagicPrice(choice);

			if (cost > player.Gold)
			{
				NothingPurchased("You're short on gold.");
				return true;
			}

			player.Items[item.ItemID] += purchaseCount;
			player.Gold -= purchaseCount * Extender.MagicPrice(choice);

			XleCore.TextArea.Clear();
			XleCore.TextArea.PrintLine(" " + purchaseCount.ToString() + " " +
				((purchaseCount != 1) ? item.PluralName : item.Name) + " purchased.");
			XleCore.TextArea.PrintLine();

			StoreSound(LotaSound.Sale);

			return true;
		}

		private void NothingPurchased(string message)
		{
			XleCore.TextArea.Clear();
			XleCore.TextArea.PrintLine(message);

			StoreSound(LotaSound.Medium);
		}


		public int MagicPrice(MagicSpell magicSpell)
		{
			return (int)(magicSpell.BasePrice * CostFactor);
		}
	}
}
