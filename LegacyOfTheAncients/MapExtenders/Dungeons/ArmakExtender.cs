using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Dungeons
{
	class ArmakExtender : LotaDungeonExtenderBase
	{
		protected override bool IsComplete(Player player)
		{
			return Lota.Story.ArmakComplete;
		}
		protected override void SetComplete(Player player)
		{
			Lota.Story.ArmakComplete = true;
		}

		protected override int StrengthBoost
		{
			get { return 15; }
		}
	}
}
