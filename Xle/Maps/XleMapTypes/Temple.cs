using ERY.Xle.Maps.XleMapTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps.XleMapTypes
{
	public class Temple : Map2D
	{
		public ITempleExtender Extender { get; private set; }

		protected override IMapExtender CreateExtenderImpl()
		{
			if (XleCore.Factory == null)
			{
				Extender = new NullTempleExtender();
			}
			else
			{
				Extender = XleCore.Factory.CreateMapExtender(this);
			}

			return Extender;
		}

		protected override void PlayerFight(Player player, Direction fightDir)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Nothing much hit.");

			SoundMan.PlaySound(LotaSound.Bump);
		}
	}
}
