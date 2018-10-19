using System;

using AgateLib.Mathematics.Geometry;

using Xle.Data;
using Xle.Maps.Dungeons.Commands;
using Xle.Maps.XleMapTypes;
using Xle.Services;
using Xle.Services.Commands.Implementation;
using Xle.Services.ScreenModel;
using Xle.Services.XleSystem;

namespace Xle.Ancients.MapExtenders.Dungeons.Commands
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
