using ERY.Xle.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Maps.Outdoors
{
    public class TerrainMeasurement : ITerrainMeasurement
    {
        public GameState GameState { get; set; }

        XleMap TheMap {  get { return GameState.Map; } }

        public TerrainType TerrainAtPlayer()
        {
            return TerrainAt(GameState.Player.X, GameState.Player.Y);
        }
        public TerrainType TerrainAt(int x, int y)
        {
            int[,] t = new int[2, 2] { { 0, 0 }, { 0, 0 } };
            int[] tc = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };

            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < 2; i++)
                {
                    t[j, i] = TheMap[x + i, y + j];
                }
            }

            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < 2; i++)
                {
                    tc[t[j, i] / 32]++;

                    if (t[j, i] % 32 <= 1)
                        tc[t[j, i] / 32] += 1;
                }
            }

            if (tc[(int)TerrainType.Mountain] > 4)
            {
                return TerrainType.Mountain;
            }

            if (tc[(int)TerrainType.Mountain] > 0)
            {
                return TerrainType.Foothills;
            }

            if (tc[(int)TerrainType.Desert] >= 1)
            {
                return TerrainType.Desert;
            }

            if (tc[(int)TerrainType.Swamp] > 1)
            {
                return TerrainType.Swamp;
            }

            for (int i = 0; i < 8; i++)
            {
                if (tc[i] > 3)
                {
                    return (TerrainType)i;
                }
                else if (tc[i] == 2 && i != 1)
                {
                    return TerrainType.Mixed;
                }
            }

            return (TerrainType)2;
        }
    }
}
