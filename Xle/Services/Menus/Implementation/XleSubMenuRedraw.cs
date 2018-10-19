using Xle.Services.Game;
using Xle.Services.Rendering;
using System;

namespace Xle.Services.Menus.Implementation
{
    public class XleSubMenuRedraw : IXleSubMenuRedraw
    {
        private readonly IXleGameControl gameControl;
        private readonly GameState GameState;
        private readonly IMenuRenderer menuRenderer;
        private readonly IXleRenderer Renderer;

        public XleSubMenuRedraw(
            IXleRenderer renderer,
            IMenuRenderer menuRenderer,
            IXleGameControl gameControl,
            GameState gameState)
        {
            this.Renderer = renderer;
            this.menuRenderer = menuRenderer;
            this.gameControl = gameControl;
            this.GameState = gameState;
        }

        public SubMenu Menu { get; set; }

        public void Redraw()
        {
            Renderer.UpdateAnim();

            throw new NotImplementedException();
            //Display.BeginFrame();
            //Renderer.SetProjectionAndBackColors(GameState.Map.ColorScheme);

            //Renderer.Draw();
            //menuRenderer.DrawMenu(Menu);

            //Display.EndFrame();
            //gameControl.KeepAlive();
        }
    }
}
