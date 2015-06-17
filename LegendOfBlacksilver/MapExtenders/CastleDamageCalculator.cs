using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services.Implementation;

namespace ERY.Xle.LoB.MapExtenders
{
	class CastleDamageCalculator
	{
		public double v5, v6, v7;


		public double ChanceToHitGuard(Player player, int distance)
		{
			double distanceChance = Math.Pow(distance - 1, 0.8) / 10.0;
			double hitChance = (player.Attribute[Attributes.dexterity] + 33) *
				(99 + player.CurrentWeapon.ID * 11) / 9900.0 / v5;

			return hitChance * (1 - distanceChance);
		}

		public int RollDamageToGuard(Player player)
		{
			var dam = (player.Attribute[Attributes.strength] + 5) *
				(player.CurrentWeapon.ID / 2.0 + 1) / 7.0 / v7;

			dam += 2 * XleCore.random.NextDouble() * dam;

			return (int)dam;
		}

		public int RollDamageToPlayer(Player player)
		{
			var dam = v6 * (300 + XleCore.random.NextDouble()*600) / 
				(player.CurrentArmor.ID + 2) / 
				Math.Pow(player.Attribute[Attributes.endurance], 0.9) + 2;

			return (int)dam;
		}

		public double ChanceToHitPlayer(Player player)
		{
			return 1 - player.Attribute[Attributes.dexterity] / 99.0;
		}
	}
}
