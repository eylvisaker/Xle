using Xle.LoB.MapExtenders.Labyrinth.EventExtenders;
using Xle.Maps.Castles;
using Xle.Services;
using Xle.Services.Commands;
using Xle.XleEventTypes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xle.Maps;
using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;

namespace Xle.LoB.MapExtenders.Labyrinth
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

            var fight = (LobCastleFight)CommandFactory.Fight("LobCastleFight");
            fight.DamageCalculator = cdc;

            commands.Items.Add(fight);
            commands.Items.Add(CommandFactory.Magic("LobCastleMagic"));
            commands.Items.Add(CommandFactory.Open());
            commands.Items.Add(CommandFactory.Speak());
            commands.Items.Add(CommandFactory.Take());
            commands.Items.Add(CommandFactory.Use("LobUse"));
            commands.Items.Add(CommandFactory.Xamine());
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
            if (playerPoint.Y < 22 && playerPoint.X > 35 && playerPoint.X < 70)
                return base.GetOutsideTile(playerPoint, x, y);
            else
                return 22;
        }
    }
}
