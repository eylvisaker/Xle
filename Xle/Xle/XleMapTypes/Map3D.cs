﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace ERY.Xle.XleMapTypes
{
	public abstract class Map3D : XleMap 
	{
		protected abstract Surface Backdrop { get; }
		protected abstract Surface Wall { get; }
		protected abstract Surface SidePassages { get; }
		protected virtual Surface Door { get { return null; } }
		protected virtual Surface MuseumExhibitFrame
		{
			get { return g.MuseumExhibitFrame; }
		}
		protected virtual Surface MuseumExhibitStatic
		{
			get { return g.MuseumExhibitStatic; }
		}

		readonly Size imageSize = new Size(368, 272);

		protected override void DrawImpl(int x, int y, Direction faceDirection, Rectangle inRect)
		{
			Point stepDir = StepDirection(faceDirection);
			Point leftDir = LeftDirection(faceDirection);
			Point rightDir = RightDirection(faceDirection);

			Backdrop.Draw(inRect);

			Point loc = new Point(x, y);

			// draw up to terminal wall
			for (int distance = 0; distance < 6; distance++)
			{
				loc.X = x + distance * stepDir.X;
				loc.Y = y + distance * stepDir.Y;

				int val = this[loc.X, loc.Y];

				DrawSidePassages(loc, leftDir, rightDir, distance, inRect);

				if (IsPassable(val) == false)
				{
					DrawWall(distance, inRect);
					DrawWallOverlay(distance, inRect, val);

					break;
				}
			}
		}


		private void DrawSidePassages(Point loc, Point leftDir, Point rightDir, int distance, Rectangle destRect)
		{
			int leftValue = this[loc.X + leftDir.X, loc.Y + leftDir.Y];
			int rightValue = this[loc.X + rightDir.X, loc.Y + rightDir.Y];

			bool drawLeft = IsPassable(leftValue);
			bool drawRight = IsPassable(rightValue);

			if (drawLeft == false && drawRight == false)
				return;

			Rectangle srcRect = new Rectangle(0, distance * imageSize.Height, imageSize.Width, imageSize.Height);

			if (drawLeft == false)
			{
				srcRect.X = srcRect.Width / 2;
				srcRect.Width /= 2;

				destRect.X += destRect.Width / 2;
				destRect.Width /= 2;
			}
			else if (drawRight == false)
			{
				srcRect.Width /= 2;
				destRect.Width /= 2;
			}

			SidePassages.Draw(srcRect, destRect);
		}

		protected bool IsPassable(int value)
		{
			if (value >= 0x10 && value <= 0x3f) 
				return true;
			else
				return false;
		}


		private void DrawWall(int distance, Rectangle destRect)
		{
			Rectangle srcRect = new Rectangle(0, (distance - 1) * imageSize.Height, imageSize.Width, imageSize.Height);
			
			Wall.Draw(srcRect, destRect);
		}


		private void DrawWallOverlay(int distance, Rectangle destRect, int val)
		{
			Surface source = null;
			
			if (val == 0x01)
			{
				// torch
			}
			else if (val == 0x02)
			{
				// door
				source = Door;
			}
			else if (val >= 0x50 && val <= 0x5f)
			{
				source = MuseumExhibitFrame;

				DrawMuseumExhibit(distance, destRect, val);
			}

			if (source == null)
				return;
			
			Rectangle srcRect = new Rectangle(0, (distance - 1) * imageSize.Height, imageSize.Width, imageSize.Height);

			source.Draw(srcRect, destRect);
		}

		protected virtual void DrawMuseumExhibit(int distance, Rectangle destRect, int val)
		{
		}
		
	}
}
