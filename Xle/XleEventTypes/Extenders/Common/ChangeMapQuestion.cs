using AgateLib;
using System.Threading.Tasks;
using Xle.Services.Menus;

namespace Xle.XleEventTypes.Extenders.Common
{
    [Transient("ChangeMapQuestion")]
    public class ChangeMapQuestion : ChangeMap
    {
        public IQuickMenu QuickMenu { get; set; }

        protected override async Task<bool> OnStepOnImpl()
        {
            string newMapName = GetMapName();
            await TextArea.PrintLine();
            await TextArea.PrintLine("Enter " + newMapName + "?");

            SoundMan.PlaySound(LotaSound.Question);

            int choice = await QuickMenu.QuickMenu(new MenuItemList("Yes", "No"), 3);

            if (choice == 1)
                return false;
            else if (string.IsNullOrEmpty(TheEvent.CommandText) == false)
            {
                await TextArea.PrintLine();
                await TextArea.PrintLine(
                     string.Format(TheEvent.CommandText,
                     Map.MapName, newMapName));

                await TextArea.PrintLine();
                await GameControl.WaitAsync(500);
            }

            await ExecuteMapChange();
            return true;
        }
    }
}
