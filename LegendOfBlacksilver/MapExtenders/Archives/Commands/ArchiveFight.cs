using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Services;
using Xle.Services.Commands.Implementation;

namespace Xle.LoB.MapExtenders.Archives.Commands
{
    [ServiceName("ArchiveFight")]
    public class ArchiveFight : Fight
    {
        public override void Execute()
        {
            TextArea.PrintLine("\n\nNothing to fight.");
        }
    }
}
