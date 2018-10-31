using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Maps;
using Xle.Maps.Towns;
using Xle.Services;

namespace Xle.Blacksilver.MapExtenders
{
    [ServiceName("LobCastleFight")]
    public class LobCastleFight : FightAgainstGuard
    {
        public CastleDamageCalculator DamageCalculator { get; set; }

        public override double ChanceToHitGuard(Guard guard, int distance)
        {
            return DamageCalculator.ChanceToHitGuard(Player, distance);
        }

        public override int RollDamageToGuard(Guard guard)
        {
            return DamageCalculator.RollDamageToGuard(Player);
        }
    }
}
