﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Maps.Towns;
using ERY.Xle.Services;
using ERY.Xle.Services.Commands;

namespace ERY.Xle.LoB.MapExtenders.Towns
{
    public class LobTown : TownExtender
    {
        public override void SetCommands(ICommandList commands)
        {
            commands.Items.AddRange(LobProgram.CommonLobCommands);

            commands.Items.Add(CommandFactory.Rob());
            commands.Items.Add(CommandFactory.Leave());
            commands.Items.Add(CommandFactory.Magic());
            commands.Items.Add(CommandFactory.Speak("TownSpeak"));
            commands.Items.Add(CommandFactory.Xamine());
        }
    }
}
