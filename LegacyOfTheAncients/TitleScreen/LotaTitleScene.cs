using AgateLib;
using AgateLib.Scenes;
using ERY.Xle.LotA.TitleScreen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xle.Ancients.TitleScreen
{
    [Transient]
    public class LotaTitleScene : Scene
    {
        private readonly ILotaTitleScreen titleScreen;
        private readonly GraphicsDevice device;
        private readonly IRectangleRenderer rects;
        private readonly SpriteBatch spriteBatch;

        public LotaTitleScene(ILotaTitleScreen titleScreen, GraphicsDevice device, IRectangleRenderer rects)
        {
            this.titleScreen = titleScreen;
            this.device = device;
            this.rects = rects;
            spriteBatch = new SpriteBatch(device);
        }

        protected override void OnUpdate(GameTime time)
        {
            base.OnUpdate(time);

            titleScreen.Update(time);
        }

        protected override void DrawScene(GameTime time)
        {
            device.Clear(titleScreen.Colors.BorderColor);

            //Display.Clear(Colors.BorderColor);
            //Display.FillRect(new Rectangle(0, 0, 640, 400), Colors.BackColor);

            base.DrawScene(time);

            spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(new Vector3(20, 20, 0)));
            rects.Fill(spriteBatch, new Rectangle(0, 0, 640, 400), titleScreen.Colors.BackColor);

            titleScreen.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
