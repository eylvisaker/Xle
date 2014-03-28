using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Stores
{

	public class StoreVault : Store
	{
		protected override void AfterReadData()
		{
			ExtenderName = "StoreVault";
		}

		public override bool Speak(GameState state)
		{
			return false;
		}
	}

}
