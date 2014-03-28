using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Castle.EventExtenders
{
	class AngryOrcs : EventExtender
	{
		private DurekCastle durekCastle;

		public AngryOrcs(DurekCastle durekCastle)
		{
			this.durekCastle = durekCastle;
		}

		public override void StepOn(GameState state, ref bool handled)
		{
			handled = true;

			if (durekCastle.InOrcArea == false)
			{
				durekCastle.StoredAngryFlag = durekCastle.IsAngry;

				durekCastle.IsAngry = true;
				durekCastle.InOrcArea = true;
			}
		}
	}
}
