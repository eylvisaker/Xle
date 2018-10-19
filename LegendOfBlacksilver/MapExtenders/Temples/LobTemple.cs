using Xle.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Maps.Temples;
using Xle.Services;
using Xle.Services.Commands;

namespace Xle.LoB.MapExtenders.Temples
{
    public class LobTemple : TempleExtender
    {
        public override void SetCommands(ICommandList commands)
        {
            commands.Items.AddRange(LobProgram.CommonLobCommands);

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
