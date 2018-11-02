using AgateLib;
using System.Threading.Tasks;

namespace Xle.Blacksilver.MapExtenders.Archives.Exhibits
{
    [Transient]
    public class StormingGear : LobExhibit
    {
        public StormingGear()
            : base("Storming Gear", Coin.RedGarnet)
        { }

        public override ExhibitIdentifier ExhibitIdentifier
        {
            get { return ExhibitIdentifier.StormingGear; }
        }

        public override bool IsClosed
        {
            get { return Player.Items[LobItem.RopeAndPulley] > 0; }
        }

        public override async Task RunExhibit()
        {
            await base.RunExhibit();

            await TextArea.PrintLine();
            await TextArea.PrintLine("Do you want to borrow this gear?");
            await TextArea.PrintLine();

            if (0 == await QuickMenu.QuickMenuYesNo())
            {
                Player.Items[LobItem.RopeAndPulley] += 1;

                await TextArea.PrintLine();
                await TextArea.PrintLine("The equipment is now");
                await TextArea.PrintLine("in your possession.");
            }
            else
            {
                await ReturnGem();
            }
        }

    }
}
