using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services.Implementation;

namespace ERY.Xle.LotA.MapExtenders.Outside
{
    public class Flight : Tarmalon
    {
        public override void OnBeforeEntry(GameState state, ref int targetEntryPoint)
        {
            base.OnBeforeEntry(state, ref targetEntryPoint);

        }

        public override void OnAfterEntry(GameState state)
        {
            if (Player.X == 11 && Player.Y == 8)
            {
                RaftData pegasus = new RaftData(11, 8, TheMap.MapID);
                pegasus.RaftImage = 1;

                Player.Rafts.Add(pegasus);
                Player.BoardedRaft = pegasus;

                PegasusFlightToIsland(state);

                TextArea.PrintLine("Pegasus sets you down.");
                SoundMan.PlaySound(LotaSound.WalkOutside);

                Player.FaceDirection = Direction.South;

                Player.BoardedRaft = null;
                pegasus.X += 3;
            }
        }

        Tuple<int, int, int>[] points = new Tuple<int, int, int>[]
		{
			new Tuple<int,int,int>(11,33,8),
			new Tuple<int,int,int>(34,62,7),
			new Tuple<int,int,int>(63,88,-4),
			new Tuple<int,int,int>(34,58,22),
			new Tuple<int,int,int>(63,108,12)
		};

        private void PegasusFlightToIsland(GameState state)
        {
            for (int i = 0; i < points.Length; i++)
            {
                var range = points[i];

                if (i == points.Length - 1)
                    WaterAnimLevel = 0;

                for (int x = range.Item1; x <= range.Item2; x++)
                {
                    SetPosition(x, range.Item3);
                }

                if (i == 0)
                    WaterAnimLevel = 1;

                if (i < points.Length - 1)
                {
                    int count = 15 + Random.Next(25);

                    for (int j = 0; j < count; j++)
                    {
                        MapRenderer.ClearWaves();
                        SetPosition(range.Item2 + 1, range.Item3);
                    }
                }
            }
        }

        private void SetPosition(int x, int y)
        {
            Player.X = x;
            Player.Y = y;
            Player.Food -= 0.1;

            GameControl.Wait(250);
        }

    }
}
