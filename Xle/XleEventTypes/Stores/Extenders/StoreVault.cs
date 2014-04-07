using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Stores.Extenders
{
	public class StoreVault : StoreExtender
	{
		public override bool AllowInteractionWhenLoanOverdue { get { return true; } }

		public override bool Speak(GameState state)
		{
			return false;
		}


		public override bool AllowRobWhenNotAngry
		{
			get
			{
				return true;
			}
		}
	}

}
