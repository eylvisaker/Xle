using ERY.Xle.Services;
using ERY.Xle.XleEventTypes.Extenders;

namespace ERY.Xle.LotA.MapExtenders.Castle.Events
{
    public class Casandra : LotaEvent
    {
        public IQuickMenu QuickMenu { get; set; }

        public override bool Speak(GameState state)
        {
            SoundMan.PlaySound(LotaSound.VeryGood);

            TextArea.Clear(true);
            TextArea.PrintLine();
            TextArea.PrintLine("     casandra the temptress", XleColor.Yellow);
            TextArea.PrintLine();

            TextArea.FlashLinesWhile(() => SoundMan.IsPlaying(LotaSound.VeryGood), XleColor.Yellow, XleColor.Cyan, 250);

            if (Story.VisitedCasandra == false)
            {
                OfferGoldOrCharm(state);
            }
            else
            {
                BegoneMessage(state);
            }

            GameControl.Wait(5000);
            return true;
        }

        private void BegoneMessage(GameState state)
        {
            TextArea.PrintLine("I helped you already - be gone.");
            TextArea.PrintLine();
        }

        private void OfferGoldOrCharm(GameState state)
        {
            TextArea.PrintLineSlow("You may visit my magical room", XleColor.Green);
            TextArea.PrintLineSlow("only this once.  My power can", XleColor.Cyan);
            TextArea.PrintLineSlow("bring you different rewards.", XleColor.Yellow);

            int choice = QuickMenu.QuickMenu(new MenuItemList("Gold", "Charm"), 2);

            TextArea.PrintLine();

            if (choice == 0)
            {
                GiveGold(state);
            }
            if (choice == 1)
            {
                GiveCharm(state);
            }

            TextArea.PrintLine();

            var old = state.Map.ColorScheme.BorderColor;
            Map.ColorScheme.BorderColor = XleColor.White;

            SoundMan.PlaySoundSync(LotaSound.VeryGood);

            Map.ColorScheme.BorderColor = old;

            Story.VisitedCasandra = true;

            if (Story.SearchingForTulip)
            {
                PassageHint(state);
            }
        }

        private void PassageHint(GameState state)
        {
            TextArea.PrintLine();
            TextArea.PrintLine();
            TextArea.PrintLine("You should know that there are many");
            TextArea.PrintLine("secret passageways.  The entrace to");
            TextArea.PrintLine("one is between two flower gardens.");
        }

        private void GiveCharm(GameState state)
        {
            TextArea.PrintLine("Charm  +15");
            Player.Attribute[Attributes.charm] += 15;
        }

        private void GiveGold(GameState state)
        {
            TextArea.PrintLine("Gold  +5,000");
            Player.Gold += 5000;
        }
    }
}
