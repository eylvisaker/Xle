using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Maps.Towns;
using Xle.Services;
using Xle.Services.Commands.Implementation;

namespace Xle.Maps.Castles
{
    [ServiceName("CastleSpeak")]
    public class CastleSpeak : TownSpeak
    {
        protected override void SpeakToGuard()
        {
            TextArea.PrintLine("\n\nThe guard ignores you.");
        }
    }
}
