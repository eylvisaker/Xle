using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services.Implementation;

namespace ERY.Xle.Commands
{
	public class ArmorCommand : Command
	{
		public override string Name
		{
			get { return "Armor"; }
		}
		public override void Execute(GameState state)
		{
			XleCore.TextArea.PrintLine("-choose above", XleColor.Cyan);

			state.Player.CurrentArmor = XleCore.PickArmor(state.Player.CurrentArmor);
		}
	}
}
