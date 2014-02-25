using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;
using ERY.Xle.XleMapTypes.MuseumDisplays;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
	class AncientArtifact : LotaExhibit
	{
		public AncientArtifact() : base("Ancient Artifact", Coin.None) { }
		public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.AncientArtifact; } }

		public override string LongName
		{
			get { return "An ancient artifact"; }
		}
		public override string InsertCoinText
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
		public override bool StaticBeforeCoin
		{
			get { return false; }
		}
	}
}
