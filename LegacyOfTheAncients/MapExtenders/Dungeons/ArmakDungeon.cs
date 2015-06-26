using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Dungeons
{
	public class ArmakDungeon : LotaDungeon
	{
		protected override bool IsCompleted
		{
			get { return Story.ArmakComplete; }
			set { Story.ArmakComplete = value; }
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

			GivePermanentStrengthBoost();
		}

		public override Maps.Map3DSurfaces Surfaces(GameState state)
		{
			return Lota3DSurfaces.DungeonBrown;
		}
	}
}
