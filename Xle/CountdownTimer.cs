using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Xle
{
	public class CountdownTimer
	{
		Stopwatch watch = new Stopwatch();

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
