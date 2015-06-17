using ERY.Xle.Data;
using ERY.Xle.Maps.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services;
using ERY.Xle.Services.Implementation;
using ERY.Xle.Services.Implementation.Commands;

namespace ERY.Xle.LoB.MapExtenders.Temples
{
	class LobTempleExtender : TempleExtender
	{
		public override void SetCommands(ICommandList commands)
		{
			commands.Items.AddRange(LobProgram.CommonLobCommands);

			commands.Items.Add(new Climb());
			commands.Items.Add(new Leave());
			commands.Items.Add(new Magic());
			commands.Items.Add(new Speak());
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
