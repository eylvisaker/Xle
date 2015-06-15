using AgateLib.DisplayLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle
{
    public interface IXleGameFactory : IXleService
    {
        void LoadSurfaces();
        FontSurface Font { get; }

        void SetGameSpeed(GameState GameState, int p);
    }
}
