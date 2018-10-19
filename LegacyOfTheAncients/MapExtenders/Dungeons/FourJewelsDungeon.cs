using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xle.Maps;

namespace Xle.Ancients.MapExtenders.Dungeons
{
    public class FourJewelsDungeon : LotaDungeon
    {
        public override Map3DSurfaces Surfaces()
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

        public override int GetTreasure(int dungeonLevel, int chestID)
        {
            if (Player.Items[LotaItem.GuardJewel] >= 4)
                return 0;

            return (int)LotaItem.GuardJewel;
        }

        public override void OnPlayerExitDungeon()
        {
            if (IsCompleted)
                return;

            if (Player.Items[LotaItem.GuardJewel] >= 4)
            {
                IsCompleted = true;
                GivePermanentStrengthBoost();
            }
        }
    }
}
