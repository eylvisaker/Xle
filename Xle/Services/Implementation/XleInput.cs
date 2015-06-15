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
        public XleInput()
        {
            Keyboard.KeyDown += new InputEventHandler(Keyboard_KeyDown);
        }

        private void Keyboard_KeyDown(InputEventArgs e)
        {
            if (AcceptKey == false)
                return;

            try
            {
                AcceptKey = false;
                XleCore.GameState.Commands.DoCommand(e.KeyCode);
            }
            finally
            {
                AcceptKey = true;
            }
        }

        public bool AcceptKey { get; set; }
    }
}
