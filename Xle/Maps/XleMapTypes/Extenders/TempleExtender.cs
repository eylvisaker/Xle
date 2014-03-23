using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps.XleMapTypes.Extenders
{
	public class TempleExtender : TownExtender
	{
		protected override void PlayerFight(GameState state, Direction fightDir)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Nothing much hit.");

			SoundMan.PlaySound(LotaSound.Bump);
		}
	}
}
