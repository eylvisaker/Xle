using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services.Implementation;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
	class MorningStar : LobExhibit
	{
		public MorningStar()
			: base("Morning Star", Coin.Emerald)
		{ }

		public override ExhibitIdentifier ExhibitIdentifier
		{
			get { return ExhibitIdentifier.MorningStar; }
		}

		public override bool IsClosed(Player player)
		{
			return Lob.Story.ClosedMorningStar;
		}

		public override void RunExhibit(Player player)
		{
			base.RunExhibit(player);

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Do you want to borrow this item?");
			XleCore.TextArea.PrintLine();

			if (0 == XleCore.QuickMenuYesNo())
			{
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("Fight bravely.");

				SoundMan.PlaySoundSync(LotaSound.VeryGood);

				player.AddWeapon(9, 4);
				Lob.Story.ClosedMorningStar = true;

			}
			else
				ReturnGem(player);
		}
	}
}
