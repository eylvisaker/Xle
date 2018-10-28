using AgateLib;
using System.Threading.Tasks;
using Xle.XleEventTypes.Extenders;

namespace Xle.Ancients.MapExtenders.Fortress.FirstArea
{
    [Transient("GuardWarning")]
    public class GuardWarning : EventExtender
    {
        public override async Task<bool> StepOn()
        {
            Enabled = false;

            await TextArea.PrintLine();
            await TextArea.PrintLine();
            await TextArea.PrintLine("The guards eye you warily", XleColor.Yellow);

            await SoundMan.PlaySoundWait(LotaSound.VeryBad);

            return true;
        }
    }
}
