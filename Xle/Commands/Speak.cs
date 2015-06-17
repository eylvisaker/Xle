using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services.Implementation;

namespace ERY.Xle.Commands
{
	public class Speak : Command
	{
		public override void Execute(GameState state)
		{
			if (state.MapExtender.PlayerSpeak(state) == false)
			{
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("No response.");
			}
		}
	}
}
