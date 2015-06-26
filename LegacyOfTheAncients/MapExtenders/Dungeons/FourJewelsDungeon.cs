using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Dungeons
{
    public class FourJewelsDungeon : LotaDungeon
    {
        public override Maps.Map3DSurfaces Surfaces(GameState state)
        {
            return Lota3DSurfaces.DungeonBlue;
        }
        protected override bool IsCompleted
        {
            get { return Story.FourJewelsComplete; }
            set { Story.FourJewelsComplete = value; }
        }

        protected override int StrengthBoost
        {
            get { return 10; }
        }

        public override int GetTreasure(GameState state, int dungeonLevel, int chestID)
        {
            if (state.Player.Items[LotaItem.GuardJewel] >= 4)
                return 0;

            return (int)LotaItem.GuardJewel;
        }

        public override void OnPlayerExitDungeon(Player player)
        {
            if (IsCompleted)
                return;

            if (player.Items[LotaItem.GuardJewel] >= 4)
            {
                IsCompleted = true;
                GivePermanentStrengthBoost();
            }
        }
    }
}
