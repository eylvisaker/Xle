using System;
using AgateLib.DisplayLib;
using AgateLib.Mathematics.Geometry;

namespace ERY.Xle.Services.Menus
{
    public interface IQuickMenu : IXleService
    {
        int QuickMenuYesNo(bool defaultAtNo = false);

        int QuickMenu(MenuItemList items, int spaces, int value = 0, Color? clrInit = null, Color? clrChanged = null, Action redraw = null);

        [Obsolete("Use overload with optional redraw parameter instead.")]
        int QuickMenu(Action redraw, MenuItemList items, int spaces, int value = 0, Color? clrInit = null, Color? clrChanged = null);
    }
}
