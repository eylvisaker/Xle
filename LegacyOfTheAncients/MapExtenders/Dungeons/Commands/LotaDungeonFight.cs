using AgateLib;
using System;

using Xle.Maps;
using Xle.Maps.Dungeons.Commands;

namespace Xle.Ancients.MapExtenders.Dungeons.Commands
{
    [Transient("LotaDungeonFight")]
    public class LotaDungeonFight : DungeonFight
    {
        protected override bool RollToHitMonster(DungeonMonster monster)
        {
            return Random.NextDouble() * 70 < Player.Attribute[Attributes.dexterity] + 30;
        }

        protected override int RollDamageToMonster(DungeonMonster monster)
        {
            double damage = Player.Attribute[Attributes.strength] + 30;
            damage /= 45;

            var weapon = Player.CurrentWeapon;

            double vd = weapon.ID + 1 + weapon.Quality / 2.8;

            damage *= vd + 4;

            if (Player.WeaponEnchantTurnsRemaining > 0)
            {
                damage *= 2.5;
            }
            else
                damage *= 1.5;

            return (int)(damage * (0.5 + Random.NextDouble()));
        }

    }
}
