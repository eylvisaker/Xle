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
            TextArea.PrintLine("\n\nNothing to climb.");
        }
    }
}
