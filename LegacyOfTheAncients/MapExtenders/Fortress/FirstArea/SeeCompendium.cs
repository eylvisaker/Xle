using AgateLib.Geometry;
using ERY.Xle.Maps;
using ERY.Xle.Maps.Extenders;
using ERY.Xle.Services.Implementation;
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

        public override bool StepOn(GameState state)
        {
            if (paralyzed)
                return false;

            // make sure the player is entirely contained by the event.
            if (Player.X != TheEvent.Location.X)
                return false;

            TextArea.PrintLine();

            paralyzed = true;

            Guard warlord = new Guard();
            warlord.Location = new AgateLib.Geometry.Point(106, 47);
            warlord.Color = XleColor.LightGreen;

            state.Map.Guards.Add(warlord);

            PrintSeeCompendiumMessage(state);
            DoSonicMagic(state, warlord);

            TextArea.PrintLine("The warlord appears at the wall.");
            TextArea.PrintLine();

            MoveWarlordToCompendium(warlord);
            HitPlayer(state);
            WarlordSpeech();
            RemoveCompendium(state);

            MoveWarlordOut(warlord);

            state.Map.Guards.Remove(warlord);

            return true;
        }

        private void RemoveCompendium(GameState state)
        {
            TreasureChestEvent evt = GameState.Map.Events.OfType<TreasureChestEvent>()
                .First(x => x.ExtenderName.Equals("CompendiumFirst", StringComparison.InvariantCultureIgnoreCase));

            evt.Enabled = false;
            evt.SetOpenTilesOnMap(GameState.Map);
        }

        public override void TryToStepOn(GameState state, int dx, int dy, out bool allowStep)
        {
            allowStep = true;
            if (Player.HP > 30 && paralyzed)
            {
                paralyzed = false;
                TheEvent.Enabled = false;
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

        private void HitPlayer(GameState state)
        {
            state.Player.HP = 28;

            XleCore.FlashHPWhile(XleColor.Red, XleColor.Yellow, new CountdownTimer(1500).StillRunning);

        }

        private void MoveWarlord(Guard warlord, int dx, int dy)
        {
            warlord.X += dx;
            warlord.Y += dy;
            warlord.Facing = new Point(dx, dy).ToDirection();

            SoundMan.PlaySound(LotaSound.WalkOutside);
            GameControl.Wait(750);
        }

        private void PrintSeeCompendiumMessage(GameState state)
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
