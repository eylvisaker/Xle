using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

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
		Surface Image { get; set; }

		public void LoadImage()
		{
			if (string.IsNullOrEmpty(ImageFile))
				return;

			Image = new Surface("Museum/Exhibits/" + ImageFile);
		}

		public void DrawImage(Rectangle destRect, int id)
		{
			Rectangle srcRect = new Rectangle(0, 128 * id, 240, 128);

			if (Image != null)
			{
				Image.Draw(srcRect, destRect);
			}
			else
			{
				System.Diagnostics.Debug.Print("Null image in exhibit: " + Text);
			}
		}
	}
}
