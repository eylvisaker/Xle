using AgateLib;
using System.Threading.Tasks;

namespace Xle.Blacksilver.MapExtenders.Archives.Exhibits
{
    [Transient]
    public class FlaxtonIncense : LobExhibit
    {
        public FlaxtonIncense()
            : base("Flaxton Incense", Coin.WhiteDiamond)
        { }

        public override ExhibitIdentifier ExhibitIdentifier
        {
            get { return ExhibitIdentifier.FlaxtonIncense; }
        }

        public override bool IsClosed
        {
            get { return Story.EatenFlaxton; }
        }

        public override async Task RunExhibit()
        {
            await base.RunExhibit();

            await TextArea.PrintLine();
            await TextArea.PrintLine("Do you want to partake?");
            await TextArea.PrintLine();

            if (0 == await QuickMenu.QuickMenuYesNo())
            {
                await TextArea.PrintLine();
                await TextArea.PrintLine();
                await TextArea.PrintLine("It's sour but doesn't taste that bad.");

                Story.EatenFlaxton = true;
            }
            else
                await ReturnGem();
        }
    }
}
