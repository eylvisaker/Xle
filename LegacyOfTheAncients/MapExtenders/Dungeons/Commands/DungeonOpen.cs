using AgateLib;
using System;
using System.Threading.Tasks;
using Xle.Maps.Dungeons.Commands;
using Xle.Maps.XleMapTypes;

namespace Xle.Ancients.MapExtenders.Dungeons.Commands
{
    [Transient("DungeonOpen")]
    public class DungeonOpen : DungeonOpenCommand
    {
        public DungeonOpen()
        {

        }

        private Dungeon TheMap { get { return (Dungeon)GameState.Map; } }

        private LotaDungeon Map { get { return (LotaDungeon)GameState.MapExtender; } }

        protected override async Task GiveBoxContents()
        {
            if (await GiveCompass() == false)
                await base.GiveBoxContents();
        }

        private async Task<bool> GiveCompass()
        {
            if (Player.DungeonLevel == 0)
                return false;
            if (Player.Items[LotaItem.Compass] > 0)
                return false;

            if (Random.NextDouble() < .6)
            {
                await TextArea.PrintLine("You find a compass!", XleColor.Yellow);
                Player.Items[LotaItem.Compass] += 1;

                SoundMan.PlaySound(LotaSound.VeryGood);

                return true;
            }

            return false;
        }

    }
}
