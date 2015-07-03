namespace ERY.Xle.LotA.MapExtenders.Castle.Events
{
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

        public override bool Speak()
        {
            if (Story.HasGuardianPassword)
            {
                TextArea.PrintLine(" password.");
                SoundMan.PlaySoundSync(LotaSound.VeryGood);

                RemoveDoor();
                return true;
            }
            else
                return false;
        }
    }
}
