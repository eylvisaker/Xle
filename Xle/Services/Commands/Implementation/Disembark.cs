
using ERY.Xle.Maps.Outdoors;
using ERY.Xle.Services.ScreenModel;
using ERY.Xle.Services.XleSystem;
using Microsoft.Xna.Framework.Input;

namespace ERY.Xle.Services.Commands.Implementation
{
    public class Disembark : Command
    {
        public IXleScreen Screen { get; set; }
        public ISoundMan SoundMan { get; set; }
        public IXleInput Input { get; set; }

        private IOutsideExtender Map
        {
            get { return (IOutsideExtender)GameState.MapExtender; }
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

            Input.PromptToContinueOnWait = false;

            var key = Input.WaitForKey(Keys.Left, Keys.Up, Keys.Right, Keys.Down);

            Direction dir = key.ToDirection();

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
