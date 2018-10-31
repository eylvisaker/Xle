using AgateLib;
using Microsoft.Xna.Framework;
using Xle.Maps;
using Xle.Maps.Castles;
using Xle.Services.Commands;

namespace Xle.Blacksilver.MapExtenders.Citadel
{
    [Transient("CitadelGround")]
    public class CitadelGround : CastleExtender
    {
        private CastleDamageCalculator cdc;

        public CitadelGround()
        {
            cdc = new CastleDamageCalculator(Random) { v5 = 1.3, v6 = 1.5, v7 = 1.5 };
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
            if (playerPoint.X >= 83 && playerPoint.Y >= 65)
                return 33;
            else
                return 23;
        }

    }
}
