using AgateLib;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xle.Services.Game;

namespace Xle.Blacksilver.MapExtenders.Castle.EventExtenders
{
    [Transient("Seravol")]
    public class Seravol : LobEvent
    {
        public Random Random { get; set; }

        public override async Task<bool> Speak()
        {
            await Greetings();
            bool handled = false;

            if (Player.Level > 1)
            {
                if (Player.Items[LobItem.GoldKey] == 0 &&
                    AllOrcsKilled((this).GameState))
                {
                    await GiveGoldKey((this).GameState);
                    handled = true;
                }
            }
            else
            {
                await Introduction((this).GameState);
                handled = true;
            }

            if (Random.NextDouble() < 0.4)
            {
                await QuakeMessage();
            }

            if (handled == false)
            {
                await NothingToTellMessage((this).GameState);
            }

            return true;
        }

        private async Task NothingToTellMessage(GameState unused)
        {
            await TextArea.PrintLineSlow();
            await TextArea.PrintLine("I have nothing else to tell you.");
            await TextArea.PrintLine();
            await GameControl.Wait(1000);
            await TextArea.PrintLine("Perhaps the prince could");
            await TextArea.PrintLine("Help you further.");
            await GameControl.WaitForKey();
        }

        private async Task GiveGoldKey(GameState unused)
        {
            await TextArea.PrintLineSlow();
            await TextArea.PrintLineSlow("I've taken your small and wooden");
            await TextArea.PrintLineSlow("keys in exchange for this gold key.");
            await TextArea.PrintLineSlow("I suspect you'll find the new one");
            await TextArea.PrintLineSlow("more convenient.");
            await TextArea.PrintLineSlow();
            await GameControl.WaitForKey();

            Player.Items[LobItem.GoldKey] = 1;
            Player.Items[LobItem.WoodenKey] = 0;
            Player.Items[LobItem.SmallKey] = 0;

            Story.ClearedRockSlide = true;
            Story.DefeatedOrcs = true;

            await TextArea.PrintLineSlow("My orb of vision has been stolen and");
            await TextArea.PrintLineSlow("removed from this castle.  Without ");
            await TextArea.PrintLineSlow("it, I'm almost blind.  With it, there");
            await TextArea.PrintLineSlow("is much I could tell you.");
            await TextArea.PrintLineSlow();
            await GameControl.WaitForKey();

            SoundMan.PlaySound(LotaSound.VeryGood);
            await TextArea.PrintLineSlow("To spur you on, I present this red");
            await TextArea.PrintLineSlow("garnet gem.  Use it wisely.");
            await GameControl.WaitForKey();

            Player.Items[LobItem.RedGarnet] += 1;
        }

        private async Task QuakeMessage()
        {
            await TextArea.PrintLineSlow("I've heard that the quakes seem to");
            await TextArea.PrintLineSlow("be disturbing the tides of bantross.");
            await GameControl.WaitForKey();
        }

        private bool AllOrcsKilled(GameState unused)
        {
            if (Map.Guards.Any(x => x.Color == XleColor.Blue))
                return false;
            else
                return true;
        }

        private async Task Introduction(GameState unused)
        {
            await TextArea.PrintLineSlow("Ah, it is a pleasure to at last be");
            await TextArea.PrintLineSlow("free from these vile and troublesome");
            await TextArea.PrintLineSlow("orc guards.  One good turn deserves");
            await TextArea.PrintLineSlow("another.");
            await TextArea.PrintLineSlow();

            await GameControl.WaitForKey();

            TextArea.Clear(true);
            await TextArea.PrintLine();
            await TextArea.PrintLine();
            await TextArea.PrintLine("I promote you to apprentice.");
            await TextArea.PrintLine();

            SoundMan.PlaySound(LotaSound.VeryGood);
            await TextArea.FlashLinesWhile(() => SoundMan.IsPlaying(LotaSound.VeryGood),
                XleColor.White, XleColor.Red, 250);

            await TextArea.PrintLineSlow("(Admittedly, it's not much, but");
            await TextArea.PrintLineSlow("far better than being a serf.)");
            await TextArea.PrintLineSlow();

            SoundMan.PlaySound(LotaSound.Good);
            await TextArea.PrintLineSlow("Charisma: +5");

            Player.Attribute[Attributes.charm] += 5;
            Player.Level = 2;

            await TextArea.PrintLineSlow();
            await GameControl.WaitForKey();

            if (AllOrcsKilled(GameState))
            {
                await GiveGoldKey(GameState);
            }
            else
            {
                await TextArea.PrintLineSlow("Please get rid of the rest of these");
                await TextArea.PrintLineSlow("orcs.  If you don't, they'll only");
                await TextArea.PrintLineSlow("come back.");
                await TextArea.PrintLineSlow();
            }

            await GameControl.WaitForKey();
        }

        private async Task Greetings()
        {
            TextArea.Clear(true);
            await TextArea.PrintLine();
            await TextArea.PrintLine();
            await TextArea.PrintLine("Greetings from the wizard seravol!");
            await TextArea.PrintLine();
            await TextArea.PrintLine();

            SoundMan.PlaySound(LotaSound.VeryGood);
            await TextArea.FlashLinesWhile(
                () => SoundMan.IsPlaying(LotaSound.VeryGood),
                XleColor.White, XleColor.Yellow, 250);
        }
    }
}
