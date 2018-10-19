using System;

using AgateLib.Mathematics.Geometry;

using Xle.Data;
using Xle.Maps.Dungeons.Commands;
using Xle.Services;
using Xle.Services.Commands.Implementation;
using Xle.Services.ScreenModel;
using Xle.Services.XleSystem;

namespace Xle.LoB.MapExtenders.Dungeon.Commands
{
    [ServiceName("DungeonOpen")]
    public class DungeonOpen : DungeonOpenCommand
    {
        Maps.XleMapTypes.Dungeon TheMap { get { return (Maps.XleMapTypes.Dungeon)GameState.Map; } }
        LobDungeon Map { get { return (LobDungeon)GameState.MapExtender; } }

        protected override void GiveChestContents(int val)
        {
            if (Map is PitsOfBlackmire && Player.DungeonLevel == 1 && val == 3)
            {
                TextArea.PrintLine("You need a key.", XleColor.Yellow);
            }
            else 
                base.GiveChestContents(val);
        }
    }
}
