using ERY.Xle.Maps.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services;
using ERY.Xle.Services.Commands;
using ERY.Xle.Services.Rendering;
using ERY.Xle.Services.Rendering.Maps;

namespace ERY.Xle.LoB.MapExtenders.Archives
{
    public class LobArchives : MuseumExtender
    {
        public override void SetCommands(ICommandList commands)
        {
            commands.Items.AddRange(LobProgram.CommonLobCommands);

            commands.Items.Add(CommandFactory.Leave("Leave the archives?"));
            commands.Items.Add(CommandFactory.Open());
            commands.Items.Add(CommandFactory.Rob());
            commands.Items.Add(CommandFactory.Take());
        }

        public override Maps.Map3DSurfaces Surfaces(GameState state)
        {
            return Lob3DSurfaces.Archives;
        }

        public override XleMapRenderer CreateMapRenderer(IMapRendererFactory factory)
        {
            return factory.MuseumRenderer(this, "ArchiveRenderer");
        }

        public override bool PlayerXamine(GameState state)
        {
            TextArea.PrintLine();
            TextArea.PrintLine();
            TextArea.PrintLine("You are in ancient archives.");

            return true;
        }
        public override bool PlayerOpen(GameState state)
        {
            if (IsFacingDoor(state))
            {
                TextArea.PrintLine(" door");
                TextArea.PrintLine();

                LeaveMap(state);

                return true;
            }

            TextArea.PrintLine();
            TextArea.PrintLine();

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
