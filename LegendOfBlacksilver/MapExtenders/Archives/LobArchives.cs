using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Maps;
using Xle.Maps.Museums;
using Xle.Services;
using Xle.Services.Commands;
using Xle.Services.Rendering;
using Xle.Services.Rendering.Maps;

namespace Xle.LoB.MapExtenders.Archives
{
    public class LobArchives : MuseumExtender
    {
        public override void SetCommands(ICommandList commands)
        {
            commands.Items.AddRange(LobProgram.CommonLobCommands);

            commands.Items.Add(CommandFactory.Fight("ArchiveFight"));
            commands.Items.Add(CommandFactory.Leave(promptText: "Leave the archives?"));
            commands.Items.Add(CommandFactory.Open("ArchiveOpen"));
            commands.Items.Add(CommandFactory.Rob());
            commands.Items.Add(CommandFactory.Take());
            commands.Items.Add(CommandFactory.Use("LobUse"));
            commands.Items.Add(CommandFactory.Xamine("ArchiveXamine"));
        }

        public override Map3DSurfaces Surfaces()
        {
            return Lob3DSurfaces.Archives;
        }

        public override XleMapRenderer CreateMapRenderer(IMapRendererFactory factory)
        {
            return factory.MuseumRenderer(this, "ArchiveRenderer");
        }
    }
}
