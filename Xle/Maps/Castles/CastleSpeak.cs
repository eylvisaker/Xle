using AgateLib;
using System.Threading.Tasks;

using Xle.Maps.Towns;

namespace Xle.Maps.Castles
{
    [Transient("CastleSpeak")]
    public class CastleSpeak : TownSpeak
    {
        protected override async Task SpeakToGuard()
        {
            await TextArea.PrintLine("\n\nThe guard ignores you.");
        }
    }
}
