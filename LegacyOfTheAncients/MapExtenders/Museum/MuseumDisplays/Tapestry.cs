using ERY.Xle.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
	class Tapestry : LotaExhibit
	{
		public Tapestry() : base("A Tapestry", Coin.Amethyst) { }
		public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.Tapestry; } }
	}
}
