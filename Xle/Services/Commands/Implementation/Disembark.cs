
using Xle.Maps.Outdoors;
using Xle.Services.ScreenModel;
using Xle.Services.XleSystem;
using Microsoft.Xna.Framework.Input;
using System.Threading.Tasks;

namespace Xle.Services.Commands.Implementation
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

        public override async Task Execute()
        {
            await TextArea.PrintLine(" raft");

            if (Player.IsOnRaft == false)
            {
                await TextArea.PrintLine("\nNothing to disembark.", XleColor.Yellow);
                return;
            }

            await TextArea.PrintLine();
            await TextArea.PrintLine("Disembark in which direction?");

            Input.PromptToContinueOnWait = false;

            var key = await Input.WaitForKey(Keys.Left, Keys.Up, Keys.Right, Keys.Down);

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
