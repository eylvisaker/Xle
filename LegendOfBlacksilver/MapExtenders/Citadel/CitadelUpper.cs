using ERY.Xle.LoB.MapExtenders.Labyrinth;
using ERY.Xle.XleEventTypes;
using ERY.Xle.XleMapTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Citadel
{
	class CitadelUpper  : NullCastleExtender
	{
		public override int GetOutsideTile(AgateLib.Geometry.Point playerPoint, int x, int y)
		{
			return 71;
		}

		public override XleEventTypes.Extenders.IEventExtender CreateEventExtender(XleEvent evt, Type defaultExtender)
		{
			if (evt is ChangeMapEvent)
				return new ChangeMapTeleporter();

			return base.CreateEventExtender(evt, defaultExtender);
		}
	}
}
