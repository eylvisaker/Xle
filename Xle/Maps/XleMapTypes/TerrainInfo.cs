using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps.XleMapTypes
{
    public class TerrainInfo
    {
        public string TerrainName { get; set; }

        public string TravelText { get; set; }

        public string FoodUseText { get; set; }

        public LotaSound WalkSound { get; set; }

        public double StepTimeDays { get; set; }
    }
}
