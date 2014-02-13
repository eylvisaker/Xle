using ERY.Xle.XleMapTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders
{
	class DurekCastle : NullCastleExtender
	{
		public override int GetOutsideTile(AgateLib.Geometry.Point playerPoint, int x, int y)
		{
			if (playerPoint.X < 12 && playerPoint.Y < 12)
				return 0;

			if (playerPoint.X < 45 && playerPoint.Y < 22)
				return 17;

			if (playerPoint.X > 50 && playerPoint.Y > 75)
				return 16;

			return 32;
		}
	}
}
