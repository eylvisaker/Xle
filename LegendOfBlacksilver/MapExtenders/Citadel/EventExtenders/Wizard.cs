using ERY.Xle.Services.Implementation;
using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Citadel.EventExtenders
{
    public class Wizard : LobEvent
    {
        public override bool Speak(GameState state)
        {
            TextArea.PrintLine();
            TextArea.PrintLine();

            if (Story.BoughtOrb == false)
                OfferOrbForSale();
            else if (Player.Level >= 3 && Story.BoughtStrengthFromWizard)
                OfferStrengthForSale();
            else if (Story.BoughtStrengthFromWizard == false)
                ComeBackLater();
            else
                NoMoreHelp();

            GameControl.Wait(state.GameSpeed.AfterSpeakTime);
            return true;
        }

        private void ComeBackLater()
        {
            TextArea.PrintLine("Come back later.");
        }

        private void OfferOrbForSale()
        {
            TextArea.PrintLineSlow("I have a certain orb for sale.");
            TextArea.PrintLineSlow("The price is only 500 gold, but it");
            TextArea.PrintLineSlow("is not cheap.");

            int choice = QuickMenu.QuickMenuYesNo();

            if (choice == 1)
                return;

            if (Player.Gold < 500)
            {
                TextArea.PrintLine();
                TextArea.PrintLine("You don't have enough gold.");
                return;
            }

            Player.Gold -= 500;
            Player.Items[LobItem.GlassOrb] += 1;
            Story.BoughtOrb = true;

            SoundMan.PlaySound(LotaSound.VeryBad);

            for (int i = 0; i < 15; i++)
            {
                Player.Attribute[Attributes.strength]--;
                TextArea.PrintLine(string.Format(
                    "Your strength is now: {0}", Player.Attribute[Attributes.strength]));

                GameControl.Wait(250);
            }
        }

        private void OfferStrengthForSale()
        {
            TextArea.PrintLineSlow("I believe our last dealings");
            TextArea.PrintLineSlow("cost you some strength.");
            TextArea.PrintLineSlow("I can sell you some back for");
            TextArea.PrintLineSlow("1,500 gold.");

            int choice = QuickMenu.QuickMenuYesNo();

            if (choice == 1)
                return;

            if (Player.Gold < 1500)
            {
                TextArea.PrintLine();
                TextArea.PrintLine("You don't have enough gold.");
                return;
            }

            Player.Attribute[Attributes.strength] += 5;

            Story.BoughtStrengthFromWizard = true;
        }

        private void NoMoreHelp()
        {
            TextArea.PrintLine("We have no more dealings.");
        }
    }
}
