using AgateLib.Geometry;

namespace ERY.Xle.Maps
{
    public abstract class Map3DExtender : MapExtender
    {
        protected virtual bool ShowDirections(Player player)
        {
            return true;
        }

        protected abstract void PlayPlayerMoveSound();



        public abstract Map3DSurfaces Surfaces(GameState state);

        protected virtual void CommandTextForInvalidMovement(ref string command)
        {
        }

        public override void PlayerCursorMovement(GameState state, Direction dir)
        {
            var player = state.Player;
            Point stepDirection;
            string command;

            OnBeforePlayerMove(state, dir);

            _MoveDungeon(player, dir, ShowDirections(player), out command, out stepDirection);

            if (stepDirection.IsEmpty == false)
            {
                if (CanPlayerStepIntoImpl(player, player.X + stepDirection.X, player.Y + stepDirection.Y) == false)
                {
                    CommandTextForInvalidMovement(ref command);
                    TextArea.PrintLine(command);
                    SoundMan.PlaySound(LotaSound.Bump);
                }
                else
                {
                    TextArea.PrintLine(command);

                    PlayPlayerMoveSound();
                    MovePlayer(GameState, stepDirection);
                }
            }
            else
            {
                // Turning in place
                TextArea.PrintLine(command);

                PlayPlayerMoveSound();
            }
        }

        protected virtual void OnBeforePlayerMove(GameState state, Direction dir)
        {
        }

        protected void _MoveDungeon(Player player, Direction dir, bool haveCompass, out string command, out Point stepDirection)
        {
            Direction newDirection;
            command = "";
            stepDirection = Point.Empty;

            switch (dir)
            {
                case Direction.East:
                    command = "Turn Right";

                    newDirection = player.FaceDirection - 1;

                    if (newDirection < Direction.East)
                        newDirection = Direction.South;


                    player.FaceDirection = (Direction)newDirection;

                    if (haveCompass)
                        command = "Turn " + player.FaceDirection.ToString();

                    break;

                case Direction.North:
                    command = "Move Forward";

                    stepDirection = player.FaceDirection.StepDirection();

                    if (haveCompass)
                        command = "Walk " + player.FaceDirection.ToString();

                    player.TimeQuality += TheMap.StepQuality;

                    break;

                case Direction.West:
                    command = "Turn Left";

                    newDirection = player.FaceDirection + 1;


                    if (newDirection > Direction.South)
                        newDirection = Direction.East;

                    player.FaceDirection = (Direction)newDirection;

                    if (haveCompass)
                        command = "Turn " + player.FaceDirection.ToString();

                    break;

                case Direction.South:
                    command = "Move Backward";

                    if (player.FaceDirection == Direction.East)
                        stepDirection = new Point(-1, 0);
                    if (player.FaceDirection == Direction.North)
                        stepDirection = new Point(0, 1);
                    if (player.FaceDirection == Direction.West)
                        stepDirection = new Point(1, 0);
                    if (player.FaceDirection == Direction.South)
                        stepDirection = new Point(0, -1);

                    if (haveCompass)
                    {
                        // we're walking backwards here, so make the text work right!
                        command = "Walk ";
                        switch (player.FaceDirection)
                        {
                            case Direction.East: command += "West"; break;
                            case Direction.West: command += "East"; break;
                            case Direction.North: command += "South"; break;
                            case Direction.South: command += "North"; break;
                        }
                    }

                    player.TimeQuality += TheMap.StepQuality;


                    break;
            }
        }

        public override bool CanPlayerStepIntoImpl(Player player, int xx, int yy)
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
