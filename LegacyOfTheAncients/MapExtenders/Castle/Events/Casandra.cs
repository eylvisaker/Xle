using AgateLib;
using System.Threading.Tasks;
using Xle.Services.Menus;

namespace Xle.Ancients.MapExtenders.Castle.Events
{
    [Transient("Casandra")]
    public class Casandra : LotaEvent
    {
        public IQuickMenu QuickMenu { get; set; }

        public override async Task<bool> Speak()
        {
            SoundMan.PlaySound(LotaSound.VeryGood);

            TextArea.Clear(true);
            await TextArea.PrintLine();
            await TextArea.PrintLine("     casandra the temptress", XleColor.Yellow);
            await TextArea.PrintLine();

            await TextArea.FlashLinesWhile(() => SoundMan.IsPlaying(LotaSound.VeryGood), XleColor.Yellow, XleColor.Cyan, 250);

            if (Story.VisitedCasandra == false)
            {
                await OfferGoldOrCharm();
            }
            else
            {
                await BegoneMessage();
            }

            await GameControl.WaitAsync(5000);
            return true;
        }

        private async Task BegoneMessage()
        {
            await TextArea.PrintLine("I helped you already - be gone.");
            await TextArea.PrintLine();
        }

        private async Task OfferGoldOrCharm()
        {
            await TextArea.PrintLineSlow("You may visit my magical room", XleColor.Green);
            await TextArea.PrintLineSlow("only this once.  My power can", XleColor.Cyan);
            await TextArea.PrintLineSlow("bring you different rewards.", XleColor.Yellow);

            int choice = await QuickMenu.QuickMenu(new MenuItemList("Gold", "Charm"), 2);

            await TextArea.PrintLine();

            if (choice == 0)
            {
               await TextArea.PrintLine("Gold  +5,000");
                Player.Gold += 5000;
            }
            if (choice == 1)
            {
                await TextArea.PrintLine("Charm  +15");
                Player.Attribute[Attributes.charm] += 15;
            }

            await TextArea.PrintLine();

            var old = Map.ColorScheme.BorderColor;
            Map.ColorScheme.BorderColor = XleColor.White;

            await SoundMan.PlaySoundWait(LotaSound.VeryGood);

            Map.ColorScheme.BorderColor = old;

            Story.VisitedCasandra = true;

            if (Story.SearchingForTulip)
            {
                await PassageHint();
            }
        }

        private async Task PassageHint()
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine();
            await TextArea.PrintLine("You should know that there are many");
            await TextArea.PrintLine("secret passageways.  The entrace to");
            await TextArea.PrintLine("one is between two flower gardens.");
        }
    }
}
