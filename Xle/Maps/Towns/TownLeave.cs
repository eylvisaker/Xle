using AgateLib;
using System.Threading.Tasks;
using Xle.Services.Commands.Implementation;
using Xle.Services.Game;

namespace Xle.Maps.Towns
{
    [Transient("TownLeave")]
    public class TownLeave : Leave
    {
        public IXleGameControl GameControl { get; set; }

        public override async Task Execute()
        {
            if (!await ConfirmLeave())
                return;

            if (GameState.Map.Guards.IsAngry)
            {
                await TextArea.PrintLine("Walk out yourself.");
                await GameControl.WaitAsync(200);
            }
            else
            {
                await GameState.MapExtender.LeaveMap();
            }
        }
    }
}
