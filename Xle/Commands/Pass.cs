using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services.Implementation;

namespace ERY.Xle.Commands
{
	public class Pass : Command
	{
		public override void Execute(GameState state)
		{
			XleCore.TextArea.PrintLine();
		}
	}
}
