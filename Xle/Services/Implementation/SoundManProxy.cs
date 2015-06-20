using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Services.Implementation
{
    public class SoundManProxy : ISoundMan
    {
        public void Load()
        {
            SoundMan.Load();
        }

        public bool IsPlaying(LotaSound lotaSound)
        {
            return SoundMan.IsPlaying(lotaSound);
        }

        public void StopSound(LotaSound lotaSound)
        {
            SoundMan.StopSound(lotaSound);
        }

        public void PlaySound(LotaSound lotaSound)
        {
            SoundMan.PlaySound(lotaSound);
        }

        public void PlaySoundSync(LotaSound lotaSound)
        {
            SoundMan.PlaySoundSync(lotaSound);
        }

        public void PlaySoundSync(Action redraw, LotaSound sound)
        {
            SoundMan.PlaySoundSync(redraw, sound);
        }


        public void FinishSounds()
        {
            SoundMan.FinishSounds();
        }


        public void PlayMagicSound(LotaSound sound, LotaSound endSound, int distance)
        {
            SoundMan.PlayMagicSound(sound, endSound, distance);
        }


        public bool IsAnyPlaying()
        {
            return SoundMan.IsAnyPlaying();
        }
    }
}
