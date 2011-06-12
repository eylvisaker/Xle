using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.MuseumDisplays
{
	class Fountain : Exhibit
	{
		public Fountain() : base("A Fountain", Coin.Jade) { }
		public override int ExhibitID { get { return 4; } }
		public override string LongName
		{
			get
			{
				return "Enchanted flower fountain";
			}
		}
	}
}
