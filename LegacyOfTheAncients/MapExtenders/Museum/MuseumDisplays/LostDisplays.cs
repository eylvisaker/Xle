using AgateLib.Geometry;
using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
    public class LostDisplays : LotaExhibit
    {
        public LostDisplays() : base("Lost Displays", Coin.Sapphire) { }

        public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.LostDisplays; } }
        public override string LongName
        {
            get
            {
                return "The lost displays";
            }
        }

        public override Color TitleColor
        {
            get
            {
                return XleColor.Cyan;
            }
        }
    }
}
