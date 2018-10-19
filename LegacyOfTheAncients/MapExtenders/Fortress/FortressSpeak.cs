using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Maps.Towns;
using Xle.Services;

namespace Xle.Ancients.MapExtenders.Fortress
{
    [ServiceName("FortressSpeak")]
    public class FortressSpeak : TownSpeak
    {
        protected override void SpeakToGuard()
        {
            TextArea.PrintLine();
            TextArea.PrintLine();

            if (GameState.MapExtender.IsAngry)
            {
                TextArea.PrintLine("The guard ignores you.");
            }
            else
                TextArea.PrintLine("Greetings soldier.");
        }
    }
}
