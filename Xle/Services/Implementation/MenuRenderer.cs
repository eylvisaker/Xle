using AgateLib.DisplayLib;
using AgateLib.Geometry;
using ERY.Xle.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Services.Implementation
{
   public class MenuRenderer : IMenuRenderer
   {
       private GameState GameState;
       private IXleRenderer Renderer;

       public MenuRenderer(GameState gameState, IXleRenderer renderer)
       {
           this.GameState = gameState;
           this.Renderer = renderer;

       }

       /// <summary>
       /// Draws the submenu created by SubMenu.
       /// </summary>
       /// <param name="menu"></param>
       public void DrawMenu(SubMenu menu)
       {
           string thestring;
           int xx, yy, i = 0, height;
           string buffer;
           Color fontColor = GameState.Map.ColorScheme.TextColor;

           xx = 624 - menu.width * 16;
           yy = 16;
           height = (menu.theList.Count + 3) * 16;

           var vertLine = GameState.Map.ColorScheme.VerticalLinePosition;

           if (xx < vertLine + 16)
           {
               xx = vertLine + 16;
               i = 1;
           }

           Display.FillRect(xx, yy, 624 - xx, height, menu.BackColor);


           if (i == 0)
           {
               xx += 16;
           }

           thestring = menu.title;

           Renderer.WriteText(xx + (int)((624 - xx) / 32) * 16 - (int)(thestring.Length / 2) * 16,
                      yy, thestring, fontColor);

           yy += 16;

           for (i = 0; i < menu.theList.Count; i++)
           {
               yy += 16;
               buffer = menu.theList[i];

               if (i > 9)
                   thestring = ((char)(i + 'A' - 10)).ToString();
               else
                   thestring = i.ToString();

               thestring += ". " + buffer;

               Renderer.WriteText(xx, yy, thestring);

               if (i == menu.value)
               {
                   int xx1;

                   xx1 = xx + thestring.Length * 16;
                   Renderer.WriteText(xx1, yy, "`");
               }
           }
       }

    }
}
