﻿using System;

using AgateLib.Diagnostics;
using AgateLib.InputLib;
using AgateLib.InputLib.Legacy;

using ERY.Xle.Services.Commands;
using ERY.Xle.Services.ScreenModel;

namespace ERY.Xle.Services.XleSystem.Implementation
{
    public class XleInput : IXleInput
    {
        ICommandExecutor commands;
        private GameState gameState;
        private IXleScreen screen;

        public XleInput(
            ICommandExecutor commands,
            IXleScreen screen,
            GameState gameState)
        {
            this.commands = commands;
            this.screen = screen;
            this.gameState = gameState;

            screen.Update += gameControl_Update;
            Keyboard.KeyDown += Keyboard_KeyDown;
        }

        void gameControl_Update(object sender, EventArgs e)
        {
            if (AgateConsole.IsVisible == false)
            {
                CheckArrowKeys();
            }
        }

        private void Keyboard_KeyDown(InputEventArgs e)
        {
            if (AcceptKey == false)
                return;
            if (e.KeyCode == AgateConsole.VisibleToggleKey)
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
            return WaitForKey(screen.OnDraw, keys);
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

            screen.PromptToContinue = PromptToContinueOnWait;

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

            screen.PromptToContinue = false;
            PromptToContinueOnWait = true;

            return key;
        }


        /// <summary>
        /// Set to false to have WaitForKey not display a prompt 
        /// with the standard drawing method.
        /// </summary>
        public bool PromptToContinueOnWait { get; set; }
    }
}