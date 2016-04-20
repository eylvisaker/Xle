using System;

using AgateLib.Geometry;
using AgateLib.Platform;

namespace ERY.Xle.Services.Rendering.Maps
{
    public class OutsideRenderer : Map2DRenderer, IOutsideEncounterRenderer
	{
        int[] waves;
        Rectangle drawRect;
        int mWaterAnimLevel;
        int lastAnimate = 0;

        public int DisplayMonsterID { get; set; } = -1;

        public Direction MonsterDrawDirection { get; set; }

        /// <summary>
        /// Gets or sets whether or not the player is in stormy water
        /// </summary>
        /// <returns></returns>
        public int WaterAnimLevel
        {
            get { return mWaterAnimLevel; }
            set
            {
                System.Diagnostics.Debug.Assert(value >= 0);

                mWaterAnimLevel = value;
            }
        }

        public override void Draw(Point playerPos, Direction faceDirection, Rectangle inRect)
        {
            int x = playerPos.X;
            int y = playerPos.Y;

            Draw2D(x, y, faceDirection, inRect);

            if (DisplayMonsterID > -1)
            {
                Point dir = MonsterDrawDirection.ToPoint();
                Point pos = Renderer.PlayerDrawPoint;

                pos.X -= 15;
                pos.Y -= 9;

                if (dir.X < 0)
                    pos.X -= 35;
                else if (dir.X > 0)
                    pos.X += 35;
                if (dir.Y < 0)
                    pos.Y -= 35;
                else if (dir.Y > 0)
                    pos.Y += 35;

                Renderer.DrawMonster(pos.X, pos.Y, DisplayMonsterID);
            }
        }
        protected override int TileToDraw(int x, int y)
        {
            if (TheMap[x, y] == 0)
            {
                int index = (y - topLeftPoint.Y) * drawRect.Width +
                    (x - topLeftPoint.X);

                return waves[index];
            }
            else
                return TheMap[x, y];
        }

        protected override void AnimateTiles(Rectangle rectangle)
        {
            int now = (int)Timing.TotalMilliseconds;

            if (rectangle != drawRect)
            {
                ClearWaves();

                drawRect = rectangle;
            }
            if (lastAnimate + 400 > now)
                return;

            if (waves == null || waves.Length != rectangle.Width * rectangle.Height)
            {
                waves = new int[rectangle.Width * rectangle.Height];
            }

            lastAnimate = now;

            for (int j = 0; j < rectangle.Height; j++)
            {
                for (int i = 0; i < rectangle.Width; i++)
                {
                    int x = i + rectangle.Left;
                    int y = j + rectangle.Top;
                    int index = j * rectangle.Width + i;

                    int tile = TheMap[x, y];

                    if (tile == 0)
                    {
                        if (waves[index] == 0)
                        {
                            if (Random.Next(0, 1000) < 20 * (WaterAnimLevel + 1))
                            {
                                waves[index] = Random.Next(1, 3);
                            }
                        }
                        else if (Random.Next(0, 100) < 25)
                        {
                            waves[index] = 0;
                        }
                    }
                }
            }
        }

        public void ClearWaves()
        {
            if (waves != null)
                Array.Clear(waves, 0, waves.Length);

            int now = (int)Timing.TotalMilliseconds;

            // force an update.
            lastAnimate = now - 500;
        }

    }

	public interface IOutsideEncounterRenderer
	{
		Direction MonsterDrawDirection { get; set; }
		int DisplayMonsterID { get; set; }
	}
}
