using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
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
