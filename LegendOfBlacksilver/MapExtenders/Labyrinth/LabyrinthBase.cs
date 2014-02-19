using ERY.Xle.XleEventTypes;
using ERY.Xle.XleMapTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Labyrinth
{
	class LabyrinthBase :NullCastleExtender
	{
		public override XleEventTypes.Extenders.IEventExtender CreateEventExtender(XleEvent evt, Type defaultExtender)
		{
			if (evt is ChangeMapEvent)
				return new ChangeMapTeleporter();

			return base.CreateEventExtender(evt, defaultExtender);
		}
	}
}
