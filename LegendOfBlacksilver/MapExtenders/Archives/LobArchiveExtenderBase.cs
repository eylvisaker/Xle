using ERY.Xle.XleMapTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Archives
{
	class LobArchiveExtenderBase : NullMuseumExtender
	{
		public override void SetCommands(Commands.CommandList commands)
		{
			commands.Items.AddRange(LobProgram.CommonLobCommands);

			commands.Items.Add(new Commands.Leave { PromptText = "Leave the archives?" });
			commands.Items.Add(new Commands.Open());
			commands.Items.Add(new Commands.Rob());
			commands.Items.Add(new Commands.Take());
		}
	}
}
