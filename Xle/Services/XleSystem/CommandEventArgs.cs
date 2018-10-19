using Microsoft.Xna.Framework.Input;
using System;

namespace Xle.Services.XleSystem
{
    public class CommandEventArgs : EventArgs
    {
        public CommandEventArgs(Keys command)
        {
            this.Command = command;
        }

        public Keys Command { get; private set; }
    }
}
