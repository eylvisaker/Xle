using AgateLib.Geometry;
using ERY.Xle.Maps;
using ERY.Xle.Services.Rendering;
using ERY.Xle.Services.ScreenModel;
using ERY.Xle.XleEventTypes;
using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Fortress.FirstArea
{
    public class SeeCompendium : EventExtender
    {
        bool paralyzed = false;

        public IStatsDisplay StatsDisplay { get; set; }

        public override bool StepOn()
        {
            if (paralyzed)
                return false;

            // make sure the player is entirely contained by the event.
            if (Player.X != TheEvent.Location.X)
                return false;

            TextArea.PrintLine();

            paralyzed = true;

            Guard warlord = new Guard();
            warlord.Location = new Point(106, 47);
            warlord.Color = XleColor.LightGreen;

            Map.Guards.Add(warlord);

            PrintSeeCompendiumMessage();
            DoSonicMagic(GameState, warlord);

            TextArea.PrintLine("The warlord appears at the wall.");
            TextArea.PrintLine();

            MoveWarlordToCompendium(warlord);
            HitPlayer();
            WarlordSpeech();
            RemoveCompendium();

            MoveWarlordOut(warlord);

            GameState.Map.Guards.Remove(warlord);

            return true;
        }

        private void RemoveCompendium()
        {
            var evt = MapExtender.Events.OfType<CompendiumFirst>().First();

            evt.Enabled = false;
            evt.TheEvent.SetOpenTilesOnMap(Map);
        }

        public override void TryToStepOn(int dx, int dy, out bool allowStep)
        {
            allowStep = true;
            if (Player.HP > 30 && paralyzed)
            {
                paralyzed = false;
                Enabled = false;
            }
            else if (paralyzed)
            {
                TextArea.PrintLine("Legs paralyzed.");
                allowStep = false;
            }
        }
        private void DoSonicMagic(GameState state, Guard warlord)
        {
            TextArea.PrintLine("Sonic magic...");
            TextArea.PrintLine();
            GameControl.Wait(3000);
            TextArea.PrintLine("You can't move.");
            TextArea.PrintLine();
            GameControl.Wait(3000);

        }

        private void MoveWarlordOut(Guard warlord)
        {
            for (int i = 0; i < 6; i++)
            {
                MoveWarlord(warlord, 1, 0);
            }

            for (int i = 0; i < 2; i++)
            {
                MoveWarlord(warlord, 0, 1);
            }

            GameControl.Wait(1000);
        }

        private void MoveWarlordToCompendium(Guard warlord)
        {

            for (int i = 0; i < 5; i++)
            {
                MoveWarlord(warlord, -1, 0);
            }

            for (int i = 0; i < 2; i++)
            {
                MoveWarlord(warlord, 0, -1);
            }

            for (int i = 0; i < 2; i++)
            {
                MoveWarlord(warlord, -1, 0);
            }

            warlord.Facing = Direction.South;
        }

        private void WarlordSpeech()
        {
            TextArea.PrintSlow("You fool!  ", XleColor.Yellow);
            TextArea.PrintSlow("You can't stop me!  ", XleColor.Yellow);
            TextArea.PrintLineSlow("as you", XleColor.Yellow);
            TextArea.PrintLineSlow("stand helpless, I'll use this scroll", XleColor.Yellow);
            TextArea.PrintSlow("to cast the ", XleColor.Yellow);
            TextArea.PrintSlow("spell of death. ");
            TextArea.PrintLineSlow("All life", XleColor.Yellow);
            TextArea.PrintLineSlow("outside this fortress will cease.", XleColor.Yellow);

            TextArea.SetLineColor(XleColor.Red, 0, 1, 2, 3, 4);

            GameControl.Wait(2000);
        }

        private void HitPlayer()
        {
            Player.HP = 28;

            StatsDisplay.FlashHPWhile(XleColor.Red, XleColor.Yellow, new CountdownTimer(1500).StillRunning);
        }

        private void MoveWarlord(Guard warlord, int dx, int dy)
        {
            warlord.X += dx;
            warlord.Y += dy;
            warlord.Facing = new Point(dx, dy).ToDirection();

            SoundMan.PlaySound(LotaSound.WalkOutside);
            GameControl.Wait(750);
        }

        private void PrintSeeCompendiumMessage()
        {
            TextArea.Clear(true);
            TextArea.PrintLine();
            TextArea.PrintLine("    you see the compendium!");
            TextArea.PrintLine();

            SoundMan.PlaySound(LotaSound.VeryGood);
            GameControl.Wait(1500);

            TextArea.FlashLinesWhile(() => SoundMan.IsPlaying(LotaSound.VeryGood), XleColor.Yellow, XleColor.Cyan, 100);

            GameControl.Wait(1000);
        }
    }
}
