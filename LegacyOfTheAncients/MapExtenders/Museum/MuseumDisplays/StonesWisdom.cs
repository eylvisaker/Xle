using Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xle.Ancients.MapExtenders.Museum.MuseumDisplays
{
    public class StonesWisdom : LotaExhibit
    {
        public StonesWisdom() : base("Stones of Wisdom", Coin.Amethyst) { }
        public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.StonesWisdom; } }

        public override bool StaticBeforeCoin
        {
            get
            {
                return false;
            }
        }


    }
}
