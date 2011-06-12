using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.MuseumDisplays
{
	class KnightsTest : Exhibit
	{
		public KnightsTest() : base("KnightsTest", Coin.Sapphire) { }
		public override int ExhibitID { get { return 11; } }
	}
}
