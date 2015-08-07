using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services;
using ERY.Xle.Services.Commands.Implementation;

namespace ERY.Xle.LoB.MapExtenders.Castle.Commands
{
    [ServiceName("DurekCastleUse")]
    public class DurekCastleUse : LobUse
    {
        protected override bool UseWithMap(int item)
        {
            if (item == (int)LobItem.FalconFeather)
            {
                TextArea.PrintLine("You're not by a door.");
                return true;
            }

            return false;
        }
    }
}
