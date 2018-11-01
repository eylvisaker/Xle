using AgateLib;
using System.Threading.Tasks;

namespace Xle.Blacksilver.MapExtenders.Citadel.EventExtenders
{
    [Transient("Wizard")]
    public class Wizard : LobEvent
    {
        public override async Task<bool> Speak()
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine();

            if (Story.BoughtOrb == false)
                await OfferOrbForSale();
            else if (Player.Level >= 3 && Story.BoughtStrengthFromWizard)
                await OfferStrengthForSale();
            else if (Story.BoughtStrengthFromWizard == false)
                await ComeBackLater();
            else
                await NoMoreHelp();

            await GameControl.WaitAsync(GameState.GameSpeed.AfterSpeakTime);
            return true;
        }

        private Task ComeBackLater() => TextArea.PrintLine("Come back later.");


        private async Task OfferOrbForSale()
        {
            await TextArea.PrintLineSlow("I have a certain orb for sale.");
            await TextArea.PrintLineSlow("The price is only 500 gold, but it");
            await TextArea.PrintLineSlow("is not cheap.");

            int choice = await QuickMenu.QuickMenuYesNo();

            if (choice == 1)
                return;

            if (Player.Gold < 500)
            {
                await TextArea.PrintLine();
                await TextArea.PrintLine("You don't have enough gold.");
                return;
            }

            Player.Gold -= 500;
            Player.Items[LobItem.GlassOrb] += 1;
            Story.BoughtOrb = true;

            SoundMan.PlaySound(LotaSound.VeryBad);

            for (int i = 0; i < 15; i++)
            {
                Player.Attribute[Attributes.strength]--;
                await TextArea.PrintLine(string.Format(
                    "Your strength is now: {0}", Player.Attribute[Attributes.strength]));

                await GameControl.WaitAsync(250);
            }
        }

        private async Task OfferStrengthForSale()
        {
            await TextArea.PrintLineSlow("I believe our last dealings");
            await TextArea.PrintLineSlow("cost you some strength.");
            await TextArea.PrintLineSlow("I can sell you some back for");
            await TextArea.PrintLineSlow("1,500 gold.");

            int choice = await QuickMenu.QuickMenuYesNo();

            if (choice == 1)
                return;

            if (Player.Gold < 1500)
            {
                await TextArea.PrintLine();
                await TextArea.PrintLine("You don't have enough gold.");
                return;
            }

            Player.Attribute[Attributes.strength] += 5;

            Story.BoughtStrengthFromWizard = true;
        }

        private async Task NoMoreHelp()
        {
            await TextArea.PrintLine("We have no more dealings.");
        }
    }
}
