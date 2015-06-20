using System;
using AgateLib.Diagnostics;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

using ERY.Xle.Rendering;
using AgateLib;

namespace ERY.Xle.Services.Implementation
{
    public class XleScreen : IXleScreen
    {
        private IXleRenderer renderer;
        private XleSystemState systemState;
        private GameState gameState;

        public XleScreen(
            IXleRenderer renderer,
            GameState gameState,
            XleSystemState systemState)
        {
            this.renderer = renderer;
            this.gameState = gameState;
            this.systemState = systemState;

            InitializeScreenSize();
        }

        private void InitializeScreenSize()
        {
            Rectangle coords = renderer.Coordinates;
            int height = coords.Height - systemState.WindowBorderSize.Height * 2;
            int width = (int)(320 / 200.0 * height);

            systemState.WindowBorderSize = new Size(
                (coords.Width - width) / 2,
                systemState.WindowBorderSize.Height);
        }


        public void Redraw()
        {
            Display.BeginFrame();

            renderer.Draw();

            Display.EndFrame();
            Core.KeepAlive();
        }

        public bool CurrentWindowClosed
        {
            get { return Display.CurrentWindow.IsClosed; }
        }
    }
}
