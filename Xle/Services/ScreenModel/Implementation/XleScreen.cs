using AgateLib.Mathematics.Geometry;
using ERY.Xle.Services.XleSystem;
using Microsoft.Xna.Framework;
using System;

namespace ERY.Xle.Services.ScreenModel.Implementation
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

        public Rectangle Coordinates { get => throw new NotImplementedException(); }
        public bool PromptToContinue { get; set; }

        public Color FontColor { get; set; }

        public void OnDraw()
        {
            throw new NotImplementedException();
            //Display.BeginFrame();

            //Draw?.Invoke(this, EventArgs.Empty);

            //Display.EndFrame();
            //AgateApp.KeepAlive();
        }

        public event EventHandler Draw;
        public event EventHandler Update;

        public bool CurrentWindowClosed
        {
            get => throw new NotImplementedException();
        }

        public void OnUpdate()
        {
            if (Update != null)
                Update(this, EventArgs.Empty);
        }

    }
}
