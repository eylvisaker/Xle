using AgateLib;
using System.Threading.Tasks;
using Xle.Menus;

namespace Xle.Ancients.MapExtenders.Castle.Events
{
    [Transient("Wizard")]
    public class Wizard : LotaEvent
    {
        public IQuickMenu QuickMenu { get; set; }

        public override async Task<bool> Speak()
        {
            SoundMan.PlaySound(LotaSound.VeryGood);

            TextArea.Clear(true);
            await TextArea.PrintLine();
            await TextArea.PrintLine("    Meet the wizard of potions!!", XleColor.Cyan);
            await TextArea.PrintLine();

            await TextArea.FlashLinesWhile(() => SoundMan.IsPlaying(LotaSound.VeryGood), XleColor.Green, XleColor.Cyan, 250);

            if (Story.BoughtPotion)
            {
                await BegoneMessage();
            }
            else
            {
                await OfferPotion();
            }

            await GameControl.WaitAsync(5000);
            return true;
        }

        private async Task OfferPotion()
        {
            await TextArea.PrintLine("My potion can help you.");
            await TextArea.PrintLine("It will cost 2,500 gold.");
            await TextArea.PrintLine();

            if (await QuickMenu.QuickMenuYesNo() == 0)
            {
                if (Player.Gold < 2500)
                {
                    await TextArea.PrintLine();
                    await TextArea.PrintLine("you haven't the gold.");
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
                    await TextArea.PrintLine();
                    await TextArea.PrintLine("Check your attributes.");
                    await TextArea.PrintLine();

                    SoundMan.PlaySound(LotaSound.VeryGood);

                    await TextArea.FlashLinesWhile(() => SoundMan.IsPlaying(LotaSound.VeryGood), XleColor.White, XleColor.Cyan, 250);

                }
            }
            else
            {
                await TextArea.PrintLine();
                await TextArea.PrintLine("No?  Maybe later.");
            }
        }

        private async Task BegoneMessage()
        {
            await TextArea.PrintLine("I can do no more for you.");
            await TextArea.PrintLine();
        }
    }
}
