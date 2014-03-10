using ERY.Xle.Maps.XleMapTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Outside
{
	class TarmalonExtender : NullOutsideExtender
	{
		public override void OnLoad(GameState state)
		{
			Lota.Story.Invisible = false;

			XleCore.Renderer.PlayerColor = XleColor.White;
		}

		public int Stormy
		{
			get { return TheMap.Stormy; }
			set { TheMap.Stormy = value; }
		}

		public override void SetCommands(Commands.CommandList commands)
		{
			commands.Items.AddRange(LotaProgram.CommonLotaCommands);

			commands.Items.Add(new Commands.Disembark());
			commands.Items.Add(new Commands.End());
			commands.Items.Add(new Commands.Magic());
			commands.Items.Add(new Commands.Speak());
		}

		public override void SetColorScheme(ColorScheme scheme)
		{
			base.SetColorScheme(scheme);

			scheme.MapAreaWidth = 23;
		}
	}
}
