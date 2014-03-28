using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Extenders
{
	public class ChangeMapExtender : EventExtender
	{
		public new ChangeMapEvent TheEvent { get { return (ChangeMapEvent)base.TheEvent; } }

		public virtual void OnStepOn(GameState state, ref bool cancel)
		{
		}
	}
}
