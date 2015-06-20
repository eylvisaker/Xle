using ERY.Xle.Data;
using ERY.Xle.Maps.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services;
using ERY.Xle.Services.Implementation;
using ERY.Xle.Services.Implementation.Commands;

namespace ERY.Xle.LoB.MapExtenders.Temples
{
    class LobTempleExtender : TempleExtender
    {
        private ICommandFactory CommandFactory { get; set; }

        public override void SetCommands(ICommandList commands)
        {
            commands.Items.AddRange(LobProgram.CommonLobCommands);

            commands.Items.Add(CommandFactory.Climb());
            commands.Items.Add(CommandFactory.Leave());
            commands.Items.Add(CommandFactory.Magic());
            commands.Items.Add(CommandFactory.Speak());
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
                yield return XleCore.Data.MagicSpells[3];
                yield return XleCore.Data.MagicSpells[4];
            }
        }
    }
}
