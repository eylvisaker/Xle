using ERY.Xle.Services.Implementation;

namespace ERY.Xle.LotA.MapExtenders.Castle.Events
{
	class PasswordDoor : CastleDoor
	{
		public override bool ItemUnlocksDoor(GameState state, int item)
		{
			return false ;
		}

		public override bool Speak(GameState state)
		{
			if (Lota.Story.HasGuardianPassword)
			{
				XleCore.TextArea.PrintLine(" password.");
				SoundMan.PlaySoundSync(LotaSound.VeryGood);

				RemoveDoor(state);
				return true;
			}
			else
				return false;
		}
	}
}
