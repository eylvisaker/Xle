using ERY.Xle.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
	class IslandCaverns : Exhibit
	{
		public IslandCaverns() : base("Island Caverns", Coin.Sapphire) { }

		public override ExhibitIdentifier ExhibitID
		{
			get { return (ExhibitIdentifier)0x0d; }
		}
		public override string LongName
		{
			get
			{
				return base.LongName;
			}
		}

		public override void PlayerXamine(Player player)
		{
			base.PlayerXamine(player);

			XleCore.ChangeMap(player, 1, 1, 0, 0);
		}
	}
}
