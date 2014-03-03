using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
	class StormingGear : LobExhibit
	{
		public StormingGear()
			: base("Storming Gear", Coin.RedGarnet)
		{ }

		public override ExhibitIdentifier ExhibitIdentifier
		{
			get { return ExhibitIdentifier.StormingGear; }
		}

		public override bool IsClosed(Player player)
		{
			return player.Items[LobItem.RopeAndPulley] > 0;
		}
		public override void RunExhibit(Player player)
		{
			base.RunExhibit(player);

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Do you want to borrow this gear?");
			XleCore.TextArea.PrintLine();

			if (0 == XleCore.QuickMenuYesNo())
			{
				player.Items[LobItem.RopeAndPulley] += 1;

				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("The equipment is now");
				XleCore.TextArea.PrintLine("in your possession.");
			}
			else
			{
				ReturnGem(player);
			}
		}

	}
}
