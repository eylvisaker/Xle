
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Data
{
	public class MonsterInfo
	{
		public string Name { get; set; }

		public int HP { get; set; }

		public int Attack { get; set; }

		public int Defense { get; set; }

		public int Gold { get; set; }

		public int Food { get; set; }

		public TerrainType Terrain { get; set; }

		public int Vulnerability { get; set; }

		public bool Toxic { get; set; }

		public int ID { get; set; }

		public int IntimidateChance { get; set; }

		public bool Intelligent { get; set; }
	}
}
