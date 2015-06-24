using System;

namespace ERY.Xle.Services.Game
{
    public interface IXleGameControl : IXleService
    {
        void Wait(int howLong, bool keyBreak = false, Action redraw = null);

        void RunRedrawLoop();

        void Redraw();

        void KeepAlive();
    }
}
