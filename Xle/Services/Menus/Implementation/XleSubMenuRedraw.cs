using AgateLib.DisplayLib;
using ERY.Xle.Services.Game;
using ERY.Xle.Services.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Services.Menus.Implementation
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

            Display.BeginFrame();
            Renderer.SetProjectionAndBackColors(GameState.Map.ColorScheme);

            Renderer.Draw();
            menuRenderer.DrawMenu(Menu);

            Display.EndFrame();
            gameControl.KeepAlive();
        }
    }
}
