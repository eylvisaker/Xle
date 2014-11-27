using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle
{
	public class GameSpeed
	{
		public GameSpeed()
		{
			AfterExecuteCommandTime = 200;
			LeaveMapTime = 2000;

		}
		public int CastleOpenChestTime { get; set; }

		public int AfterSetGamespeedTime { get; set; }

		public int CastleOpenChestSoundTime { get; set; }

		public int DungeonOpenChestSoundTime { get; set; }

		public int OutsideStepTime { get; set; }

		public int GeneralStepTime { get; set; }

		public int DungeonStepTime { get; set; }

		public int AfterExecuteCommandTime { get; set; }

		public int LeaveMapTime { get; set; }
	}
}
