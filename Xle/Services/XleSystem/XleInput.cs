using AgateLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Xle.Services.ScreenModel;

namespace Xle.Services.XleSystem
{
    public interface IXleInput
    {
        bool AcceptKey { get; set; }

        Keys WaitForKey(params Keys[] keys);
        Keys WaitForKey(Action redraw, params Keys[] keys);

        bool PromptToContinueOnWait { get; set; }

        event EventHandler<CommandEventArgs> DoCommand;

        void OnKeyPress(Keys key, string keyString);

        void OnKeyDown(Keys key);

        void OnKeyUp(Keys key);

        void Update(GameTime gameTime);
    }

    [Singleton]
    public class XleInput : IXleInput
    {
        private GameState gameState;
        private IXleScreen screen;
        private HashSet<Keys> pressedKeys = new HashSet<Keys>();

        public XleInput(
            IXleScreen screen,
            GameState gameState)
        {
            this.screen = screen;
            this.gameState = gameState;
        }


        public void OnKeyDown(Keys key)
        {
            pressedKeys.Add(key);
        }

        public void OnKeyUp(Keys key)
        {
            pressedKeys.Remove(key);
        }

        public async void OnKeyPress(Keys key, string keyString)
        {
            if (AcceptKey == false)
                return;

            try
            {
                AcceptKey = false;
                OnDoCommand(key, keyString);
            }
            finally
            {
                AcceptKey = true;
            }
        }

        private void OnDoCommand(Keys command, string keyString)
        {
            DoCommand?.Invoke(this, new CommandEventArgs(command, keyString));
        }

        public event EventHandler<CommandEventArgs> DoCommand;

        public bool AcceptKey { get; set; }

        private Keys[] arrowKeys = new[] {
            Keys.Down, Keys.Left, Keys.Up, Keys.Right
        };

        public void Update(GameTime gameTime)
        {
            if (AcceptKey == false)
                return;
            if (gameState == null)
                return;

            try
            {
                AcceptKey = false;

                foreach (var key in arrowKeys)
                {
                    if (pressedKeys.Contains(key))
                    {
                        OnDoCommand(key, "");
                        break;
                    }
                }
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
