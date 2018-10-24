using AgateLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Maps.Towns;
using Xle.Services;

namespace Xle.Ancients.MapExtenders.Fortress
{
    [Transient("FortressSpeak")]
    public class FortressSpeak : TownSpeak
    {
        protected override async Task SpeakToGuard()
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine();

            if (GameState.MapExtender.IsAngry)
            {
                await TextArea.PrintLine("The guard ignores you.");
            }
            else
                await TextArea.PrintLine("Greetings soldier.");
        }
    }
}
