namespace ERY.Xle.LotA.MapExtenders.Castle.Events
{
	public class PasswordDoor : CastleDoor
    {
        public LotaStory Story
        {
            get { return GameState.Story(); }
        }

		public override bool ItemUnlocksDoor(GameState state, int item)
		{
			return false ;
		}

		public override bool Speak(GameState state)
		{
			if (Story.HasGuardianPassword)
			{
				TextArea.PrintLine(" password.");
				SoundMan.PlaySoundSync(LotaSound.VeryGood);

				RemoveDoor(state);
				return true;
			}
			else
				return false;
		}
	}
}
