using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Citadel.EventExtenders
{
	class Jester : EventExtender
	{
		public override bool Speak(GameState state)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();

			if (Lob.Story.CitadelPassword)
			{
				CantHelpMessage();
			}
			else if (state.Player.Items[LobItem.Lute] > 0)
			{
				state.Player.Items[LobItem.Lute] = 0;
				Lob.Story.CitadelPassword = true;

				AcceptMessage();
			}
			else
			{
				GiveQuestMessage();
			}

			XleCore.Wait(2500);

			return true;
		}

		private void CantHelpMessage()
		{
			
		}

		private void AcceptMessage()
		{
			XleCore.TextArea.PrintLine("I will take my lute.");
			XleCore.TextArea.PrintLine("Here's the password!");
			XleCore.TextArea.PrintLine("Have fun storming the castle!");
		}

		private void GiveQuestMessage()
		{
			XleCore.TextArea.PrintLine("Go get my lute!");
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();
		}
	}
}
