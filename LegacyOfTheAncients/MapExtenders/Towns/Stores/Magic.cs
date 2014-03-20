using ERY.Xle.XleEventTypes.Stores.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Towns.Stores
{
	class Magic : StoreMagicExtender
	{
		public override int GetItemValue(int choice)
		{
			return choice - 1 + (int)LotaItem.MagicFlame;
		}
	}
}
