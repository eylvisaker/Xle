using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.Xle
{
	public class MenuItemList : List<string>
	{
		public MenuItemList()
		{ }
		public MenuItemList(params string[] args)
		{
			foreach (string s in args)
				Add(s);
		}

		public static MenuItemList Numbers(int start, int end)
		{
			MenuItemList result = new MenuItemList();

			for (int i = start; i <= end; i++)
				result.Add(i.ToString());

			return result;
		}

    }
}
