using ERY.Xle.LotA.MapExtenders.Castle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public override void Use(GameState state, int item, ref bool handled)
		{
			base.Use(state, item, ref handled);

			fortressFinal.CompendiumAttacking = true;
		}
	}
}
