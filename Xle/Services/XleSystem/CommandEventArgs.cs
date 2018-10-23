using Microsoft.Xna.Framework.Input;
using System;

namespace Xle.Services.XleSystem
{
    public class CommandEventArgs : EventArgs
    {
        public CommandEventArgs(Keys command, string keyString)
        {
            this.Command = command;
            KeyString = keyString;
        }

        public Keys Command { get; private set; }

        public string KeyString { get; private set; }
    }
}
