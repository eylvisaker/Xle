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
			var name = evt.ExtenderName.ToLowerInvariant();

			if (name == "spiral")
				return new Spiral();
			else if (name == "spiralsuccess")
				return new SpiralSuccess();

			return base.CreateEventExtender(evt, defaultExtender);
		}
	}
}
