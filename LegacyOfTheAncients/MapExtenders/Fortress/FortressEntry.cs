using AgateLib;
using Microsoft.Xna.Framework;
using Xle.Ancients.MapExtenders.Castle;

namespace Xle.Ancients.MapExtenders.Fortress
{
    [Transient("FortressEntry")]
    public class FortressEntry : CastleGround
    {
        public FortressEntry()
        {
            WhichCastle = 2;
            CastleLevel = 1;
            GuardAttack = 3.5;
        }

        public override void SetColorScheme(ColorScheme scheme)
        {
            scheme.TextColor = XleColor.White;

            scheme.FrameColor = XleColor.Gray;
            scheme.FrameHighlightColor = XleColor.Yellow;
        }

        protected override void OnSetAngry(bool value)
        {
            base.OnSetAngry(value);

            Player.RenderColor = XleColor.White;
        }

        public override int GetOutsideTile(Point playerPoint, int x, int y)
        {
            if (x >= 0)
                return 11;
            else
                return 0;
        }

    }
}
