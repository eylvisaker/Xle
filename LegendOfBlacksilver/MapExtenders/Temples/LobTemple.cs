using ERY.Xle.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Maps.Temples;
using ERY.Xle.Services;
using ERY.Xle.Services.Commands;

namespace ERY.Xle.LoB.MapExtenders.Temples
{
    public class LobTemple : TempleExtender
    {
        public override void SetCommands(ICommandList commands)
        {
            commands.Items.AddRange(LobProgram.CommonLobCommands);

            commands.Items.Add(CommandFactory.Climb("TempleClimb"));
            commands.Items.Add(CommandFactory.Fight());
            commands.Items.Add(CommandFactory.Leave());
            commands.Items.Add(CommandFactory.Magic());
            commands.Items.Add(CommandFactory.Speak());
            commands.Items.Add(CommandFactory.Xamine());
        }

        public override void SetColorScheme(ColorScheme scheme)
        {
            base.SetColorScheme(scheme);

            scheme.FrameColor = XleColor.LightGray;
        }

        public override IEnumerable<MagicSpell> ValidMagic
        {
            get
            {
                yield return Data.MagicSpells[3];
                yield return Data.MagicSpells[4];
            }
        }
    }
}
