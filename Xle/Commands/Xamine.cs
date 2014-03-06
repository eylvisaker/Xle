using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Commands
{
	public class Xamine : Command
	{
		public override void Execute(GameState state)
		{
			state.Map.PlayerXamine(state.Player);
		}
	}
}
