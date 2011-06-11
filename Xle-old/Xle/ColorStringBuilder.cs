using System;
using System.Collections.Generic;
using System.Text;

using AgateLib;
using AgateLib.Geometry;

namespace ERY.Xle
{
	public class ColorStringBuilder
	{
		private string text;
		private List<Color> colors = new List<Color>();

		public void AddText(string text, Color color)
		{
			if (text == null)
				return;

			this.text += text;

			for (int i = 0; i < text.Length; i++)
				colors.Add(color);
		}

		public string Text
		{
			get { return text; }
		}
		public Color[] Colors
		{
			get { return colors.ToArray(); }
		}

		public void Clear()
		{
			text = "";
			colors.Clear();
		}

	}
}
