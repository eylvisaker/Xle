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
			MenuItemList retval = new MenuItemList();

			for (int i = start; i <= end; i++)
				retval.Add(i.ToString());

			return retval;
		}
	}
}
