using Xle.Blacksilver.MapExtenders.Citadel.EventExtenders;
using Xle.Maps.Castles;
using Xle.Services;
using Xle.Services.Commands;
using Xle.XleEventTypes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics.Geometry;
using Xle.Maps;
using Microsoft.Xna.Framework;

namespace Xle.Blacksilver.MapExtenders.Citadel
{
    public class CitadelGround : CastleExtender
    {
        CastleDamageCalculator cdc;

        public CitadelGround()
        {
            cdc = new CastleDamageCalculator (Random) { v5 = 1.3, v6 = 1.5, v7 = 1.5 };
        }

        public override void SetColorScheme(ColorScheme scheme)
        {
            scheme.TextColor = XleColor.White;

            scheme.FrameColor = XleColor.Orange;
            scheme.FrameHighlightColor = XleColor.Yellow;
        }

        public override void SetCommands(ICommandList commands)
        {
            commands.Items.AddRange(LobProgram.CommonLobCommands);

            var fight = (LobCastleFight)CommandFactory.Fight("LobCastleFight");
            fight.DamageCalculator = cdc;

            commands.Items.Add(fight);
            
            commands.Items.Add(CommandFactory.Open());
            commands.Items.Add(CommandFactory.Magic("LobCastleMagic"));
            commands.Items.Add(CommandFactory.Take());
            commands.Items.Add(CommandFactory.Use("LobUse"));
            commands.Items.Add(CommandFactory.Speak());
            commands.Items.Add(CommandFactory.Xamine());
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
            if (playerPoint.X >= 83 && playerPoint.Y >= 65)
                return 33;
            else
                return 23;
        }

    }
}
