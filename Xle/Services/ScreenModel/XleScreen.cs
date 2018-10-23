using AgateLib;
using AgateLib.Mathematics.Geometry;
using Xle.Services.XleSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Xle.Services.ScreenModel
{
    public interface IXleScreen
    {
        void OnDraw();

        Color FontColor { get; set; }
        bool CurrentWindowClosed { get; }

        /// <summary>
        /// Set to true to show the (press to cont) prompt.
        /// </summary>
        bool PromptToContinue { get; set; }
        Color BorderColor { get; set; }

        [Obsolete]
        event EventHandler Draw;
        [Obsolete]
        event EventHandler Update;

        void OnUpdate();
    }

    [Singleton]
    public class XleScreen : IXleScreen
    {
        private readonly GraphicsDevice graphicsDevice;
        private readonly GameState gameState;
        private readonly XleSystemState systemState;

        public XleScreen(
            GraphicsDevice graphicsDevice,
            GameState gameState,
            XleSystemState systemState)
        {
            this.graphicsDevice = graphicsDevice;
            this.gameState = gameState;
            this.systemState = systemState;

            InitializeScreenSize();
        }

        private void InitializeScreenSize()
        {
            Rectangle coords = new Rectangle(0,
                                             0,
                                             graphicsDevice.PresentationParameters.BackBufferWidth,
                                             graphicsDevice.PresentationParameters.BackBufferHeight);

            int height = coords.Height - systemState.WindowBorderSize.Height * 2;
            int width = (int)(320 / 200.0 * height);

            systemState.WindowBorderSize = new Size(
                (coords.Width - width) / 2,
                systemState.WindowBorderSize.Height);
        }

        public bool PromptToContinue { get; set; }

        public Color FontColor { get; set; }

        public Color BorderColor { get; set; } = XleColor.DarkGray;

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
