using AgateLib;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Xle.XleSystem
{
    public interface ISoundMan
    {
        bool IsPlaying(LotaSound lotaSound);

        void StopSound(LotaSound lotaSound);

        void PlaySound(LotaSound lotaSound, float volume = 1);

        bool IsAnyPlaying();

        event Action<string> ErrorMessage;
    }

    [Singleton]
    [InjectProperties]
    public class SoundMan : ISoundMan
    {
        private class SoundData
        {
            private float volume = 1;

            public SoundEffect SoundEffect { get; set; }
            public SoundEffectInstance Instance { get; set; }

            public bool IsPlaying => (Instance?.State ?? SoundState.Stopped) == SoundState.Playing;

            public float Volume
            {
                get => volume;
                set
                {
                    volume = value;
                    if (Instance != null)
                        Instance.Volume = volume;
                }
            }

            public void Play()
            {
                Instance = SoundEffect.CreateInstance();
                Instance.Volume = volume;
                Instance.Play();
            }

            public void Stop()
            {
                Instance?.Stop();
            }
        }

        private Dictionary<LotaSound, SoundData> mSounds = new Dictionary<LotaSound, SoundData>();

        public SoundMan(IContentProvider content)
        {
            foreach (LotaSound s in Enum.GetValues(typeof(LotaSound)))
            {
                string name = Enum.GetName(typeof(LotaSound), s);

                try
                {
                    mSounds[s] = new SoundData { SoundEffect = content.Load<SoundEffect>("Audio/" + name) };
                }
                catch (Exception e)
                {
                    Debug.Print("Could not load sound {0}.", s);
                    Debug.Print(e.Message);
                }
            }
        }

        public event Action<string> ErrorMessage;

        public void PlaySound(LotaSound sound, float volume = 1)
        {
            if (mSounds.ContainsKey(sound) == false)
            {
                ErrorMessage?.Invoke("\nCould not play sound " + sound.ToString());
                return;
            }

            mSounds[sound].Volume = volume;
            mSounds[sound].Play();
        }

        public void StopSound(LotaSound sound)
        {
            if (mSounds.ContainsKey(sound) == false)
            {
                return;
            }

            mSounds[sound].Stop();
        }

        public bool IsAnyPlaying()
        {
            foreach (var kvp in mSounds)
            {
                if (kvp.Value.IsPlaying)
                    return true;
            }

            return false;
        }

        public bool IsPlaying(LotaSound sound)
        {
            if (mSounds.ContainsKey(sound) == false)
                return false;

            var result = mSounds[sound].IsPlaying;

            return result;
        }

        public void StopAllSounds()
        {
            foreach (var kvp in mSounds)
            {
                kvp.Value.Stop();
            }
        }
    }
}
