using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Services;
using Xle.Services.Commands.Implementation;

namespace Xle.Maps.Museums
{
    [ServiceName("MuseumSpeak")]
    public class MuseumSpeak : Speak
    {
        public override void Execute()
        {
            TextArea.PrintLine("\n\nThere is no reply.");
        }
    }
}
