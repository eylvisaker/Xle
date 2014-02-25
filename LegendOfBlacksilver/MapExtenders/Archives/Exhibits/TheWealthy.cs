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
	}
}
