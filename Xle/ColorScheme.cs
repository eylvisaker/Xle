using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle
{
	public class ColorScheme
	{
		public ColorScheme()
		{
			TextColor = XleColor.White;
			BackColor = XleColor.Black;
			FrameColor = XleColor.Orange;
			FrameHighlightColor = XleColor.Yellow;
			TitleColor = XleColor.White;
			TextAreaBackColor = XleColor.Black;
			BorderColor = XleColor.DarkGray;

			MapAreaWidth = 25;
		}

		public Color TextColor { get; set; }
		public Color FrameColor { get; set; }
		public Color FrameHighlightColor { get; set; }

		[Obsolete]
		public int VerticalLinePosition
		{
			get
			{
				return (38 - MapAreaWidth) * 16;
			}
			set
			{
				int chars = value / 16;

				MapAreaWidth = 38 - chars;
			}
		}

		public int MapAreaWidth { get; set; }
		public Color BackColor { get; set; }

		public Color TitleColor { get; set; }

		public Color TextAreaBackColor { get; set; }

		public Color BorderColor { get; set; }
	}
}
