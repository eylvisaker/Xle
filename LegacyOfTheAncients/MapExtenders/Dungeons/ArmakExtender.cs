using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.Extenders.Dungeons
{
	class ArmakExtender : LotaDungeonExtenderBase
	{

		protected override string CompleteVariable
		{
			get { return "ArmakComplete"; }
		}
		protected override int StrengthBoost
		{
			get { return 15; }
		}
	}
}
