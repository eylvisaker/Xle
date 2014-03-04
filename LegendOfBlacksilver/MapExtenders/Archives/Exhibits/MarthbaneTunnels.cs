using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
	class MarthbaneTunnels : LobExhibit
	{
		public MarthbaneTunnels()
			: base("Marthbane Tunnels", Coin.Emerald)
		{ }

		public override ExhibitIdentifier ExhibitIdentifier
		{
			get { return ExhibitIdentifier.MarthbaneTunnels; }
		}

		public override bool RequiresCoin(Player player)
		{
			if (HasBeenVisited(player))
				return false;

			return base.RequiresCoin(player);
		}

		public override void RunExhibit(Player player)
		{
			base.RunExhibit(player);

			XleCore.TextArea.PrintLine("Would you like to go");
			XleCore.TextArea.PrintLine("to Marthbane tunnels?");
			XleCore.TextArea.PrintLine();

			if (0 == XleCore.QuickMenuYesNo())
			{
				XleCore.ChangeMap(player, 4, 0);
			}
		}
	}
}
