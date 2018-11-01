using AgateLib;
using Microsoft.Xna.Framework;
using System;
using System.Threading.Tasks;
using Xle.XleEventTypes.Extenders;

namespace Xle.Blacksilver.MapExtenders.Citadel.EventExtenders
{
    [Transient("FloorPuzzle")]
    public class FloorPuzzle : EventExtender
    {
        private double time;
        private int[,] tiles;
        private bool failed;
        private const double secondsToTileUpdate = 3;

        public override Task<bool> StepOn()
        {
            if (failed)
                return Task.FromResult(false);

            Point loc = Player.Location;
            loc.X -= TheEvent.X;
            loc.Y -= TheEvent.Y;

            StepOn(loc.X / 2, loc.Y / 2);
            StepOn((loc.X + 1) / 2, loc.Y / 2);
            StepOn((loc.X + 1) / 2, (loc.Y + 1) / 2);
            StepOn(loc.X / 2, (loc.Y + 1) / 2);

            return Task.FromResult(true);
        }

        private void StepOn(int x, int y)
        {
            if (y > tiles.GetUpperBound(1))
                return;

            int tileSteppedOn = tiles[x, y];

            if (tileSteppedOn <= 0)
                tiles[x, y] = -1;
            else
            {
                FailPuzzle(tileSteppedOn);
            }
        }

        private void FailPuzzle(int tileSteppedOn)
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

            WriteMapTiles();

            int[] t = new int[2];

            for (int i = 1; i < TheEvent.Width - 1; i++)
            {
                int x = TheEvent.X + i;

                for (int j = 0; j < 2; j++)
                {
                    int y = TheEvent.Y + TheEvent.Height + j;

                    if (i == 1)
                        t[j] = Map[x, y];
                    else
                        Map[x, y] = t[j];
                }
            }
        }

        public override void OnLoad()
        {
            tiles = new int[TheEvent.Width / 2, TheEvent.Height / 2];

            for (int i = 0; i < TheEvent.Width; i += 2)
            {
                for (int j = 0; j < TheEvent.Height; j += 2)
                {
                    var loc = TheEvent.Location;
                    loc.X += i;
                    loc.Y += j;

                    tiles[i / 2, j / 2] = Map[loc];
                }
            }

            tiles[4, 5] = -1;
        }

        public override void OnUpdate(GameTime gameTime)
        {
            double newtime = time + gameTime.ElapsedGameTime.TotalSeconds / secondsToTileUpdate;

            if ((int)newtime != (int)time)
            {
                RotateTiles();
            }

            time = newtime;
        }

        private void RotateTiles()
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

            WriteMapTiles();
        }

        private void WriteMapTiles()
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
                            Map[mapLoc.X + ii, mapLoc.Y + jj] = tile;
                        }
                    }
                }
            }
        }
    }
}
