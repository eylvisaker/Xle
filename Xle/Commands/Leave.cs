using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Commands
{
	public class Leave : Command
	{
		public Leave()
		{
			ConfirmPrompt = true;
		}

		public override void Execute(GameState state)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();

			if (ConfirmPrompt)
			{
				if (XleCore.QuickMenuYesNo() == 1)
					return;
			}

			state.Map.PlayerLeave(state.Player);
		}

		public bool ConfirmPrompt { get; set; }
	}
}
