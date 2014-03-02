using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Commands
{
	public class Fight : Command
	{
		public override void Execute(GameState state)
		{
			XleCore.Map.PlayerFight(state.Player);
		}
	}
}
