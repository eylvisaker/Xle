using Xle.Maps;
using Xle.Services;

namespace Xle.Ancients.MapExtenders.Castle.Commands
{
    [ServiceName("CastleUse")]
    public class CastleUse : LotaUse
    {
        LotaStory Story { get { return GameState.Story(); } }
        XleMap TheMap { get { return GameState.Map; } }

        protected override bool UseWithMap(int item)
        {
            switch (item)
            {
                case (int)LotaItem.MagicSeed:
                    return UseMagicSeeds();
            }

            return false;
        }
        private bool UseMagicSeeds()
        {
            GameControl.Wait(150);

            Story.Invisible = true;
            TextArea.PrintLine("You're invisible.");
            Player.RenderColor = XleColor.DarkGray;

            TheMap.Guards.IsAngry = false;

            GameControl.Wait(500);

            Player.Items[LotaItem.MagicSeed]--;

            return true;
        }
    }
}