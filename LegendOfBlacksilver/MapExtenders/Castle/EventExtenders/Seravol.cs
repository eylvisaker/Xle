using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Castle.EventExtenders
{
    public class Seravol : LobEvent
    {
        public Random Random { get; set; }

        public override bool Speak(GameState unused)
        {
            Greetings();
            bool handled = false;

            if (Player.Level > 1)
            {
                if (Player.Items[LobItem.GoldKey] == 0 &&
                    AllOrcsKilled(GameState))
                {
                    GiveGoldKey(GameState);
                    handled = true;
                }
            }
            else
            {
                Introduction(GameState);
                handled = true;
            }

            if (Random.NextDouble() < 0.4)
            {
                QuakeMessage();
            }

            if (handled == false)
            {
                NothingToTellMessage(GameState);
            }

            return true;
        }

        private void NothingToTellMessage(GameState unused)
        {
            TextArea.PrintLineSlow();
            TextArea.PrintLine("I have nothing else to tell you.");
            TextArea.PrintLine();
            GameControl.Wait(1000);
            TextArea.PrintLine("Perhaps the prince could");
            TextArea.PrintLine("Help you further.");
            Input.WaitForKey();
        }

        private void GiveGoldKey(GameState unused)
        {
            TextArea.PrintLineSlow();
            TextArea.PrintLineSlow("I've taken your small and wooden");
            TextArea.PrintLineSlow("keys in exchange for this gold key.");
            TextArea.PrintLineSlow("I suspect you'll find the new one");
            TextArea.PrintLineSlow("more convenient.");
            TextArea.PrintLineSlow();
            Input.WaitForKey();

            Player.Items[LobItem.GoldKey] = 1;
            Player.Items[LobItem.WoodenKey] = 0;
            Player.Items[LobItem.SmallKey] = 0;

            Story.ClearedRockSlide = true;
            Story.DefeatedOrcs = true;

            TextArea.PrintLineSlow("My orb of vision has been stolen and");
            TextArea.PrintLineSlow("removed from this castle.  Without ");
            TextArea.PrintLineSlow("it, I'm almost blind.  With it, there");
            TextArea.PrintLineSlow("is much I could tell you.");
            TextArea.PrintLineSlow();
            Input.WaitForKey();

            SoundMan.PlaySound(LotaSound.VeryGood);
            TextArea.PrintLineSlow("To spur you on, I present this red");
            TextArea.PrintLineSlow("garnet gem.  Use it wisely.");
            Input.WaitForKey();

            Player.Items[LobItem.RedGarnet] += 1;
        }

        private void QuakeMessage()
        {
            TextArea.PrintLineSlow("I've heard that the quakes seem to");
            TextArea.PrintLineSlow("be disturbing the tides of bantross.");
            Input.WaitForKey();
        }

        private bool AllOrcsKilled(GameState unused)
        {
            if (Map.Guards.Any(x => x.Color == XleColor.Blue))
                return false;
            else
                return true;
        }

        private void Introduction(GameState unused)
        {
            TextArea.PrintLineSlow("Ah, it is a pleasure to at last be");
            TextArea.PrintLineSlow("free from these vile and troublesome");
            TextArea.PrintLineSlow("orc guards.  One good turn deserves");
            TextArea.PrintLineSlow("another.");
            TextArea.PrintLineSlow();

            Input.WaitForKey();

            TextArea.Clear(true);
            TextArea.PrintLine();
            TextArea.PrintLine();
            TextArea.PrintLine("I promote you to apprentice.");
            TextArea.PrintLine();

            SoundMan.PlaySound(LotaSound.VeryGood);
            TextArea.FlashLinesWhile(() => SoundMan.IsPlaying(LotaSound.VeryGood),
                XleColor.White, XleColor.Red, 250);

            TextArea.PrintLineSlow("(Admittedly, it's not much, but");
            TextArea.PrintLineSlow("far better than being a serf.)");
            TextArea.PrintLineSlow();

            SoundMan.PlaySound(LotaSound.Good);
            TextArea.PrintLineSlow("Charisma: +5");

            Player.Attribute[Attributes.charm] += 5;
            Player.Level = 2;

            TextArea.PrintLineSlow();
            Input.WaitForKey();

            if (AllOrcsKilled(GameState))
            {
                GiveGoldKey(GameState);
            }
            else
            {
                TextArea.PrintLineSlow("Please get rid of the rest of these");
                TextArea.PrintLineSlow("orcs.  If you don't, they'll only");
                TextArea.PrintLineSlow("come back.");
                TextArea.PrintLineSlow();
            }

            Input.WaitForKey();
        }

        private void Greetings()
        {
            TextArea.Clear(true);
            TextArea.PrintLine();
            TextArea.PrintLine();
            TextArea.PrintLine("Greetings from the wizard seravol!");
            TextArea.PrintLine();
            TextArea.PrintLine();

            SoundMan.PlaySound(LotaSound.VeryGood);
            TextArea.FlashLinesWhile(
                () => SoundMan.IsPlaying(LotaSound.VeryGood),
                XleColor.White, XleColor.Yellow, 250);
        }
    }
}
