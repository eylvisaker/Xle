namespace ERY.Xle.Services.Implementation.Commands
{
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

        public override void Execute(GameState state)
        {
            MenuItemList theList = new MenuItemList("1", "2", "3", "4", "5");

            TextArea.PrintLine();
            TextArea.PrintLine("** Change gamespeed **", XleColor.Yellow);
            TextArea.PrintLine("    (1 is fastest)", XleColor.Yellow);
            TextArea.PrintLine();

            state.Player.Gamespeed = 1 + QuickMenu.QuickMenu(theList, 2, state.Player.Gamespeed - 1);

            TextArea.Print("Gamespeed is: ", XleColor.Yellow);
            TextArea.PrintLine(state.Player.Gamespeed.ToString(), XleColor.White);

            systemState.Factory.SetGameSpeed(GameState, state.Player.Gamespeed);

            gameControl.Wait(GameState.GameSpeed.AfterSetGamespeedTime);

        }
    }
}
