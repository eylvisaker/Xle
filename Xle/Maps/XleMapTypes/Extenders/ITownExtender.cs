using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps.XleMapTypes.Extenders
{
	public interface ITownExtender : IMapExtender
	{
		void SpeakToGuard(GameState state, ref bool handled);

		void OnSetAngry(bool value);

		double ChanceToHitGuard(Player player, Guard guard, int distance);

		int RollDamageToGuard(Player player, Guard guard);

	}
}
