using AgateLib;
using System.Threading.Tasks;

using Xle.Maps;

namespace Xle.Blacksilver.MapExtenders.Dungeon.Commands
{
    [Transient("BlackmireUse")]
    public class BlackmireUse : LobUse
    {
        private XleMap TheMap { get { return GameState.Map; } }

        protected override async Task<bool> UseWithMap(int item)
        {
            if (item == (int)LobItem.RustyKey)
            {
                if (Player.DungeonLevel + 1 == 2 &&
                    TheMap[Player.X, Player.Y] == 0x33)
                {
                    await TextArea.PrintLine();
                    await TextArea.PrintLine("A hole appears!", XleColor.White);

                    TheMap[Player.X, Player.Y] = 0x12;

                    await GameControl.PlaySoundWait(LotaSound.VeryGood);

                    return true;
                }
            }

            return false;
        }
    }
}
