using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Maps;
using ERY.Xle.Maps.Towns;
using ERY.Xle.Services;

namespace ERY.Xle.LoB.MapExtenders
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
