using ERY.Xle.Services.Game;
using ERY.Xle.Services.Menus;
using ERY.Xle.Services.XleSystem;
using Xle.Ancients;

namespace ERY.Xle.Services.Commands.Implementation
{
    public class End : Command
    {
        private IQuickMenu menu;
        private IXleGameControl gameControl;
        private XleSystemState systemState;
        private readonly IGamePersistance gamePersistance;

        public End(IQuickMenu menu, IXleGameControl gameControl, XleSystemState systemState, IGamePersistance gamePersistance)
        {
            this.menu = menu;
            this.gameControl = gameControl;
            this.systemState = systemState;
            this.gamePersistance = gamePersistance;
        }

        public override void Execute()
        {
            MenuItemList menuItems = new MenuItemList("Yes", "No");
            int choice;
            bool saved = false;

            TextArea.PrintLine("\nWould you like to save");
            TextArea.PrintLine("the game in progress?");
            TextArea.PrintLine();

            choice = menu.QuickMenu(menuItems, 2);

            if (choice == 0)
            {
                gamePersistance.Save(Player);

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
