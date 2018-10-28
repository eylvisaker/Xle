using AgateLib;
using System.Threading.Tasks;
using Xle.XleEventTypes.Extenders;

namespace Xle.Ancients.MapExtenders.Fortress.SecondArea
{
    [Transient("DoorShut")]
    public class DoorShut : EventExtender
    {
        private int replacementTile = 40;

        public override async Task<bool> StepOn()
        {
            for (int i = TheEvent.Rectangle.X; i < TheEvent.Rectangle.Right; i++)
            {
                Map[i, TheEvent.Rectangle.Bottom - 1] = replacementTile;
            }

            Enabled = false;

            return true;
        }
    }
}
