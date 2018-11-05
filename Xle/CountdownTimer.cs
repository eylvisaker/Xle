using System.Diagnostics;

namespace Xle
{
    public class CountdownTimer
    {
        private Stopwatch watch = new Stopwatch();

        public CountdownTimer(int timeInMs)
        {
            Time = timeInMs;
            watch.Start();
        }

        public int Time { get; set; }

        public bool StillRunning()
        {
            return watch.ElapsedMilliseconds < Time;
        }

    }
}
