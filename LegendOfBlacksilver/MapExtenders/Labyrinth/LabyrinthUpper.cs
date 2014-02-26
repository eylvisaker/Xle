using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Labyrinth
{
	class LabyrinthUpper : LabyrinthBase
	{
		public override int GetOutsideTile(AgateLib.Geometry.Point playerPoint, int x, int y)
		{
			return TheMap.OutsideTile;
		}
	}
}
