using AgateLib;
using System.Threading.Tasks;
using Xle.Maps.Towns;

namespace Xle.Ancients.MapExtenders.Castle.Commands
{
    [Transient("CastleSpeak")]
    public class CastleSpeak : TownSpeak
    {
        protected LotaStory Story { get { return GameState.Player.Story(); } }

        protected override async Task SpeakToGuard()
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine();

            if (Story.Invisible)
            {
                await TextArea.PrintLine("The guard looks startled.");
            }
            else
            {
                await TextArea.PrintLine("The guard ignores you.");
            }
        }
    }
}
