using AgateLib.InputLib;
using AgateLib.InputLib.Legacy;

using ERY.Xle.Maps.Outdoors;
using ERY.Xle.Services.ScreenModel;
using ERY.Xle.Services.XleSystem;

namespace ERY.Xle.Services.Commands.Implementation
{
    public class Disembark : Command
    {
        public IXleScreen Screen { get; set; }
        public ISoundMan SoundMan { get; set; }

        OutsideExtender Map
        {
            get { return (OutsideExtender)GameState.MapExtender; } 
        }

        public override void Execute()
        {
            TextArea.PrintLine(" raft");

            if (Player.IsOnRaft == false)
            {
                TextArea.PrintLine("\nNothing to disembark.", XleColor.Yellow);
                return;
            }

            TextArea.PrintLine();
            TextArea.PrintLine("Disembark in which direction?");

            do
            {
                Screen.OnDraw();

            } while (!(
                Keyboard.Keys[KeyCode.Left] || Keyboard.Keys[KeyCode.Right] ||
                Keyboard.Keys[KeyCode.Up] || Keyboard.Keys[KeyCode.Down]));

            int newx = Player.X;
            int newy = Player.Y;

            Direction dir = Direction.East;

            if (Keyboard.Keys[KeyCode.Left])
                dir = Direction.West;
            else if (Keyboard.Keys[KeyCode.Up])
                dir = Direction.North;
            else if (Keyboard.Keys[KeyCode.Down])
                dir = Direction.South;
            else if (Keyboard.Keys[KeyCode.Right])
                dir = Direction.East;

            PlayerDisembark(dir);
        }

        private void PlayerDisembark(Direction dir)
        {
            Player.BoardedRaft = null;
            Map.PlayerCursorMovement(dir);

            SoundMan.StopSound(LotaSound.Raft1);
        }
    }
}
