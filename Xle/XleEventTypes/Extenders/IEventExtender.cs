using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Extenders
{
	public interface IEventExtender
	{
		XleEvent TheEvent { get; set; }
	}
}
