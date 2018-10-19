using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Services;
using Xle.Services.Commands.Implementation;

namespace Xle.Maps.Museums.Commands
{
    [ServiceName("MuseumTake")]
    public class MuseumTake : Take
    {
        public override void Execute()
        {
            TextArea.PrintLine("\n\nThere is nothing to take.");
        }
    }
}
