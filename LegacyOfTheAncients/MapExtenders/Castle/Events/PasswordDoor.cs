using AgateLib;
using System.Threading.Tasks;

namespace Xle.Ancients.MapExtenders.Castle.Events
{
    [Transient("PasswordDoor")]
    public class PasswordDoor : CastleDoor
    {
        public LotaStory Story
        {
            get { return GameState.Story(); }
        }

        public override bool ItemUnlocksDoor(int item)
        {
            return false;
        }

        public override async Task<bool> Speak()
        {
            if (Story.HasGuardianPassword)
            {
                await TextArea.PrintLine(" password.");
                await GameControl.PlaySoundWait(LotaSound.VeryGood);

                RemoveDoor();
                return true;
            }
            else
                return false;
        }
    }
}
