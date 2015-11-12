using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services;
using ERY.Xle.Services.Commands.Implementation;

namespace ERY.Xle.LoB.MapExtenders.Temples
{
    [ServiceName("TempleClimb")]
    public class TempleClimb : Climb
    {
        public override void Execute()
        {
            var stairs = GameState.MapExtender.Events.OfType<TempleStairs>().FirstOrDefault();

            if (stairs != null && stairs.Enabled && 
                stairs.Rectangle.X == Player.X && stairs.Rectangle.Y == Player.Y)
            {
                stairs.ExecuteMapChange();
            }
            else
            {
                TextArea.PrintLine("\n\nNothing to climb.");
            }
        }
    }
}
