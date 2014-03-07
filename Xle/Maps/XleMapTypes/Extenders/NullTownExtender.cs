using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps.XleMapTypes.Extenders
{
	public class NullTownExtender : NullMapExtender, ITownExtender
	{
		public override void SetColorScheme(ColorScheme scheme)
		{
			scheme.TextColor = XleColor.White;

			scheme.FrameColor = XleColor.Orange;
			scheme.FrameHighlightColor = XleColor.Yellow;
		}

		public virtual void SpeakToGuard(GameState state, ref bool handled)
		{
		}


		public virtual void OnSetAngry(bool value)
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
	}
}
