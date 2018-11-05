using AgateLib;
using Microsoft.Xna.Framework;
using System;
using Xle.Maps;
using Xle.Maps.Castles;
using Xle.Commands;

namespace Xle.Blacksilver.MapExtenders.Citadel
{
    [Transient("CitadelUpper")]
    public class CitadelUpper : CastleExtender
    {
        private CastleDamageCalculator cdc;

        public CitadelUpper(Random random)
        {
            cdc = new CastleDamageCalculator(random) { v5 = 1.6, v6 = 5.5, v7 = 2.3 };
        }
        public override void SetColorScheme(ColorScheme scheme)
        {
            scheme.TextColor = XleColor.White;

            scheme.FrameColor = XleColor.Orange;
            scheme.FrameHighlightColor = XleColor.Yellow;
        }

        public override void SetCommands(ICommandList commands)
        {
            commands.Items.Add(CommandFactory.Armor());
            commands.Items.Add(CommandFactory.Gamespeed());
            commands.Items.Add(CommandFactory.Inventory());
            commands.Items.Add(CommandFactory.Pass());
            commands.Items.Add(CommandFactory.Weapon());

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
            return TheMap.OutsideTile;
        }

    }
}
