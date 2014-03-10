using ERY.Xle.LotA.MapExtenders.Towns.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Towns
{
	class EagleHollow : LotaTown
	{
		public override XleEventTypes.Extenders.IEventExtender CreateEventExtender(XleEvent evt, Type defaultExtender)
		{
			if (evt.ExtenderName == "Healer")
			{
				return new EagleHollowHealer();
			}

			return base.CreateEventExtender(evt, defaultExtender);
		}


	}
}
