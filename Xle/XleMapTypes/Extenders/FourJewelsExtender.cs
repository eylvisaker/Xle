using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.Extenders
{
	class FourJewelsExtender : LotaDungeonExtenderBase
	{
		protected override string CompleteVariable
		{
			get { return "FourJewelComplete"; }
		}
		protected override int StrengthBoost
		{
			get { return 10; }
		}
		public override void OnBeforeGiveItem(Player player, ref int treasure, ref bool handled)
		{
			// guard jewel
			if (treasure == 14)
			{
				if (player.Item(14) >= 4)
					treasure = 0;
			}
		}
	}
}
