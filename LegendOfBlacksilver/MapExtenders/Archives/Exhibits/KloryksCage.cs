using AgateLib;
using System.Threading.Tasks;

namespace Xle.Blacksilver.MapExtenders.Archives.Exhibits
{
    [Transient]
    public class KloryksCage : LobExhibit
    {
        public KloryksCage()
            : base("Kloryk's Cage", Coin.WhiteDiamond)
        { }

        public override ExhibitIdentifier ExhibitIdentifier
        {
            get { return ExhibitIdentifier.KloryksCage; }
        }

        public override bool IsClosed
        {
            get { return HasBeenVisited; }
        }

        public override async Task RunExhibit()
        {
            await base.RunExhibit();

            await ReturnGem();
        }
    }
}
