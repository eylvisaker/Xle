using ERY.Xle.LoB.MapExtenders.Outside.Events;
using ERY.Xle.XleEventTypes;
using ERY.Xle.Maps.XleMapTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Outside
{
	public class LobBaseOutside : NullOutsideExtender
	{
		public override XleEventTypes.Extenders.IEventExtender CreateEventExtender(XleEvent evt, Type defaultExtender)
		{
			if (evt is ChangeMapEvent)
			{
				ChangeMapEvent e = (ChangeMapEvent)evt;

				if (e.MapID == 40 || e.MapID == 45)
				{
					return new Drawbridge();
				}
			}
			return base.CreateEventExtender(evt, defaultExtender);
		}
		public override void SetCommands(Commands.CommandList commands)
		{
			commands.Items.AddRange(LobProgram.CommonLobCommands);

			commands.Items.Add(new Commands.Disembark());
			commands.Items.Add(new Commands.End());
			commands.Items.Add(new Commands.Magic());
			commands.Items.Add(new Commands.Rob());
			commands.Items.Add(new Commands.Speak());
		}
		public override int StepSize
		{
			get { return 2; }
		}
	}
}
