using AgateLib;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;

namespace Xle.Ancients.MapExtenders.Castle
{
    [Transient("CastleUpper")]
    public class CastleUpper : CastleGround
    {
        public CastleUpper()
        {
            CastleLevel = 2;
        }

        public override async Task OnAfterEntry()
        {
            if (Story.Invisible == false)
            {
                await TextArea.PrintLine("Private level!");

                IsAngry = true;
            }
        }

        public override int GetOutsideTile(Point playerPoint, int x, int y)
        {
            return TheMap.OutsideTile;
        }
    }
}
