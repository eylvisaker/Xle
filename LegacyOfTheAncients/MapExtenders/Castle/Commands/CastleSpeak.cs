using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Ancients;
using Xle.Maps.Towns;
using Xle.Services;
using Xle.Services.Commands.Implementation;

namespace Xle.Ancients.MapExtenders.Castle.Commands
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
