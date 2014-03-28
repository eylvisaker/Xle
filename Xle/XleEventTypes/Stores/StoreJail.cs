using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Stores
{

	public class StoreJail : Store
	{
		protected override void AfterReadData()
		{
			ExtenderName = "Jail";
		}

	}
}
