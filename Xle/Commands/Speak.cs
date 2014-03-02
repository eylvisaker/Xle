using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Commands
{
	public class Speak : Command
	{
		public override void Execute(GameState state)
		{
			if (state.Map.PlayerSpeak(state) == false)
			{
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("No response.");
			}
		}
	}
}
