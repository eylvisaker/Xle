using ERY.Xle.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
	class IslandRetreat : LobExhibit
	{
		public IslandRetreat() : base("Island Retreat", Coin.BlueGem) { }

		public override ExhibitIdentifier ExhibitIdentifier
		{
			get { return ExhibitIdentifier.IslandRetreat; }
		}

		public override void PlayerXamine(Player player)
		{
			base.PlayerXamine(player);

			XleCore.ChangeMap(player, 1, 1, 0, 0);
		}
	}
}
