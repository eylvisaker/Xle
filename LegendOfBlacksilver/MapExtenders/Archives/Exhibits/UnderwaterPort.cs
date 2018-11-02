using AgateLib;

namespace Xle.Blacksilver.MapExtenders.Archives.Exhibits
{
    [Transient]
    public class UnderwaterPort : LobExhibit
    {
        public UnderwaterPort()
            : base("Underwater Port", Coin.YellowDiamond)
        { }

        public override bool IsClosed
        {
            get { return HasBeenVisited; }
        }

        public override ExhibitIdentifier ExhibitIdentifier
        {
            get { return ExhibitIdentifier.UnderwaterPort; }
        }
    }
}
