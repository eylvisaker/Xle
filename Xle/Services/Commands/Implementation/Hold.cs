using Xle.Data;
using Xle.Services.Menus;
using Xle.Services.ScreenModel;

namespace Xle.Services.Commands.Implementation
{
    public class Hold : Command
    {
        public IItemChooser ItemChooser { get; set; }

        public override void Execute()
        {
            Player.Hold = ItemChooser.ChooseItem();
        }
    }
}
