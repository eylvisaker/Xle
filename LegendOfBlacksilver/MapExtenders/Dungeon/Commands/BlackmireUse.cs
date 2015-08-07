using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Maps;
using ERY.Xle.Services;

namespace ERY.Xle.LoB.MapExtenders.Dungeon.Commands
{
    [ServiceName("BlackmireUse")]
    public class BlackmireUse : LobUse
    {
        XleMap TheMap { get { return GameState.Map; } }

        protected override bool UseWithMap(int item)
        {
            if (item == (int)LobItem.RustyKey)
            {
                if (Player.DungeonLevel + 1 == 2 &&
                    TheMap[Player.X, Player.Y] == 0x33)
                {
                    TextArea.PrintLine();
                    TextArea.PrintLine("A hole appears!", XleColor.White);

                    TheMap[Player.X, Player.Y] = 0x12;

                    SoundMan.PlaySoundSync(LotaSound.VeryGood);

                    return true;
                }
            }

            return false;
        }
    }
}
