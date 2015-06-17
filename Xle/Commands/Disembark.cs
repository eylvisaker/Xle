using AgateLib.InputLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services.Implementation;

namespace ERY.Xle.Commands
{
	public class Disembark : Command
	{
		public override void Execute(GameState state)
		{
			if (state.MapExtender.PlayerDisembark(state))
				return;

			XleCore.TextArea.PrintLine("\nNothing to disembark.", XleColor.Yellow);
		}
	}
}
