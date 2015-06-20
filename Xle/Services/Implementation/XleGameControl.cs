﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgateLib;
using AgateLib.Diagnostics;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib.Legacy;
using AgateLib.Platform;

using ERY.Xle.Maps;
using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Rendering;

namespace ERY.Xle.Services.Implementation
{
    public class XleGameControl : IXleGameControl
    {
        private IXleScreen screen;
        private IXleRenderer renderer;
        private IXleInput input;
        private GameState gameState;
        private XleSystemState systemState;

        public XleGameControl(
            IXleScreen screen,
            IXleRenderer renderer,
            IXleInput input,
            GameState gameState,
            XleSystemState systemState)
        {
            this.screen = screen;
            this.renderer = renderer;
            this.input = input;
            this.gameState = gameState;
            this.systemState = systemState;
        }

        public void Wait(int howLong, bool keyBreak = false, Action redraw = null)
        {
            if (redraw == null)
                redraw = screen.Redraw;

            IStopwatch watch = Timing.CreateStopWatch();

            do
            {
                renderer.UpdateAnim();

                redraw();
                KeepAlive();

                if (keyBreak && Keyboard.AnyKeyPressed)
                    break;

            } while (watch.TotalMilliseconds < howLong);
        }

        public void KeepAlive()
        {
            if (gameState.Map != null)
            {
                gameState.MapExtender.CheckSounds(gameState);
            }

            Core.KeepAlive();

            if (screen.CurrentWindowClosed)
                throw new MainWindowClosedException();
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
            Update();
            screen.Redraw();

            KeepAlive();

            if (AgateConsole.IsVisible == false)
            {
                CheckArrowKeys();
            }
        }

        private void Update()
        {
            renderer.UpdateAnim();

            if (gameState != null && gameState.Map != null)
            {
                gameState.Map.OnUpdate(gameState, Display.DeltaTime / 1000.0);
            }
        }

        private void CheckArrowKeys()
        {
            input.CheckArrowKeys();
        }


    }
}
