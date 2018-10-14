﻿using ERY.Xle.Services.Game;
using ERY.Xle.Services.ScreenModel;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ERY.Xle.Services.XleSystem.Implementation
{
    public class SoundMan : ISoundMan
    {
        public IXleGameControl GameControl { get; set; }
        public ITextArea TextArea { get; set; }

        private Dictionary<LotaSound, SoundEffect> mSounds = new Dictionary<LotaSound, SoundEffect>();

        public void Load()
        {
            foreach (LotaSound s in Enum.GetValues(typeof(LotaSound)))
            {
                string name = Enum.GetName(typeof(LotaSound), s);
                name += ".ogg";

                try
                {
                    mSounds[s] = new SoundBuffer("Audio/" + name);
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

            //mSounds[sound].Stop();
            throw new NotImplementedException();
        }

        public bool IsAnyPlaying()
        {
            throw new NotImplementedException();
            //foreach (var kvp in mSounds)
            //{
            //    if (kvp.Value.IsPlaying)
            //        return true;
            //}

            //return false;
        }
        public bool IsPlaying(LotaSound sound)
        {
            if (mSounds.ContainsKey(sound) == false)
                return false;

            //return mSounds[sound].IsPlaying;
            throw new NotImplementedException();
        }

        public void StopAllSounds()
        {
            //foreach (var kvp in mSounds)
            //{
            //    kvp.Value.Stop();
            //}
            throw new NotImplementedException();
        }


        public void SetSoundVolume(LotaSound sound, double volume)
        {
            if (mSounds.ContainsKey(sound) == false)
                return;

            //mSounds[sound].Volume = volume;
            throw new NotImplementedException();
        }

        public void PlaySoundSync(LotaSound lotaSound)
        {
            PlaySound(lotaSound);

            int time = 0;
            while (IsPlaying(lotaSound))
            {
                GameControl.Wait(50);

                time += 50;
                if (time > 10000)
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
