using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
	class MagicEtherium : LobExhibit
	{
		public MagicEtherium()
			: base("Magic Etherium", Coin.AmethystGem)
		{ }

		public override ExhibitIdentifier ExhibitIdentifier
		{
			get { return ExhibitIdentifier.MagicEtherium; }
		}

		public override bool IsClosed(Player player)
		{
			return player.Story().DrankEtherium;
		}

		public override void RunExhibit(Player player)
		{
			base.RunExhibit(player);

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Do you want to drink the etherium?");
			XleCore.TextArea.PrintLine();

			if (0 == XleCore.QuickMenuYesNo())
			{
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("You feel dizzy.");
				XleCore.Wait(1500);
				XleCore.TextArea.PrintLine("The feeling passes.");

				player.Story().DrankEtherium = true;
			}
			else
				ReturnGem(player);
		}
	}
}
