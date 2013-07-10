using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace ERY.Xle.XleMapTypes.MuseumDisplays
{
	class AncientArtifact : Exhibit
	{
		public AncientArtifact() : base("Ancient Artifact", Coin.None) { }
		public override ExhibitIdentifier ExhibitID { get { return ExhibitIdentifier.AncientArtifact; } }

		public override string LongName
		{
			get { return "An ancient artifact"; }
		}
		public override string CoinString
		{
			get { return string.Empty; }
		}
		public override bool IsClosed(Player player)
		{
			return true;
		}
		public override Color ExhibitColor
		{
			get { return XleColor.Cyan; }
		}
	}
}
