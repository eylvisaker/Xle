
using AgateLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Threading.Tasks;
using Xle.Services.Game;
using Xle.Services.Menus.Implementation;
using Xle.Services.Rendering;

namespace Xle.Services.Menus
{
    public interface IXleSubMenu
    {
        Task<int> SubMenu(string title, int choice, MenuItemList items, Color? backColor = null);
    }

    [Transient]
    public class XleSubMenu : IXleSubMenu
    {
        private readonly IXleGameControl gameControl;
        private readonly IMenuRenderer menuRenderer;
        private readonly IXleScreenCapture screenCapture;
        private SubMenu menu;

        public XleSubMenu(
            IXleGameControl gameControl,
            IMenuRenderer menuRenderer)
        {
            this.gameControl = gameControl;
            this.menuRenderer = menuRenderer;
        }

        /// <summary>
        /// This function creates a sub menu in the top of the map section and
        /// forces the player to chose an option from the list provided.	
        /// </summary>
        /// <param name="title"></param>
        /// <param name="choice"></param>
        /// <param name="items">A MenuItemList collection of menu items</param>
        /// <returns>The choice the user made.</returns>
        public Task<int> SubMenu(string title, int choice, MenuItemList items, Color? backColor = null)
        {
            SubMenu menu = new SubMenu();

            menu.title = title;
            menu.value = choice;
            menu.theList = items;
            menu.BackColor = backColor ?? XleColor.Black;

            return RunSubMenu(menu);
        }

        private async Task<int> RunSubMenu(SubMenu menu)
        {
            this.menu = menu;

            menuRenderer.Menu = menu;

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

            try
            {
                gameControl.PushRenderer(menuRenderer);

                Keys key;

                do
                {
                    key = await gameControl.WaitForKey(showPrompt: false);

                    if (key == Keys.Up)
                    {
                        menu.value--;
                        if (menu.value < 0)
                            menu.value = 0;
                    }
                    if (key == Keys.Down)
                    {
                        menu.value++;
                        if (menu.value >= menu.theList.Count)
                            menu.value = menu.theList.Count - 1;
                    }
                    else if (key >= Keys.D0)
                    {
                        int v;

                        if (key >= Keys.A)
                        {
                            v = (int)(key) - (int)(Keys.A);
                            v += 10;
                        }
                        else
                        {
                            v = key - Keys.D0;
                        }

                        if (v < menu.theList.Count)
                        {
                            menu.value = v;
                            key = Keys.Enter;
                        }
                    }
                } while (key != Keys.Enter);

                await gameControl.WaitAsync(300);
            }
            finally
            {
                gameControl.PopRenderer(menuRenderer);
            }

            return menu.value;
        }

    }
}
