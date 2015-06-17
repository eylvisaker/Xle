﻿using ERY.Xle.LotA.MapExtenders.Castle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.LotA.MapExtenders.Castle.Events;

namespace ERY.Xle.LotA.MapExtenders.Fortress.SecondArea
{
	class FinalMagicIce : MagicIce
	{
		private FortressFinal fortressFinal;

		public FinalMagicIce(FortressFinal fortressFinal)
		{
			// TODO: Complete member initialization
			this.fortressFinal = fortressFinal;
		}

		public override bool Use(GameState state, int item)
		{
			base.Use(state, item);

			fortressFinal.CompendiumAttacking = true;

			return true;
		}
	}
}
