using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services.Implementation;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
    public class Mountains : LobExhibit
	{
		public Mountains()
			: base("Mountains", Coin.AmethystGem)
		{ }

		public override ExhibitIdentifier ExhibitIdentifier
		{
			get { return ExhibitIdentifier.Mountains; }
		}

		public override bool IsClosed(Player unused)
		{
			return Player.Items[LobItem.ClimbingGear] > 0;
		}

		public override void RunExhibit(Player unused)
		{
			base.RunExhibit(Player);

			TextArea.PrintLine();
			TextArea.PrintLine("Do you want a set?");
			TextArea.PrintLine();

			if (0 == QuickMenu.QuickMenuYesNo())
			{
				TextArea.PrintLine();
				TextArea.PrintLine();
				TextArea.PrintLine("Use it carefully.");

				SoundMan.PlaySoundSync(LotaSound.Good);

				Player.Items[LobItem.ClimbingGear] += 1;
			}
			else
				ReturnGem(Player);
		}
	}
}
