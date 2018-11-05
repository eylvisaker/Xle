using AgateLib;
using AgateLib.Input;
using AgateLib.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading.Tasks;
using Xle.Rendering;

namespace Xle
{
    [Transient, InjectProperties]
    public class WaitScene : Scene
    {
        private float timeLeft_ms;
        private bool allowKeyBreak;
        private KeyboardEvents keyboard = new KeyboardEvents();
        private readonly GraphicsDevice graphics;
        private SpriteBatch spriteBatch;
        private IRenderer renderer;

        public WaitScene(GraphicsDevice graphics)
        {
            this.graphics = graphics;
            this.spriteBatch = new SpriteBatch(graphics);

            keyboard.KeyPress += Keyboard_KeyPress;
            keyboard.KeyUp += Keyboard_KeyUp;

            DrawBelow = true;
        }

        public IXleRenderer XleRenderer { get; set; }

        public Keys? PressedKey { get; private set; }

        public TimeSpan TimeLeft => TimeSpan.FromMilliseconds(timeLeft_ms);

        private void Keyboard_KeyPress(object sender, KeyPressEventArgs e)
        {
            PressedKey = e.Key;

            if (allowKeyBreak)
                timeLeft_ms = 0;
        }

        private void Keyboard_KeyUp(object sender, KeyEventArgs e)
        {
            //PressedKey = e.Key;

            //if (allowKeyBreak)
            //    timeLeft_ms = 0;
        }

        public void Initialize(int howLong_ms, bool allowKeyBreak, IRenderer renderer = null)
        {
            timeLeft_ms = howLong_ms;
            this.allowKeyBreak = allowKeyBreak;

            IsFinished = false;
            PressedKey = null;

            this.renderer = renderer;
        }

        protected override void OnUpdateInput(IInputState input)
        {
            base.OnUpdateInput(input);

            keyboard.Update(input);
        }

        protected override void OnUpdate(GameTime time)
        {
            base.OnUpdate(time);
            timeLeft_ms -= (float)time.ElapsedGameTime.TotalMilliseconds;

            if (timeLeft_ms <= 0)
                IsFinished = true;

            renderer?.Update(time);
            XleRenderer.Update(time);

            DrawBelow = renderer == null;
        }

        protected override void DrawScene(GameTime time)
        {
            if (renderer != null)
            {
                spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(20, 20, 0));

                renderer.Draw(spriteBatch);

                spriteBatch.End();
            }

            base.DrawScene(time);
        }

        public void TopOff(int howLong_ms)
        {
            timeLeft_ms = Math.Max(timeLeft_ms, howLong_ms);
        }

        public async Task Wait(ISceneStack sceneStack)
        {
            while (timeLeft_ms > 0)
            {
                sceneStack.AddOrBringToTop(this);

                await Task.Yield();
            }
        }
    }
}
