using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Stores
{
	public class StoreMagic : StoreFront
	{
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

			theWindow[0] = ShopName;


			int i = 1;
			theWindow[i++] = "";
			SetColor(i, XleColor.Blue);
			theWindow[i++] = "General Purpose      Prices";
			theWindow[i++] = "";
			theWindow[i++] = "1. Magic flame        " + MagicPrice(1);
			theWindow[i++] = "2. Firebolt           " + MagicPrice(2);
			theWindow[i++] = "";
			SetColor(i, XleColor.Blue);
			theWindow[i++] = "Dungeon use only     Prices";
			theWindow[i++] = "";
			theWindow[i++] = "3. Befuddle spell     " + MagicPrice(3);
			theWindow[i++] = "4. Psycho strength    " + MagicPrice(4);
			theWindow[i++] = "5. Kill Flash         " + MagicPrice(5);
			theWindow[i++] = "";
			SetColor(i, XleColor.Blue);
			theWindow[i++] = "Outside use only     Prices";
			theWindow[i++] = "";
			theWindow[i++] = "6. Seek spell         " + MagicPrice(6);


			XleCore.TextArea.Clear();
			XleCore.TextArea.PrintLine("Make choice (hit 0 to cancel)");
			XleCore.TextArea.PrintLine();

			int choice = QuickMenu(new MenuItemList("0", "1", "2", "3", "4", "5", "6"), 2);

			if (choice == 0)
				return true;

			int maxCount = player.Gold / MagicPrice(choice);

			int purchaseCount = XleCore.ChooseNumber(maxCount);

			if (purchaseCount == 0)
				return true;




			return true;
		}

		int MagicPrice(int index)
		{
			int[] prices = { 32, 63, 152, 189, 379, 51 };

			return (int)(XleCore.MagicSpells[index].BasePrice * this.CostFactor);
		}
	}
}
