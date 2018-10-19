using AgateLib;
using Xle.Services.ScreenModel;
using Xle.Services.XleSystem;
using Microsoft.Xna.Framework;
using System;
using System.Threading.Tasks;
using Xle.Services;

namespace Xle.Services.Game
{
    public interface IXleGameControl
    {
        Task WaitAsync(int howLong, bool keyBreak = false);

        [Obsolete("await WaitAsync instead")]
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

        public async Task WaitAsync(int howLong, bool keyBreak = false)
        {
            await waiter.WaitAsync(howLong, keyBreak);
        }

        public void Wait(int howLong, bool keyBreak = false, Action redraw = null)
        {
            waiter.Wait(howLong, keyBreak);
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
