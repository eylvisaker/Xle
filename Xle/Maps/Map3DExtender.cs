using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;

namespace ERY.Xle.Maps
{
    public abstract class Map3DExtender : MapExtender
    {
        protected virtual bool ShowDirections()
        {
            return true;
        }

        protected abstract void PlayPlayerMoveSound();



        public abstract Map3DSurfaces Surfaces();

        protected virtual void CommandTextForInvalidMovement(ref string command)
        {
        }

        public override void PlayerCursorMovement(Direction dir)
        {
            Point stepDirection;
            string command;

            OnBeforePlayerMove(dir);

            _MoveDungeon(dir, ShowDirections(), out command, out stepDirection);

            if (stepDirection != Point.Zero)
            {
                if (CanPlayerStepIntoImpl(Player.X + stepDirection.X, Player.Y + stepDirection.Y) == false)
                {
                    CommandTextForInvalidMovement(ref command);
                    TextArea.PrintLine(command);
                    SoundMan.PlaySound(LotaSound.Bump);
                }
                else
                {
                    TextArea.PrintLine(command);

                    PlayPlayerMoveSound();
                    MovePlayer(stepDirection);
                }
            }
            else
            {
                // Turning in place
                TextArea.PrintLine(command);

                PlayPlayerMoveSound();
            }
        }

        protected virtual void OnBeforePlayerMove(Direction dir)
        {
        }

        protected void _MoveDungeon(Direction dir, bool haveCompass, out string command, out Point stepDirection)
        {
            Direction newDirection;
            command = "";
            stepDirection = Point.Zero;

            switch (dir)
            {
                case Direction.East:
                    command = "Turn Right";

                    newDirection = Player.FaceDirection - 1;

                    if (newDirection < Direction.East)
                        newDirection = Direction.South;


                    Player.FaceDirection = (Direction)newDirection;

                    if (haveCompass)
                        command = "Turn " + Player.FaceDirection.ToString();

                    break;

                case Direction.North:
                    command = "Move Forward";

                    stepDirection = Player.FaceDirection.StepDirection();

                    if (haveCompass)
                        command = "Walk " + Player.FaceDirection.ToString();

                    Player.TimeQuality += TheMap.StepQuality;

                    break;

                case Direction.West:
                    command = "Turn Left";

                    newDirection = Player.FaceDirection + 1;


                    if (newDirection > Direction.South)
                        newDirection = Direction.East;

                    Player.FaceDirection = (Direction)newDirection;

                    if (haveCompass)
                        command = "Turn " + Player.FaceDirection.ToString();

                    break;

                case Direction.South:
                    command = "Move Backward";

                    if (Player.FaceDirection == Direction.East)
                        stepDirection = new Point(-1, 0);
                    if (Player.FaceDirection == Direction.North)
                        stepDirection = new Point(0, 1);
                    if (Player.FaceDirection == Direction.West)
                        stepDirection = new Point(1, 0);
                    if (Player.FaceDirection == Direction.South)
                        stepDirection = new Point(0, -1);

                    if (haveCompass)
                    {
                        // we're walking backwards here, so make the text work right!
                        command = "Walk ";
                        switch (Player.FaceDirection)
                        {
                            case Direction.East: command += "West"; break;
                            case Direction.West: command += "East"; break;
                            case Direction.North: command += "South"; break;
                            case Direction.South: command += "North"; break;
                        }
                    }

                    Player.TimeQuality += TheMap.StepQuality;


                    break;
            }
        }

        public override bool CanPlayerStepIntoImpl(int xx, int yy)
        {
            if (IsMapSpaceBlocked(xx, yy))
                return false;

            return true;
        }

        protected bool IsMapSpaceBlocked(int xx, int yy)
        {
            if (TheMap[xx, yy] >= 0x40)
                return true;
            else if ((TheMap[xx, yy] & 0xf0) == 0x00)
                return true;

            return false;
        }

        public override int WaitTimeAfterStep
        {
            get
            {
                return GameState.GameSpeed.DungeonStepTime;
            }
        }

    }
}
