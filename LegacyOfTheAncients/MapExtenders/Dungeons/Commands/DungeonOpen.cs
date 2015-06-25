using System;

using AgateLib.Geometry;

using ERY.Xle.Data;
using ERY.Xle.Maps.Dungeons.Commands;
using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Services;
using ERY.Xle.Services.Commands.Implementation;
using ERY.Xle.Services.ScreenModel;
using ERY.Xle.Services.XleSystem;

namespace ERY.Xle.LotA.MapExtenders.Dungeons.Commands
{
    [ServiceName("DungeonOpen")]
    public class DungeonOpen : DungeonOpenCommand
    {
        Dungeon TheMap { get { return (Dungeon)GameState.Map; } }
        LotaDungeon Map { get { return (LotaDungeon)GameState.MapExtender; } }

        protected override void GiveBoxContents()
        {
            if (GiveCompass() == false)
                base.GiveBoxContents();
        }

        bool GiveCompass()
        {
            if (Player.DungeonLevel == 0)
                return false;
            if (Player.Items[LotaItem.Compass] > 0)
                return false;

            if (Random.NextDouble() < .6)
            {
                TextArea.PrintLine("You find a compass!", XleColor.Yellow);
                Player.Items[LotaItem.Compass] += 1;

                SoundMan.PlaySound(LotaSound.VeryGood);

                return true;
            }

            return false;
        }

    }
}
