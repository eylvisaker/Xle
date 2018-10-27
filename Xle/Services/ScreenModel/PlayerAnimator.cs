using AgateLib;
using Microsoft.Xna.Framework;
using System;

namespace Xle.Services.ScreenModel
{
    public interface IPlayerAnimator
    {
        void AnimateStep();

        bool Animating { get; }

        int AnimFrame { get; }

        void Update(GameTime time);
    }

    [Singleton]
    public class PlayerAnimator : IPlayerAnimator
    {
        private int charAnimCount;           // animation count for the player

        // character functions
        private const int frameTime = 150;
        private int animFrame;

        private float timeToNextFrame = frameTime;
        private bool paused;

        public int AnimFrame
        {
            get => animFrame;
            set
            {
                animFrame = value;

                while (animFrame < 0)
                    animFrame += 300;

                animFrame %= 3;

                timeToNextFrame = frameTime;
            }
        }
        /// <summary>
        /// sets or returns whether or not the character is animating
        /// </summary>
        /// <returns></returns>
        public bool Animating
        {
            get
            {
                if (paused == true)
                {
                    animFrame = 0;
                }

                return paused == false;
            }
            set
            {
                if (value == false)
                {
                    animFrame = 0;
                    charAnimCount = 0;

                    if (paused == false)
                        paused = true;
                }
                else
                    paused = false;
            }
        }

        public void AnimateStep()
        {
            if (Animating == false)
            {
                Animating = true;
                AnimFrame = 0;
            }

            charAnimCount = 0;
        }

        public void Update(GameTime time)
        {
            if (!Animating)
                return;

            timeToNextFrame -= (float)time.ElapsedGameTime.TotalMilliseconds;

            if (timeToNextFrame <= 0 && !paused)
            {
                timeToNextFrame += frameTime;
                animFrame++;
                animFrame %= 3;

                charAnimCount++;

                if (charAnimCount > 6)
                {
                    animFrame = 0;
                    charAnimCount = 0;
                    Animating = false;
                }
            }
        }
    }
}
