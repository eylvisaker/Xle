using AgateLib;
using System.Threading.Tasks;
using Xle.Game;
using Xle.Menus;
using Xle.XleSystem;

namespace Xle.Commands.Implementation
{
    [Transient]
    public class Gamespeed : Command
    {
        private XleSystemState systemState;
        private IXleGameControl gameControl;

        public Gamespeed(XleSystemState systemState, IXleGameControl gameControl)
        {
            this.systemState = systemState;
            this.gameControl = gameControl;
        }

        public IQuickMenu QuickMenu { get; set; }

        public override async Task Execute()
        {
            MenuItemList theList = new MenuItemList("1", "2", "3", "4", "5");

            await TextArea.PrintLine();
            await TextArea.PrintLine("** Change gamespeed **", XleColor.Yellow);
            await TextArea.PrintLine("    (1 is fastest)", XleColor.Yellow);
            await TextArea.PrintLine();

            Player.Gamespeed = 1 + await QuickMenu.QuickMenu(theList, 2, Player.Gamespeed - 1);

            await TextArea.Print("Gamespeed is: ", XleColor.Yellow);
            await TextArea.PrintLine(Player.Gamespeed.ToString(), XleColor.White);

            systemState.Factory.SetGameSpeed(GameState, Player.Gamespeed);

            await gameControl.WaitAsync(GameState.GameSpeed.AfterSetGamespeedTime);

        }
    }
}
