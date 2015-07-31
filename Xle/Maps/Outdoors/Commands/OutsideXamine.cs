using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Services;
using ERY.Xle.Services.Commands.Implementation;

namespace ERY.Xle.Maps.Outdoors.Commands
{
    [ServiceName("OutsideXamine")]
    public class OutsideXamine : Xamine
    {
        OutsideExtender Outside { get { return (OutsideExtender)GameState.MapExtender; } }

        public override void Execute()
        {
            TerrainInfo info = GetTerrainInfo();

            TextArea.PrintLine();
            TextArea.PrintLine();
            TextArea.PrintLine("You are in " + info.TerrainName + ".");

            TextArea.Print("Travel: ", XleColor.White);
            TextArea.Print(info.TravelText, XleColor.Green);
            TextArea.Print("  -  Food use: ", XleColor.White);
            TextArea.Print(info.FoodUseText, XleColor.Green);
            TextArea.PrintLine();
        }

        private TerrainInfo GetTerrainInfo()
        {
            return Outside.GetTerrainInfo();
        }
    }
}
