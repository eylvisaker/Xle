using ERY.Xle.Maps.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services.Implementation;

namespace ERY.Xle.LoB.MapExtenders.Archives
{
	class LobArchiveExtenderBase : MuseumExtender
	{
		public override void SetCommands(Commands.CommandList commands)
		{
			commands.Items.AddRange(LobProgram.CommonLobCommands);

			commands.Items.Add(new Commands.Leave { PromptText = "Leave the archives?" });
			commands.Items.Add(new Commands.Open());
			commands.Items.Add(new Commands.Rob());
			commands.Items.Add(new Commands.Take());
		}

		public override Maps.Map3DSurfaces Surfaces(GameState state)
		{
			return Lob3DSurfaces.Archives;
		}

		protected override Maps.Renderers.XleMapRenderer CreateMapRenderer()
		{
			return new ArchiveRenderer();
		}

		public override bool PlayerXamine(GameState state)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("You are in ancient archives.");

			return true; 
		}
		public override bool PlayerOpen(GameState state)
		{
			if (IsFacingDoor(state))
			{
				XleCore.TextArea.PrintLine(" door");
				XleCore.TextArea.PrintLine();

				LeaveMap(state);

				return true;
			}

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();

			if (InteractWithDisplay(state))
				return true;

			return false;
		}

		public override bool PlayerLeave(GameState state)
		{
			LeaveMap(state);

			return true;
		}
	}
}
