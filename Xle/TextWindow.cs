﻿using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle
{
	public class TextWindow
	{
		ColorStringBuilder csb = new ColorStringBuilder();

		public TextWindow()
		{
			DefaultTextColor = XleColor.White;
		}

		public Point Location { get; set; }

		public void SetColor(Color color)
		{
			for (int i = 0; i < csb.Text.Length; i++)
				csb.SetColor(i, color);
		}
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

		public string Text
		{
			get { return csb.Text; }
			set
			{
				csb.Text = value;
			}
		}

		public void Clear()
		{
			csb.Clear();
		}
	}
}