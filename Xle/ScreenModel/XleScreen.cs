using AgateLib;
using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xle.XleSystem;

namespace Xle.ScreenModel
{
    public interface IXleScreen
    {
        Color FontColor { get; set; }

        /// <summary>
        /// Set to true to show the (press to cont) prompt.
        /// </summary>
        bool PromptToContinue { get; set; }
        Color BorderColor { get; set; }

        IRenderer Renderer { get; set; }
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
        public IRenderer Renderer { get; set; }
    }
}
