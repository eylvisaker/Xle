using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Stores
{
	public class StoreMinigame : Store
	{ }

	public class StoreWeaponTraining : Store
	{
		protected override void AfterReadData()
		{
			ExtenderName = "StoreWeaponTraining";
		}

	}
	public class StoreArmorTraining : Store
	{
		protected override void AfterReadData()
		{
			ExtenderName = "StoreArmorTraining";
		}

	}

	
	public class StoreBlackjack : Store
	{
		protected override void AfterReadData()
		{
			ExtenderName = "StoreBlackjack";
		}

	}


	public class StoreFlipFlop : Store
	{
		protected override void AfterReadData()
		{
			ExtenderName = "StoreFlipFlop";
		}

	}
}
