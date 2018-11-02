using AgateLib;
using System.Threading.Tasks;

namespace Xle.Blacksilver.MapExtenders.Archives.Exhibits
{
    [Transient]
    public class VaseOfSouls : LobExhibit
    {
        public VaseOfSouls()
            : base("Vase of Souls", Coin.AmethystGem)
        { }

        public override ExhibitIdentifier ExhibitIdentifier
        {
            get { return ExhibitIdentifier.VaseOfSouls; }
        }

        public override bool IsClosed
        {
            get { return Story.ClosedVaseOfSouls; }
        }

        public override async Task RunExhibit()
        {
            await base.RunExhibit();

            await ReturnGem();

            Story.ClosedVaseOfSouls = true;
        }
    }
}
