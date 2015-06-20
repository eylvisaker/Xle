using ERY.Xle.Services.Implementation;
using ERY.Xle.XleEventTypes.Extenders;

namespace ERY.Xle.LotA.MapExtenders.Castle.Events
{
    public class Casandra : EventExtender
    {
        public override bool Speak(GameState state)
        {
            SoundMan.PlaySound(LotaSound.VeryGood);

            XleCore.TextArea.Clear(true);
            XleCore.TextArea.PrintLine();
            XleCore.TextArea.PrintLine("     casandra the temptress", XleColor.Yellow);
            XleCore.TextArea.PrintLine();

            XleCore.TextArea.FlashLinesWhile(() => SoundMan.IsPlaying(LotaSound.VeryGood), XleColor.Yellow, XleColor.Cyan, 250);

            if (Lota.Story.VisitedCasandra == false)
            {
                OfferGoldOrCharm(state);
            }
            else
            {
                BegoneMessage(state);
            }

            XleCore.Wait(5000);
            return true;
        }

        private void BegoneMessage(GameState state)
        {
            XleCore.TextArea.PrintLine("I helped you already - be gone.");
            XleCore.TextArea.PrintLine();
        }

        private void OfferGoldOrCharm(GameState state)
        {
            XleCore.TextArea.PrintLineSlow("You may visit my magical room", XleColor.Green);
            XleCore.TextArea.PrintLineSlow("only this once.  My power can", XleColor.Cyan);
            XleCore.TextArea.PrintLineSlow("bring you different rewards.", XleColor.Yellow);

            int choice = XleCore.QuickMenu(new MenuItemList("Gold", "Charm"), 2);

            XleCore.TextArea.PrintLine();

            if (choice == 0)
            {
                GiveGold(state);
            }
            if (choice == 1)
            {
                GiveCharm(state);
            }

            XleCore.TextArea.PrintLine();

            var old = state.Map.ColorScheme.BorderColor;
            state.Map.ColorScheme.BorderColor = XleColor.White;

            SoundMan.PlaySoundSync(LotaSound.VeryGood);

            state.Map.ColorScheme.BorderColor = old;

            Lota.Story.VisitedCasandra = true;

            if (Lota.Story.SearchingForTulip)
            {
                PassageHint(state);
            }
        }

        private void PassageHint(GameState state)
        {
            XleCore.TextArea.PrintLine();
            XleCore.TextArea.PrintLine();
            XleCore.TextArea.PrintLine("You should know that there are many");
            XleCore.TextArea.PrintLine("secret passageways.  The entrace to");
            XleCore.TextArea.PrintLine("one is between two flower gardens.");


        }

        private void GiveCharm(GameState state)
        {
            XleCore.TextArea.PrintLine("Charm  +15");
            XleCore.GameState.Player.Attribute[Attributes.charm] += 15;
        }

        private void GiveGold(GameState state)
        {
            XleCore.TextArea.PrintLine("Gold  +5,000");
            XleCore.GameState.Player.Gold += 5000;
        }
    }
}
