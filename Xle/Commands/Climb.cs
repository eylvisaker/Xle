using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Commands
{
	public class Climb : Command 
	{
		public override void Execute(GameState state)
		{
			if (state.Map.PlayerClimb(state.Player) == false)
			{
				g.AddBottom("");
				g.AddBottom("Nothing to climb");
			}
		}
	}
}
