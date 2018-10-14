
using ERY.Xle.Maps;
using ERY.Xle.Services.Game;
using ERY.Xle.Services.XleSystem;
using Microsoft.Xna.Framework;
using System;

namespace ERY.Xle.Services.ScreenModel.Implementation
{
    public class StatsDisplay : IStatsDisplay
    {
        private bool mOverrideHPColor;
        private Color mHPColor;

        public ISoundMan SoundMan { get; set; }
        public IXleScreen Screen { get; set; }
        public IXleGameControl GameControl { get; set; }
        public GameState GameState { get; set; }

        private Player Player { get { return GameState.Player; } }
        private XleMap Map { get { return GameState.Map; } }
        private IMapExtender MapExtender { get { return GameState.MapExtender; } }

        public Color HPColor
        {
            get
            {
                if (mOverrideHPColor)
                    return mHPColor;
                else
                    return Map.ColorScheme.TextColor;
            }
        }

        public int HP { get { return Player.HP; } }
        public int Gold { get { return Player.Gold; } }
        public int Food { get { return (int)Player.Food; } }

        public void ResetColor()
        {
            mHPColor = Map.ColorScheme.TextColor;
        }
        public void FlashHPWhileSound(Color color1, Color? color2 = null)
        {
            FlashHPWhile(color1, color2 ?? Screen.FontColor, () => SoundMan.IsAnyPlaying());
        }
        public void FlashHPWhile(Color clr, Color clr2, Func<bool> pred)
        {
            Color lastColor = clr2;

            mOverrideHPColor = true;
            int count = 0;

            while (pred())
            {
                if (lastColor == clr)
                    lastColor = clr2;
                else
                    lastColor = clr;

                mHPColor = lastColor;

                GameControl.Wait(80);

                count++;

                if (count > 10000 / 80)
                    break;
            }

            mOverrideHPColor = false;
        }
    }
}
