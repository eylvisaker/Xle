using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Commands
{
	public class Hold : Command
	{
		public override void Execute(GameState state)
		{
			Use.ChooseHeldItem(state);
		}
	}
}
