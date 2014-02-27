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
	class CitadelUpper  : NullCastleExtender
	{
		public override int GetOutsideTile(AgateLib.Geometry.Point playerPoint, int x, int y)
		{
			return TheMap.OutsideTile;
		}

		public override XleEventTypes.Extenders.IEventExtender CreateEventExtender(XleEvent evt, Type defaultExtender)
		{
			string name = evt.ExtenderName.ToLowerInvariant();

			if (name == "mantrek")
			{
				var mantrek = new Mantrek();
				
				if (XleCore.GameState.Story().MantrekKilled)
					mantrek.EraseMantrek(TheMap);

				return mantrek;
			}
			if (name == "staffportal")
				return new StaffPortal();
			if (name == "elf")
				return new Elf();
			if (name == "tattoo")
				return new Tattoo();

			if (evt is ChangeMapEvent)
				return new ChangeMapTeleporter();

			return base.CreateEventExtender(evt, defaultExtender);
		}
	}
}
