
using AgateLib;
using Microsoft.Xna.Framework.Input;
using System.Threading.Tasks;
using Xle.Maps.Outdoors;
using Xle.Game;
using Xle.ScreenModel;
using Xle.XleSystem;

namespace Xle.Commands.Implementation
{
    [Transient]
    public class Disembark : Command
    {
        public IXleScreen Screen { get; set; }
        public ISoundMan SoundMan { get; set; }
        public IXleGameControl GameControl { get; set; }

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

            var key = await GameControl.WaitForKey(false, Keys.Left, Keys.Up, Keys.Right, Keys.Down);

            Direction dir = key.ToDirection();

            await PlayerDisembark(dir);
        }

        private async Task PlayerDisembark(Direction dir)
        {
            Player.BoardedRaft = null;
            await Map.PlayerCursorMovement(dir);

            SoundMan.StopSound(LotaSound.Raft1);
        }
    }
}
