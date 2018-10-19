using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Services;
using Xle.Services.Commands.Implementation;

namespace Xle.LoB.MapExtenders.Archives.Commands
{
    [ServiceName("ArchiveXamine")]
    public class ArchiveXamine : Xamine
    {
        public override void Execute()
        {
            TextArea.PrintLine();
            TextArea.PrintLine();
            TextArea.PrintLine("You are in ancient archives.");
        }
    }
}
