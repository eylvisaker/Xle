using System.Threading.Tasks;
using Xle.Services.Game;

namespace Xle.Blacksilver.MapExtenders.Archives.Exhibits
{
    public class MagicEtherium : LobExhibit
    {
        public MagicEtherium()
            : base("Magic Etherium", Coin.AmethystGem)
        { }

        public override ExhibitIdentifier ExhibitIdentifier
        {
            get { return ExhibitIdentifier.MagicEtherium; }
        }

        public override bool IsClosed
        {
            get { return Story.DrankEtherium; }
        }

        public override async Task RunExhibit()
        {
            await base.RunExhibit();

            await TextArea.PrintLine();
            await TextArea.PrintLine("Do you want to drink the etherium?");
            await TextArea.PrintLine();

            if (0 == await QuickMenu.QuickMenuYesNo())
            {
                await TextArea.PrintLine();
                await TextArea.PrintLine("You feel dizzy.");
                await GameControl.Wait(1500);
                await TextArea.PrintLine("The feeling passes.");

                Story.DrankEtherium = true;
            }
            else
                await ReturnGem();
        }
    }
}
