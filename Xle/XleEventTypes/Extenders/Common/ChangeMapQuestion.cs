﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xle.Services;
using Xle.Services.Menus;

namespace Xle.XleEventTypes.Extenders.Common
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
            throw new NotImplementedException();

            //int choice = QuickMenu.QuickMenu(new MenuItemList("Yes", "No"), 3);

            //if (choice == 1)
            //    return false;
            //else if (string.IsNullOrEmpty(TheEvent.CommandText) == false)
            //{
            //    TextArea.PrintLine();
            //    TextArea.PrintLine(
            //        string.Format(TheEvent.CommandText,
            //        Map.MapName, newMapName));

            //    TextArea.PrintLine();
            //    GameControl.Wait(500);
            //}

            ExecuteMapChange();
            return true;
        }
    }
}
