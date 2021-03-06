﻿using AgateLib;
using Microsoft.Xna.Framework;

namespace Xle.Ancients.MapExtenders.Museum.MuseumDisplays
{
    [Transient, InjectProperties]
    public class AncientArtifact : LotaExhibit
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

        public override bool IsClosed
        {
            get { return true; }
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
