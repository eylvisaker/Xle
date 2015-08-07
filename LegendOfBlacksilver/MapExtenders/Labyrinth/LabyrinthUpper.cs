using AgateLib.Geometry;
using ERY.Xle.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Maps.Castles;
using ERY.Xle.Services.Commands;

namespace ERY.Xle.LoB.MapExtenders.Labyrinth
{
    public class LabyrinthUpper : CastleExtender
    {
        CastleDamageCalculator cdc;

        public LabyrinthUpper(Random random)
        {
            cdc = new CastleDamageCalculator(random) { v5 = 2.1, v6 = 29, v7 = 2.5 };
        }

        public override void SetCommands(ICommandList commands)
        {
            commands.Items.AddRange(LobProgram.CommonLobCommands);

            var fight = (LobCastleFight)CommandFactory.Fight("LobCastleFight");
            fight.DamageCalculator = cdc;

            commands.Items.Add(fight);
            commands.Items.Add(CommandFactory.Magic());
            commands.Items.Add(CommandFactory.Open());
            commands.Items.Add(CommandFactory.Speak());
            commands.Items.Add(CommandFactory.Take());
            commands.Items.Add(CommandFactory.Use("LobUse"));
            commands.Items.Add(CommandFactory.Xamine());
        }

        public override void SetColorScheme(ColorScheme scheme)
        {
            scheme.TextColor = XleColor.White;

            scheme.FrameColor = XleColor.DarkGray;
            scheme.FrameHighlightColor = XleColor.Yellow;
        }

        public override double ChanceToHitPlayer(Guard guard)
        {
            return cdc.ChanceToHitPlayer(Player);
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
