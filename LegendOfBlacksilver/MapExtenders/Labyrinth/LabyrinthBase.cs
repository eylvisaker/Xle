using AgateLib;
using Microsoft.Xna.Framework;
using System;
using Xle.Maps;
using Xle.Maps.Castles;
using Xle.Services.Commands;

namespace Xle.Blacksilver.MapExtenders.Labyrinth
{
    [Transient("LabyrinthBase")]
    public class LabyrinthBase : CastleExtender
    {
        private CastleDamageCalculator cdc;

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
            commands.Items.Add(CommandFactory.Armor());
            commands.Items.Add(CommandFactory.Gamespeed());
            commands.Items.Add(CommandFactory.Inventory());
            commands.Items.Add(CommandFactory.Pass());
            commands.Items.Add(CommandFactory.Weapon());

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
