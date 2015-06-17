using AgateLib.InputLib.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Services.Implementation
{
    public class XleInput : IXleInput
    {
        ICommandList commands;

        public XleInput(ICommandList commands)
        {
            this.commands = commands;

            Keyboard.KeyDown += Keyboard_KeyDown;
        }

        private void Keyboard_KeyDown(InputEventArgs e)
        {
            if (AcceptKey == false)
                return;

            try
            {
                AcceptKey = false;
                commands.DoCommand(e.KeyCode);
            }
            finally
            {
                AcceptKey = true;
            }
        }

        public bool AcceptKey { get; set; }
    }
}
