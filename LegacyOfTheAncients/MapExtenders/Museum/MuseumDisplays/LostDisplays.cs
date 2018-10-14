using Microsoft.Xna.Framework;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
    public class LostDisplays : LotaExhibit
    {
        public LostDisplays() : base("Lost Displays", Coin.Sapphire) { }

        public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.LostDisplays; } }

        public override string LongName => "The lost displays";

        public override Color TitleColor => XleColor.Cyan;
    }
}
