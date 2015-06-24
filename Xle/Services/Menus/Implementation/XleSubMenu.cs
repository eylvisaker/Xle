using System;

using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;

using ERY.Xle.Services.Game;
using ERY.Xle.Services.Rendering;
using ERY.Xle.Services.XleSystem;

namespace ERY.Xle.Services.Menus.Implementation
{
    public class XleSubMenu : IXleSubMenu
    {
        private IXleRenderer Renderer;
        private IXleGameControl gameControl;
        private GameState GameState;
        private IMenuRenderer menuRenderer;
        private IXleInput input;

        public XleSubMenu(
            IXleRenderer renderer, 
            IXleGameControl gameControl, 
            IMenuRenderer menuRenderer,
            IXleInput input,
            GameState gameState)
        {
            this.Renderer = renderer;
            this.gameControl = gameControl;
            this.menuRenderer = menuRenderer;
            this.input = input;
            this.GameState = gameState;
        }

        /// <summary>
        /// This function creates a sub menu in the top of the map section and
        /// forces the player to chose an option from the list provided.	
        /// </summary>
        /// <param name="title"></param>
        /// <param name="choice"></param>
        /// <param name="items">A MenuItemList collection of menu items</param>
        /// <returns>The choice the user made.</returns>
        public int SubMenu(string title, int choice, MenuItemList items, Color? backColor = null)
        {
            SubMenu menu = new SubMenu();

            menu.title = title;
            menu.value = choice;
            menu.theList = items;
            menu.BackColor = backColor ?? XleColor.Black;

            return RunSubMenu(menu);
        }

        private int RunSubMenu(SubMenu menu)
        {
            for (int i = 0; i < menu.theList.Count; i++)
            {
                if (menu.theList[i].Length + 6 > menu.width)
                {
                    menu.width = menu.theList[i].Length + 6;
                }

            }

            string displayTitle = "Choose " + menu.title;

            if (displayTitle.Length + 2 > menu.width)
            {
                menu.width = displayTitle.Length + 2;
            }

            Action redraw = () =>
            {
                Renderer.UpdateAnim();

                Display.BeginFrame();
                Renderer.SetProjectionAndBackColors(GameState.Map.ColorScheme);

                Renderer.Draw();
                menuRenderer.DrawMenu(menu);

                Display.EndFrame();
                gameControl.KeepAlive();
            };

            KeyCode key;

            do
            {
                key = input.WaitForKey(redraw);

                if (key == KeyCode.Up)
                {
                    menu.value--;
                    if (menu.value < 0)
                        menu.value = 0;
                }
                if (key == KeyCode.Down)
                {
                    menu.value++;
                    if (menu.value >= menu.theList.Count)
                        menu.value = menu.theList.Count - 1;
                }
                else if (key >= KeyCode.D0)
                {
                    int v;

                    if (key >= KeyCode.A)
                    {
                        v = (int)(key) - (int)(KeyCode.A);
                    }
                    else
                    {
                        v = key - KeyCode.D0;
                    }

                    if (v < menu.theList.Count)
                    {
                        menu.value = v;
                        key = KeyCode.Return;
                    }
                }
            } while (key != KeyCode.Return);

            gameControl.Wait(300);

            return menu.value;
        }

    }
}
