﻿using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Castle.EventExtenders
{
	class AngryCastle : EventExtender
	{
		private DurekCastle durekCastle;

		public AngryCastle(DurekCastle durekCastle)
		{
			this.durekCastle = durekCastle;
		}

		public override void StepOn(GameState state, ref bool handled)
		{
			handled = true;

			durekCastle.IsAngry = durekCastle.StoredAngryFlag;
			durekCastle.InOrcArea = false;
		}
	}
}
