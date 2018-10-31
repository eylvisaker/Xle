using System.Threading.Tasks;
using Xle.Services.Game;

namespace Xle.Blacksilver.MapExtenders.Archives.Exhibits
{
    public class Blacksmith : LobExhibit
    {
        public Blacksmith()
            : base("Blacksmith", Coin.WhiteDiamond)
        { }

        public override ExhibitIdentifier ExhibitIdentifier
        {
            get { return ExhibitIdentifier.Blacksmith; }
        }

        public override bool IsClosed
        {
            get { return Story.ProcuredSteelHammer; }
        }

        public override async Task RunExhibit()
        {
            await base.RunExhibit();

            await TextArea.PrintLine();
            await TextArea.PrintLine("Do you want to have it?");
            await TextArea.PrintLine();

            if (0 == await QuickMenu.QuickMenuYesNo())
            {
                await TextArea.PrintLine();
                await TextArea.PrintLine();
                await TextArea.PrintLine("It is yours.");

                await GameControl.PlaySoundSync(LotaSound.Good);

                Player.Items[LobItem.SteelHammer] = 1;

                Story.ProcuredSteelHammer = true;
            }
            else
                await ReturnGem();
        }
    }
}
