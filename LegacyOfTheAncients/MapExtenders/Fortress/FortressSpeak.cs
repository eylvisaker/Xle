using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Maps.Towns;
using ERY.Xle.Services;

namespace ERY.Xle.LotA.MapExtenders.Fortress
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
