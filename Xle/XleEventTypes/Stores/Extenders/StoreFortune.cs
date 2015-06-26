using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services;
using ERY.Xle.Services.Menus;

namespace ERY.Xle.XleEventTypes.Stores.Extenders
{
    public class StoreFortune : StoreExtender
    {
        public IQuickMenu QuickMenu { get; set; }

        protected override bool SpeakImpl()
        {
            MenuItemList theList = new MenuItemList("Yes", "No");
            int choice;

            TextArea.PrintLine();
            TextArea.PrintLine(TheEvent.ShopName, XleColor.Green);
            TextArea.PrintLine();
            TextArea.PrintLine(string.Format(
                "Read your fortune for {0} gold?", (int)(6 * TheEvent.CostFactor)));

            choice = QuickMenu.QuickMenu(theList, 3, 1);

            if (choice == 0)
            {


            }

            return true;
        }
    }
}
