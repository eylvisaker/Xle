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
			if (state.MapExtender.PlayerClimb(state) == false)
			{
				XleCore.TextArea.PrintLine("\n\nNothing to climb");
			}
		}
	}
}
