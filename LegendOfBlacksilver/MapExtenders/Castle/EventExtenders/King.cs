using Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.LoB.MapExtenders.Castle.EventExtenders
{
    public class King : LobEvent
    {
        bool asked;

        public override bool Speak()
        {
            TextArea.PrintLine(" to the " + Name() + ".");
            TextArea.PrintLine();
            SoundMan.PlaySoundSync(LotaSound.VeryGood);

            if (Player.Items[LobItem.FalconFeather] > 0)
            {
                PrinceAskForFeather();
            }
            else if (Player.Items[LobItem.SmallKey] == 0 && Player.Items[LobItem.GoldKey] == 0)
            {
                PrinceAskForHelp();
            }

            else
            {
                NothingToTell();
            }

            GameControl.Wait(2000);

            return true;
        }

        private void NothingToTell()
        {
            TextArea.PrintLine();
            TextArea.PrintLine("     There is no more I can");
            TextArea.PrintLine("     tell you right now.  Our");
            TextArea.PrintLine("     hopes are still with you.");
            TextArea.PrintLine("");

            Input.WaitForKey();
        }

        private void PrinceAskForFeather()
        {
            asked = false;

            if (asked)
            {
                TextArea.PrintLineSlow("I ask again...");

                GameControl.Wait(1000);
                TextArea.PrintLineSlow("");
            }
            else
            {
                TextArea.Clear();
                TextArea.PrintLineSlow("I am the prince of this castle");
                TextArea.PrintLineSlow("and all areas around here.");
                TextArea.PrintLineSlow();

                TextArea.FlashLines(2000, XleColor.Yellow, 250);
                GameControl.Wait(500);
            }

            TextArea.PrintLineSlow("I see you have an interesting");
            TextArea.PrintLineSlow("feather. May I have it?");

            if (1 == QuickMenu.QuickMenuYesNo())
            {
                TextArea.PrintLineSlow("I've confused you with a brave");
                TextArea.PrintLineSlow("warrior who I was expecting.");

                asked = true;
            }
            else
            {
                Player.Items[LobItem.FalconFeather] = 0;

                TextArea.PrintLineSlow("Since you come with the feather, I");
                TextArea.PrintLineSlow("know my sister selected you to be our");
                TextArea.PrintLineSlow("champion.  I only pray that you are");
                TextArea.PrintLineSlow("not too late.  It's been many days");
                TextArea.PrintLineSlow("since the king was kidnapped.");

                Input.WaitForKey();
                TextArea.PrintLineSlow();

                PrinceAskForHelp();
            }
        }
        void PrinceAskForHelp()
        {
            TextArea.PrintLineSlow("There is much to do.  The earthquakes");
            TextArea.PrintLineSlow("continue to batter the castle.  A");
            TextArea.PrintLineSlow("landslide now blocks entrance to the");
            TextArea.PrintLineSlow("inner chambers. The wizard seravol is");
            TextArea.PrintLineSlow("trapped inside, imprisoned by orcs.");

            Input.WaitForKey();

            TextArea.PrintLineSlow();
            TextArea.PrintLineSlow("Will you help?");
            TextArea.PrintLineSlow();

            if (1 == QuickMenu.QuickMenuYesNo())
            {
                TextArea.PrintLineSlow("I don't blame your fear.");
                TextArea.PrintLineSlow("Godspeed on your journeys.");
            }
            else
            {
                TextArea.PrintLineSlow("Here is a key.  You must find a way");
                TextArea.PrintLineSlow("To get past the landslide.  Once you");
                TextArea.PrintLineSlow("do, you'll have to defeat the orcs");
                TextArea.PrintLineSlow("who have captured the inner castle");
                TextArea.PrintLineSlow("chambers.");

                Player.Items[LobItem.SmallKey] = 1;
                Input.WaitForKey();

                TextArea.PrintLineSlow();
                TextArea.PrintLineSlow("My gold is yours - provided you use");
                TextArea.PrintLineSlow("it for the good.  The wizard seravol");
                TextArea.PrintLineSlow("has much to offer, if you can defeat");
                TextArea.PrintLineSlow("the orcs and free him.  Godspeed.");
                TextArea.PrintLineSlow();

                Input.WaitForKey();
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
