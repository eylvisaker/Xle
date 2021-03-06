﻿using AgateLib;
using System.Linq;
using System.Threading.Tasks;
using Xle.Commands.Implementation;

namespace Xle.Blacksilver.MapExtenders.Temples
{
    [Transient("TempleClimb")]
    public class TempleClimb : Climb
    {
        public override async Task Execute()
        {
            var stairs = GameState.MapExtender.Events.OfType<TempleStairs>().FirstOrDefault();

            if (stairs != null && stairs.Enabled &&
                stairs.Rectangle.X == Player.X && stairs.Rectangle.Y == Player.Y)
            {
                await stairs.ExecuteMapChange();
            }
            else
            {
                await TextArea.PrintLine("\n\nNothing to climb.");
            }
        }
    }
}
