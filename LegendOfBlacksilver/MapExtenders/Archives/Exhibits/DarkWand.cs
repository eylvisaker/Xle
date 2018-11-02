using AgateLib;

namespace Xle.Blacksilver.MapExtenders.Archives.Exhibits
{
    [Transient]
    public class DarkWand : LobExhibit
    {
        public DarkWand()
            : base("Dark Wand", Coin.YellowDiamond)
        { }

        public override ExhibitIdentifier ExhibitIdentifier
        {
            get { return ExhibitIdentifier.DarkWand; }
        }
    }
}
