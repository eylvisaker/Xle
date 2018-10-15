using AgateLib;
using AgateLib.Scenes;
using ERY.Xle.LotA.TitleScreen;
using Microsoft.Xna.Framework;

namespace Xle.Ancients.TitleScreen
{
    [Transient]
    public class LotaTitleScene : Scene
    {
        private readonly ILotaTitleScreen titleScreen;

        public LotaTitleScene(ILotaTitleScreen titleScreen)
        {
            this.titleScreen = titleScreen;
        }

        protected override void OnUpdate(GameTime time)
        {
            base.OnUpdate(time);

            titleScreen.Update(time);
        }

        protected override void DrawScene(GameTime time)
        {
            base.DrawScene(time);

            
        }
    }
}
