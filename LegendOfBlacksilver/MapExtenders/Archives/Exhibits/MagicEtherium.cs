using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services.Implementation;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
    public class MagicEtherium : LobExhibit
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
			return Story.DrankEtherium;
		}

		public override void RunExhibit(Player unused)
		{
			base.RunExhibit(Player);

			TextArea.PrintLine();
			TextArea.PrintLine("Do you want to drink the etherium?");
			TextArea.PrintLine();

			if (0 == QuickMenu.QuickMenuYesNo())
			{
				TextArea.PrintLine();
				TextArea.PrintLine("You feel dizzy.");
				GameControl.Wait(1500);
				TextArea.PrintLine("The feeling passes.");

				Story.DrankEtherium = true;
			}
			else
				ReturnGem(Player);
		}
	}
}
