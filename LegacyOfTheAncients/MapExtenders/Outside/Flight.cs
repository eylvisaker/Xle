using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Outside
{
	class Flight : TarmalonExtender
	{
		public override void OnBeforeEntry(GameState state, ref int targetEntryPoint)
		{
			base.OnBeforeEntry(state, ref targetEntryPoint);

		}

		public override void OnAfterEntry(GameState state)
		{
			if (state.Player.X == 11 && state.Player.Y == 8)
			{
				RaftData pegasus = new RaftData(11, 8, TheMap.MapID);
				pegasus.RaftImage = 1;

				state.Player.Rafts.Add(pegasus);
				state.Player.BoardedRaft = pegasus;

				PegasusFlightToIsland(state);
				XleCore.TextArea.PrintLine("Pegasus sets you down.");
				SoundMan.PlaySound(LotaSound.WalkOutside);

				state.Player.FaceDirection = Direction.South;

				state.Player.BoardedRaft = null;
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
			for(int i = 0; i < points.Length; i++)
			{
				var range = points[i];

				if (i == points.Length - 1)
					WaterAnimLevel = 0;

				for (int x = range.Item1; x <= range.Item2; x++)
				{
					SetPosition(state, x, range.Item3);
				}

				if (i == 0)
					WaterAnimLevel = 1;

				if (i < points.Length - 1)
				{
					int count = 15 + XleCore.random.Next(25);

					for (int j = 0; j < count; j++)
					{
						TheMap.ClearWaves();
						SetPosition(state, range.Item2+1, range.Item3);
					}
				}
			}
		}

		private static void SetPosition(GameState state, int x, int y)
		{
			state.Player.X = x;
			state.Player.Y = y;
			state.Player.Food -= 0.1;

			XleCore.Wait(250);
		}

	}
}
