using AgateLib;
using Microsoft.Xna.Framework;

namespace Xle.Ancients.MapExtenders.Museum.MuseumDisplays
{
    [Transient, InjectProperties]
    public class LostDisplays : LotaExhibit
    {
        public LostDisplays() : base("Lost Displays", Coin.Sapphire) { }

        public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.LostDisplays; } }

        public override string LongName => "The lost displays";

        public override Color TitleColor => XleColor.Cyan;
    }
}
