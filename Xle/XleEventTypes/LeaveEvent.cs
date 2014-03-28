using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes
{
	public class LeaveEvent : XleEvent
	{
		protected override void AfterReadData()
		{
			if (string.IsNullOrEmpty(ExtenderName))
				ExtenderName = "LeaveMap";
		}

		public override bool StepOn(GameState state)
		{
			state.MapExtender.LeaveMap(state.Player);

			return true;
		}
	}
}
