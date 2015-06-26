﻿using System;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.InputLib.Legacy;
using AgateLib.Platform;

using ERY.Xle.Services.ScreenModel;
using ERY.Xle.Services.XleSystem;

namespace ERY.Xle.Services.Game.Implementation
{
    public class XleGameControl : IXleGameControl
    {
        private IXleScreen screen;
        private GameState gameState;
        private XleSystemState systemState;

        public XleGameControl(
            IXleScreen screen,
            GameState gameState,
            XleSystemState systemState)
        {
            this.screen = screen;
            this.gameState = gameState;
            this.systemState = systemState;
        }

        public void Wait(int howLong, bool keyBreak = false, Action redraw = null)
        {
            if (redraw == null)
                redraw = screen.OnDraw;

            IStopwatch watch = Timing.CreateStopWatch();

            do
            {
                screen.OnUpdate();

                redraw();
                KeepAlive();

                if (keyBreak && Keyboard.AnyKeyPressed)
                    break;

            } while (watch.TotalMilliseconds < howLong);
        }

        public void KeepAlive()
        {
            if (gameState.MapExtender != null)
            {
                gameState.MapExtender.CheckSounds(gameState);
            }

            if (screen.CurrentWindowClosed)
                throw new MainWindowClosedException();

            Core.KeepAlive();
        }

        public void RunRedrawLoop()
        {
            while (screen.CurrentWindowClosed == false &&
                systemState.ReturnToTitle == false)
            {
                Redraw();
            }
        }

        public void Redraw()
        {
            OnUpdate();
            screen.OnDraw();

            KeepAlive();
        }

        private void OnUpdate()
        {
            if (gameState != null && gameState.MapExtender != null)
            {
                gameState.MapExtender.OnUpdate(Display.DeltaTime / 1000.0);
            }

            screen.OnUpdate();
        }

    }
}