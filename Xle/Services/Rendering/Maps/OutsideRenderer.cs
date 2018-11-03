using AgateLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Xle.Maps.Outdoors;

namespace Xle.Services.Rendering.Maps
{
    [Transient]
    public class OutsideRenderer : Map2DRenderer
    {
        private int[] waves= new int[35];
        private Rectangle drawRect;
        private int mWaterAnimLevel;
        private float timeToNextAnimate = 0;

        public OutsideRenderState RenderState => Extender.RenderState;

        int DisplayMonsterID => RenderState.DisplayMonsterID;

        Direction MonsterDrawDirection => RenderState.MonsterDrawDirection;

        public new OutsideExtender Extender => (OutsideExtender)base.Extender;

        public int WaterAnimLevel => RenderState.WaterAnimLevel;

        protected override void OnExtenderSet()
        {
            base.OnExtenderSet();

            RenderState.ClearWaves = ClearWaves;
        }

        public override void Draw(SpriteBatch spriteBatch, Point playerPos, Direction faceDirection, Rectangle inRect)
        {
            int x = playerPos.X;
            int y = playerPos.Y;

            Draw2D(spriteBatch, x, y, faceDirection, inRect);

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

                Renderer.DrawMonster(spriteBatch, pos.X, pos.Y, DisplayMonsterID);
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

        protected override void AnimateTiles(GameTime time, Rectangle rectangle)
        {
            if (rectangle != drawRect)
            {
                ClearWaves();

                drawRect = rectangle;
            }

            timeToNextAnimate -= (float)time.ElapsedGameTime.TotalMilliseconds;

            if (waves == null || waves.Length != rectangle.Width * rectangle.Height)
            {
                waves = new int[rectangle.Width * rectangle.Height];
            }

            if (timeToNextAnimate > 0)
                return;

            timeToNextAnimate = 400;

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
            
            // force an update.
            timeToNextAnimate = 500;
        }
    }
}
