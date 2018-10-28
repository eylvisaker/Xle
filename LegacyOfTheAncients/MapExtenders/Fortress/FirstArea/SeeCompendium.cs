using AgateLib;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Threading.Tasks;
using Xle.Maps;
using Xle.Services.ScreenModel;
using Xle.XleEventTypes.Extenders;

namespace Xle.Ancients.MapExtenders.Fortress.FirstArea
{
    [Transient("SeeCompendium")]
    public class SeeCompendium : EventExtender
    {
        private bool paralyzed = false;

        public IStatsDisplay StatsDisplay { get; set; }

        public override async Task<bool> StepOn()
        {
            if (paralyzed)
                return false;

            // make sure the player is entirely contained by the event.
            if (Player.X != TheEvent.Location.X)
                return false;

            await TextArea.PrintLine();

            paralyzed = true;

            Guard warlord = new Guard();
            warlord.Location = new Point(106, 47);
            warlord.Color = XleColor.LightGreen;

            Map.Guards.Add(warlord);

            await PrintSeeCompendiumMessage();
            await DoSonicMagic(GameState, warlord);

            await TextArea.PrintLine("The warlord appears at the wall.");
            await TextArea.PrintLine();

            await MoveWarlordToCompendium(warlord);
            await HitPlayer();
            await WarlordSpeech();
            RemoveCompendium();

            await MoveWarlordOut(warlord);

            GameState.Map.Guards.Remove(warlord);

            return true;
        }

        private void RemoveCompendium()
        {
            var evt = MapExtender.Events.OfType<CompendiumFirst>().First();

            evt.Enabled = false;
            evt.TheEvent.SetOpenTilesOnMap(Map);
        }

        public override async Task<bool> TryToStepOn(int dx, int dy)
        {
            if (Player.HP > 30 && paralyzed)
            {
                paralyzed = false;
                Enabled = false;
            }
            else if (paralyzed)
            {
                await TextArea.PrintLine("Legs paralyzed.");
                return false;
            }

            return true;
        }
        private async Task DoSonicMagic(GameState state, Guard warlord)
        {
            await TextArea.PrintLine("Sonic magic...");
            await TextArea.PrintLine();
            await GameControl.WaitAsync(3000);
            await TextArea.PrintLine("You can't move.");
            await TextArea.PrintLine();
            await GameControl.WaitAsync(3000);
        }

        private async Task MoveWarlordOut(Guard warlord)
        {
            for (int i = 0; i < 6; i++)
            {
                await MoveWarlord(warlord, 1, 0);
            }

            for (int i = 0; i < 2; i++)
            {
                await MoveWarlord(warlord, 0, 1);
            }

            await GameControl.WaitAsync(1000);
        }

        private async Task MoveWarlordToCompendium(Guard warlord)
        {

            for (int i = 0; i < 5; i++)
            {
                await MoveWarlord(warlord, -1, 0);
            }

            for (int i = 0; i < 2; i++)
            {
                await MoveWarlord(warlord, 0, -1);
            }

            for (int i = 0; i < 2; i++)
            {
                await MoveWarlord(warlord, -1, 0);
            }

            warlord.Facing = Direction.South;
        }

        private async Task WarlordSpeech()
        {
            await TextArea.PrintSlow("You fool!  ", XleColor.Yellow);
            await TextArea.PrintSlow("You can't stop me!  ", XleColor.Yellow);
            await TextArea.PrintLineSlow("as you", XleColor.Yellow);
            await TextArea.PrintLineSlow("stand helpless, I'll use this scroll", XleColor.Yellow);
            await TextArea.PrintSlow("to cast the ", XleColor.Yellow);
            await TextArea.PrintSlow("spell of death. ");
            await TextArea.PrintLineSlow("All life", XleColor.Yellow);
            await TextArea.PrintLineSlow("outside this fortress will cease.", XleColor.Yellow);

            TextArea.SetLineColor(XleColor.Red, 0, 1, 2, 3, 4);

            await GameControl.WaitAsync(2000);
        }

        private async Task HitPlayer()
        {
            Player.HP = 28;

            await GameControl.FlashHPWhile(XleColor.Red, XleColor.Yellow, new CountdownTimer(1500).StillRunning);
        }

        private async Task MoveWarlord(Guard warlord, int dx, int dy)
        {
            warlord.X += dx;
            warlord.Y += dy;
            warlord.Facing = new Point(dx, dy).ToDirection();

            SoundMan.PlaySound(LotaSound.WalkOutside);
            await GameControl.WaitAsync(750);
        }

        private async Task PrintSeeCompendiumMessage()
        {
            TextArea.Clear(true);
            await TextArea.PrintLine();
            await TextArea.PrintLine("    you see the compendium!");
            await TextArea.PrintLine();

            SoundMan.PlaySound(LotaSound.VeryGood);
            await GameControl.WaitAsync(1500);

            await TextArea.FlashLinesWhile(() => SoundMan.IsPlaying(LotaSound.VeryGood), XleColor.Yellow, XleColor.Cyan, 100);

            await GameControl.WaitAsync(1000);
        }
    }
}
