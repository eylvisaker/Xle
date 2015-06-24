using System;

namespace ERY.Xle.Services.XleSystem
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
