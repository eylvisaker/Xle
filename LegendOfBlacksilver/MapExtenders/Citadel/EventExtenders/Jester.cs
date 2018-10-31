using System.Threading.Tasks;

namespace Xle.LoB.MapExtenders.Citadel.EventExtenders
{
    public class Jester : LobEvent
    {
        public override async Task<bool> Speak()
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine();

            if (Story.CitadelPassword)
            {
                await CantHelpMessage();
            }
            else if (Player.Items[LobItem.Lute] > 0)
            {
                Player.Items[LobItem.Lute] = 0;
                Story.CitadelPassword = true;

                await AcceptMessage();
            }
            else
            {
                await GiveQuestMessage();
            }

            await GameControl.WaitAsync(2500);

            return true;
        }

        private async Task CantHelpMessage()
        {

        }

        private async Task AcceptMessage()
        {
            await TextArea.PrintLine("I will take my lute.");
            await TextArea.PrintLine("Here's the password!");
            await TextArea.PrintLine("Have fun storming the castle!");
        }

        private async Task GiveQuestMessage()
        {
            await TextArea.PrintLine("Go get my lute!");
            await TextArea.PrintLine();
            await TextArea.PrintLine();
        }
    }
}
