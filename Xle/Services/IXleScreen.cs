using System;
using AgateLib.Geometry;

namespace ERY.Xle.Services
{
    public interface IXleScreen : IXleService
    {
        void OnDraw();

        Color FontColor { get; set; }
        bool CurrentWindowClosed { get; }

        event EventHandler Draw;
        event EventHandler Update;

        void OnUpdate();
    }
}
