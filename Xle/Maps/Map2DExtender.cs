using System;

using AgateLib.Geometry;
using AgateLib.InputLib;

namespace ERY.Xle.Maps
{
    public abstract class Map2DExtender : MapExtender
    {
        protected void _Move2D(Direction dir, string textStart, out string command, out Point stepDirection)
        {
            Player.FaceDirection = dir;

            command = textStart + " " + dir.ToString();
            int stepSize = StepSize;

            switch (dir)
            {
                case Direction.West:
                    stepDirection = new Point(-stepSize, 0);
                    break;

                case Direction.North:
                    stepDirection = new Point(0, -stepSize);
                    break;

                case Direction.East:
                    stepDirection = new Point(stepSize, 0);
                    break;

                case Direction.South:
                    stepDirection = new Point(0, stepSize);
                    break;

                default:
                    stepDirection = Point.Empty;
                    break;
            }

        }

        public override bool CanPlayerStepIntoImpl(int xx, int yy)
        {
            int test = 0;

            if (GuardInSpot(xx, yy))
                return false;

            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < 2; i++)
                {
                    test = TheMap[xx + i, yy + j];

                    if (IsTileBlocked(test))
                        return false;
                }
            }

            return true;
        }

        protected virtual bool GuardInSpot(int xx, int yy)
        {
            return false;
        }

        protected virtual bool IsTileBlocked(int tile)
        {
            return TheMap.TileSet[tile] == TileInfo.Blocked;
        }

        public override void PlayerCursorMovement(Direction dir)
        {
            string command;
            Point stepDirection;

            _Move2D(dir, "Move", out command, out stepDirection);

            if (CanPlayerStep(stepDirection))
            {
                TextArea.PrintLine(command);

                MovePlayer(stepDirection);
                SoundMan.PlaySound(LotaSound.WalkTown);

                Player.TimeQuality += 0.03;
            }
            else
            {
                SoundMan.PlaySound(LotaSound.Invalid);

                //Commands.CommandList.UpdateCommand("Move Nowhere");
                TextArea.PrintLine("Move nowhere");
            }
        }

        public override bool PlayerFight()
        {
            TextArea.PrintLine();
            TextArea.PrintLine();

            TextArea.PrintLine("Fight with " + Player.CurrentWeapon.BaseName(Data));
            TextArea.Print("Enter direction: ");

            KeyCode key = Input.WaitForKey(KeyCode.Up, KeyCode.Down, KeyCode.Left, KeyCode.Right);

            Direction fightDir;

            switch (key)
            {
                case KeyCode.Right: fightDir = Direction.East; break;
                case KeyCode.Up: fightDir = Direction.North; break;
                case KeyCode.Left: fightDir = Direction.West; break;
                case KeyCode.Down: fightDir = Direction.South; break;
                default:
                    throw new InvalidOperationException();
            }

            TextArea.PrintLine(fightDir.ToString());

            PlayerFight(fightDir);

            return true;
        }

        protected virtual void PlayerFight(Direction fightDir)
        { }

        public override bool PlayerXamine()
        {
            TextArea.PrintLine();
            TextArea.PrintLine();
            TextArea.PrintLine("You are in " + TheMap.MapName + ".");
            TextArea.PrintLine("Look about to see more.");

            return true;
        }

        public override bool PlayerLeave()
        {
            if (TheMap.HasGuards && TheMap.Guards.IsAngry)
            {
                TextArea.PrintLine("Walk out yourself.");
                GameControl.Wait(200);
            }
            else
            {
                LeaveMap();
            }

            return true;
        }

    }
}
