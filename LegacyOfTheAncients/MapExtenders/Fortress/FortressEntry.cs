using ERY.Xle.LotA.MapExtenders.Castle;
using ERY.Xle.XleMapTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Fortress
{
	class FortressEntry : CastleGround
	{
		public FortressEntry()
		{
			WhichCastle = 2;
			CastleLevel = 1;
			GuardAttack = 3.5;
		}

		public override XleEventTypes.Extenders.IEventExtender CreateEventExtender(XleEvent evt, Type defaultExtender)
		{
			string name = evt.ExtenderName.ToLowerInvariant();

			if (name == "magicice")
				return new MagicIce();
			else if (name == "elevator")
				return new Elevator();
			else if (name == "gastrap")
				return new GasTrap();

			return base.CreateEventExtender(evt, defaultExtender);
		}
	}
}
