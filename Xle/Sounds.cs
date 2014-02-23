using System;
using System.Collections.Generic;
using System.Text;
using AgateLib;
using AgateLib.AudioLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace ERY.Xle
{
	public enum LotaSound
	{
		WalkOutside,
		WalkTown,
		WalkMuseum,
		WalkDungeon,
		WalkSwamp,
		WalkDesert,
		Bump,
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
		Teleporter,
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

				try
				{
					mSounds[s] = new SoundBuffer(name);
				}
				catch
				{ }
			}
		}

		static public void PlaySound(LotaSound sound)
		{
			if (mSounds.ContainsKey(sound) == false)
			{
				g.AddBottom("Could not play sound " + sound.ToString(), XleColor.Red);
				return;
			}

			mSounds[sound].Play();
		}

		public static void StopSound(LotaSound sound)
		{
			if (mSounds[sound] == null)
			{
				return;
			}

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
			if (mSounds.ContainsKey(sound) == false)
				return false;

			return mSounds[sound].IsPlaying;
		}

		public static void StopAllSounds()
		{
			foreach (var kvp in mSounds)
			{
				kvp.Value.Stop();
			}
		}

		public static void PlaySoundSync(LotaSound lotaSound)
		{
			PlaySound(lotaSound);

			while (IsPlaying(lotaSound))
				XleCore.Wait(50);
		}
		public static void PlaySoundSync(Action redraw, LotaSound lotaSound)
		{
			PlaySound(lotaSound);

			while (IsPlaying(lotaSound))
				XleCore.Wait(50, redraw);
		}

		public static void FinishSounds()
		{
			while (IsAnyPlaying())
				XleCore.Wait(10);
		}
	}
}
