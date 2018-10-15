using Microsoft.Xna.Framework;
using System;

namespace ERY.Xle.Services.Game
{
    public interface IXleGameControl : IXleService
    {
        void Wait(int howLong, bool keyBreak = false, Action redraw = null);

        void RunRedrawLoop();

        void Redraw(GameTime time);

        void KeepAlive(GameTime time);
    }
}
