using System.Threading.Tasks;
using Xle.XleEventTypes.Extenders;

namespace Xle.Blacksilver.MapExtenders.Temples
{
    public class TempleStairs : ChangeMap
    {
        public TempleStairs()
        {
            base.Enabled = false;
        }

        protected override Task<bool> OnStepOnImpl()
        {
            return Task.FromResult(false);
        }
    }
}
