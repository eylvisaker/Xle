
using AgateLib;
using Xle.Maps.Temples;
using Xle.Services.Commands;

namespace Xle.Blacksilver.MapExtenders.Temples
{
    [Transient("LobTemple")]
    public class LobTemple : TempleExtender
    {
        public override void SetCommands(ICommandList commands)
        {
            commands.Items.Add(CommandFactory.Armor());
            commands.Items.Add(CommandFactory.Gamespeed());
            commands.Items.Add(CommandFactory.Inventory());
            commands.Items.Add(CommandFactory.Pass());
            commands.Items.Add(CommandFactory.Weapon());

            commands.Items.Add(CommandFactory.Climb("TempleClimb"));
            commands.Items.Add(CommandFactory.Fight("TempleFight"));
            commands.Items.Add(CommandFactory.Leave());
            commands.Items.Add(CommandFactory.Magic("TempleMagic"));
            commands.Items.Add(CommandFactory.Speak());
            commands.Items.Add(CommandFactory.Use("LobUse"));
            commands.Items.Add(CommandFactory.Xamine());
        }

        public override void SetColorScheme(ColorScheme scheme)
        {
            base.SetColorScheme(scheme);

            scheme.FrameColor = XleColor.LightGray;
        }

    }
}
