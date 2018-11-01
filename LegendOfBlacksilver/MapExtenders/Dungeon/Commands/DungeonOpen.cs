using AgateLib;
using System.Threading.Tasks;
using Xle.Maps.Dungeons.Commands;

namespace Xle.Blacksilver.MapExtenders.Dungeon.Commands
{
    [Transient("DungeonOpen")]
    public class DungeonOpen : DungeonOpenCommand
    {
        private Maps.XleMapTypes.Dungeon TheMap { get { return (Maps.XleMapTypes.Dungeon)GameState.Map; } }

        private LobDungeon Map { get { return (LobDungeon)GameState.MapExtender; } }

        protected override async Task GiveChestContents(int val)
        {
            if (Map is PitsOfBlackmire && Player.DungeonLevel == 1 && val == 3)
            {
                await TextArea.PrintLine("You need a key.", XleColor.Yellow);
            }
            else
                await base.GiveChestContents(val);
        }
    }
}
