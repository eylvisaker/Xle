using AgateLib.Geometry;
using AgateLib.InputLib;

using ERY.Xle.LotA.MapExtenders.Fortress.FirstArea;
using ERY.Xle.LotA.MapExtenders.Fortress.SecondArea;
using ERY.Xle.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services.ScreenModel;
using ERY.Xle.XleEventTypes;
using ERY.Xle.Services;

namespace ERY.Xle.LotA.MapExtenders.Fortress
{
    public class FortressFinal : FortressEntry
    {
        Guard warlord;

        int borderIndex;
        Color flashColor = XleColor.LightGreen;

        double compendiumStrength = 140;

        public FortressFinal()
        {
            WhichCastle = 2;
            CastleLevel = 2;
            GuardAttack = 3.5;
        }

        public IXleScreen Screen { get; set; }

        public override int GetOutsideTile(Point playerPoint, int x, int y)
        {
            if (y >= TheMap.Height)
                return 0;
            else
                return 11;
        }

        public bool CompendiumAttacking { get; set; }

        public override void AfterExecuteCommand(KeyCode cmd)
        {
            if (warlord != null)
            {
                WarlordAttack();
            }
            else if (CompendiumAttacking)
            {
                CompendiumAttack();
            }

            base.AfterExecuteCommand(cmd);
        }

        private void CompendiumAttack()
        {
            int damage = Random.Next((int)compendiumStrength / 2, (int)compendiumStrength);

            GameControl.Wait(75);
            TextArea.PrintLine();
            TextArea.PrintLine("Compendium attack - blow " + damage + " H.P.", XleColor.Green);

            SoundMan.PlayMagicSound(LotaSound.MagicBolt, LotaSound.MagicBoltHit, 2);
            GameControl.Wait(250, redraw:FlashBorder);
            TheMap.ColorScheme.FrameColor = XleColor.Gray;

            Player.HP -= damage;

            GameControl.Wait(75);

            if (Player.Items[LotaItem.HealingHerb] > 24)
            {
                int amount = (int)(Player.Items[LotaItem.HealingHerb] / 9);

                amount = (int)(amount * (1 + Random.NextDouble()) * 0.5);

                TextArea.PrintLine("** " + amount.ToString() + " healing herbs destroyed! **", XleColor.Yellow);

                Player.Items[LotaItem.HealingHerb] -= amount;

                GameControl.Wait(75);
            }
        }

        private void FlashBorder()
        {
            borderIndex++;

            if (borderIndex % 4 < 2)
                TheMap.ColorScheme.FrameColor = flashColor;
            else
                TheMap.ColorScheme.FrameColor = XleColor.Gray;

            Screen.OnDraw();
        }

        private void WarlordAttack()
        {
            int damage = (int)(99 * Random.NextDouble() + 80);

            TheMap.ColorScheme.FrameColor = XleColor.Pink;
            flashColor = XleColor.Red;

            TextArea.PrintLine();
            TextArea.PrintLine("Warlord attack - blow " + damage.ToString() + " H.P.", XleColor.Yellow);

            Player.HP -= damage;

            SoundMan.PlayMagicSound(LotaSound.MagicFlame, LotaSound.MagicFlameHit, 2);
            GameControl.Wait(250, redraw:FlashBorder);
            TheMap.ColorScheme.FrameColor = XleColor.Gray;

            GameControl.Wait(150);
        }


        public void CreateWarlord()
        {
            warlord = new Guard
            {
                X = 5,
                Y = 45,
                HP = 420,
                Color = XleColor.LightGreen,
                Name = "Warlord",
                OnGuardDead = (state, unused) => WarlordDead(unused),
                SkipAttacking = true,
                SkipMovement = true,
            };

            TheMap.Guards.Add(warlord);
        }

        private bool WarlordDead(Guard unused)
        {
            this.warlord = null;

            TextArea.Clear(true);
            TextArea.PrintLine();
            TextArea.PrintLine("        ** warlord killed **");

            for (int i = 0; i < 5; i++)
            {
                SoundMan.PlaySound(LotaSound.Good);
                GameControl.Wait(750);
            }
            GameControl.Wait(1000);

            SoundMan.PlaySoundSync(LotaSound.VeryGood);

            PrintSecurityAlertMessage();

            return true;
        }

        private void PrintSecurityAlertMessage()
        {
        }
    }
}
