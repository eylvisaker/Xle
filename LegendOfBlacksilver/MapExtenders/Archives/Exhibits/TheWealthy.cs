using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
	class TheWealthy : LobExhibit
	{
		public TheWealthy()
			: base("The Wealthy", Coin.RedGarnet)
		{ }

		public override ExhibitIdentifier ExhibitIdentifier
		{
			get { return ExhibitIdentifier.TheWealthy; }
		}

		public override void RunExhibit(Player player)
		{
			base.RunExhibit(player);

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Do you want some gold?");
			XleCore.TextArea.PrintLine();
			
			if (0 == XleCore.QuickMenuYesNo())
			{
				int amount = (int)(400 + Math.Pow(player.Level, 1.35) + XleCore.random.Next(100));

				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("Gold + " + amount);
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine();

				player.Gold += amount;

				SoundMan.PlaySoundSync(LotaSound.VeryGood);
			}
		}
	}
}
