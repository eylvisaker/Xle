using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Maps.XleMapTypes;
using Xle.Services;
using Xle.Services.Commands.Implementation;

namespace Xle.Maps.Outdoors.Commands
{
    [ServiceName("OutsideXamine")]
    public class OutsideXamine : Xamine
    {
        OutsideExtender Outside { get { return (OutsideExtender)GameState.MapExtender; } }

        public override async Task Execute()
        {
            TerrainInfo info = GetTerrainInfo();

            await TextArea.PrintLine();
            await TextArea.PrintLine();
            await TextArea.PrintLine("You are in " + info.TerrainName + ".");

            await TextArea.Print("Travel: ", XleColor.White);
            await TextArea.Print(info.TravelText, XleColor.Green);
            await TextArea.Print("  -  Food use: ", XleColor.White);
            await TextArea.Print(info.FoodUseText, XleColor.Green);
            await TextArea.PrintLine();
        }

        private TerrainInfo GetTerrainInfo()
        {
            return Outside.GetTerrainInfo();
        }
    }
}
