using AgateLib;

using Xle.Maps;
using Xle.Maps.Museums;
using Xle.Services.Commands;
using Xle.Services.Rendering;
using Xle.Services.Rendering.Maps;

namespace Xle.Blacksilver.MapExtenders.Archives
{
    [Transient("LobArchives")]
    public class LobArchives : MuseumExtender
    {
        public override void SetCommands(ICommandList commands)
        {
            commands.Items.Add(CommandFactory.Armor());
            commands.Items.Add(CommandFactory.Gamespeed());
            commands.Items.Add(CommandFactory.Inventory());
            commands.Items.Add(CommandFactory.Pass());
            commands.Items.Add(CommandFactory.Weapon());

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

        public override IXleMapRenderer CreateMapRenderer(IMapRendererFactory factory)
        {
            return factory.MuseumRenderer(this, "ArchiveRenderer");
        }
    }
}
