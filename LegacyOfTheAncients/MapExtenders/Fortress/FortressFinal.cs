using ERY.Xle.LotA.MapExtenders.Fortress.SecondArea;
using ERY.Xle.Services.ScreenModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace ERY.Xle.LotA.MapExtenders.Fortress
{
    public class FortressFinal : FortressEntry
    {
        private IFortressFinalActivator fortressActivator;
        private int borderIndex;
        private Color flashColor = XleColor.LightGreen;
        private double compendiumStrength = 140;

        public FortressFinal(IFortressFinalActivator fortressActivator)
        {
            this.fortressActivator = fortressActivator;
            fortressActivator.Reset();
            fortressActivator.WarlordCreated += (sender, e) => TheMap.Guards.Add(fortressActivator.Warlord);

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

        public override void AfterExecuteCommand(Keys cmd)
        {
            if (fortressActivator.Warlord != null)
            {
                WarlordAttack();
            }
            else if (fortressActivator.CompendiumAttacking)
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
            GameControl.Wait(250, redraw: FlashBorder);
            TheMap.ColorScheme.FrameColor = XleColor.Gray;

            Player.HP -= damage;

            GameControl.Wait(75);

            if (Player.Items[LotaItem.HealingHerb] > 24)
            {
                int amount = Player.Items[LotaItem.HealingHerb] / 9;

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
            GameControl.Wait(250, redraw: FlashBorder);
            TheMap.ColorScheme.FrameColor = XleColor.Gray;

            GameControl.Wait(150);
        }
    }
}
