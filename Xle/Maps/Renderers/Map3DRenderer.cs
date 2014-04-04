using AgateLib.DisplayLib;
using AgateLib.Geometry;
using ERY.Xle.Maps.Extenders;
using ERY.Xle.Maps.XleMapTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps.Renderers
{
	public abstract class Map3DRenderer : XleMapRenderer
	{
		public new Map3D TheMap { get { return (Map3D)base.TheMap; } }
		public new Map3DExtender Extender { get { return (Map3DExtender)base.Extender; } }

		enum SidePassageType
		{
			Wall,
			Parallel,
			Corner,
			Corridor,
		}

		protected enum ExtraType
		{
			None,
			Chest,
			Box,
			GoUp,
			GoDown,
			Needle,
			Slime,
			TripWire,
			GasVent,
			DisplayCaseLeft,
			DisplayCaseRight,
			TorchLeft,
			TorchRight,
			DoorLeft,
			DoorRight,
		}

		readonly Size imageSize = new Size(368, 272);

		protected override void OnExtenderSet()
		{
			LoadImages();
		}
		public void LoadImages()
		{
			if (XleCore.Factory == null)
				return;

			Surfaces = Extender.Surfaces(XleCore.GameState);
		}

		public Map3DSurfaces Surfaces { get; set; }
		protected virtual bool ExtraScale { get { return false; } }

		public override void Draw(Point playerPos, Direction faceDirection, Rectangle inRect)
		{
			DrawImpl(playerPos.X, playerPos.Y, faceDirection, inRect);
		}

		void DrawImpl(int x, int y, Direction faceDirection, Rectangle inRect)
		{
			if (DrawCloseup)
			{
				DrawCloseupImpl(inRect);
				return;
			}

			Point stepDir = faceDirection.StepDirection();
			Point leftDir = faceDirection.LeftDirection();
			Point rightDir = faceDirection.RightDirection();

			// Draw the backdrop
			Surfaces.Walls.Draw(new Rectangle(Point.Empty, screenSize), inRect);

			Point loc = new Point(x, y);

			int maxDistance = 0;

			// draw from terminal wall back
			for (int distance = 0; distance < 6; distance++)
			{
				loc.X = x + distance * stepDir.X;
				loc.Y = y + distance * stepDir.Y;

				int val = TheMap[loc.X, loc.Y];

				maxDistance = distance;

				if (IsPassable(val) == false)
				{
					break;
				}
			}

			for (int distance = maxDistance; distance >= 0; distance--)
			{
				loc.X = x + distance * stepDir.X;
				loc.Y = y + distance * stepDir.Y;

				int val = TheMap[loc.X, loc.Y];

				DrawSidePassages(loc, stepDir, leftDir, rightDir, distance, inRect);

				if (distance == maxDistance && IsPassable(val) == false)
				{
					DrawWall(maxDistance, inRect);
					DrawWallOverlay(maxDistance, inRect, val);
				}
			}

			for (int distance = 0; distance < maxDistance; distance++)
			{
				for (int dir = -1; dir <= 1; dir++)
				{
					loc.X = x + distance * stepDir.X + dir * rightDir.X;
					loc.Y = y + distance * stepDir.Y + dir * rightDir.Y;

					int val = TheMap[loc.X, loc.Y];

					if (val == 0) continue;
					if (val == 0x10) continue;

					DrawExtras(val, dir, loc, distance, inRect);
				}
			}

			DrawMonsters(x, y, faceDirection, inRect, maxDistance);
		}

		protected virtual void DrawMonsters(int x, int y, Direction faceDirection, Rectangle inRect, int maxDistance)
		{
		}

		public bool DrawCloseup { get; set; }
		protected virtual void DrawCloseupImpl(Rectangle inRect)
		{
			throw new NotImplementedException();
		}

		protected bool IsPassable(int value)
		{
			if (value >= 0x10 && value <= 0x3f)
				return true;
			else
				return false;
		}

		private SidePassageType GetSidePassageType(int distance, int sideVal, int sideForwardVal, int forwardVal)
		{
			SidePassageType sideType;

			if (IsPassable(sideVal) == false)
			{
				return SidePassageType.Wall;
			}
			else if (IsPassable(forwardVal))
				sideType = SidePassageType.Corridor;
			else if (IsPassable(sideForwardVal))
				sideType = SidePassageType.Parallel;
			else
				sideType = SidePassageType.Corner;

			return sideType;
		}

		private void DrawSidePassages(Point loc, Point lookDir, Point leftDir, Point rightDir, int distance, Rectangle maindestRect)
		{
			Point forwardPt = new Point(loc.X + lookDir.X, loc.Y + lookDir.Y);
			Point leftPt = new Point(loc.X + leftDir.X, loc.Y + leftDir.Y);
			Point rightPt = new Point(loc.X + rightDir.X, loc.Y + rightDir.Y);
			Point leftForwardPt = new Point(leftPt.X + lookDir.X, leftPt.Y + lookDir.Y);
			Point rightForwardPt = new Point(rightPt.X + lookDir.X, rightPt.Y + lookDir.Y);

			int forwardValue = MapValueAt(forwardPt);
			int leftValue = MapValueAt(leftPt);
			int rightValue = MapValueAt(rightPt);
			int leftForwardValue = MapValueAt(leftForwardPt);
			int rightForwardValue = MapValueAt(rightForwardPt);

			SidePassageType leftType = GetSidePassageType(distance, leftValue, leftForwardValue, forwardValue);
			SidePassageType rightType = GetSidePassageType(distance, rightValue, rightForwardValue, forwardValue);

			Rectangle srcRect, destRect;

			srcRect = GetSidePassageSrcRect(distance, false, leftType);
			destRect = GetSidePassageDestRect(distance, false, maindestRect);
			Surfaces.Walls.Draw(srcRect, destRect);

			srcRect = GetSidePassageSrcRect(distance, true, rightType);
			destRect = GetSidePassageDestRect(distance, true, maindestRect);
			Surfaces.Walls.Draw(srcRect, destRect);

		}

		private int MapValueAt(Point leftPt)
		{
			return TheMap[leftPt.X, leftPt.Y];
		}

		readonly int[] sideWidth = new int[] { 3, 3, 2, 1, 1, 1, 1 };

		private Rectangle GetSidePassageSrcRect(int distance, bool rightSide, SidePassageType type)
		{
			Rectangle retval = new Rectangle();

			retval.Width = sideWidth[distance];// +sideWidth[distance + 1];

			for (int i = 0; i < distance; i++)
			{
				retval.X += sideWidth[i];
			}

			retval.X *= 16;
			retval.Y *= 16;
			retval.Width *= 16;
			retval.Height *= 16;

			retval.Y += imageSize.Height * (int)type;
			retval.Height = imageSize.Height;

			if (rightSide)
			{
				retval.X = imageSize.Width - retval.X - retval.Width;
			}

			if (distance % 2 == 1)
				retval.X += imageSize.Width;

			return retval;
		}
		private Rectangle GetSidePassageDestRect(int distance, bool rightSide, Rectangle inRect)
		{
			Rectangle retval = new Rectangle();

			for (int i = 0; i < distance; i++)
			{
				retval.X += sideWidth[i];
			}

			retval.Width = sideWidth[distance];// +sideWidth[distance + 1];

			retval.X *= 16;
			retval.Y *= 16;
			retval.Width *= 16;
			retval.Height *= 16;

			retval.Height = imageSize.Height;

			if (rightSide)
			{
				retval.X = imageSize.Width - retval.X - retval.Width;
			}

			retval.X += inRect.X;
			retval.Y += inRect.Y;

			return retval;
		}

		private void DrawExtras(int val, int side, Point loc, int distance, Rectangle mainDestRect)
		{
			ExtraType extraType = GetExtraType(val, side);

			if (extraType == ExtraType.None)
				return;

			Map3DExtraInfo info = XleCore.Data.Map3DExtraInfo[(int)extraType];

			if (info.Images.ContainsKey(distance) == false)
				return;

			Rectangle srcRect = info.Images[distance].SrcRect;
			Rectangle destRect = info.Images[distance].DestRect;

			if (ExtraScale)
			{
				srcRect.X /= 2; srcRect.Y /= 2; srcRect.Width /= 2; srcRect.Height /= 2;
			}

			destRect.X += mainDestRect.X;
			destRect.Y += mainDestRect.Y;

			if (srcRect.Width != 0 && srcRect.Height != 0)
				Surfaces.Extras.Draw(srcRect, destRect);

			AnimateExtra(extraType, loc, distance, destRect.Location);
		}

		private void AnimateExtra(ExtraType extraType, Point loc, int distance, Point extraDestRect)
		{
			Map3DExtraInfo info = XleCore.Data.Map3DExtraInfo[(int)extraType];
			if (info.Images.ContainsKey(distance) == false)
				return;

			var img = info.Images[distance];

			if (img.Animations.Count == 0)
				return;

			Color clr = ExtraColor(loc);
			var anim = img.Animations[img.CurrentAnimation];

			if (anim.FrameTime > 0)
			{
				anim.TimeToNextFrame -= Display.DeltaTime;
				if (anim.TimeToNextFrame < 0)
				{
					anim.CurrentFrame++;
					anim.TimeToNextFrame += anim.FrameTime;

					if (anim.TimeToNextFrame < 0) anim.TimeToNextFrame = 0;

					if (anim.CurrentFrame >= anim.Images.Count)
					{
						if (img.Animations.Count > 1)
						{
							int lastAnim = img.CurrentAnimation;

							img.CurrentAnimation = XleCore.random.Next(img.Animations.Count - 1);
							if (img.CurrentAnimation == lastAnim)
								img.CurrentAnimation++;

							anim = img.Animations[img.CurrentAnimation];
							anim.CurrentFrame = 0;
						}
						else
							anim.CurrentFrame = 0;
					}
				}
			}

			int currentFrame = anim.CurrentFrame;

			Rectangle srcRect = anim.Images[currentFrame].SrcRect;
			Rectangle destRect = anim.Images[currentFrame].DestRect;

			if (ExtraScale)
			{
				srcRect.X /= 2; srcRect.Y /= 2; srcRect.Width /= 2; srcRect.Height /= 2;
				Surfaces.Extras.InterpolationHint = InterpolationMode.Fastest;
			}

			destRect.X += extraDestRect.X;
			destRect.Y += extraDestRect.Y;

			Surfaces.Extras.Color = clr;
			Surfaces.Extras.Draw(srcRect, destRect);
			Surfaces.Extras.Color = XleColor.White;
		}

		protected virtual Color ExtraColor(Point location)
		{
			return Color.White;
		}

		protected abstract ExtraType GetExtraType(int val, int side);

		readonly Size screenSize = new Size(23 * 16, 17 * 16);

		private void DrawWall(int distance, Rectangle main_destRect)
		{
			Surfaces.Walls.Draw(
				new Rectangle(
					screenSize.Width * 2, 
					(distance - 1) * screenSize.Height, 
					screenSize.Width, 
					screenSize.Height),
				main_destRect);

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
				source = Surfaces.Door;
			}
			else if (val >= 0x50 && val <= 0x5f)
			{
				source = Surfaces.MuseumExhibitFrame;

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
