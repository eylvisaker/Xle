using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERY.Xle;
using AgateLib;
using AgateLib.DisplayLib;

namespace XleMapEditor
{
	public class EditorState
	{
		public XleMap TheMap { get; set; }
		public Surface TileSurface { get; set; }
		public int TileSize { get; set; }
		public int DisplaySize { get; set; }

		public EditorState()
		{
			TileSize = 16;
			DisplaySize = 16;
		}


		internal void LoadTiles()
		{
			TileSurface = new Surface(
				MainModule.LotaPath + @"\images\" + TheMap.TileSet);
		}
	}
}
