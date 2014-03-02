using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Commands
{
	public class Leave : Command
	{
		public override void Execute(GameState state)
		{
			XleCore.Map.PlayerLeave(state.Player);
		}

		public bool ConfirmPrompt { get; set; }
	}
}
