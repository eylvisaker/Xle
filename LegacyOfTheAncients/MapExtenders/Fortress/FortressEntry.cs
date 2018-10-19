using Xle.Ancients.MapExtenders.Castle;
using Xle.Ancients.MapExtenders.Castle.Events;
using Xle.Ancients.MapExtenders.Fortress.FirstArea;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics.Geometry;
using Xle.XleEventTypes;
using Microsoft.Xna.Framework;

namespace Xle.Ancients.MapExtenders.Fortress
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
