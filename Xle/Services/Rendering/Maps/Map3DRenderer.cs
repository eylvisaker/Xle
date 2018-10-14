using AgateLib.Mathematics.Geometry;
using ERY.Xle.Maps;
using ERY.Xle.Maps.XleMapTypes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ERY.Xle.Services.Rendering.Maps
{
    public abstract class Map3DRenderer : XleMapRenderer
    {
        private readonly Dictionary<int, TorchAnim> torchAnims = new Dictionary<int, TorchAnim>();

        protected int exhibitFrame { get; private set; }

        private double exhibitAnimTime;
        private const double exhibitFrameTime = 50;

        public new Map3D TheMap { get { return (Map3D)base.TheMap; } }
        public new Map3DExtender Extender { get { return (Map3DExtender)base.Extender; } }

        private enum SideWallType
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
            Urn,
        }

        private readonly Size imageSize = new Size(368, 272);

        protected override void OnExtenderSet()
        {
        }

        public Map3DSurfaces Surfaces { get; set; }
        protected virtual bool ExtraScale { get { return false; } }

        public override void Draw(Point playerPos, Direction faceDirection, Rectangle inRect)
        {
            Surfaces = Extender.Surfaces();

            DrawImpl(playerPos.X, playerPos.Y, faceDirection, inRect);
        }

        private void AdvanceAnimation(GameTime time)
        {
            exhibitAnimTime += time.ElapsedGameTime.TotalMilliseconds;

            if (exhibitAnimTime > exhibitFrameTime)
            {
                exhibitAnimTime %= exhibitFrameTime;
                exhibitFrame++;
                exhibitFrame %= 3;
            }

            AnimateTorches();
        }

        private void AnimateTorches(GameTime time)
        {
            foreach (var ta in torchAnims.Values)
            {
                ta.NextAnimTime -= time.ElapsedGameTime.TotalMilliseconds;

                if (ta.NextAnimTime < 0)
                {
                    ta.SetNextAnimTime();
                    ta.AdvanceFrame();
                }
            }
        }

        private void DrawImpl(int x, int y, Direction faceDirection, Rectangle inRect)
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
            Surfaces.Walls.Draw(new Rectangle(Point.Zero, screenSize), inRect);

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

            int terminalVal = -1;

            for (int distance = maxDistance; distance >= 0; distance--)
            {
                loc.X = x + distance * stepDir.X;
                loc.Y = y + distance * stepDir.Y;

                int val = TheMap[loc.X, loc.Y];

                DrawSidePassages(loc, stepDir, leftDir, rightDir, distance, inRect);

                if (distance == maxDistance && IsPassable(val) == false)
                {
                    DrawTerminalWall(val, maxDistance, inRect);
                    terminalVal = val;
                }
            }

            if (terminalVal >= 0)
            {
                DrawWallOverlay(maxDistance, inRect, terminalVal);
            }
            for (int distance = 0; distance < maxDistance; distance++)
            {
                for (int dir = -1; dir <= 1; dir++)
                {
                    loc.X = x + distance * stepDir.X + dir * rightDir.X;
                    loc.Y = y + distance * stepDir.Y + dir * rightDir.Y;

                    int val = TheMap[loc.X, loc.Y];

                    DrawTraps(val, dir, loc, distance, inRect);

                    if (dir == 0)
                    {
                        loc.X += stepDir.X;
                        loc.Y += stepDir.Y;
                        val = TheMap[loc.X, loc.Y];
                    }

                    if (val == 0) continue;
                    if (val == 0x10) continue;

                    if (val == 0x01)
                    {
                        DrawTorch(dir, distance, inRect);

                        continue;
                    }
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
                torchAnims[taHash] = new TorchAnim(Random);

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

        private readonly int[] sideWidth = new int[] { 3, 3, 2, 1, 1, 1, 1 };

        private Rectangle GetSidePassageSrcRect(int distance, bool rightSide, SideWallType type)
        {
            Rectangle result = new Rectangle();

            result.Width = sideWidth[distance];

            switch (type)
            {
                case SideWallType.Corner:
                case SideWallType.Corridor:
                case SideWallType.Parallel:
                    result.Width += sideWidth[distance + 1];
                    break;
            }

            for (int i = 0; i < distance; i++)
            {
                result.X += sideWidth[i];
            }

            result.X *= 16;
            result.Y *= 16;
            result.Width *= 16;
            result.Height *= 16;

            result.Y += imageSize.Height * (int)type;
            result.Height = imageSize.Height;

            if (rightSide)
            {
                result.X = imageSize.Width - result.X - result.Width;
            }

            if (distance % 2 == 1)
                result.X += imageSize.Width;

            return result;
        }
        private Rectangle GetSidePassageDestRect(int distance, bool rightSide, SideWallType type, Rectangle inRect)
        {
            Rectangle result = new Rectangle();

            for (int i = 0; i < distance; i++)
            {
                result.X += sideWidth[i];
            }

            result.Width = sideWidth[distance];

            switch (type)
            {
                case SideWallType.Corner:
                case SideWallType.Corridor:
                case SideWallType.Parallel:
                    result.Width += sideWidth[distance + 1];
                    break;
            }


            result.X *= 16;
            result.Y *= 16;
            result.Width *= 16;
            result.Height *= 16;

            result.Height = imageSize.Height;

            if (rightSide)
            {
                result.X = imageSize.Width - result.X - result.Width;
            }

            result.X += inRect.X;
            result.Y += inRect.Y;

            return result;
        }

        private void DrawTraps(int val, int side, Point loc, int distance, Rectangle mainDestRect)
        {
            ExtraType extraType = GetExtraType(val, side);

            if (extraType == ExtraType.None)
                return;

            if (extraType == ExtraType.TorchLeft || extraType == ExtraType.TorchRight)
            {
                return;
            }

            if (IsTrap(extraType) == false)
                return;

            Size boxSize = new Size(192, 240);
            Rectangle srcRect = new Rectangle(0, 192, 192, 48);
            int index = -1;
            bool above = false;

            switch (extraType)
            {
                case ExtraType.Chest: index = 0; break;
                case ExtraType.Box: index = 1; break;
                case ExtraType.Urn: index = 2; break;
                case ExtraType.GoUp: index = 4; above = true; break;
                case ExtraType.GoDown: index = 5; break;
                case ExtraType.Needle: index = 6; break;
                case ExtraType.Slime: index = 7; break;
                case ExtraType.TripWire: index = 8; break;
                case ExtraType.GasVent: index = 9; above = true; break;
            }

            srcRect.X += (index % 4) * boxSize.Width;
            srcRect.Y += (index / 4) * boxSize.Height;
            srcRect.Y -= distance * 48;

            Rectangle destRect = srcRect;

            destRect.X = 96;
            if (above)
            {
                destRect.Y = -16;

                destRect.Y += 16 * distance;
                destRect.Y += (distance >= 1) ? 16 : 0;
                destRect.Y += (distance >= 2) ? 16 : 0;
            }
            else
            {
                destRect.Y = 224;

                destRect.Y -= 16 * distance;
                destRect.Y -= (distance >= 1) ? 16 : 0;
            }

            destRect.X += mainDestRect.X;
            destRect.Y += mainDestRect.Y;

            if (srcRect.Width != 0 && srcRect.Height != 0)
                Surfaces.Traps.Draw(srcRect, destRect);

        }


        private bool IsTrap(ExtraType extraType)
        {
            switch (extraType)
            {
                case ExtraType.TorchLeft:
                case ExtraType.TorchRight:
                case ExtraType.DisplayCaseLeft:
                case ExtraType.DisplayCaseRight:
                case ExtraType.DoorLeft:
                case ExtraType.DoorRight:
                    return false;

                default:
                    return true;
            }
        }

        protected virtual Color ExtraColor(Point location)
        {
            return Color.White;
        }

        protected abstract ExtraType GetExtraType(int val, int side);

        private readonly Size screenSize = new Size(23 * 16, 17 * 16);

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
        }

        protected void DrawExhibitStatic(Rectangle main_destRect, Rectangle staticRect, Color color)
        {
            Rectangle srcRect = new Rectangle(
                imageSize.Width * 4,
                imageSize.Height * (2 + exhibitFrame),
                staticRect.Width, staticRect.Height);

            staticRect.X += main_destRect.X;
            staticRect.Y += main_destRect.Y;

            Surfaces.Walls.Color = color;
            Surfaces.Walls.Draw(srcRect, staticRect);

            Surfaces.Walls.Color = Color.White;
        }

        private void DrawWallOverlay(int distance, Rectangle destRect, int val)
        {
            if (val >= 0x50 && val <= 0x5f && 0 < distance && distance <= 2)
            {
                var srcRect = new Rectangle(
                        screenSize.Width * 2,
                        (distance - 1) * screenSize.Height,
                        screenSize.Width,
                        screenSize.Height);

                srcRect.X += screenSize.Width * 2;

                Surfaces.Walls.Draw(
                    srcRect,
                    destRect);

                Rectangle staticRect = new Rectangle(96, 96, 160, 96);

                if (distance == 2)
                    staticRect = new Rectangle(128, 112, 112, 64);

                if (AnimateExhibits)
                    DrawExhibitStatic(destRect, staticRect, ExhibitColor(val));

                DrawMuseumExhibit(distance, destRect, val);
            }
        }

        protected virtual void DrawMuseumExhibit(int distance, Rectangle destRect, int val)
        {
        }

        protected virtual Color ExhibitColor(int val) { return XleColor.White; }

        public bool AnimateExhibits { get; set; }
    }
}
