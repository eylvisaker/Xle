using ERY.Xle.LoB.MapExtenders.Labyrinth.EventExtenders;
using ERY.Xle.Services;
using ERY.Xle.Services.Implementation.Commands;
using ERY.Xle.XleEventTypes;
using ERY.Xle.Maps.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERY.Xle.Maps;
using AgateLib.Geometry;

namespace ERY.Xle.LoB.MapExtenders.Labyrinth
{
    public class LabyrinthBase : CastleExtender
    {
        CastleDamageCalculator cdc;

        public LabyrinthBase(Random random)
        {
            cdc = new CastleDamageCalculator(random) { v5 = 1.7, v6 = 18, v7 = 1.7 };
        }

        public override void SetColorScheme(ColorScheme scheme)
        {
            scheme.TextColor = XleColor.White;

            scheme.FrameColor = XleColor.Gray;
            scheme.FrameHighlightColor = XleColor.Yellow;
        }

        public override void SetCommands(ICommandList commands)
        {
            commands.Items.AddRange(LobProgram.CommonLobCommands);

            commands.Items.Add(CommandFactory.Open());
            commands.Items.Add(CommandFactory.Magic());
            commands.Items.Add(CommandFactory.Take());
            commands.Items.Add(CommandFactory.Speak());
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
            if (playerPoint.Y < 22 && playerPoint.X > 35 && playerPoint.X < 70)
                return base.GetOutsideTile(playerPoint, x, y);
            else
                return 22;
        }
    }
}
