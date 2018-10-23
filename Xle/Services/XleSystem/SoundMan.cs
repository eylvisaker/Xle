﻿using AgateLib;
using Xle.Services.Game;
using Xle.Services.ScreenModel;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Xle.Services.XleSystem
{
    public interface ISoundMan : IXleService
    {
        bool IsPlaying(LotaSound lotaSound);

        void StopSound(LotaSound lotaSound);

        void PlaySound(LotaSound lotaSound);
        void PlayMagicSound(LotaSound sound, LotaSound endSound, int distance);

        Task PlaySoundWait(LotaSound lotaSound, float maxTime_ms = 15000);

        [Obsolete("await PlaySoundWait instead.")]
        void PlaySoundSync(LotaSound lotaSound, float maxTime_ms = 15000);
        void PlaySoundSync(Action redraw, LotaSound sound);

        void FinishSounds();

        bool IsAnyPlaying();
    }

    [Singleton]
    [InjectProperties]
    public class SoundMan : ISoundMan
    {
        class SoundData
        {
            float volume = 1;

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
                Instance.Stop();
            }
        }

        public IXleGameControl GameControl { get; set; }
        public ITextArea TextArea { get; set; }

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

        public void PlaySound(LotaSound sound)
        {
            if (mSounds.ContainsKey(sound) == false)
            {
                TextArea.PrintLine("\nCould not play sound " + sound.ToString(), XleColor.Red);
                return;
            }

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


        public void SetSoundVolume(LotaSound sound, double volume)
        {
            if (mSounds.ContainsKey(sound) == false)
                return;

            mSounds[sound].Volume = (float)volume;
        }

        public async Task PlaySoundWait(LotaSound lotaSound, float maxTime_ms = 15000)
        {
            PlaySound(lotaSound);

            await GameControl.WaitAsync(150);

            int time = 0;
            while (IsPlaying(lotaSound))
            {
                Debug.WriteLine($"Waiting for sound {lotaSound}... {time}");

                await GameControl.WaitAsync(50);

                time += 50;
                if (time > maxTime_ms)
                    break;
            }
        }

        public void PlaySoundSync(LotaSound lotaSound, float maxTime_ms = 15000)
        {
            PlaySound(lotaSound);

            GameControl.Wait(150);

            int time = 0;
            while (IsPlaying(lotaSound))
            {
                GameControl.Wait(50);

                time += 50;
                if (time > maxTime_ms)
                    break;
            }
        }

        public void PlaySoundSync(Action redraw, LotaSound lotaSound)
        {
            PlaySound(lotaSound);

            while (IsPlaying(lotaSound))
                GameControl.Wait(50, redraw: redraw);
        }

        public void PlayMagicSound(LotaSound sound, LotaSound endSound, int distance)
        {
            if (distance <= 0)
                throw new ArgumentOutOfRangeException("distance", "Distance must be greater than zero.");

            PlaySound(sound);
            GameControl.Wait(250 * distance + 450 * (distance - 1));
            StopSound(sound);

            SetSoundVolume(endSound, Math.Pow(distance, -0.5));
            PlaySound(endSound);
        }

        public void FinishSounds()
        {
            while (IsAnyPlaying())
                GameControl.Wait(10);
        }
    }
}