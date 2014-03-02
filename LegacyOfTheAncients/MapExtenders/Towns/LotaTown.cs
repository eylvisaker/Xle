using ERY.Xle.XleMapTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Towns
{
	class LotaTown : NullTownExtender
	{
		public override void SetCommands(Commands.CommandList commands)
		{
			commands.Items.AddRange(LotaProgram.CommonLotaCommands);

			commands.Items.Add(new Commands.Magic());
			commands.Items.Add(new Commands.Leave { ConfirmPrompt = LotaOptions.EnhancedUserInterface });
			commands.Items.Add(new Commands.Rob());
		}
	}
}
