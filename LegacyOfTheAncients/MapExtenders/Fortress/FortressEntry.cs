using ERY.Xle.LotA.MapExtenders.Castle;
using ERY.Xle.LotA.MapExtenders.Castle.Events;
using ERY.Xle.LotA.MapExtenders.Fortress.FirstArea;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics.Geometry;
using ERY.Xle.XleEventTypes;

namespace ERY.Xle.LotA.MapExtenders.Fortress
{
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
