﻿using AgateLib.Geometry;
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

        public override double ChanceToHitGuard(Player player, Guard guard, int distance)
        {
            return cdc.ChanceToHitGuard(player, distance);
        }
        public override double ChanceToHitPlayer(Player player, Guard guard)
        {
            return cdc.ChanceToHitPlayer(player);
        }
        public override int RollDamageToGuard(Player player, Guard guard)
        {
            return cdc.RollDamageToGuard(player);
        }
        public override int RollDamageToPlayer(Player player, Guard guard)
        {
            return cdc.RollDamageToPlayer(player);
        }

        public override int GetOutsideTile(Point playerPoint, int x, int y)
        {
            return TheMap.OutsideTile;
        }
    }
}
