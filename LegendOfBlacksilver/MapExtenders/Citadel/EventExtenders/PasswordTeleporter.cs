using System.Threading.Tasks;

namespace Xle.LoB.MapExtenders.Citadel.EventExtenders
{
    public class PasswordTeleporter : ChangeMapTeleporter
    {
        protected override async Task<bool> OnStepOnImpl()
        {
            return false;
        }

        public override async Task<bool> Speak()
        {
            if (Story.CitadelPassword)
            {
                await ExecuteTeleportation();
                return true;
            }

            return await base.Speak();
        }
    }
}
