using AgateLib;
using System.Threading.Tasks;

namespace Xle.Ancients.MapExtenders.Castle.Events
{
    [Transient("SpiralSuccess")]
    public class SpiralSuccess : Spiral
    {
        public override async Task<bool> StepOn()
        {
            if (AnyBad)
                return false;

            await SoundMan.PlaySoundWait(LotaSound.VeryGood);

            ClearSpiral();
            RemoveSpiralEvents();

            for (int y = 3; y < TheEvent.Rectangle.Y - 1; y++)
            {
                for (int x = TheEvent.Rectangle.X - 3; x <= TheEvent.Rectangle.X; x++)
                {
                    Map[x, y] = 0;
                }
            }

            return true;
        }
    }
}