using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.MuseumDisplays
{
	class Tapestry : Exhibit
	{
		public Tapestry() : base("A Tapestry", Coin.Amethyst) { }
		public override int ExhibitID { get { return 9; } }
	}
}
