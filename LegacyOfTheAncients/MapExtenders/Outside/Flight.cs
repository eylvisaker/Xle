using AgateLib;
using System;
using System.Threading.Tasks;

namespace Xle.Ancients.MapExtenders.Outside
{
    [Transient("Flight")]
    public class Flight : Tarmalon
    {
        public override async Task OnAfterEntry()
        {
            if (Player.X == 11 && Player.Y == 8)
            {
                RaftData pegasus = new RaftData(11, 8, TheMap.MapID);
                pegasus.RaftImage = 1;

                Player.Rafts.Add(pegasus);
                Player.BoardedRaft = pegasus;

                await PegasusFlightToIsland();

                await TextArea.PrintLine("Pegasus sets you down.");
                SoundMan.PlaySound(LotaSound.WalkOutside);

                Player.FaceDirection = Direction.South;

                Player.BoardedRaft = null;
                pegasus.X += 3;
            }
        }

        private Tuple<int, int, int>[] points = new Tuple<int, int, int>[]
        {
            new Tuple<int,int,int>(11,33,8),
            new Tuple<int,int,int>(34,62,7),
            new Tuple<int,int,int>(63,88,-4),
            new Tuple<int,int,int>(34,58,22),
            new Tuple<int,int,int>(63,108,12)
        };

        private async Task PegasusFlightToIsland()
        {
            for (int i = 0; i < points.Length; i++)
            {
                var range = points[i];

                if (i == points.Length - 1)
                    WaterAnimLevel = 0;

                for (int x = range.Item1; x <= range.Item2; x++)
                {
                    await SetPosition(x, range.Item3);
                }

                if (i == 0)
                    WaterAnimLevel = 1;

                if (i < points.Length - 1)
                {
                    int count = 15 + Random.Next(25);

                    for (int j = 0; j < count; j++)
                    {
                        RenderState.ClearWaves?.Invoke();
                        await SetPosition(range.Item2 + 1, range.Item3);
                    }
                }
            }
        }

        private async Task SetPosition(int x, int y)
        {
            Player.X = x;
            Player.Y = y;
            Player.Food -= 0.1;

            await GameControl.WaitAsync(250);
        }

    }
}
