using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Commands
{
	public class Open : Command 
	{
		public override void Execute(GameState state)
		{
			if (state.Map.PlayerOpen(state.Player) == false)
			{
				g.AddBottom("");
				g.AddBottom("Nothing opens.");

				XleCore.Wait(500);
			}
		}
	}
}
