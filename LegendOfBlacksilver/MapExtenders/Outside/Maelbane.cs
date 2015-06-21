using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Outside
{
    public class Maelbane : LobBaseOutside
    {
        public override void SetColorScheme(ColorScheme scheme)
        {
            base.SetColorScheme(scheme);

            scheme.FrameColor = XleColor.Orange;
            scheme.FrameHighlightColor = XleColor.Green;
            scheme.TextColor = XleColor.White;
        }

        public override void ModifyTerrainInfo(Maps.XleMapTypes.TerrainInfo info, TerrainType terrain)
        {
            switch (terrain)
            {
                case TerrainType.Desert:
                    info.TerrainName = "scrubland";
                    info.TravelText = "average";
                    info.FoodUseText = "medium";
                    info.WalkSound = LotaSound.WalkOutside;
                    info.StepTimeDays = 0.5;
                    break;

                case TerrainType.Swamp:
                    info.TerrainName = "marsh";
                    break;
            }
        }
    }
}
