using System;
using System.Collections.Generic;
using System.Text;
using AgateLib;
using AgateLib.AudioLib;
using AgateLib.DisplayLib;

namespace ERY.Xle
{
	public enum LotaSound
	{
		WalkOutside,
		Bump,
		Swamp,
		Desert,
		Question,
		Invalid,
		Raft,
		VeryBad,
		Raft1,
		Ocean1,
		Ocean2,
		VeryGood,
		Mountains,
		BoardRaft,
		EnemyDie,
		EnemyHit,
		EnemyMiss,
		PlayerHit,
		PlayerMiss,
		Medium,
		TownWalk,
		Sale,
		BuildingClose,
		BuildingOpen,
		Good,
		OpenChest,
		UnlockDoor,
		Encounter,
		Xamine,
		XamineDetected,
		Drip0,
		Drip1,
		Bad,
	}

	public static class SoundMan
	{
		static Dictionary<LotaSound, SoundBuffer> mSounds = new Dictionary<LotaSound, SoundBuffer>();

		static public void Load()
		{
			foreach (LotaSound s in Enum.GetValues(typeof(LotaSound)))
			{
				string name = Enum.GetName(typeof(LotaSound), s);
				name += ".wav";

				mSounds[s] = new SoundBuffer(name);
			}
		}

		static public void PlaySound(LotaSound sound)
		{
			mSounds[sound].Play();
		}

		public static void StopSound(LotaSound sound)
		{
			mSounds[sound].Stop();
		}

		public static bool IsAnyPlaying()
		{
			foreach (var kvp in mSounds)
			{
				if (kvp.Value.IsPlaying)
					return true;
			}

			return false;
		}
		public static bool IsPlaying(LotaSound sound)
		{
			return mSounds[sound].IsPlaying;
		}

		internal static void StopAllSounds()
		{
			foreach (var kvp in mSounds)
			{
				kvp.Value.Stop();
			}
		}

		internal static void PlaySoundSync(LotaSound lotaSound)
		{
			PlaySound(lotaSound);

			while (IsPlaying(lotaSound))
				XleCore.wait(50);
		}
		internal static void PlaySoundSync(XleCore.RedrawDelegate redraw, LotaSound lotaSound)
		{
			PlaySound(lotaSound);

			while (IsPlaying(lotaSound))
				XleCore.wait(redraw, 50);
		}

		internal static void FinishSounds()
		{
			while (IsAnyPlaying())
				XleCore.wait(10);
		}
	}
}
