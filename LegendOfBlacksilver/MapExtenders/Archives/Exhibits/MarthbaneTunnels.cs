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
	}
}
