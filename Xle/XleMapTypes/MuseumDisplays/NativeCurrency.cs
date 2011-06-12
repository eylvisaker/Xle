using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.MuseumDisplays
{
	class NativeCurrency : Exhibit
	{
		public NativeCurrency() : base("Native Currency", Coin.Topaz) { }
		public override int ExhibitID { get { return 7; } }
	}
}
