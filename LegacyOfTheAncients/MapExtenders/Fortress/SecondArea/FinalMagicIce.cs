using ERY.Xle.LotA.MapExtenders.Castle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.LotA.MapExtenders.Castle.Events;

namespace ERY.Xle.LotA.MapExtenders.Fortress.SecondArea
{
	public class FinalMagicIce : MagicIce
	{
		FortressFinal fortressFinal {  get { return (FortressFinal)GameState.MapExtender; } }

		public override bool Use(int item)
		{
			base.Use(item);

			fortressFinal.CompendiumAttacking = true;

			return true;
		}
	}
}
