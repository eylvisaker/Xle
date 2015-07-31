using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.LotA;
using ERY.Xle.Maps.Towns;
using ERY.Xle.Services;
using ERY.Xle.Services.Commands.Implementation;

namespace ERY.Xle.Maps.Castles
{
    [ServiceName("CastleSpeak")]
    public class CastleSpeak : TownSpeak
    {
        protected LotaStory Story { get { return GameState.Player.Story(); } }

        protected override void SpeakToGuard()
        {
            TextArea.PrintLine();
            TextArea.PrintLine();

            if (Story.Invisible)
            {
                TextArea.PrintLine("The guard looks startled.");
            }
            else
            {
                TextArea.PrintLine("The guard ignores you.");
            }
        }
    }
}
