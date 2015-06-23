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
        private XleSystemState systemState;
        private GameState gameState;

        public XleScreen(
            GameState gameState,
            XleSystemState systemState)
        {
            this.gameState = gameState;
            this.systemState = systemState;

            InitializeScreenSize();
        }

        private void InitializeScreenSize()
        {
            Rectangle coords = Coordinates;
            int height = coords.Height - systemState.WindowBorderSize.Height * 2;
            int width = (int)(320 / 200.0 * height);

            systemState.WindowBorderSize = new Size(
                (coords.Width - width) / 2,
                systemState.WindowBorderSize.Height);
        }

        public Rectangle Coordinates { get { return Display.Coordinates; } }
        public bool PromptToContinue { get; set; }

        public Color FontColor { get; set; }

        public void OnDraw()
        {
            Display.BeginFrame();

            if (Draw != null)
                Draw(this, EventArgs.Empty);

            Display.EndFrame();
            Core.KeepAlive();
        }

        public event EventHandler Draw;
        public event EventHandler Update;

        public bool CurrentWindowClosed
        {
            get { return Display.CurrentWindow.IsClosed; }
        }


        public void OnUpdate()
        {
            if (Update != null)
                Update(this, EventArgs.Empty);
        }

    }
}
