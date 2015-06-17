using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services.Implementation;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
	class Blacksmith : LobExhibit
	{
		public Blacksmith()
			: base("Blacksmith", Coin.WhiteDiamond)
		{ }

		public override ExhibitIdentifier ExhibitIdentifier
		{
			get { return ExhibitIdentifier.Blacksmith; }
		}

		public override bool IsClosed(Player player)
		{
			return Lob.Story.ProcuredSteelHammer;
		}

		public override void RunExhibit(Player player)
		{
			base.RunExhibit(player);

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Do you want to have it?");
			XleCore.TextArea.PrintLine();

			if (0 == XleCore.QuickMenuYesNo())
			{
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("It is yours.");

				SoundMan.PlaySoundSync(LotaSound.Good);

				player.Items[LobItem.SteelHammer] = 1;

				Lob.Story.ProcuredSteelHammer = true;
			}
			else
				ReturnGem(player);
		}
	}
}
