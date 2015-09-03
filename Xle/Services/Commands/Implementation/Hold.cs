using ERY.Xle.Data;
using ERY.Xle.Services.Menus;
using ERY.Xle.Services.ScreenModel;

namespace ERY.Xle.Services.Commands.Implementation
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
