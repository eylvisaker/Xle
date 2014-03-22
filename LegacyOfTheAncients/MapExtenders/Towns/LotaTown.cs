using ERY.Xle.LotA.MapExtenders.Towns.Stores;
using ERY.Xle.Maps.XleMapTypes.Extenders;
using ERY.Xle.XleEventTypes.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Towns
{
	class LotaTown : TownExtender
	{
		ExtenderDictionary mExtenders = new ExtenderDictionary();

		public LotaTown()
		{
			mExtenders.Add("Healer", new Healer());
		}

		public override void OnLoad(GameState state)
		{
			Lota.SetMuseumCoinOffers(XleCore.GameState);

			CheckLoan(state);
		}


		static void CheckLoan(GameState state)
		{
			if (state.Map.Events.Any(x => x is StoreLending))
			{
				if (state.Player.loan > 0 && state.Player.dueDate - state.Player.TimeDays <= 0)
				{
					XleCore.TextArea.PrintLine("This is your friendly lender.");
					XleCore.TextArea.PrintLine("You owe me money!");

					XleCore.Wait(1000);
				}
			}
		}

		public override void SetCommands(Commands.CommandList commands)
		{
			commands.Items.AddRange(LotaProgram.CommonLotaCommands);

			commands.Items.Add(new Commands.Magic());
			commands.Items.Add(new Commands.Leave { ConfirmPrompt = XleCore.Options.EnhancedUserInterface });
			commands.Items.Add(new Commands.Rob());
			commands.Items.Add(new Commands.Speak());
		}

		public override XleEventTypes.Extenders.IEventExtender CreateEventExtender(XleEvent evt, Type defaultExtender)
		{
			return mExtenders.Find(evt.ExtenderName) ?? base.CreateEventExtender(evt, defaultExtender);
		}
	}
}
