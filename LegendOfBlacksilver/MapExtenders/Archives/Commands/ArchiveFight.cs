using AgateLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle;
using Xle.Commands.Implementation;

namespace Xle.Blacksilver.MapExtenders.Archives.Commands
{
    [Transient("ArchiveFight")]
    public class ArchiveFight : Fight
    {
        public override async Task Execute()
        {
            await TextArea.PrintLine("\n\nNothing to fight.");
        }
    }
}
