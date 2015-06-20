using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Services
{
    public interface IQuickMenu : IXleService
    {
        int QuickMenuYesNo(bool defaultAtNo = false);

        int QuickMenu(MenuItemList items, int spaces, int value = 0, Color? clrInit = null, Color? clrChanged = null, Action redraw = null);

        [Obsolete("Use overload with optional redraw parameter instead.")]
        int QuickMenu(Action redraw, MenuItemList items, int spaces, int value = 0, Color? clrInit = null, Color? clrChanged = null);
    }
}
