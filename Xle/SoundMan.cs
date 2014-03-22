using System;
using System.Collections.Generic;
using System.Text;
using AgateLib;
using AgateLib.AudioLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using System.Diagnostics;

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
		MagicFlame,
		MagicFlameHit,
		MagicBolt,
		MagicBoltHit,
		MagicFizzle,
	}

	public static class SoundMan
	{
		static Dictionary<LotaSound, SoundBuffer> mSounds = new Dictionary<LotaSound, SoundBuffer>();

		static public void Load()
		{
			foreach (LotaSound s in Enum.GetValues(typeof(LotaSound)))
			{
				string name = Enum.GetName(typeof(LotaSound), s);
				name += ".ogg";

				try
				{
					mSounds[s] = new SoundBuffer(name);
				}
				catch (Exception e)
				{
					Debug.Print("Could not load sound {0}.", s);
					Debug.Print(e.Message);
				}
			}
		}

		static public void PlaySound(LotaSound sound)
		{
			if (mSounds.ContainsKey(sound) == false)
			{
				XleCore.TextArea.PrintLine("Could not play sound " + sound.ToString(), XleColor.Red);
				return;
			}

			mSounds[sound].Play();
		}

		public static void StopSound(LotaSound sound)
		{
			if (mSounds.ContainsKey(sound) == false)
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


		public static void SetSoundVolume(LotaSound sound, double volume)
		{
			if (mSounds.ContainsKey(sound) == false)
				return;

			mSounds[sound].Volume = volume;
		}

		public static void PlaySoundSync(LotaSound lotaSound)
		{
			PlaySound(lotaSound);

			int time = 0;
			while (IsPlaying(lotaSound))
			{
				XleCore.Wait(50);

				time += 50;
				if (time > 10000)
					break;
			}
		}
		public static void PlaySoundSync(Action redraw, LotaSound lotaSound)
		{
			PlaySound(lotaSound);

			while (IsPlaying(lotaSound))
				XleCore.Wait(50, redraw);
		}

		public static void PlayMagicSound(LotaSound sound, LotaSound endSound, int distance)
		{
			if (distance <= 0)
				throw new ArgumentOutOfRangeException("distance", "Distance must be greater than zero.");

			PlaySound(sound);
			XleCore.Wait(250 * distance + 450 * (distance - 1));
			StopSound(sound);

			SetSoundVolume(endSound, Math.Pow(distance, -0.5));
			PlaySound(endSound);
		}

		public static void FinishSounds()
		{
			while (IsAnyPlaying())
				XleCore.Wait(10);
		}
	}
}
