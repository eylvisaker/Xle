using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle
{
	public class DungeonMonster
	{
		public int MonsterID { get; set; }
		public int DungeonLevel { get; set; }
		public Point Location { get; set; }

		public int HP { get; set; }
	}
}
