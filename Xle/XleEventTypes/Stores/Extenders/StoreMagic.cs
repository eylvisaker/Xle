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

			window.Location = new AgateLib.Geometry.Point(8, 2);

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
			get { return XleCore.Data.MagicSpells.Values; }
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
			Windows.AddRange(CreateStoreWindows());

			Title = TheEvent.ShopName;

			XleCore.TextArea.Clear();
			XleCore.TextArea.PrintLine("Make choice (hit 0 to cancel)");
			XleCore.TextArea.PrintLine();

			IEnumerable<MagicSpell> magicSpells = AvailableSpells;

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

			int cost = purchaseCount * MagicPrice(choice);

			if (cost > player.Gold)
			{
				NothingPurchased("You're short on gold.");
				return true;
			}

			player.Items[item.ItemID] += purchaseCount;
			player.Gold -= purchaseCount * MagicPrice(choice);

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

		int MagicPrice(int id)
		{
			return MagicPrice(XleCore.Data.MagicSpells[id]);
		}
		int MagicPrice(MagicSpell magicSpell)
		{
			return (int)(magicSpell.BasePrice * TheEvent.CostFactor);
		}
	}
}
