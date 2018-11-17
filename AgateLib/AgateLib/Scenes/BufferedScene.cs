using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Scenes
{
    /// <summary>
    /// Base class for a scene which has its drawing buffered to a render target of a specific size.
    /// </summary>
    public class BufferedScene : Scene
    {
        private readonly GraphicsDevice graphics;
        private readonly SpriteBatch spriteBatch;

        private RenderTarget2D renderTarget;
        private Size _backBufferSize;
        private bool lockSize = false;

        public BufferedScene(GraphicsDevice graphics, int width, int height)
            :this(graphics, new Size(width, height))
        { }

        public BufferedScene(GraphicsDevice graphics, Size backBufferSize)
        {
            this.graphics = graphics;
            this.spriteBatch = new SpriteBatch(graphics);

            this.BackBufferSize = backBufferSize;
        }

        public Size BackBufferSize
        {
            get => _backBufferSize;
            set
            {
                if (lockSize)
                    throw new InvalidOperationException("Cannot resize during drawing.");

                _backBufferSize = value;

                this.renderTarget = new RenderTarget2D(graphics, _backBufferSize.Width, _backBufferSize.Height);
            }
        }

        public Color ClearColor { get; set; } = new Color(0, 0, 0, 0);

        protected override void OnUpdate(GameTime time)
        {
            base.OnUpdate(time);

            //DrawToBackBuffer(time);
        }

        protected override void DrawWithEvents(GameTime time)
        {
            DrawToBackBuffer(time);

            DrawBackBuffer();
        }

        private void DrawToBackBuffer(GameTime time)
        {
            try
            {
                lockSize = true;

                var savedRenderTargets = graphics.GetRenderTargets();

                graphics.SetRenderTarget(renderTarget);
                graphics.Clear(ClearColor);

                OnBeforeDraw(time);
                DrawScene(time);
                OnDraw(time);

                graphics.SetRenderTargets(savedRenderTargets);

            }
            finally
            {
                lockSize = false;
            }
        }

        private void DrawBackBuffer()
        {
            var destRect = new Rectangle(0, 0, 
                graphics.PresentationParameters.BackBufferWidth,
                graphics.PresentationParameters.BackBufferHeight);

            //var destRect = new Rectangle(0, 0, renderTarget.Width, renderTarget.Height);

            spriteBatch.Begin(/*transformMatrix: Matrix.CreateOrthographicOffCenter(destRect, 1, -1)*/);
            spriteBatch.Draw(renderTarget, destRect, Color.White);
            spriteBatch.End();
        }
    }
}
