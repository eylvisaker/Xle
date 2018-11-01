using AgateLib;

using Xle.Maps;
using Xle.Maps.Towns;

namespace Xle.Blacksilver.MapExtenders
{
    [Transient("LobCastleFight")]
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
