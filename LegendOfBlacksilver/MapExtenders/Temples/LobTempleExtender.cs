using ERY.Xle.Maps.XleMapTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Temples
{
	class LobTempleExtender : TempleExtender
	{
		public override void SetCommands(Commands.CommandList commands)
		{
			commands.Items.AddRange(LobProgram.CommonLobCommands);

			commands.Items.Add(new Commands.Climb());
			commands.Items.Add(new Commands.Leave());
			commands.Items.Add(new Commands.Magic());
			commands.Items.Add(new Commands.Speak());
		}

		public override void SetColorScheme(ColorScheme scheme)
		{
			base.SetColorScheme(scheme);

			scheme.FrameColor = XleColor.LightGray;
		}


		public override IEnumerable<MagicSpell> ValidMagic
		{
			get
			{
				yield return XleCore.Data.MagicSpells[3];
				yield return XleCore.Data.MagicSpells[4];
			}
		}
	}
}
