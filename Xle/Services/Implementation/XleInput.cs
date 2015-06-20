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
        private IXleScreen screen;

        public XleInput(
            ICommandList commands,
            IXleScreen screen,
            GameState gameState)
        {
            this.commands = commands;
            this.screen = screen;
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


        /// <summary>
        /// Waits for one of the specified keys, while redrawing the screen.
        /// </summary>
        /// <param name="keys">A list of keys which will break out of the wait. 
        /// Pass none for any key to break out.</param>
        /// <returns></returns>
        public KeyCode WaitForKey(params KeyCode[] keys)
        {
            return WaitForKey(screen.Redraw, keys);
        }

        /// <summary>
        /// Waits for one of the specified keys, while calling the delegate
        /// to redraw the screen.
        /// </summary>
        /// <param name="redraw"></param>
        /// <param name="keys">A list of keys which will break out of the wait. 
        /// Pass none for any key to break out.</param>
        /// <returns></returns>
        public KeyCode WaitForKey(Action redraw, params KeyCode[] keys)
        {
            KeyCode key = KeyCode.None;
            bool done = false;

            InputEventHandler keyhandler = e => key = e.KeyCode;

            PromptToContinue = PromptToContinueOnWait;

            Keyboard.ReleaseAllKeys();
            Keyboard.KeyDown += keyhandler;

            do
            {
                redraw();

                if (screen.CurrentWindowClosed == true)
                {
                    if (keys.Length > 0)
                        key = keys[0];
                    else
                        key = KeyCode.Escape;

                    break;
                }

                if ((keys == null || keys.Length == 0) && key != KeyCode.None)
                    break;

                for (int i = 0; i < keys.Length; i++)
                {
                    if (keys[i] == key)
                    {
                        done = true;
                        break;
                    }
                }

            } while (!done && screen.CurrentWindowClosed == false);

            Keyboard.KeyDown -= keyhandler;

            PromptToContinue = false;
            PromptToContinueOnWait = true;

            return key;
        }


        public bool PromptToContinueOnWait
        {
            get { return XleCore.PromptToContinueOnWait; }
            set { XleCore.PromptToContinueOnWait = value; }
        }

        public bool PromptToContinue
        {
            get { return XleCore.PromptToContinue; }
            set { XleCore.PromptToContinue = value; }
        }
    }
}
