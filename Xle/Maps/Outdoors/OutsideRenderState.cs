using System;

namespace Xle.Maps.Outdoors
{
    public class OutsideRenderState
    {
        public Direction MonsterDrawDirection { get; set; }

        public int DisplayMonsterID { get; set; } = -1;

        public Action ClearWaves { get; set; }

        /// <summary>
        /// Gets or sets whether or not the player is in stormy water
        /// </summary>
        /// <returns></returns>
        public int WaterAnimLevel { get; set; }
    }
}
