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
			if (state.MapExtender.PlayerOpen(state) == false)
			{
				XleCore.TextArea.PrintLine("\n\nNothing opens.");

				XleCore.Wait(500);
			}
		}
	}
}
