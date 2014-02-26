using ERY.Xle.LoB.MapExtenders.Citadel.EventExtenders;
using ERY.Xle.XleEventTypes;
using ERY.Xle.XleMapTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Citadel
{
	class CitadelGround : NullCastleExtender
	{
		public override int GetOutsideTile(AgateLib.Geometry.Point playerPoint, int x, int y)
		{
			if (playerPoint.X >= 83 && playerPoint.Y >= 65)
				return 33;
			else
				return 23;
		}

		public override XleEventTypes.Extenders.IEventExtender CreateEventExtender(XleEvent evt, Type defaultExtender)
		{
			if (evt is Door)
				return new CitadelDoor();

			return base.CreateEventExtender(evt, defaultExtender);
		}
	}
}
