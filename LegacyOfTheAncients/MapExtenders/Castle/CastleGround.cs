using ERY.Xle.XleMapTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Castle
{
	class CastleGround : NullCastleExtender
	{
		public override int GetOutsideTile(AgateLib.Geometry.Point playerPoint, int x, int y)
		{
			if (y >= TheMap.Height)
				return 16;
			else
				return base.GetOutsideTile(playerPoint, x, y);
		}
	}
}
