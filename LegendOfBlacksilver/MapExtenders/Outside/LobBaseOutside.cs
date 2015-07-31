using ERY.Xle.LoB.MapExtenders.Outside.Events;
using ERY.Xle.Maps.Outdoors;
using ERY.Xle.Services;
using ERY.Xle.Services.Commands;
using ERY.Xle.XleEventTypes;

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
			commands.Items.Add(CommandFactory.Speak("OutsideSpeak"));
            commands.Items.Add(CommandFactory.Xamine("OutsideXamine"));

		}
		public override int StepSize
		{
			get { return 2; }
		}

		public override void AfterPlayerStep()
		{
			if (Player.X % 2 == 1) Player.X--;
			if (Player.Y % 2 == 1) Player.Y--;

			base.AfterPlayerStep();
		}

		public override void PlayerUse(int item, ref bool handled)
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
