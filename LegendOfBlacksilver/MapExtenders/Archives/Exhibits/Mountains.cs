using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services.Implementation;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
	class Mountains : LobExhibit
	{
		public Mountains()
			: base("Mountains", Coin.AmethystGem)
		{ }

		public override ExhibitIdentifier ExhibitIdentifier
		{
			get { return ExhibitIdentifier.Mountains; }
		}

		public override bool IsClosed(Player player)
		{
			return player.Items[LobItem.ClimbingGear] > 0;
		}

		public override void RunExhibit(Player player)
		{
			base.RunExhibit(player);

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Do you want a set?");
			XleCore.TextArea.PrintLine();

			if (0 == XleCore.QuickMenuYesNo())
			{
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("Use it carefully.");

				SoundMan.PlaySoundSync(LotaSound.Good);

				player.Items[LobItem.ClimbingGear] += 1;
			}
			else
				ReturnGem(player);
		}
	}
}
