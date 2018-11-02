using AgateLib;
using AgateLib.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xle.Services.ScreenModel;

namespace Xle.Services.XleSystem
{
    public interface IXleInput
    {
        bool AcceptKey { get; set; }

        event EventHandler<CommandEventArgs> DoCommand;

        void OnKeyPress(KeyPressEventArgs e);

        void OnKeyDown(Keys key);

        void OnKeyUp(Keys key);

        void Update(GameTime gameTime);
    }

    [Singleton]
    public class XleInput : IXleInput
    {
        private static readonly Keys[] arrowKeys = new[] {
            Keys.Down, Keys.Left, Keys.Up, Keys.Right
        };

        private GameState gameState;
        private IXleScreen screen;
        private HashSet<Keys> pressedKeys = new HashSet<Keys>();
        private CommandEventArgs commandArgs;
        private bool waiting = false;


        public XleInput(
            IXleScreen screen,
            GameState gameState)
        {
            this.screen = screen;
            this.gameState = gameState;
        }


        public event EventHandler<CommandEventArgs> DoCommand;

        public bool AcceptKey { get; set; }

        public void OnKeyDown(Keys key)
        {
            pressedKeys.Add(key);
        }

        public void OnKeyUp(Keys key)
        {
            pressedKeys.Remove(key);
        }

        public async void OnKeyPress(KeyPressEventArgs e)
        {
            if (AcceptKey == false)
                return;

            try
            {
                AcceptKey = false;
                ProcessKeyInput(e);
            }
            finally
            {
                AcceptKey = true;
            }
        }

        private void ProcessKeyInput(KeyPressEventArgs e)
        {
            commandArgs = new CommandEventArgs(e.Key, e.KeyString);

            if (waiting)
            {
                waiting = false;
            }
            else
            {
                DoCommand?.Invoke(this, commandArgs);
            }
        }

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
                        ProcessKeyInput(new KeyPressEventArgs(key, "", null, gameTime));
                        break;
                    }
                }
            }
            finally
            {
                AcceptKey = true;
            }
        }
    }
}
