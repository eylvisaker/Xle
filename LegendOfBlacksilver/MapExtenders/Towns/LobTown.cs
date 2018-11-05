using AgateLib;

using Xle.Maps.Towns;
using Xle.Commands;

namespace Xle.Blacksilver.MapExtenders.Towns
{
    [Transient("LobTown")]
    public class LobTown : TownExtender
    {
        public override void SetCommands(ICommandList commands)
        {
            commands.Items.Add(CommandFactory.Armor());
            commands.Items.Add(CommandFactory.Gamespeed());
            commands.Items.Add(CommandFactory.Inventory());
            commands.Items.Add(CommandFactory.Pass());
            commands.Items.Add(CommandFactory.Weapon());

            commands.Items.Add(CommandFactory.Fight("FightAgainstGuard"));
            commands.Items.Add(CommandFactory.Leave("TownLeave"));
            commands.Items.Add(CommandFactory.Magic("TownMagic"));
            commands.Items.Add(CommandFactory.Rob());
            commands.Items.Add(CommandFactory.Speak("TownSpeak"));
            commands.Items.Add(CommandFactory.Use("LobUse"));
            commands.Items.Add(CommandFactory.Xamine());
        }
    }
}
