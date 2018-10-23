using AgateLib;

namespace Xle.Ancients.MapExtenders.Museum.MuseumDisplays
{
    [Transient, InjectProperties]
    public class Tapestry : LotaExhibit
    {
        public Tapestry() : base("A Tapestry", Coin.Amethyst) { }
        public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.Tapestry; } }
    }
}
