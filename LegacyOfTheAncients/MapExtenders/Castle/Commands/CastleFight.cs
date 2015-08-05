using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Maps;
using ERY.Xle.Maps.Towns;
using ERY.Xle.Services;

namespace ERY.Xle.LotA.MapExtenders.Castle.Commands
{
    [ServiceName("CastleFight")]
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
