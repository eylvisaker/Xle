using ERY.Xle.Maps.XleMapTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Towns
{
	class LobTownExtender : NullTownExtender
	{

		public override void SetCommands(Commands.CommandList commands)
		{
			commands.Items.AddRange(LobProgram.CommonLobCommands);

			commands.Items.Add(new Commands.Rob());
			commands.Items.Add(new Commands.Leave());
			commands.Items.Add(new Commands.Magic());
			commands.Items.Add(new Commands.Speak());
		}
	}
}
