﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Stores.Extenders
{
	public class StoreJail : StoreExtender
	{
		public override bool AllowInteractionWhenLoanOverdue { get { return true; } }

	}
}
