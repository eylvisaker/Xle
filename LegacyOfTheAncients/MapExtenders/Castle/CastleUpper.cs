using ERY.Xle.XleEventTypes;
using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Castle
{
	class CastleUpper : CastleGround
	{
		public CastleUpper()
		{
			CastleLevel = 2;
		}
		public override EventExtender CreateEventExtender(XleEvent evt, Type defaultExtender)
		{
			var name = evt.ExtenderName.ToLowerInvariant();

			if (name == "spiral")
				return new Spiral();
			else if (name == "spiralsuccess")
				return new SpiralSuccess();
			else if (name == "password")
				return new PasswordDoor();
			else if (name == "arovyn")
				return new Arovyn();
			else if (name == "casandra")
				return new Casandra();
			else if (name == "wizard")
				return new Wizard();
			else if (evt is TreasureChestEvent)
				return new Chest { CastleLevel = 2 };

			return base.CreateEventExtender(evt, defaultExtender);
		}

		public override void OnAfterEntry(GameState state)
		{
			if (Lota.Story.Invisible == false)
			{
				XleCore.TextArea.PrintLine("Private level!");

				IsAngry = true;
			}
		}

		public override int GetOutsideTile(AgateLib.Geometry.Point playerPoint, int x, int y)
		{
			return TheMap.OutsideTile;
		}
	}
}
