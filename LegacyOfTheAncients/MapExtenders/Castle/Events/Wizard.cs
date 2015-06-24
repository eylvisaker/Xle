using ERY.Xle.Services;
using ERY.Xle.Services.Menus;
using ERY.Xle.XleEventTypes.Extenders;

namespace ERY.Xle.LotA.MapExtenders.Castle.Events
{
    public class Wizard : LotaEvent
    {
        public IQuickMenu QuickMenu { get; set; }

        public override bool Speak(GameState state)
        {
            SoundMan.PlaySound(LotaSound.VeryGood);

            TextArea.Clear(true);
            TextArea.PrintLine();
            TextArea.PrintLine("    Meet the wizard of potions!!", XleColor.Cyan);
            TextArea.PrintLine();

            TextArea.FlashLinesWhile(() => SoundMan.IsPlaying(LotaSound.VeryGood), XleColor.Green, XleColor.Cyan, 250);

            if (Story.BoughtPotion)
            {
                BegoneMessage();
            }
            else
            {
                OfferPotion(state);
            }

            GameControl.Wait(5000);
            return true;
        }

        private void OfferPotion(GameState state)
        {
            TextArea.PrintLine("My potion can help you.");
            TextArea.PrintLine("It will cost 2,500 gold.");
            TextArea.PrintLine();

            if (QuickMenu.QuickMenuYesNo() == 0)
            {
                if (Player.Gold < 2500)
                {
                    TextArea.PrintLine();
                    TextArea.PrintLine("you haven't the gold.");
                }
                else
                {
                    Player.Gold -= 2500;
                    Story.BoughtPotion = true;

                    if (Player.Attribute[Attributes.dexterity] <= Player.Attribute[Attributes.endurance])
                    {
                        Player.Attribute[Attributes.dexterity] = 36;
                        Player.Attribute[Attributes.endurance] += 5;
                    }
                    else
                    {
                        Player.Attribute[Attributes.dexterity] += 5;
                        Player.Attribute[Attributes.endurance] = 36;
                    }

                    TextArea.Clear(true);
                    TextArea.PrintLine();
                    TextArea.PrintLine("Check your attributes.");
                    TextArea.PrintLine();

                    SoundMan.PlaySound(LotaSound.VeryGood);

                    TextArea.FlashLinesWhile(() => SoundMan.IsPlaying(LotaSound.VeryGood), XleColor.White, XleColor.Cyan, 250);

                }
            }
            else
            {
                TextArea.PrintLine();
                TextArea.PrintLine("No?  Maybe later.");
            }
        }

        private void BegoneMessage()
        {
            TextArea.PrintLine("I can do no more for you.");
            TextArea.PrintLine();
        }
    }
}
