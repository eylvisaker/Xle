using AgateLib;
using System.Threading.Tasks;
using Xle.Game;

namespace Xle.Blacksilver.MapExtenders.Castle.EventExtenders
{
    [Transient("King")]
    public class King : LobEvent
    {
        private bool asked;

        public override async Task<bool> Speak()
        {
            await TextArea.PrintLine(" to the " + Name() + ".");
            await TextArea.PrintLine();
            await GameControl.PlaySoundSync(LotaSound.VeryGood);

            if (Player.Items[LobItem.FalconFeather] > 0)
            {
                await PrinceAskForFeather();
            }
            else if (Player.Items[LobItem.SmallKey] == 0 && Player.Items[LobItem.GoldKey] == 0)
            {
                await PrinceAskForHelp();
            }

            else
            {
                await NothingToTell();
            }

            await GameControl.Wait(2000);

            return true;
        }

        private async Task NothingToTell()
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine("     There is no more I can");
            await TextArea.PrintLine("     tell you right now.  Our");
            await TextArea.PrintLine("     hopes are still with you.");
            await TextArea.PrintLine("");

            await GameControl.WaitForKey();
        }

        private async Task PrinceAskForFeather()
        {
            asked = false;

            if (asked)
            {
                await TextArea.PrintLineSlow("I ask again...");

                await GameControl.Wait(1000);
                await TextArea.PrintLineSlow("");
            }
            else
            {
                TextArea.Clear();
                await TextArea.PrintLineSlow("I am the prince of this castle");
                await TextArea.PrintLineSlow("and all areas around here.");
                await TextArea.PrintLineSlow();

                await TextArea.FlashLines(2000, XleColor.Yellow, 250);
                await GameControl.Wait(500);
            }

            await TextArea.PrintLineSlow("I see you have an interesting");
            await TextArea.PrintLineSlow("feather. May I have it?");

            if (1 == await QuickMenu.QuickMenuYesNo())
            {
                await TextArea.PrintLineSlow("I've confused you with a brave");
                await TextArea.PrintLineSlow("warrior who I was expecting.");

                asked = true;
            }
            else
            {
                Player.Items[LobItem.FalconFeather] = 0;

                await TextArea.PrintLineSlow("Since you come with the feather, I");
                await TextArea.PrintLineSlow("know my sister selected you to be our");
                await TextArea.PrintLineSlow("champion.  I only pray that you are");
                await TextArea.PrintLineSlow("not too late.  It's been many days");
                await TextArea.PrintLineSlow("since the king was kidnapped.");

                await GameControl.WaitForKey();
                await TextArea.PrintLineSlow();

                await PrinceAskForHelp();
            }
        }

        private async Task PrinceAskForHelp()
        {
            await TextArea.PrintLineSlow("There is much to do.  The earthquakes");
            await TextArea.PrintLineSlow("continue to batter the castle.  A");
            await TextArea.PrintLineSlow("landslide now blocks entrance to the");
            await TextArea.PrintLineSlow("inner chambers. The wizard seravol is");
            await TextArea.PrintLineSlow("trapped inside, imprisoned by orcs.");

            await GameControl.WaitForKey();

            await TextArea.PrintLineSlow();
            await TextArea.PrintLineSlow("Will you help?");
            await TextArea.PrintLineSlow();

            if (1 == await QuickMenu.QuickMenuYesNo())
            {
                await TextArea.PrintLineSlow("I don't blame your fear.");
                await TextArea.PrintLineSlow("Godspeed on your journeys.");
            }
            else
            {
                await TextArea.PrintLineSlow("Here is a key.  You must find a way");
                await TextArea.PrintLineSlow("To get past the landslide.  Once you");
                await TextArea.PrintLineSlow("do, you'll have to defeat the orcs");
                await TextArea.PrintLineSlow("who have captured the inner castle");
                await TextArea.PrintLineSlow("chambers.");

                Player.Items[LobItem.SmallKey] = 1;
                await GameControl.WaitForKey();

                await TextArea.PrintLineSlow();
                await TextArea.PrintLineSlow("My gold is yours - provided you use");
                await TextArea.PrintLineSlow("it for the good.  The wizard seravol");
                await TextArea.PrintLineSlow("has much to offer, if you can defeat");
                await TextArea.PrintLineSlow("the orcs and free him.  Godspeed.");
                await TextArea.PrintLineSlow();

                await GameControl.WaitForKey();
            }
        }

        private string Name()
        {
            if (Player.Level > 4)
                return "king";
            else
                return "prince";
        }
    }
}
