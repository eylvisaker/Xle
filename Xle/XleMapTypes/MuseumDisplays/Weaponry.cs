using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.MuseumDisplays
{
	class Weaponry : Exhibit
	{
		public Weaponry() : base("Weaponry", Coin.Jade) { }
		public override int ExhibitID { get { return 2; } }
		public override string LongName
		{
			get
			{
				return "The ancient art of weaponry";
			}
		}
	}
	
}
