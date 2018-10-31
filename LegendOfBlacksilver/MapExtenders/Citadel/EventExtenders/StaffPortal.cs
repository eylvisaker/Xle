using System.Threading.Tasks;

namespace Xle.Blacksilver.MapExtenders.Citadel.EventExtenders
{
    public class StaffPortal : ChangeMapTeleporter
    {
        public override Task<bool> StepOn()
        {
            return Task.FromResult(true);
        }

        public override async Task<bool> Use(int item)
        {
            if (item != (int)LobItem.Staff)
                return false;

            await TeleportAnimation();

            await ExecuteMapChange();

            return true;
        }
    }
}
