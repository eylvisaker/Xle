using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Castle.EventExtenders
{
	class AngryCastle : NullEventExtender
	{
		private DurekCastle durekCastle;

		public AngryCastle(DurekCastle durekCastle)
		{
			this.durekCastle = durekCastle;
		}

		public override void StepOn(GameState state, ref bool handled)
		{
			handled = true;

			durekCastle.TheMap.IsAngry = durekCastle.StoredAngryFlag;
			durekCastle.InOrcArea = false;
		}
	}
}
