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

        //public MenuItemList	operator=(MenuItemList theList);		// assignment operator
        [Obsolete("Use Add instead")]
        public void AddItem(string text)
        {
            Add(text);
        }

        /// <summary>
        /// 		// returns the total number of items
        /// </summary>
        [Obsolete("Use Count instead")]
        public int TotalItems { get { return Count; } }


    }
}
