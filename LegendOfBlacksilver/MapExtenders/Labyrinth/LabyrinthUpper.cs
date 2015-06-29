using AgateLib.Geometry;
using ERY.Xle.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Labyrinth
{
    public class LabyrinthUpper : LabyrinthBase
    {
        CastleDamageCalculator cdc;

        public LabyrinthUpper(Random random) : base(random)
        {
            cdc = new CastleDamageCalculator(random) { v5 = 2.1, v6 = 29, v7 = 2.5 };
        }

        public override void SetColorScheme(ColorScheme scheme)
        {
            scheme.TextColor = XleColor.White;

            scheme.FrameColor = XleColor.DarkGray;
            scheme.FrameHighlightColor = XleColor.Yellow;
        }

        public override double ChanceToHitGuard(Guard guard, int distance)
        {
            return cdc.ChanceToHitGuard(Player, distance);
        }
        public override double ChanceToHitPlayer(Guard guard)
        {
            return cdc.ChanceToHitPlayer(Player);
        }
        public override int RollDamageToGuard(Guard guard)
        {
            return cdc.RollDamageToGuard(Player);
        }
        public override int RollDamageToPlayer(Guard guard)
        {
            return cdc.RollDamageToPlayer(Player);
        }

        public override int GetOutsideTile(Point playerPoint, int x, int y)
        {
            return TheMap.OutsideTile;
        }
    }
}
