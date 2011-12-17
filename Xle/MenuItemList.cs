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
	}
}
