using AgateLib.Geometry;
using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Citadel.EventExtenders
{
	class FloorPuzzle : EventExtender
	{
		double time;
		int[,] tiles;
		bool failed;
		const double secondsToTileUpdate = 3;

		public override bool StepOn(GameState state)
		{
			if (failed)
				return false;

			Point loc = state.Player.Location;
			loc.X -= TheEvent.X;
			loc.Y -= TheEvent.Y;

			StepOn(state, loc.X / 2, loc.Y / 2);
			StepOn(state, (loc.X + 1) / 2, loc.Y / 2);
			StepOn(state, (loc.X + 1) / 2, (loc.Y + 1) / 2);
			StepOn(state, loc.X / 2, (loc.Y + 1) / 2);

			return true;
		}

		private void StepOn(GameState state, int x, int y)
		{
			if (y > tiles.GetUpperBound(1))
				return;

			int tileSteppedOn = tiles[x, y];

			if (tileSteppedOn <= 0)
				tiles[x, y] = -1;
			else
			{
				FailPuzzle(state, tileSteppedOn);
			}
		}

		private void FailPuzzle(GameState state, int tileSteppedOn)
		{
			if (tileSteppedOn == -1)
				throw new ArgumentException("Tile stepped on of -1 is not a failure.");

			failed = true;
			time = 0;

			SoundMan.PlaySound(LotaSound.VeryBad);

			for (int i = 0; i < TheEvent.Width / 2; i++)
			{
				for (int j = 0; j < TheEvent.Height / 2; j++)
				{
					tiles[i, j] = tileSteppedOn;
				}
			}

			WriteMapTiles(state);

			int[] t = new int[2];

			for (int i = 1; i < TheEvent.Width - 1; i++)
			{
				int x = TheEvent.X + i;

				for (int j = 0; j < 2; j++)
				{
					int y = TheEvent.Y + TheEvent.Height + j;

					if (i == 1)
						t[j] = state.Map[x, y];
					else
						state.Map[x, y] = t[j];
				}
			}
		}

		public override void OnLoad(GameState state)
		{
			tiles = new int[TheEvent.Width / 2, TheEvent.Height / 2];

			for (int i = 0; i < TheEvent.Width; i += 2)
			{
				for (int j = 0; j < TheEvent.Height; j += 2)
				{
					var loc = new Point(TheEvent.Location);
					loc.X += i;
					loc.Y += j;

					tiles[i / 2, j / 2] = state.Map[loc];
				}
			}

			tiles[4, 5] = -1;
		}

		public override void OnUpdate(GameState state, double deltaTime)
		{
			double newtime = time + deltaTime / secondsToTileUpdate;

			if ((int)newtime != (int)time)
			{
				RotateTiles(state);
			}

			time = newtime;
		}

		private void RotateTiles(GameState state)
		{
			for (int i = 0; i < TheEvent.Width / 2; i++)
			{
				for (int j = 0; j < TheEvent.Height / 2; j++)
				{
					if (tiles[i, j] != -1)
					{
						tiles[i, j]--;
						if (tiles[i, j] < 0)
							tiles[i, j] = 3;
					}
				}
			}

			WriteMapTiles(state);
		}

		private void WriteMapTiles(GameState state)
		{
			for (int i = 0; i < TheEvent.Width / 2; i++)
			{
				for (int j = 0; j < TheEvent.Height / 2; j++)
				{
					Point mapLoc = TheEvent.Location;
					mapLoc.X += i * 2;
					mapLoc.Y += j * 2;

					int tile = tiles[i, j];

					if (tiles[i, j] == -1)
					{
						tile = 0;
					}

					for (int ii = 0; ii < 2; ii++)
					{
						for (int jj = 0; jj < 2; jj++)
						{
							state.Map[mapLoc.X + ii, mapLoc.Y + jj] = tile;
						}
					}
				}
			}
		}
	}
}
