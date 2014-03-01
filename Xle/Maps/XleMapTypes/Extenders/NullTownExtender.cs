using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.Extenders
{
	public class NullTownExtender : NullMapExtender, ITownExtender
	{
		public override void SetColorScheme(ColorScheme scheme)
		{
			scheme.TextColor = XleColor.White;

			scheme.FrameColor = XleColor.Orange;
			scheme.FrameHighlightColor = XleColor.Yellow;

			scheme.VerticalLinePosition = 13 * 16;
		}

		public virtual void SpeakToGuard(GameState state, ref bool handled)
		{
		}


		public virtual void SetAngry(bool value)
		{
		}



		public virtual double ChanceToHitGuard(Player player, Guard guard, int distance)
		{
			int weaponType = player.CurrentWeaponType;

			return (player.Attribute[Attributes.dexterity] + 16)
				* (99 + weaponType * 8) / 7000.0 / guard.Defense * 99;
		}


		public virtual int RollDamageToGuard(Player player, Guard guard)
		{
			int weaponType = player.CurrentWeaponType;

			double damage = 1 + player.Attribute[Attributes.strength] *
					   (weaponType / 2 + 1) / 4;

			damage *= 0.5 + XleCore.random.NextDouble();

			return (int)Math.Round(damage);
		}


		public virtual double ChanceToHitPlayer(Player player, Guard guard)
		{
			return (player.Attribute[Attributes.dexterity] / 80.0);
		}


		public virtual int RollDamageToPlayer(Player player, Guard guard)
		{
			int armorType = player.CurrentArmorType;

			double damage = guard.Attack / 99.0 *
							   (120 + XleCore.random.NextDouble() * 250) /
							   Math.Pow(armorType + 3, 0.8) /
								   Math.Pow(player.Attribute[Attributes.endurance], 0.8) + 3;

			return (int)Math.Round(damage);
		}
	}
}
