using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Commands
{
	public class Take : Command
	{
		public override void Execute(GameState state)
		{

			if (state.Map.PlayerTake(state.Player) == false)
			{
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("Nothing to take.");

				XleCore.Wait(500);
			}
		}
	}
}
