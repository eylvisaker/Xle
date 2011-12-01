using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace ERY.Xle.XleMapTypes
{
	public abstract class Map3D : XleMap
	{
		enum SidePassageType
		{
			Standard,
			Parallel,
			Wall,
		}

		enum ExtraType
		{
			None = -1,
			Chest,
			Box,
			GoUp,
			GoDown,
			Needle,
			Slime,
			TripWire,
		}

		protected abstract Surface Backdrop { get; }
		protected abstract Surface Wall { get; }
		protected abstract Surface SidePassages { get; }
		protected virtual Surface Extras { get { return g.DungeonBlueExtras; } }
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
			if (DrawCloseup)
			{
				DrawCloseupImpl(inRect);
				return;
			}

			Point stepDir = StepDirection(faceDirection);
			Point leftDir = LeftDirection(faceDirection);
			Point rightDir = RightDirection(faceDirection);

			Backdrop.Draw(inRect);

			Point loc = new Point(x, y);

			int maxDistance = 0;

			// draw up to terminal wall
			for (int distance = 0; distance < 6; distance++)
			{
				loc.X = x + distance * stepDir.X;
				loc.Y = y + distance * stepDir.Y;

				int val = this[loc.X, loc.Y];

				DrawSidePassages(loc, stepDir, leftDir, rightDir, distance, inRect);
				maxDistance = distance;

				if (IsPassable(val) == false)
				{
					DrawWall(distance, inRect);
					DrawWallOverlay(distance, inRect, val);

					break;
				}
			}

			for (int distance = 0; distance < maxDistance; distance++)
			{
				loc.X = x + distance * stepDir.X;
				loc.Y = y + distance * stepDir.Y;

				int val = this[loc.X, loc.Y];

				DrawExtras(val, loc, distance, inRect);
			}
		}

		protected bool DrawCloseup { get; set; }
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

		private void DrawSidePassages(Point loc, Point lookDir, Point leftDir, Point rightDir, int distance, Rectangle maindestRect)
		{
			int leftValue = this[loc.X + leftDir.X, loc.Y + leftDir.Y];
			int rightValue = this[loc.X + rightDir.X, loc.Y + rightDir.Y];

			bool drawLeft = IsPassable(leftValue);
			bool drawRight = IsPassable(rightValue);
			SidePassageType type;

			Rectangle srcRect, destRect;

			if (drawLeft)
			{
				type = SidePassageType.Standard;

				if (IsPassable(this[loc.X + lookDir.X, loc.Y + lookDir.Y]) == false)
				{
					if (IsPassable(this[loc.X + lookDir.X + leftDir.X, loc.Y + lookDir.Y + leftDir.Y]))
					{
						type = SidePassageType.Parallel;
					}
					else
						type = SidePassageType.Wall;
				}

				srcRect = GetSidePassageSrcRect(distance, false, type);
				destRect = GetSidePassageDestRect(distance, false);

				destRect.X += maindestRect.X;
				destRect.Y += maindestRect.Y;

				SidePassages.Draw(srcRect, destRect);
			}
			if (drawRight)
			{
				type = SidePassageType.Standard;

				if (IsPassable(this[loc.X + lookDir.X, loc.Y + lookDir.Y]) == false)
				{
					if (IsPassable(this[loc.X + lookDir.X + rightDir.X, loc.Y + lookDir.Y + rightDir.Y]))
					{
						type = SidePassageType.Parallel;
					}
					else
						type = SidePassageType.Wall;
				}

				srcRect = GetSidePassageSrcRect(distance, true, type);
				destRect = GetSidePassageDestRect(distance, true);

				destRect.X += maindestRect.X;
				destRect.Y += maindestRect.Y;

				SidePassages.Draw(srcRect, destRect);
			}

		}
		private Rectangle GetSidePassageSrcRect(int distance, bool rightSide, SidePassageType type)
		{
			Rectangle retval = new Rectangle();

			switch (distance)
			{
				case 0: retval = new Rectangle(0, 0, 24, 112); break;
				case 1: retval = new Rectangle(24, 0, 24, 80); break;
				case 2: retval = new Rectangle(48, 0, 16, 64); break;
				case 3: retval = new Rectangle(64, 0, 8, 48); break;
				case 4: retval = new Rectangle(72, 0, 8, 40); break;
			}

			switch (type)
			{
				case SidePassageType.Parallel: retval.Y += 112; break;
				case SidePassageType.Wall: retval.Y += 224; break;
			}

			if (rightSide)
				retval.X += 80;

			retval.X *= 2;
			retval.Y *= 2;
			retval.Width *= 2;
			retval.Height *= 2;
			
			return retval;
		}
		private Rectangle GetSidePassageDestRect(int distance, bool rightSide)
		{
			Rectangle retval = new Rectangle();

			switch (distance)
			{
				case 0: retval = new Rectangle(0, 24, 24, 112); break;
				case 1: retval = new Rectangle(24, 40, 24, 80); break;
				case 2: retval = new Rectangle(48, 48, 16, 64); break;
				case 3: retval = new Rectangle(64, 56, 8, 48); break;
				case 4: retval = new Rectangle(72, 56, 8, 40); break;
			}

			if (rightSide)
			{
				retval.X = 184 - retval.X - retval.Width;
			}

			retval.X *= 2;
			retval.Y *= 2;
			retval.Width *= 2;
			retval.Height *= 2;

			return retval;
		}

		private void DrawExtras(int val, Point loc, int distance, Rectangle mainDestRect)
		{
			ExtraType extraType = GetExtraType(val);

			if (extraType == ExtraType.None)
				return;

			DungeonExtraInfo info = XleCore.DungeonExtraInfo[(int)extraType];

			if (info.Images.ContainsKey(distance) == false)
				return;

			Rectangle srcRect = info.Images[distance].SrcRect;
			Rectangle destRect = info.Images[distance].DestRect;

			//srcRect.X /= 2; srcRect.Y /= 2; srcRect.Width /= 2; srcRect.Height /= 2;

			destRect.X += mainDestRect.X;
			destRect.Y += mainDestRect.Y;

			Extras.Draw(srcRect, destRect);
		}

		private static ExtraType GetExtraType(int val)
		{
			ExtraType extraType = ExtraType.None;

			switch (val)
			{
				case 0x11:
					extraType = ExtraType.GoUp;
					break;
				case 0x12:
					extraType = ExtraType.GoDown;
					break;
				case 0x13:
					extraType = ExtraType.Needle;
					break;
				case 0x14:
					extraType = ExtraType.Slime;
					break;
				case 0x15:
					extraType = ExtraType.TripWire;
					break;
				case 0x1e:
					extraType = ExtraType.Box;
					break;
				case 0x30:
				case 0x31:
				case 0x32:
				case 0x33:
				case 0x34:
				case 0x35:
				case 0x36:
				case 0x37:
				case 0x38:
				case 0x39:
				case 0x3a:
				case 0x3b:
				case 0x3c:
				case 0x3d:
				case 0x3e:
				case 0x3f:
					extraType = ExtraType.Chest;
					break;

			}
			return extraType;
		}

		private void DrawWall(int distance, Rectangle main_destRect)
		{
			Rectangle srcRect = GetWallSrcRect(distance);
			Rectangle destRect = GetWallDestRect(distance);

			destRect.X += main_destRect.X;
			destRect.Y += main_destRect.Y;

			Wall.Draw(srcRect, destRect);
		}

		private Rectangle GetWallSrcRect(int distance)
		{
			Rectangle retval = new Rectangle();

			switch (distance)
			{
				case 1: retval = new Rectangle(0, 0, 136, 112); break;
				case 2: retval = new Rectangle(136, 0, 88, 80); break;
				case 3: retval = new Rectangle(224, 0, 56, 64); break;
				case 4: retval = new Rectangle(280, 0, 40, 48); break;
				case 5: retval = new Rectangle(320, 0, 24, 40); break;
			}

			retval.X *= 2;
			retval.Y *= 2;
			retval.Width *= 2;
			retval.Height *= 2;

			return retval;
		}
		private Rectangle GetWallDestRect(int distance)
		{
			Rectangle retval = new Rectangle();

			switch (distance)
			{
				case 1: retval = new Rectangle(24, 8, 136, 112); break;
				case 2: retval = new Rectangle(48, 32, 88, 80); break;
				case 3: retval = new Rectangle(64, 40, 56, 64); break;
				case 4: retval = new Rectangle(72, 48, 40, 48); break;
				case 5: retval = new Rectangle(80, 56, 24, 40); break;
			}

			retval.X *= 2;
			retval.Y *= 2;
			retval.Width *= 2;
			retval.Height *= 2;

			return retval;
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

		public override bool CanPlayerStepInto(Player player, int xx, int yy)
		{
			if (this[xx, yy] >= 0x40)
				return false;
			else if ((this[xx, yy] & 0xf0) == 0x00)
				return false;
			else
				return true;
		}
		protected override bool CheckMovementImpl(Player player, int dx, int dy)
		{
			return CanPlayerStepInto(player, player.X + dx, player.Y + dy);
		}

		public override void PlayerCursorMovement(Player player, Direction dir)
		{
			string command;
			Point stepDirection;

			DrawCloseup = false;

			_MoveDungeon(player, dir, ShowDirections(player), out command, out stepDirection);

			if (stepDirection.IsEmpty == false)
			{
				if (CanPlayerStepInto(player, player.X + stepDirection.X, player.Y + stepDirection.Y) == false)
				{
					command = "Bump into wall";
					SoundMan.PlaySound(LotaSound.Bump);
				}
				//else
				//    SoundMan.PlaySound(LotaSound.MuseumWalk);
			}
			Commands.UpdateCommand(command);

			if (stepDirection.IsEmpty == false)
			{
				player.Move(stepDirection.X, stepDirection.Y);
			}

			Commands.UpdateCommand(command);

			OnPlayerEnterPosition(player, player.X, player.Y);
		}

		protected virtual bool ShowDirections(Player player)
		{
			return true;
		}
		protected virtual void OnPlayerEnterPosition(Player player, int x, int y)
		{
		}

		public override bool AutoDrawPlayer
		{
			get { return false; }
		}
	}

}
