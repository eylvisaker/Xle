using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Stores
{

	public class StoreVault : Store
	{
		public override bool Speak(GameState state)
		{
			var player = state.Player;

			return false;
		}
	}

}
