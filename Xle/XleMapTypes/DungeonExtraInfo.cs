using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace ERY.Xle.XleMapTypes
{
	public class DungeonExtraInfo
	{
		Dictionary<int, DungeonExtraImage> mImages = new Dictionary<int, DungeonExtraImage>();

		public Dictionary<int, DungeonExtraImage> Images
		{
			get { return mImages; }
		}
	}

	public class DungeonExtraImage
	{
		public Rectangle SrcRect { get; set; }
		public Rectangle DestRect { get; set; }
	}
}
