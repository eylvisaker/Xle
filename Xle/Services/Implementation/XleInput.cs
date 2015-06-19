using AgateLib.InputLib;
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
        private GameState gameState;

        public XleInput(
            ICommandList commands,
            GameState gameState)
        {
            this.commands = commands;
            this.gameState = gameState;

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

        public void CheckArrowKeys()
        {
            if (AcceptKey == false)
                return;
            if (gameState == null)
                return;

            try
            {
                AcceptKey = false;

                if (Keyboard.Keys[KeyCode.Down]) commands.DoCommand(KeyCode.Down);
                else if (Keyboard.Keys[KeyCode.Left]) commands.DoCommand(KeyCode.Left);
                else if (Keyboard.Keys[KeyCode.Up]) commands.DoCommand(KeyCode.Up);
                else if (Keyboard.Keys[KeyCode.Right]) commands.DoCommand(KeyCode.Right);
            }
            finally
            {
                AcceptKey = true;
            }
        }
    }
}
