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
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("Nothing opens.");

				XleCore.Wait(500);
			}
		}
	}
}
