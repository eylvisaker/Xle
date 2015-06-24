using AgateLib.Platform;

namespace ERY.Xle.Services.ScreenModel.Implementation
{
    public class PlayerAnimator : IPlayerAnimator
    {
        int charAnimCount;			// animation count for the player
        
        // character functions
        IStopwatch animWatch = AgateLib.Platform.Timing.CreateStopWatch();
        const int frameTime = 150;

        int animFrame;

        public int AnimFrame
        {
            get
            {
                int oldAnim = animFrame;

                if (animWatch.IsPaused == false)
                    animFrame = (((int)animWatch.TotalMilliseconds) / frameTime);

                if (oldAnim != animFrame)
                {
                    charAnimCount++;

                    if (charAnimCount > 6)
                    {
                        animFrame = 0;
                        charAnimCount = 0;
                        Animating = false;
                    }
                }

                return animFrame;
            }
            set
            {
                animFrame = value;

                while (animFrame < 0)
                    animFrame += 3;

                animWatch.Reset();
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
                if (animWatch.IsPaused == true)
                {
                    animFrame = 0;
                }

                return animWatch.IsPaused == false;
            }
            set
            {
                if (value == false)
                {
                    animFrame = 0;
                    charAnimCount = 0;

                    if (animWatch.IsPaused == false)
                        animWatch.Pause();
                }
                else
                    animWatch.Resume();

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

    }
}
