using AgateLib;
using System.Threading.Tasks;
using Xle.Services.Game;

namespace Xle.Services.Commands.Implementation
{
    public class InventoryState
    {

    }

    [Transient]
    public class Inventory : Command
    {
        private readonly InventoryScreenRenderer renderer;
        private readonly IXleGameControl gameControl;

        public Inventory(InventoryScreenRenderer renderer, IXleGameControl gameControl)
        {
            this.renderer = renderer;
            this.gameControl = gameControl;
        }

        public override async Task Execute()
        {
            await TextArea.PrintLine();

            var player = GameState.Player;

            renderer.InventoryScreen = 0;

            while (renderer.InventoryScreen < 2)
            {
                await gameControl.WaitAsync(150, redraw: renderer);
                await gameControl.WaitForKey(renderer);

                renderer.InventoryScreen++;
            }
        }
    }
}
