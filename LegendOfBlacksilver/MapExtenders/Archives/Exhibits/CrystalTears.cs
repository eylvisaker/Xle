using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
	class CrystalTears : LobExhibit
	{
		public CrystalTears()
			: base("Crystal Tears", Coin.BlackOpal)
		{ }

		public override ExhibitIdentifier ExhibitIdentifier
		{
			get { return ExhibitIdentifier.CrystalTears; }
		}

		public override void RunExhibit(Player player)
		{
			base.RunExhibit(player);

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Do you want to borrow them?");
			XleCore.TextArea.PrintLine();

			if (0 == XleCore.QuickMenuYesNo())
			{
				player.Items[LobItem.DragonTear] += 2;

				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("You receive two dragon's tears.");

				SoundMan.PlaySoundSync(LotaSound.VeryGood);
			}
			else
			{
				ReturnGem(player);
			}
		}
	}
}
