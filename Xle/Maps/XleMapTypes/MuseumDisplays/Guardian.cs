using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.MuseumDisplays
{
	class Guardian : Exhibit
	{
		public Guardian() : base("Guardian", Coin.Turquoise) { }
		public override ExhibitIdentifier ExhibitID { get { return ExhibitIdentifier.Guardian; } }
	}
}
