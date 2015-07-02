using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgateLib.InputLib;

namespace ERY.Xle.Services.XleSystem
{
    public class CommandEventArgs : EventArgs
    {
        public CommandEventArgs(KeyCode command)
        {
            this.Command = command;
        }

        public KeyCode Command { get; private set; }
    }
}
