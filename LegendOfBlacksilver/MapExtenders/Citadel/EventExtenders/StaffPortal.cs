using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Citadel.EventExtenders
{
	class StaffPortal : ChangeMapTeleporter
	{
		public override bool StepOn(GameState state)
		{
			return true;
		}

		public override bool Use(GameState state, int item)
		{
			if (item != (int)LobItem.Staff)
				return false;

			TeleportAnimation();

			TheEvent.ExecuteMapChange(state.Player);

			return true;			
		}
	}
}
