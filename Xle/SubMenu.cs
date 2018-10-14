using Microsoft.Xna.Framework;

namespace ERY.Xle
{
    public class SubMenu
    {
        public MenuItemList theList;                // show list
        public bool onScreen;               // display on screen
        public string title;                    // title of menu
        public int value;                   // value of menu
        public int width;                   // width in CHARACTERS!!!

        public Color BackColor { get; set; }
    }
}
