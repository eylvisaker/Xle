using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.MuseumDisplays
{
	class LostDisplays : Exhibit
	{
		public LostDisplays() : base("Lost Displays", Coin.Sapphire) { }
		public override ExhibitIdentifier ExhibitID { get { return ExhibitIdentifier.LostDisplays; } }
		public override string LongName
		{
			get
			{
				return "The lost displays";
			}
		}
	}
}
