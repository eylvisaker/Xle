using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Dungeons
{
	class ArmakExtender : LotaDungeonExtenderBase
	{
		protected override bool IsCompleted
		{
			get { return Lota.Story.ArmakComplete; }
			set { Lota.Story.ArmakComplete = value; }
		}

		protected override int StrengthBoost
		{
			get { return 15; }
		}

		public override void OnPlayerExitDungeon(Player player)
		{
			if (IsCompleted)
				return;

			IsCompleted = true;

			GivePermanentStrengthBoost(player);
		}
	}
}
