
using AgateLib;
using System.Threading.Tasks;
using Xle.Maps;

namespace Xle.Ancients.MapExtenders.Dungeons
{
    [Transient("ArmakDungeon")]
    public class ArmakDungeon : LotaDungeon
    {
        protected override bool IsCompleted
        {
            get { return Story.ArmakComplete; }
            set { Story.ArmakComplete = value; }
        }

        protected override int StrengthBoost
        {
            get { return 15; }
        }

        public override async Task OnPlayerExitDungeon()
        {
            if (IsCompleted)
                return;

            IsCompleted = true;

            await GivePermanentStrengthBoost();
        }

        public override Map3DSurfaces Surfaces()
        {
            return Lota3DSurfaces.DungeonBrown;
        }
    }
}
