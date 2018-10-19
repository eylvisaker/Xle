using AgateLib;
using AgateLib.Input;
using AgateLib.Scenes;
using ERY.Xle;
using ERY.Xle.LotA.TitleScreen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Xle.Ancients.TitleScreen
{
    [Transient]
    public class LotaTitleScene : Scene
    {
        private readonly ILotaTitleScreen titleScreen;
        private readonly GraphicsDevice device;
        private readonly IRectangleRenderer rects;
        private readonly SpriteBatch spriteBatch;
        private readonly KeyboardEvents keyboard;

        public LotaTitleScene(ILotaTitleScreen titleScreen, GraphicsDevice device, IRectangleRenderer rects)
        {
            this.titleScreen = titleScreen;
            this.device = device;
            this.rects = rects;

            spriteBatch = new SpriteBatch(device);

            keyboard = new KeyboardEvents();

            keyboard.KeyPress += Keyboard_KeyPress;
        }

        public event Action<Player> BeginGame;

        private async void Keyboard_KeyPress(object sender, KeyPressEventArgs e) 
        {
            await titleScreen.OnKeyPress(e);
        }

        protected override void OnUpdateInput(IInputState input)
        {
            keyboard.Update(input);

            base.OnUpdateInput(input);
        }

        protected override void OnUpdate(GameTime time)
        {
            base.OnUpdate(time);

            titleScreen.Update(time);

            if (titleScreen.Player != null)
            {
                BeginGame?.Invoke(titleScreen.Player);
            }
        }

        protected override void DrawScene(GameTime time)
        {
            device.Clear(titleScreen.Colors.BorderColor);

            base.DrawScene(time);

            spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(new Vector3(20, 20, 0)));
            rects.Fill(spriteBatch, new Rectangle(0, 0, 640, 400), titleScreen.Colors.BackColor);

            titleScreen.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
