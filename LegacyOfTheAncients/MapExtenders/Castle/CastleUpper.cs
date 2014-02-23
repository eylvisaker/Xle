using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Castle
{
	class CastleUpper : CastleGround
	{
		public override XleEventTypes.Extenders.IEventExtender CreateEventExtender(XleEvent evt, Type defaultExtender)
		{
			if (evt.ExtenderName.ToLowerInvariant() == "spiral")
				return new Spiral();

			return base.CreateEventExtender(evt, defaultExtender);
		}
	}
}
