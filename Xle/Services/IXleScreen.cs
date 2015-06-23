using System;
using AgateLib.Geometry;

namespace ERY.Xle.Services
{
    public interface IXleScreen : IXleService
    {
        void OnDraw();

        Color FontColor { get; set; }
        bool CurrentWindowClosed { get; }

        /// <summary>
        /// Set to true to show the (press to cont) prompt.
        /// </summary>
        bool PromptToContinue { get; set; }

        event EventHandler Draw;
        event EventHandler Update;

        void OnUpdate();
    }
}
