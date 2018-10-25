using System.Threading.Tasks;
using Xle.Maps;
using Xle.Services;

namespace Xle.Ancients.MapExtenders.Castle.Commands
{
    [ServiceName("CastleUse")]
    public class CastleUse : LotaUse
    {
        LotaStory Story { get { return GameState.Story(); } }
        XleMap TheMap { get { return GameState.Map; } }

        protected override async Task<bool> UseWithMap(int item)
        {
            switch (item)
            {
                case (int)LotaItem.MagicSeed:
                    return await UseMagicSeeds();
            }

            return false;
        }

        private async Task<bool> UseMagicSeeds()
        {
            await GameControl.WaitAsync(150);

            Story.Invisible = true;
            await TextArea.PrintLine("You're invisible.");
            Player.RenderColor = XleColor.DarkGray;

            TheMap.Guards.IsAngry = false;

            await GameControl.WaitAsync(500);

            Player.Items[LotaItem.MagicSeed]--;

            return true;
        }
    }
}