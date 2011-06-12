using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.MuseumDisplays
{
	class Information : Exhibit
	{
		public Information() : base("Information", Coin.None) { }
		public override int ExhibitID { get { return 0; } }
		public override string CoinString
		{
			get { return string.Empty; }
		}

	}
}
