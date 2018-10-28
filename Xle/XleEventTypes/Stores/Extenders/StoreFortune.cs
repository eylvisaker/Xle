using AgateLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xle.Services;
using Xle.Services.Menus;

namespace Xle.XleEventTypes.Stores.Extenders
{
    [Transient("StoreFortune")]
    public class StoreFortune : StoreExtender
    {
        public IQuickMenu QuickMenu { get; set; }

        protected override async Task<bool> SpeakImplAsync()
        {
            MenuItemList theList = new MenuItemList("Yes", "No");
            int choice;

            await TextArea.PrintLine();
            await TextArea.PrintLine(TheEvent.ShopName, XleColor.Green);
            await TextArea.PrintLine();
            await TextArea.PrintLine(string.Format(
                "Read your fortune for {0} gold?", (int)(6 * TheEvent.CostFactor)));

            choice = await QuickMenu.QuickMenu(theList, 3, 1);

            if (choice == 0)
            {


            }

            await TextArea.PrintLine("Fortune teller is not implemented.");
            await StoreSound(LotaSound.Medium);

            return true;
        }
    }
}
