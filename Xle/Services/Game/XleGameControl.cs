using AgateLib;
using ERY.Xle.Services.ScreenModel;
using ERY.Xle.Services.XleSystem;
using Microsoft.Xna.Framework;
using System;
using Xle.Services;

namespace ERY.Xle.Services.Game
{
    public interface IXleGameControl 
    {
        void Wait(int howLong, bool keyBreak = false, Action redraw = null);

        void RunRedrawLoop();

        void Redraw(GameTime time);

        void KeepAlive(GameTime time);
    }

    [Singleton]
    public class XleGameControl : IXleGameControl
    {
        private IXleScreen screen;
        private readonly IXleWaiter waiter;
        private GameState gameState;
        private XleSystemState systemState;

        public XleGameControl(
            IXleScreen screen,
            IXleWaiter waiter,
            GameState gameState,
            XleSystemState systemState)
        {
            this.screen = screen;
            this.waiter = waiter;
            this.gameState = gameState;
            this.systemState = systemState;
        }

        public void Wait(int howLong, bool keyBreak = false, Action redraw = null)
        {
            waiter.Wait(howLong, keyBreak);

            //if (redraw == null)
            //    redraw = screen.OnDraw;

            //IStopwatch watch = Timing.CreateStopWatch();

            //using (var input = new SimpleInputHandler())
            //{
            //    Input.Handlers.Add(input);

            //    do
            //    {
            //        screen.OnUpdate();

            //        redraw();
            //        KeepAlive();

            //        if (keyBreak && input.Keys.Any)
            //            break;

            //    } while (watch.TotalMilliseconds < howLong && AgateApp.IsAlive);
            //}
        }

        public void KeepAlive(GameTime time)
        {
            if (gameState.MapExtender != null)
            {
                gameState.MapExtender.CheckSounds(time);
            }

            if (screen.CurrentWindowClosed)
                throw new MainWindowClosedException();

            throw new NotImplementedException();
            //AgateApp.KeepAlive();
        }

        public void RunRedrawLoop()
        {
            throw new NotImplementedException();
            //while (AgateApp.IsAlive &&
            //    systemState.ReturnToTitle == false)
            //{
            //    Redraw();
            //}
        }

        public void Redraw(GameTime time)
        {
            OnUpdate(time);
            screen.OnDraw();

            KeepAlive(time);
        }

        private void OnUpdate(GameTime time)
        {
            if (gameState != null && gameState.MapExtender != null)
            {
                gameState.MapExtender.OnUpdate(time);
            }

            screen.OnUpdate();
        }

    }
}
