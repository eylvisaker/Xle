using System;

using AgateLib.Mathematics.Geometry;

using ERY.Xle.Data;
using ERY.Xle.Maps.Dungeons.Commands;
using ERY.Xle.Services;
using ERY.Xle.Services.Commands.Implementation;
using ERY.Xle.Services.ScreenModel;
using ERY.Xle.Services.XleSystem;

namespace ERY.Xle.LoB.MapExtenders.Dungeon.Commands
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
