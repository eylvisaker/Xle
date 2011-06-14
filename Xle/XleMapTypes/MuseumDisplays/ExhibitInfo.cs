using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;

namespace ERY.Xle.XleMapTypes.MuseumDisplays
{
	public class ExhibitInfo
	{
		public ExhibitInfo()
		{
			Text = new Dictionary<int, string>();
		}

		public Dictionary<int, string> Text { get; private set; }

		public string ImageFile { get; set; }
		public Surface Image { get; set; }

		public void LoadImage()
		{
			if (string.IsNullOrEmpty(ImageFile))
				return;

			Image = new Surface(ImageFile);
		}
	}
}
