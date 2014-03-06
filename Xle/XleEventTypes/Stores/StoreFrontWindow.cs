using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Stores
{
	public class StoreFrontWindow
	{
		ColorStringBuilder csb = new ColorStringBuilder();

		public StoreFrontWindow()
		{
			DefaultTextColor = XleColor.White;
		}

		public Point Location { get; set; }

		public ColorStringBuilder Csb { get { return csb; } }

		public void Draw()
		{
			var renderer = XleCore.Renderer;

			renderer.WriteText(Location.X * 16, Location.Y * 16, csb.Text, csb.Colors);
		}

		public Color DefaultTextColor { get; set; }

		public void Write(string text)
		{
			Write(text, DefaultTextColor);
		}
		public void Write(string text, Color color)
		{
			csb.AddText(text, color);
		}

		public void WriteLine(string text = "")
		{
			WriteLine(text, DefaultTextColor);
		}
		public void WriteLine(string text, Color color)
		{
			Write(text, color);
			Write("\n");
		}
	}
}
