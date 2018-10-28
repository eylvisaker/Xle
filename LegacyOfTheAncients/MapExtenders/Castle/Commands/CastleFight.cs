using AgateLib;
using System;

using Xle.Maps;
using Xle.Maps.Towns;

namespace Xle.Ancients.MapExtenders.Castle.Commands
{
    [Transient("CastleFight")]
    public class CastleFight : FightAgainstGuard
    {
        public int WhichCastle { get; set; }
        public int CastleLevel { get; set; }

        public override double ChanceToHitGuard(Guard guard, int distance)
        {
            int weaponType = Player.CurrentWeapon.ID;
            double GuardDefense = 1;

            if (WhichCastle == 2)
                GuardDefense = Player.Attribute[Attributes.dexterity] / 26.0;

            return (Player.Attribute[Attributes.dexterity] + 13)
                * (99 + weaponType * 11) / 7500.0 / GuardDefense;
        }


        public override int RollDamageToGuard(Guard guard)
        {
            int weaponType = Player.CurrentWeapon.ID;

            double damage = Player.Attribute[Attributes.strength] *
                       (weaponType / 2 + 1) / 7;

            damage *= 1 + 2 * Random.NextDouble();

            return (int)Math.Round(damage);
        }


    }
}
