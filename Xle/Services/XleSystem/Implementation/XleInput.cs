using AgateLib.Diagnostics;
using Xle.Services.ScreenModel;
using Microsoft.Xna.Framework.Input;
using System;

namespace Xle.Services.XleSystem.Implementation
{
    public class XleInput : IXleInput
    {
        private GameState gameState;
        private IXleScreen screen;

        public XleInput(
            IXleScreen screen,
            GameState gameState)
        {
            this.screen = screen;
            this.gameState = gameState;

            screen.Update += gameControl_Update;
            throw new NotImplementedException();
            //Input.Unhandled.KeyDown += Keyboard_KeyDown;
        }

        private void gameControl_Update(object sender, EventArgs e)
        {
            throw new NotImplementedException();
            //if (AgateConsole.IsVisible == false)
            //{
            //    CheckArrowKeys();
            //}
        }

        private void Keyboard_KeyDown(object sender, object /*AgateInputEventArgs */e)
        {
            if (AcceptKey == false)
                return;
            //if (e.Keys == AgateConsole.VisibleToggleKey)
            //    return;
            throw new NotImplementedException();
            //try
            //{
            //    AcceptKey = false;
            //    OnDoCommand(e.Keys);
            //}
            //finally
            //{
            //    AcceptKey = true;
            //}
        }

        private void OnDoCommand(Keys command)
        {
            DoCommand?.Invoke(this, new CommandEventArgs(command));
        }

        public event EventHandler<CommandEventArgs> DoCommand;

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

                //if (Input.Unhandled.Keys[Keys.Down])
                //    OnDoCommand(Keys.Down);
                //else if (Input.Unhandled.Keys[Keys.Left])
                //    OnDoCommand(Keys.Left);
                //else if (Input.Unhandled.Keys[Keys.Up])
                //    OnDoCommand(Keys.Up);
                //else if (Input.Unhandled.Keys[Keys.Right])
                //    OnDoCommand(Keys.Right);
                throw new NotImplementedException();
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
        public Keys WaitForKey(params Keys[] keys)
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
        public Keys WaitForKey(Action redraw, params Keys[] keys)
        {
            Keys key = Keys.None;
            bool done = false;

            throw new NotImplementedException();
            //using (var input = new SimpleInputHandler())
            //{
            //    Input.Handlers.Add(input);

            //    EventHandler<AgateInputEventArgs> keyhandler = (sender, e) => key = e.Keys;

            //    screen.PromptToContinue = PromptToContinueOnWait;

            //    input.Keys.ReleaseAll();
            //    input.KeyDown += keyhandler;

            //    do
            //    {
            //        redraw();

            //        if (screen.CurrentWindowClosed == true)
            //        {
            //            if (keys.Length > 0)
            //                key = keys[0];
            //            else
            //                key = Keys.Escape;

            //            break;
            //        }

            //        if ((keys == null || keys.Length == 0) && key != Keys.None)
            //            break;

            //        for (int i = 0; i < keys.Length; i++)
            //        {
            //            if (keys[i] == key)
            //            {
            //                done = true;
            //                break;
            //            }
            //        }

            //    } while (!done && screen.CurrentWindowClosed == false);
            //}

            //screen.PromptToContinue = false;
            //PromptToContinueOnWait = true;

            //return key;
        }


        /// <summary>
        /// Set to false to have WaitForKey not display a prompt 
        /// with the standard drawing method.
        /// </summary>
        public bool PromptToContinueOnWait { get; set; }
    }
}
