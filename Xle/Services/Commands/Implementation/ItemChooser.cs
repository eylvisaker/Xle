using Xle.Data;
using Xle.Services.Menus;
using Xle.Services.ScreenModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib;

namespace Xle.Services.Commands.Implementation
{
    public interface IItemChooser
    {
        int ChooseItem();
    }

    [Singleton]
    public class ItemChooser : IItemChooser
    {
        public ITextArea TextArea { get; set; }
        public IXleSubMenu SubMenu { get; set; }
        public XleData Data { get; set; }
        public GameState GameState { get; set; }

        Player Player { get { return GameState.Player; } }

        public int ChooseItem()
        {
            TextArea.PrintLine("-choose above", XleColor.Cyan);
            MenuItemList theList = new MenuItemList();
            int value = 0;

            theList.Add("Nothing");

            foreach (int i in from kvp in Data.ItemList
                              where Player.Items[kvp.Key] > 0 &&
                              Data.MagicSpells.Values.All(
                                  x => x.ItemID != kvp.Key)
                              select kvp.Key)
            {
                string itemName = Data.ItemList[i].Name;

                if (itemName.Contains("coin"))
                    continue;

                /*
                if (i == 9)			// mail
                {
                    itemName = XleCore.GetMapName(state.Player.mailTown) + " " + itemName;
                }*/

                if (i <= Player.Hold)
                {
                    value++;
                }

                theList.Add(itemName);
            }

            var index = SubMenu.SubMenu("Hold Item", value, theList);
            var selectedName = theList[index];

            return Data.ItemList.Where(x => x.Value.Name == selectedName)
                .Select(x => x.Key).First();
        }
    }
}
