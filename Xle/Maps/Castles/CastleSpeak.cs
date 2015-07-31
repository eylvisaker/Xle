﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Maps.Towns;
using ERY.Xle.Services;
using ERY.Xle.Services.Commands.Implementation;

namespace ERY.Xle.Maps.Castles
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
