using System.Threading.Tasks;
using Xle.Services.Game;

namespace Xle.LoB.MapExtenders.Archives.Exhibits
{
    public class Mountains : LobExhibit
    {
        public Mountains()
            : base("Mountains", Coin.AmethystGem)
        { }

        public override ExhibitIdentifier ExhibitIdentifier
        {
            get { return ExhibitIdentifier.Mountains; }
        }

        public override bool IsClosed
        {
            get { return Player.Items[LobItem.ClimbingGear] > 0; }
        }

        public override async Task RunExhibit()
        {
            await base.RunExhibit();

            await TextArea.PrintLine();
            await TextArea.PrintLine("Do you want a set?");
            await TextArea.PrintLine();

            if (0 == await QuickMenu.QuickMenuYesNo())
            {
                await TextArea.PrintLine();
                await TextArea.PrintLine();
                await TextArea.PrintLine("Use it carefully.");

                await GameControl.PlaySoundSync(LotaSound.Good);

                Player.Items[LobItem.ClimbingGear] += 1;
            }
            else
                await ReturnGem();
        }
    }
}
