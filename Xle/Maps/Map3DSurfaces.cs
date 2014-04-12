using AgateLib.DisplayLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps
{
	public class Map3DSurfaces
	{
		public Surface Walls { get; set; }
		public Surface Torches { get; set; }

		public Surface Extras { get; set; }

		public Surface ExhibitOpen { get; set; }
		public Surface ExhibitClosed { get; set; }
	}
}
