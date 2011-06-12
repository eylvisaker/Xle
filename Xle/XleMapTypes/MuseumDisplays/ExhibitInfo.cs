using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.MuseumDisplays
{
	public class ExhibitInfo
	{
		public ExhibitInfo()
		{
			Text = new Dictionary<int, string>();
		}

		public Dictionary<int, string> Text { get; private set; }
	}
}
