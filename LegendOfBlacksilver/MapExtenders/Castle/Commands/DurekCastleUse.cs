using AgateLib;
using System.Threading.Tasks;

namespace Xle.Blacksilver.MapExtenders.Castle.Commands
{
    [Transient("DurekCastleUse")]
    public class DurekCastleUse : LobUse
    {
        protected override async Task<bool> UseWithMap(int item)
        {
            if (item == (int)LobItem.FalconFeather)
            {
                await TextArea.PrintLine("You're not by a door.");
                return true;
            }

            return false;
        }
    }
}
