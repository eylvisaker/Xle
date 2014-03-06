using ERY.Xle.LotA.MapExtenders.Towns.Stores;
using ERY.Xle.Maps.XleMapTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Towns
{
	class LotaTown : NullTownExtender
	{
		ExtenderDictionary mExtenders = new ExtenderDictionary();

		public LotaTown()
		{
			mExtenders.Add("Healer", new Healer());
		}

		public override void SetCommands(Commands.CommandList commands)
		{
			commands.Items.AddRange(LotaProgram.CommonLotaCommands);

			commands.Items.Add(new Commands.Magic());
			commands.Items.Add(new Commands.Leave { ConfirmPrompt = LotaOptions.EnhancedUserInterface });
			commands.Items.Add(new Commands.Rob());
			commands.Items.Add(new Commands.Speak());
		}

		public override XleEventTypes.Extenders.IEventExtender CreateEventExtender(XleEvent evt, Type defaultExtender)
		{
			return mExtenders.Find(evt.ExtenderName) ?? base.CreateEventExtender(evt, defaultExtender);
		}
	}
}
