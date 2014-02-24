﻿using AgateLib.DisplayLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps
{
	public class Map3DSurfaces
	{
		public Surface Backdrop { get; set; }
		public Surface Wall { get; set; }
		public Surface SidePassages { get; set; }
		public Surface Door { get; set; }
		public Surface Extras { get; set; }

		public Surface MuseumExhibitFrame { get; set; }
		public Surface MuseumExhibitStatic { get; set; }
		public Surface MuseumExhibitCloseup { get; set; }
	}
}