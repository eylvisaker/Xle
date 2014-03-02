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

		public override void RunExhibit(Player player)
		{
			base.RunExhibit(player);

			player.Items[LobItem.RopeAndPulley] = 1;
		}
	}
}
