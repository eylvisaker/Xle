using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.Services.Rendering.Maps
{
    class TorchAnim
    {
        public TorchAnim(Random random)
        {
            this.Random = random;

            SetNextAnimTime();
            AdvanceFrame();
        }

        public void AdvanceFrame()
        {
            if (CurrentFrame == 3 || CurrentFrame == 4)
            {
                CurrentFrame++;
            }
            else
            {
                int newFrame = CurrentFrame;

                while (newFrame == CurrentFrame)
                    newFrame = Random.Next(77) / 25;

                CurrentFrame = newFrame;
            }
        }
        public void SetNextAnimTime()
        {
            NextAnimTime = Random.Next(100, 125);
        }

        public double NextAnimTime;
        public int CurrentFrame;
        private Random Random;
    }

}
