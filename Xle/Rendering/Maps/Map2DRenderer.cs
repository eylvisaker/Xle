﻿using Xle.Maps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Xle.Rendering.Maps
{
    public abstract class Map2DRenderer : XleMapRenderer
    {
        private Rectangle tileRect = new Rectangle(0, 0, 100, 100);

        public override void Draw(SpriteBatch spriteBatch, Point playerPos,
            Direction faceDirection, Rectangle inRect)
        {
            Draw2D(spriteBatch, playerPos.X, playerPos.Y, faceDirection, inRect);
        }

        protected Point centerPoint { get; set; }

        protected Point topLeftPoint { get; private set; }

        public override void Update(GameTime time)
        {
            base.Update(time);

            Animate(time, tileRect);
        }

        protected void Draw2D(SpriteBatch spriteBatch, int x, int y, Direction faceDirction, Rectangle inRect)
        {
            int i, j;
            int initialxx = inRect.X;
            int width = inRect.Width / 16;
            int height = inRect.Height / 16;
            int tile;

            centerPoint = new Point(x, y);

            int drawx = initialxx;
            int drawy = 16;

            topLeftPoint = new Point(x - 11, y - 7);

            tileRect = new Rectangle(topLeftPoint.X, topLeftPoint.Y,
                width, height);

            for (j = topLeftPoint.Y; j < topLeftPoint.Y + height; j++)
            {
                for (i = topLeftPoint.X; i < topLeftPoint.X + width; i++)
                {
                    tile = TileToDraw(i, j);

                    Renderer.DrawTile(spriteBatch, drawx, drawy, tile);

                    drawx += 16;
                }

                drawy += 16;
                drawx = initialxx;
            }
        }

        protected virtual void Animate(GameTime time, Rectangle tileRect)
        {
            AnimateTiles(time, tileRect);
        }

        protected virtual int TileToDraw(int x, int y)
        {
            if (y < 0 || y >= TheMap.Height || x < 0 || x >= TheMap.Width)
            {
                return Extender.GetOutsideTile(centerPoint, x, y);
            }

            return TheMap[x, y];
        }

        private IEnumerable<TileGroup> GetGroupsToAnimate(GameTime time)
        {
            if (TheMap.TileSet == null)
                yield break;

            foreach (var group in TheMap.TileSet.TileGroups)
            {
                if (group.AnimationType == AnimationType.None)
                    continue;
                if (group.Tiles.Count < 2)
                    continue;

                group.TimeSinceLastAnim += time.ElapsedGameTime.TotalMilliseconds;

                if (group.TimeSinceLastAnim >= group.AnimationTime)
                {
                    group.TimeSinceLastAnim %= group.AnimationTime;
                    yield return group;
                }
            }
        }

        protected virtual void AnimateTiles(GameTime time, Rectangle rectangle)
        {
            List<TileGroup> groupsToAnimate = GetGroupsToAnimate(time).ToList();

            if (groupsToAnimate.Count == 0)
                return;

            for (int j = 0; j <= TheMap.Height; j++)
            {
                for (int i = 0; i <= TheMap.Width; i++)
                {
                    int current = TheMap[i, j];
                    TileGroup group = groupsToAnimate.FirstOrDefault(x => x.Tiles.Contains(current));

                    if (group == null) continue;
                    if (group.AnimationType == AnimationType.None) continue;

                    int nextTile = current;

                    switch (group.AnimationType)
                    {
                        case AnimationType.Loop:
                            int index = group.Tiles.IndexOf(current);
                            if (index + 1 >= group.Tiles.Count)
                            {
                                index = 0;
                            }
                            else
                                index++;

                            nextTile = group.Tiles[index];
                            break;

                        case AnimationType.Random:
                            while (nextTile == current)
                                nextTile = group.Tiles[Random.Next(group.Tiles.Count)];

                            break;
                    }

                    if (group.AnimateChance == 100 || Random.Next(100) < group.AnimateChance)
                    {
                        TheMap[i, j] = nextTile;
                    }
                }
            }
        }

    }
}
