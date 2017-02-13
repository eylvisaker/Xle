using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Mathematics.Geometry;

namespace ERY.Xle.Maps.XleMapTypes.MuseumDisplays
{
	public class ExhibitInfo
	{
		int animFrame;
		double animTime;

		public ExhibitInfo()
		{
			Text = new Dictionary<int, string>();
			FrameTime = 100;
		}

		public Dictionary<int, string> Text { get; private set; }

		public string ImageFile { get; set; }
		Surface Image { get; set; }

		public void LoadImage()
		{
			if (string.IsNullOrEmpty(ImageFile))
				return;

			Image = new Surface("Images/Museum/Exhibits/" + ImageFile);
		}

		public int FrameTime { get; set; }
		public int Frames { get { return Image.SurfaceWidth / 240; } }

		public void DrawImage(Rectangle destRect, int id)
		{
			animTime += AgateApp.GameClock.Elapsed.TotalMilliseconds;
			if (animTime > FrameTime)
			{
				animTime %= FrameTime;
				animFrame++;
				if (animFrame >= Frames)
					animFrame = 0;
			}

			Rectangle srcRect = new Rectangle(animFrame * 240, 128 * id, 240, 128);

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
