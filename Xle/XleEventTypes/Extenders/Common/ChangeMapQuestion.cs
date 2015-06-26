using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services;
using ERY.Xle.Services.Menus;

namespace ERY.Xle.XleEventTypes.Extenders.Common
{
    public class ChangeMapQuestion : ChangeMap
    {
        public IQuickMenu QuickMenu { get; set; }

        protected override bool OnStepOnImpl(ref bool cancel)
        {
            string newMapName = GetMapName();
            TextArea.PrintLine();
            TextArea.PrintLine("Enter " + newMapName + "?");

            SoundMan.PlaySound(LotaSound.Question);

            int choice = QuickMenu.QuickMenu(new MenuItemList("Yes", "No"), 3);

            if (choice == 1)
                return false;
            else if (string.IsNullOrEmpty(TheEvent.CommandText) == false)
            {
                TextArea.PrintLine();
                TextArea.PrintLine(
                    string.Format(TheEvent.CommandText,
                    Map.MapName, newMapName));

                TextArea.PrintLine();
                GameControl.Wait(500);
            }

            ExecuteMapChange();
            return true;
        }
    }
}
