using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Services
{
    public interface IXleMenu : IXleService
    {
        int SubMenu(string title, int choice, MenuItemList items, Color backColor);
    }
}
