using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.MuseumDisplays
{
	class StonesWisdom : Exhibit
	{
		public StonesWisdom() : base("Stones of Wisdom", Coin.Amethyst) { }
		public override ExhibitIdentifier ExhibitID { get { return ExhibitIdentifier.StonesWisdom; } }

		public override bool StaticBeforeCoin
		{
			get
			{
				return false;
			}
		}


	}
}
