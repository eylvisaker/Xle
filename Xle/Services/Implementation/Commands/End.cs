namespace ERY.Xle.Services.Implementation.Commands
{
    public class End : Command
    {
        private IQuickMenu menu;
        private IXleGameControl gameControl;
        private XleSystemState systemState;

        public End(IQuickMenu menu, IXleGameControl gameControl, XleSystemState systemState)
        {
            this.menu = menu;
            this.gameControl = gameControl;
            this.systemState = systemState;
        }

        public override void Execute(GameState state)
        {
            var player = GameState.Player;

            MenuItemList menuItems = new MenuItemList("Yes", "No");
            int choice;
            bool saved = false;

            TextArea.PrintLine("\nWould you like to save");
            TextArea.PrintLine("the game in progress?");
            TextArea.PrintLine();

            choice = menu.QuickMenu(menuItems, 2);

            if (choice == 0)
            {
                player.SavePlayer();

                saved = true;

                TextArea.PrintLine();
                TextArea.PrintLine("Game Saved.");
                TextArea.PrintLine();
            }
            else
            {
                ColorStringBuilder builder = new ColorStringBuilder();

                TextArea.PrintLine();
                TextArea.Print("Game ", XleColor.White);
                TextArea.Print("not", XleColor.Yellow);
                TextArea.Print(" saved.", XleColor.White);

                TextArea.PrintLine();
                TextArea.PrintLine();
            }

            gameControl.Wait(1500);

            TextArea.PrintLine("Quit and return to title screen?");

            if (saved == false)
                TextArea.PrintLine("Unsaved progress will be lost.", XleColor.Yellow);
            else
                TextArea.PrintLine();

            TextArea.PrintLine();

            choice = menu.QuickMenu(menuItems, 2, 1);

            if (choice == 0)
            {
                systemState.ReturnToTitle = true;
            }
        }
    }
}
