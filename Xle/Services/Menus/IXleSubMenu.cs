using AgateLib.DisplayLib;
using AgateLib.Mathematics.Geometry;

namespace ERY.Xle.Services.Menus
{
	public interface IXleSubMenu : IXleService
	{
		int SubMenu(string title, int choice, MenuItemList items, Color? backColor = null);
	}
}
