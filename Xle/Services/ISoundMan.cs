﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Services
{
    public interface ISoundMan : IXleService
    {
        void Load();

        bool IsPlaying(LotaSound lotaSound);

        void StopSound(LotaSound lotaSound);

        void PlaySound(LotaSound lotaSound);
        void PlayMagicSound(LotaSound sound, LotaSound endSound, int distance);

        void PlaySoundSync(LotaSound lotaSound);
        void PlaySoundSync(Action redraw, LotaSound sound);

        void FinishSounds();


        bool IsAnyPlaying();
    }
}
