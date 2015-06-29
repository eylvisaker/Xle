using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Maps;

namespace ERY.Xle.LotA.MapExtenders.Dungeons
{
    public class PiratesLairDungeon : LotaDungeon
    {
        protected override bool IsCompleted
        {
            get { return Story.PirateComplete; }
            set { Story.PirateComplete = value; }
        }
        protected override int StrengthBoost
        {
            get { return 10; }
        }

        public override int GetTreasure(int dungeonLevel, int chestID)
        {
            if (chestID == 1 && Player.Items[LotaItem.MagicIce] == 0 && Player.Items[LotaItem.Crown] == 0) return (int)LotaItem.Crown;
            if (chestID == 2) return (int)LotaItem.SapphireCoin;

            return 0;
        }
        public override void OnPlayerExitDungeon()
        {
            if (IsCompleted)
                return;

            if (Player.Items[LotaItem.Crown] > 0 && Player.Items[LotaItem.SapphireCoin] > 0)
            {
                IsCompleted = true;

                GivePermanentStrengthBoost();
            }
        }

        public override Map3DSurfaces Surfaces()
        {
            return Lota3DSurfaces.DungeonBlue;
        }
    }
}
