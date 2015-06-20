using ERY.Xle.LoB.MapExtenders.Outside.Events;
using ERY.Xle.Services;
using ERY.Xle.Services.Implementation.Commands;
using ERY.Xle.XleEventTypes;
using ERY.Xle.Maps.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Outside
{
	public class LobBaseOutside : OutsideExtender
	{
		public override void SetCommands(ICommandList commands)
		{
			commands.Items.AddRange(LobProgram.CommonLobCommands);

			commands.Items.Add(CommandFactory.Disembark());
			commands.Items.Add(CommandFactory.End());
			commands.Items.Add(CommandFactory.Magic());
			commands.Items.Add(CommandFactory.Rob());
			commands.Items.Add(CommandFactory.Speak());
		}
		public override int StepSize
		{
			get { return 2; }
		}

		public override void AfterPlayerStep(GameState state)
		{
			if (state.Player.X % 2 == 1) state.Player.X--;
			if (state.Player.Y % 2 == 1) state.Player.Y--;

			base.AfterPlayerStep(state);
		}

		public override void PlayerUse(GameState state, int item, ref bool handled)
		{
			switch((LobItem)item)
			{
				case LobItem.ClimbingGear:
					handled = true;
					break;
			}
		}
	}
}
