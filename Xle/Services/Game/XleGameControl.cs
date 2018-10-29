using AgateLib;
using AgateLib.Mathematics;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xle.Services.ScreenModel;
using Xle.Services.XleSystem;

namespace Xle.Services.Game
{
    public interface IXleGameControl
    {
        Task WaitAsync(int howLong, bool keyBreak = false, IRenderer redraw = null);

        [Obsolete("await WaitAsync instead")]
        void Wait(int howLong, bool keyBreak = false, Action redraw = null);

        Task FlashHPWhileSound(Color color1, Color? color2 = null);

        Task FlashHPWhile(Color clr, Color clr2, Func<bool> pred);

        Task FinishSounds();

        void PlaySound(LotaSound lotaSound, float volume = 1);

        Task PlaySoundWait(LotaSound lotaSound, float maxTime_ms = 15000);

        Task PlayMagicSound(LotaSound sound, LotaSound endSound, int distance);

        Task WaitForKey(IRenderer renderer = null);
    }

    public static class ObsoleteExtensions
    {
        [Obsolete("Use PlaySoundWait instead")]
        public static Task PlaySoundSync(this IXleGameControl gameControl, LotaSound sound) => gameControl.PlaySoundWait(sound);
    }

    [Singleton]
    public class XleGameControl : IXleGameControl
    {
        private IXleScreen screen;
        private readonly IStatsDisplay statsDisplay;
        private readonly IXleWaiter waiter;
        private readonly IXleInput input;
        private readonly ISoundMan soundMan;
        private readonly ITextArea textArea;
        private GameState gameState;
        private XleSystemState systemState;

        public XleGameControl(
            IXleScreen screen,
            IStatsDisplay statsDisplay,
            IXleWaiter waiter,
            IXleInput input,
            ISoundMan soundMan,
            ITextArea textArea,
            GameState gameState,
            XleSystemState systemState)
        {
            this.screen = screen;
            this.statsDisplay = statsDisplay;
            this.waiter = waiter;
            this.input = input;
            this.soundMan = soundMan;
            this.textArea = textArea;
            this.gameState = gameState;
            this.systemState = systemState;

            soundMan.ErrorMessage += message =>
            {
                textArea.PrintLine();
                textArea.PrintLine(message);
            };

            textArea.Waiter += WaitAsync;
        }

        public async Task WaitAsync(int howLong, bool keyBreak = false, IRenderer redraw = null)
        {
            await waiter.WaitAsync(howLong, keyBreak, redraw);
        }

        public void Wait(int howLong, bool keyBreak = false, Action redraw = null)
        {
            throw new NotImplementedException();
        }

        public async Task WaitForKey(IRenderer renderer = null)
        {
            bool hitKey = false;

            while (!hitKey)
            {
                await waiter.WaitAsync(10000, true, renderer);

                if (waiter.PressedKey != null)
                    hitKey = true;
            }
        }

        public Task FlashHPWhileSound(Color color1, Color? color2 = null)
        {
            return FlashHPWhile(color1, color2 ?? screen.FontColor, () => soundMan.IsAnyPlaying());
        }

        public async Task FlashHPWhile(Color clr, Color clr2, Func<bool> pred)
        {
            Color lastColor = clr2;

            int count = 0;

            while (pred())
            {
                if (lastColor == clr)
                    lastColor = clr2;
                else
                    lastColor = clr;

                statsDisplay.HPColor = lastColor;

                await WaitAsync(80);

                count++;

                if (count > 10000 / 80)
                    break;
            }

            statsDisplay.ResetColor();
        }

        public void PlaySound(LotaSound lotaSound, float volume = 1)
        {
            soundMan.PlaySound(lotaSound, volume);
        }

        public async Task PlaySoundWait(LotaSound lotaSound, float maxTime_ms = 15000)
        {
            soundMan.PlaySound(lotaSound);

            await WaitAsync(150);

            int time = 0;
            while (soundMan.IsPlaying(lotaSound))
            {
                Debug.WriteLine($"Waiting for sound {lotaSound}... {time}");

                await WaitAsync(50);

                time += 50;
                if (time > maxTime_ms)
                    break;
            }
        }

        public async Task PlayMagicSound(LotaSound sound, LotaSound endSound, int distance)
        {
            if (distance <= 0)
                throw new ArgumentOutOfRangeException("distance", "Distance must be greater than zero.");

            soundMan.PlaySound(sound);
            await WaitAsync(250 * distance + 450 * (distance - 1));
            soundMan.StopSound(sound);

            soundMan.PlaySound(endSound, MathF.Pow(distance, -0.5f));
        }

        public async Task FinishSounds()
        {
            while (soundMan.IsAnyPlaying())
                await WaitAsync(10);
        }
    }
}
