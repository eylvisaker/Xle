using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.MuseumDisplays
{
	class Pegasus : Exhibit
	{
		public Pegasus() : base("Pegasus", Coin.Diamond) { }
		public override ExhibitIdentifier ExhibitID { get { return ExhibitIdentifier.Pegasus; } }
		public override string LongName
		{
			get { return "A flight of fancy"; }
		}

		public override bool StaticBeforeCoin
		{
			get
			{
				return false;
			}
		}
	}
}
