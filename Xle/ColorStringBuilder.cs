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
		public void AddLine(string text, Color color)
		{
			AddText(text, color);
			AddText("\n", color);
		}

		public string Text
		{
			get { return text; }
		}
		/// <summary>
		/// Read only copy of the colors.
		/// </summary>
		public Color[] Colors
		{
			get { return colors.ToArray(); }
		}
		public void SetColor(int index, Color clr)
		{
			colors[index] = clr;
		}

		public void Clear()
		{
			text = "";
			colors.Clear();
		}
	}
}
