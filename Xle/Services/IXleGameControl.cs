using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Services
{
    public interface IXleGameControl : IXleService
    {
        event EventHandler Update;

        void Wait(int howLong, bool keyBreak = false, Action redraw = null);

        void RunRedrawLoop();

        void Redraw();


        void KeepAlive();

        void PlayerIsDead();
    }
}
