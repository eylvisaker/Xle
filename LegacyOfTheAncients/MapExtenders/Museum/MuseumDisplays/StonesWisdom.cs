using AgateLib;

namespace Xle.Ancients.MapExtenders.Museum.MuseumDisplays
{
    [Transient, InjectProperties]
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
