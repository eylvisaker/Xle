using Xle.XleEventTypes.Stores;
using Xle.XleEventTypes.Stores.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.Ancients.MapExtenders.Towns.Stores
{
	public class Magic : StoreMagic
	{
		public override int GetItemValue(int choice)
		{
			return choice - 1 + (int)LotaItem.MagicFlame;
		}
	}
}
