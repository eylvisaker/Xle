using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.MuseumDisplays
{
	class StonesWisdom : Exhibit
	{
		public StonesWisdom() : base("Stones of Wisdom", Coin.Amethyst) { }
		public override int ExhibitID { get { return 8; } }
	}
}
