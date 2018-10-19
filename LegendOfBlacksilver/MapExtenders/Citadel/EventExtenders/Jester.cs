using Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.LoB.MapExtenders.Citadel.EventExtenders
{
    public class Jester : LobEvent
    {
        public override bool Speak()
        {
            TextArea.PrintLine();
            TextArea.PrintLine();

            if (Story.CitadelPassword)
            {
                CantHelpMessage();
            }
            else if (Player.Items[LobItem.Lute] > 0)
            {
                Player.Items[LobItem.Lute] = 0;
                Story.CitadelPassword = true;

                AcceptMessage();
            }
            else
            {
                GiveQuestMessage();
            }

            GameControl.Wait(2500);

            return true;
        }

        private void CantHelpMessage()
        {

        }

        private void AcceptMessage()
        {
            TextArea.PrintLine("I will take my lute.");
            TextArea.PrintLine("Here's the password!");
            TextArea.PrintLine("Have fun storming the castle!");
        }

        private void GiveQuestMessage()
        {
            TextArea.PrintLine("Go get my lute!");
            TextArea.PrintLine();
            TextArea.PrintLine();
        }
    }
}
