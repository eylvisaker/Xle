using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.MuseumDisplays
{
	class HerbOfLife : Exhibit
	{
		public HerbOfLife() : base("Herb of life", Coin.Topaz) { }
		public override int ExhibitID { get { return 6; } }
		public override string LongName
		{
			get
			{
				return "The herb of life";
			}
		}
	}
}
