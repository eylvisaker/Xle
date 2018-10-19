using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xle.Maps;

namespace Xle.Ancients.MapExtenders.Dungeons
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

		public override void OnPlayerExitDungeon()
		{
			if (IsCompleted)
				return;

			IsCompleted = true;

			GivePermanentStrengthBoost();
		}

		public override Map3DSurfaces Surfaces()
		{
			return Lota3DSurfaces.DungeonBrown;
		}
	}
}
