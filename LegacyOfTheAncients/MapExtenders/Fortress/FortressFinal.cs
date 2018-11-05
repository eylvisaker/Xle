using AgateLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading.Tasks;
using Xle.Ancients.MapExtenders.Fortress.SecondArea;
using Xle.ScreenModel;

namespace Xle.Ancients.MapExtenders.Fortress
{
    [Transient("FortressFinal")]
    public class FortressFinal : FortressEntry
    {
        private readonly IFortressFinalActivator fortressActivator;
        private readonly FlashBorderRenderer flashBorderRenderer;
        private Color flashColor = XleColor.LightGreen;
        private double compendiumStrength = 140;

        public FortressFinal(IFortressFinalActivator fortressActivator, FlashBorderRenderer flashBorderRenderer)
        {
            this.fortressActivator = fortressActivator;
            this.flashBorderRenderer = flashBorderRenderer;
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

        public override async Task AfterExecuteCommand(Keys cmd)
        {
            if (fortressActivator.Warlord != null)
            {
                await WarlordAttack();
            }
            else if (fortressActivator.CompendiumAttacking)
            {
                await CompendiumAttack();
            }

            await base.AfterExecuteCommand(cmd);
        }

        private async Task CompendiumAttack()
        {
            int damage = Random.Next((int)compendiumStrength / 2, (int)compendiumStrength);

            await GameControl.WaitAsync(75);
            await TextArea.PrintLine();
            await TextArea.PrintLine("Compendium attack - blow " + damage + " H.P.", XleColor.Green);

            await GameControl.PlayMagicSound(LotaSound.MagicBolt, LotaSound.MagicBoltHit, 2);

            await GameControl.WaitAsync(250, redraw: flashBorderRenderer);

            TheMap.ColorScheme.FrameColor = XleColor.Gray;

            Player.HP -= damage;

            await GameControl.WaitAsync(75);

            if (Player.Items[LotaItem.HealingHerb] > 24)
            {
                int amount = Player.Items[LotaItem.HealingHerb] / 9;

                amount = (int)(amount * (1 + Random.NextDouble()) * 0.5);

                await TextArea.PrintLine("** " + amount.ToString() + " healing herbs destroyed! **", XleColor.Yellow);

                Player.Items[LotaItem.HealingHerb] -= amount;

                await GameControl.WaitAsync(75);
            }
        }

        private async Task WarlordAttack()
        {
            int damage = (int)(99 * Random.NextDouble() + 80);

            TheMap.ColorScheme.FrameColor = XleColor.Pink;
            flashColor = XleColor.Red;

            await TextArea.PrintLine();
            await TextArea.PrintLine("Warlord attack - blow " + damage.ToString() + " H.P.", XleColor.Yellow);

            Player.HP -= damage;

            await GameControl.PlayMagicSound(LotaSound.MagicFlame, LotaSound.MagicFlameHit, 2);
            await GameControl.WaitAsync(250, redraw: flashBorderRenderer);
            TheMap.ColorScheme.FrameColor = XleColor.Gray;

            await GameControl.WaitAsync(150);
        }
    }

    [Transient]
    public class FlashBorderRenderer : Renderer
    {
        private int borderIndex;

        public Color FlashColor { get; set; }

        public override void Draw(SpriteBatch spriteBatch)
        {
            borderIndex++;

            if (borderIndex % 4 < 2)
                TheMap.ColorScheme.FrameColor = FlashColor;
            else
                TheMap.ColorScheme.FrameColor = XleColor.Gray;

            XleRenderer.Draw(spriteBatch);
        }
    }

}
