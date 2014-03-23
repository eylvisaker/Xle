using AgateLib.Geometry;
using ERY.Xle.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps.Renderers
{
	public class Map2DRenderer : XleMapRenderer
	{
		Dictionary<int, int> mAnimatedTiles = new Dictionary<int, int>();

		public override void Draw(Point playerPos, Direction faceDirection, Rectangle inRect)
		{
			Draw2D(playerPos.X, playerPos.Y, faceDirection, inRect);
		}

		protected Point centerPoint { get; set; }
		protected Point topLeftPoint { get; private set; }

		protected void Draw2D(int x, int y, Direction faceDirction, Rectangle inRect)
		{
			int i, j;
			int initialxx = inRect.X;
			int width = inRect.Width / 16;
			int height = inRect.Height / 16;
			int tile;

			centerPoint = new Point(x, y);

			int xx = initialxx;
			int yy = 16;

			topLeftPoint = new Point(x - 11, y - 7);

			Rectangle tileRect = new Rectangle(topLeftPoint.X, topLeftPoint.Y,
				width, height);

			Animate(tileRect);

			for (j = topLeftPoint.Y; j < topLeftPoint.Y + height; j++)
			{
				for (i = topLeftPoint.X; i < topLeftPoint.X + width; i++)
				{
					tile = TileToDraw(i, j);

					XleCore.Renderer.DrawTile(xx, yy, tile);

					xx += 16;
				}

				yy += 16;
				xx = initialxx;
			}
		}

		protected virtual void Animate(Rectangle tileRect)
		{
			AnimateTiles(tileRect);
		}

		protected virtual int TileToDraw(int x, int y)
		{
			if (y < 0 || y >= TheMap.Height || x < 0 || x >= TheMap.Width)
			{
				return Extender.GetOutsideTile(centerPoint, x, y);
			}

			return TheMap[x, y];
		}

		private IEnumerable<TileGroup> GetGroupsToAnimate()
		{
			if (TheMap.TileSet == null)
				yield break;

			foreach (var group in TheMap.TileSet.TileGroups)
			{
				if (group.AnimationType == AnimationType.None)
					continue;
				if (group.Tiles.Count < 2)
					continue;

				group.TimeSinceLastAnim += AgateLib.DisplayLib.Display.DeltaTime;

				if (group.TimeSinceLastAnim >= group.AnimationTime)
				{
					group.TimeSinceLastAnim %= group.AnimationTime;
					yield return group;
				}
			}
		}

		protected virtual void AnimateTiles(Rectangle rectangle)
		{
			List<TileGroup> groupsToAnimate = GetGroupsToAnimate().ToList();

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
								nextTile = group.Tiles[XleCore.random.Next(group.Tiles.Count)];

							break;
					}

					if (group.AnimateChance == 100 || XleCore.random.Next(100) < group.AnimateChance)
					{
						TheMap[i, j] = nextTile;
					}
				}
			}
		}

	}
}
