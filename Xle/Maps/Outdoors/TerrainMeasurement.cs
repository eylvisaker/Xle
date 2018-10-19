using Xle.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.Maps.Outdoors
{
	public class TerrainMeasurement : ITerrainMeasurement
	{
		public GameState GameState { get; set; }

		XleMap TheMap { get { return GameState.Map; } }

		public TerrainType TerrainAtPlayer()
		{
			return TerrainAt(GameState.Player.X, GameState.Player.Y);
		}
		public TerrainType TerrainAt(int x, int y)
		{
			int[,] terrainAt = new int[2, 2] { { 0, 0 }, { 0, 0 } };
			int[] terrainCount = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };

			for (int j = 0; j < 2; j++)
			{
				for (int i = 0; i < 2; i++)
				{
					terrainAt[j, i] = TheMap[x + i, y + j];
				}
			}

			for (int j = 0; j < 2; j++)
			{
				for (int i = 0; i < 2; i++)
				{
					var terrainIndex = terrainAt[j, i] / 32;

					if (terrainIndex >= terrainCount.Length)
						continue;

					terrainCount[terrainAt[j, i] / 32]++;

					if (terrainAt[j, i] % 32 <= 1)
						terrainCount[terrainAt[j, i] / 32] += 1;
				}
			}

			if (terrainCount[(int)TerrainType.Mountain] > 4)
			{
				return TerrainType.Mountain;
			}

			if (terrainCount[(int)TerrainType.Mountain] > 0)
			{
				return TerrainType.Foothills;
			}

			if (terrainCount[(int)TerrainType.Desert] >= 1)
			{
				return TerrainType.Desert;
			}

			if (terrainCount[(int)TerrainType.Swamp] > 1)
			{
				return TerrainType.Swamp;
			}

			for (int i = 0; i < 8; i++)
			{
				if (terrainCount[i] > 3)
				{
					return (TerrainType)i;
				}
				else if (terrainCount[i] == 2 && i != 1)
				{
					return TerrainType.Mixed;
				}
			}

			return (TerrainType)2;
		}
	}
}
