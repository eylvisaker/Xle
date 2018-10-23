using AgateLib;
using System;
using Xle.Maps.Dungeons.Commands;
using Xle.Maps.XleMapTypes;

namespace Xle.Ancients.MapExtenders.Dungeons.Commands
{
    [Transient("DungeonOpen")]
    public class DungeonOpen : DungeonOpenCommand
    {
        private Dungeon TheMap { get { return (Dungeon)GameState.Map; } }

        private LotaDungeon Map { get { return (LotaDungeon)GameState.MapExtender; } }

        protected override void GiveBoxContents()
        {
            if (GiveCompass() == false)
                base.GiveBoxContents();
        }

        private bool GiveCompass()
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
