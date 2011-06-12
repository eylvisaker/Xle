using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.MuseumDisplays
{
	class FourJewels : Exhibit
	{
		public FourJewels() : base("Four Jewels", Coin.Ruby) { }
		public override int ExhibitID { get { return 12; } }
	}
}
