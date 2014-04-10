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
		class TorchAnim
		{
			public TorchAnim()
			{
				SetNextAnimTime();
				AdvanceFrame();
			}

			public void AdvanceFrame()
			{
				if (CurrentFrame == 3 || CurrentFrame == 4)
				{
					CurrentFrame++;
				}
				else
				{
					int newFrame = CurrentFrame;

					while (newFrame == CurrentFrame)
						newFrame = XleCore.random.Next(77) / 25;

					CurrentFrame = newFrame;
				}
			}
			public void SetNextAnimTime()
			{
				NextAnimTime = XleCore.random.Next(100, 125);
			}

			public double NextAnimTime;
			public int CurrentFrame;
		}

		Dictionary<int, TorchAnim> torchAnims = new Dictionary<int, TorchAnim>();
		int exhibitFrame;
		double exhibitAnimTime;
		const double exhibitFrameTime = 60;

		public new Map3D TheMap { get { return (Map3D)base.TheMap; } }
		public new Map3DExtender Extender { get { return (Map3DExtender)base.Extender; } }

		enum SideWallType
		{
			Wall,
			Parallel,
			Corner,
			Corridor,
			Door,
			Exhibit
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
		}

		public Map3DSurfaces Surfaces { get; set; }
		protected virtual bool ExtraScale { get { return false; } }

		public override void Draw(Point playerPos, Direction faceDirection, Rectangle inRect)
		{
			Surfaces = Extender.Surfaces(XleCore.GameState);

			DrawImpl(playerPos.X, playerPos.Y, faceDirection, inRect);
		}

		private void AdvanceAnimation()
		{
			exhibitAnimTime += Display.DeltaTime;

			if (exhibitAnimTime > exhibitFrameTime)
			{
				exhibitAnimTime %= exhibitFrameTime;
				exhibitFrame++;
				exhibitFrame %= 3;
			}

			AnimateTorches();
		}

		void AnimateTorches()
		{
			foreach (var ta in torchAnims.Values)
			{
				ta.NextAnimTime -= Display.DeltaTime;

				if (ta.NextAnimTime < 0)
				{
					ta.SetNextAnimTime();
					ta.AdvanceFrame();
				}
			}
		}

		void DrawImpl(int x, int y, Direction faceDirection, Rectangle inRect)
		{
			AdvanceAnimation();

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
					DrawTerminalWall(val, maxDistance, inRect);
					//DrawWallOverlay(maxDistance, inRect, val);
				}
			}
			for (int distance = 0; distance < maxDistance; distance++)
			{
				for (int dir = -1; dir <= 1; dir++)
				{
					loc.X = x + distance * stepDir.X + dir * rightDir.X;
					loc.Y = y + distance * stepDir.Y + dir * rightDir.Y;

					if (dir == 0)
					{
						loc.X += stepDir.X;
						loc.Y += stepDir.Y;
					}

					int val = TheMap[loc.X, loc.Y];

					if (val == 0) continue;
					if (val == 0x10) continue;
					if (val == 0x01)
					{
						DrawTorch(dir, distance, inRect);

						continue;
					}

					//DrawExtras(val, dir, loc, distance, inRect);
				}
			}

			DrawMonsters(x, y, faceDirection, inRect, maxDistance);
		}

		/// <summary>
		/// Draws a torch
		/// </summary>
		/// <param name="side">-1 for left, 0 for center, 1 for right</param>
		/// <param name="distance"></param>
		/// <param name="mainDestRect"></param>
		private void DrawTorch(int side, int distance, Rectangle mainDestRect)
		{
			int taHash = distance * 3 + side;
			TorchAnim anim;

			if (torchAnims.ContainsKey(taHash) == false)
				torchAnims[taHash] = new TorchAnim();

			anim = torchAnims[taHash];

			Size torchSize = new Size(60, 84);

			Dictionary<int, List<Point>> destPositions = new Dictionary<int, List<Point>>();

			FillTorchDestPositions(destPositions);

			Rectangle srcRect = new Rectangle(
				anim.CurrentFrame * torchSize.Width,
				torchSize.Height * (side + 1 + 3 * distance),
				torchSize.Width,
				torchSize.Height);

			Point pos = destPositions[side][distance];
			pos.X += mainDestRect.X;
			pos.Y += mainDestRect.Y;

			Surfaces.Torches.Draw(srcRect, pos);
		}

		private void FillTorchDestPositions(Dictionary<int, List<Point>> destPositions)
		{
			destPositions[-1] = new List<Point>();
			destPositions[0] = new List<Point>();
			destPositions[1] = new List<Point>();

			int x = -1;
			destPositions[x].Add(new Point(2 * -3, 2 * 30));
			destPositions[x].Add(new Point(2 * 29, 2 * 36));
			destPositions[x].Add(new Point(2 * 50, 2 * 41));
			destPositions[x].Add(new Point(2 * 63, 2 * 44));
			destPositions[x].Add(new Point(2 * 69, 2 * 48));

			x = 1;
			destPositions[x].Add(new Point(2 * 157, 2 * 30));
			destPositions[x].Add(new Point(2 * 127, 2 * 36));
			destPositions[x].Add(new Point(2 * 109, 2 * 41));
			destPositions[x].Add(new Point(2 * 97, 2 * 44));
			destPositions[x].Add(new Point(2 * 89, 2 * 47));

			x = 0;
			destPositions[x].Add(new Point(2 * 79, 2 * 31));
			destPositions[x].Add(new Point(2 * 79, 2 * 37));
			destPositions[x].Add(new Point(2 * 79, 2 * 41));
			destPositions[x].Add(new Point(2 * 79, 2 * 44));
			destPositions[x].Add(new Point(2 * 79, 2 * 48));
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

		private SideWallType GetSideWallType(int distance, int sideVal, int sideForwardVal, int forwardVal)
		{
			SideWallType sideType;

			if (sideVal == 2)
				return SideWallType.Door;
			if (sideVal >= 0x50 && sideVal <= 0x5f)
				return SideWallType.Exhibit;

			if (IsPassable(sideVal) == false)
			{
				return SideWallType.Wall;
			}
			else if (IsPassable(forwardVal))
				return SideWallType.Corridor;
			else if (IsPassable(sideForwardVal))
				return SideWallType.Parallel;
			else
				return SideWallType.Corner;
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

			SideWallType leftType = GetSideWallType(distance, leftValue, leftForwardValue, forwardValue);
			SideWallType rightType = GetSideWallType(distance, rightValue, rightForwardValue, forwardValue);

			Rectangle srcRect, destRect;

			srcRect = GetSidePassageSrcRect(distance, false, leftType);
			destRect = GetSidePassageDestRect(distance, false, leftType, maindestRect);
			Surfaces.Walls.Draw(srcRect, destRect);

			if (leftType == SideWallType.Exhibit && AnimateExhibits)
			{
				if (distance % 2 == 0)
					srcRect.X += imageSize.Width;

				if (distance <= 2)
					Surfaces.Walls.Color = ExhibitColor(leftValue);

				srcRect.X += imageSize.Width * (1 + exhibitFrame);
				Surfaces.Walls.Draw(srcRect, destRect);

				Surfaces.Walls.Color = Color.White;
			}

			srcRect = GetSidePassageSrcRect(distance, true, rightType);
			destRect = GetSidePassageDestRect(distance, true, rightType, maindestRect);
			Surfaces.Walls.Draw(srcRect, destRect);

			if (rightType == SideWallType.Exhibit && AnimateExhibits)
			{
				if (distance % 2 == 0)
					srcRect.X += imageSize.Width;

				if (distance <= 2)
					Surfaces.Walls.Color = ExhibitColor(rightValue);

				srcRect.X += imageSize.Width * (1 + exhibitFrame);
				Surfaces.Walls.Draw(srcRect, destRect);

				Surfaces.Walls.Color = Color.White;
			}
		}

		private int MapValueAt(Point leftPt)
		{
			return TheMap[leftPt.X, leftPt.Y];
		}

		readonly int[] sideWidth = new int[] { 3, 3, 2, 1, 1, 1, 1 };

		private Rectangle GetSidePassageSrcRect(int distance, bool rightSide, SideWallType type)
		{
			Rectangle retval = new Rectangle();

			retval.Width = sideWidth[distance];// +sideWidth[distance + 1];

			//switch (type)
			//{
			//	case SideWallType.Corner:
			//	case SideWallType.Corridor:
			//	case SideWallType.Parallel:
			//		retval.Width += sideWidth[distance + 1];
			//		break;
			//}

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
		private Rectangle GetSidePassageDestRect(int distance, bool rightSide, SideWallType type, Rectangle inRect)
		{
			Rectangle retval = new Rectangle();

			for (int i = 0; i < distance; i++)
			{
				retval.X += sideWidth[i];
			}

			retval.Width = sideWidth[distance];// +sideWidth[distance + 1];

			//switch (type)
			//{
			//	case SideWallType.Corner:
			//	case SideWallType.Corridor:
			//	case SideWallType.Parallel:
			//		retval.Width += sideWidth[distance + 1];
			//		break;
			//}


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

			if (extraType == ExtraType.TorchLeft || extraType == ExtraType.TorchRight)
			{
				return;
			}

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

		private void DrawTerminalWall(int val, int distance, Rectangle main_destRect)
		{
			var srcRect = new Rectangle(
					screenSize.Width * 2,
					(distance - 1) * screenSize.Height,
					screenSize.Width,
					screenSize.Height);

			if (val == 2)
				srcRect.X += screenSize.Width;

			Surfaces.Walls.Draw(
				srcRect,
				main_destRect);

			if (val >= 0x50 && val <= 0x5f)
			{
				srcRect.X += screenSize.Width * 2;

				Surfaces.Walls.Draw(
					srcRect,
					main_destRect);
			}

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

		protected virtual Color ExhibitColor(int val) { return XleColor.White; }

		public bool AnimateExhibits { get; set; }
	}
}
