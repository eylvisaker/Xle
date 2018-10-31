using System.Threading.Tasks;
using Xle.Services.Game;

namespace Xle.LoB.MapExtenders.Archives.Exhibits
{
    public class MorningStar : LobExhibit
    {
        public MorningStar()
            : base("Morning Star", Coin.Emerald)
        { }

        public override ExhibitIdentifier ExhibitIdentifier
        {
            get { return ExhibitIdentifier.MorningStar; }
        }

        public override bool IsClosed
        {
            get { return Story.ClosedMorningStar; }
        }

        public override async Task RunExhibit()
        {
            await base.RunExhibit();

            await TextArea.PrintLine();
            await TextArea.PrintLine("Do you want to borrow this item?");
            await TextArea.PrintLine();

            if (0 == await QuickMenu.QuickMenuYesNo())
            {
                await TextArea.PrintLine();
                await TextArea.PrintLine();
                await TextArea.PrintLine("Fight bravely.");

                await GameControl.PlaySoundSync(LotaSound.VeryGood);

                Player.AddWeapon(9, 4);
                Story.ClosedMorningStar = true;
            }
            else
                await ReturnGem();
        }
    }
}
