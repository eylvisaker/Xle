using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services.Implementation;

namespace ERY.Xle.Commands
{
	public class Rob : Command
	{
		public override void Execute(GameState state)
		{
			if (state.MapExtender.PlayerRob(state) == false)
			{
				XleCore.TextArea.PrintLine("\n\nNothing to rob.");
			}
		}
	}
}
