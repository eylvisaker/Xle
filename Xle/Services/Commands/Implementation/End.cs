using AgateLib;
using System.Threading.Tasks;
using Xle.Ancients;
using Xle.Serialization;
using Xle.Services.Game;
using Xle.Services.Menus;
using Xle.Services.XleSystem;

namespace Xle.Services.Commands.Implementation
{
    [Transient]
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

        public override async Task Execute()
        {
            MenuItemList menuItems = new MenuItemList("Yes", "No");
            int choice;
            bool saved = false;

            await TextArea.PrintLine("\nWould you like to save");
            await TextArea.PrintLine("the game in progress?");
            await TextArea.PrintLine();

            choice = await menu.QuickMenu(menuItems, 2);

            if (choice == 0)
            {
                gamePersistance.Save(Player);

                saved = true;

                await TextArea.PrintLine();
                await TextArea.PrintLine("Game Saved.");
                await TextArea.PrintLine();
            }
            else
            {
                ColorStringBuilder builder = new ColorStringBuilder();

                await TextArea.PrintLine();
                await TextArea.Print("Game ", XleColor.White);
                await TextArea.Print("not", XleColor.Yellow);
                await TextArea.Print(" saved.", XleColor.White);
                
                await TextArea.PrintLine();
                await TextArea.PrintLine();
            }

            await gameControl.WaitAsync(1500);

            await TextArea.PrintLine("Quit and return to title screen?");

            if (saved == false)
                await TextArea.PrintLine("Unsaved progress will be lost.", XleColor.Yellow);
            else
                await TextArea.PrintLine();

            await TextArea.PrintLine();

            choice = await menu.QuickMenu(menuItems, 2, 1);

            if (choice == 0)
            {
                systemState.ReturnToTitle = true;
            }
        }
    }
}
