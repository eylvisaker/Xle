using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Citadel.EventExtenders
{
	class StaffPortal : ChangeMapTeleporter
	{
		public override void OnStepOn(GameState state, ref bool cancel)
		{
			cancel = true;
		}

		public override void Use(GameState state, int item, ref bool handled)
		{
			if (item != (int)LobItem.Staff)
				return;

			handled = true;
			TeleportAnimation();

			TheEvent.ExecuteMapChange(state.Player);
		}
	}
}
