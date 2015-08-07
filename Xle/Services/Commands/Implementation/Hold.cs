using ERY.Xle.Data;
using ERY.Xle.Services.Menus;
using ERY.Xle.Services.ScreenModel;

namespace ERY.Xle.Services.Commands.Implementation
{
    public class Hold : Command
    {
        public XleData Data { get; set; }
        public IXleSubMenu SubMenu { get; set; }

        public override void Execute()
        {
            Use.ChooseHeldItem(
                TextArea, 
                Data, 
                GameState.Player, 
                SubMenu);
        }
    }
}
