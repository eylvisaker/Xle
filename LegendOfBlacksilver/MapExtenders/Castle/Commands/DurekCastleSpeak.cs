using AgateLib;
using System.Threading.Tasks;

using Xle.Maps.Castles;
using Xle.Services.Game;

namespace Xle.Blacksilver.MapExtenders.Castle.Commands
{
    [Transient("DurekCastleSpeak")]
    public class DurekCastleSpeak : CastleSpeak
    {
        public IXleGameControl GameControl { get; set; }

        protected override async Task SpeakToGuard()
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine();

            if (Player.Items[LobItem.FalconFeather] > 0)
            {
                await TextArea.PrintLine("I see you have the feather,");
                await TextArea.PrintLine("why not use it?");
                await GameControl.Wait(1500);
            }
            else
            {
                await TextArea.PrintLine("I should not converse, sir.");
            }
        }
    }
}
