using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ERY.Xle
{
	public class TextArea
	{
		class TextLine
		{
			public string Text { get; set; }
			public Color[] Colors { get; set; }

			public TextLine()
			{
				Colors = new Color[40];
			}

			public void SetColor(Color color)
			{
				for (int i = 0; i < Colors.Length; i++)
					Colors[i] = color;
			}
			public void WriteText(int x, string t, Color[] newColors)
			{
				SetTextLength(x);

				Text += t;

				if (newColors != null)
				{
					for (int i = 0; i < newColors.Length; i++)
					{
						if (x + i >= Colors.Length)
							break;

						Colors[x + i] = newColors[i];
					}
				}
				else
				{
					WriteColors(x, t.Length, XleCore.FontColor);
				}
			}
			public void WriteText(int x, string t, Color? color)
			{
				SetTextLength(x);

				Text += t;

				WriteColors(x, t.Length, color ?? XleCore.FontColor);
			}

			private void WriteColors(int x, int length, Color newColor)
			{
				for (int i = 0; i < length; i++)
					Colors[x + i] = newColor;
			}


			private void SetTextLength(int x)
			{
				if (Text.Length < x)
					Text += new string(' ', x - Text.Length);
				if (Text.Length > x)
					Text = Text.Substring(0, x);
			}
			public override string ToString()
			{
				return Text;
			}

		}

		TextLine[] lines = new TextLine[5];
		Point cursor = new Point(1, 5);
		int margin = 1;

		public TextArea()
		{
			for (int i = 0; i < lines.Length; i++)
				lines[i] = new TextLine();
		}

		public int Margin { get { return margin; } set { margin = value; } }

		public void DrawArea()
		{
			for (int i = 0; i < 5; i++)
			{
				//int x = 16 + 16 * g.BottomMargin(i);

				//DrawText(x, 368 - 16 * i, g.Bottom(i), g.BottomColor(i));
				int x = 16;

				var line = lines[i];

				DrawText(x, 304 + 16 * i, line.Text, line.Colors);
			}
		}

		private void DrawText(int x, int y, string text, Color[] color)
		{
			XleCore.WriteText(x, y, text, color);
		}


		[Obsolete("This is for compatibility with g.AddBottom. Use Print and PrintLine instead.")]
		public void WriteLine(string line, Color[] colors)
		{
			if (cursor.Y == 5)
			{
				CycleLines();
			}

			PrintLine(line, colors);
		}

		private void CycleLines()
		{
			var old = lines[0];

			for (int i = 0; i < lines.Length - 1; i++)
				lines[i] = lines[i + 1];

			lines[4] = old;

			lines[4].Text = "";
			lines[4].SetColor(XleColor.White);

			cursor.Y--;

			if (cursor.Y < 0) cursor.Y = 0;
			if (cursor.Y >= lines.Length) cursor.Y = lines.Length - 1;
		}


		[Obsolete("This is for compatibility with g.AddBottom. Use Print and PrintLine instead.")]
		public void UpdateLine(int line, int x, string text, Color[] colors)
		{
			var target = lines[line];

			target.Text = new string(' ', x + margin) + text;

			if (colors != null)
			{
				if (colors.Length == 40)
					target.Colors = colors;

				else
				{
					for (int i = 0; i < colors.Length && i + x + margin < target.Colors.Length; i++)
					{
						target.Colors[i + x + margin] = colors[i];
					}
				}

			}
		}

		public string GetTextLine(int line)
		{
			return lines[line].Text.Substring(margin);
		}


		private void CycleIfNeeded()
		{
			if (cursor.Y == 5)
			{
				CycleLines();
			}
		}

		public void Print(string text, Color? color)
		{
			CycleIfNeeded();

			lines[cursor.Y].WriteText(cursor.X, text, color);
			cursor.X += text.Length;
		}
		public void Print(string text = "", Color[] colors = null)
		{
			CycleIfNeeded();

			var current = lines[cursor.Y];

			current.WriteText(cursor.X, text, colors);
			cursor.X += text.Length;
		}

		public void PrintLine(string text, Color color)
		{
			Print(text, color);

			cursor.Y++;
			cursor.X = margin;
		}
		public void PrintLine(string text = "", Color[] colors = null)
		{
			Print(text, colors);

			cursor.Y++;
			cursor.X = margin;
		}

		public void PrintSlow(string text, Color[] colors= null)
		{
			for (int i = 0; i < text.Length; i++)
			{
				if (colors != null)
				{
					Print(text[i].ToString(), colors[i]);
				}
				else
				{
					Print(text[i].ToString(), XleCore.FontColor);
				}

				XleCore.Wait(50);
			}
		}
		public void PrintLineSlow(string text = "", Color[] colors = null)
		{
			PrintSlow(text, colors);

			PrintLine();
		}

		public void Clear(bool cursorAtTop = false)
		{
			foreach (var line in lines)
			{
				line.Text = "";
				line.SetColor(XleCore.FontColor);
			}

			cursor.X = margin;
			cursor.Y = 4;

			if (cursorAtTop)
				cursor.Y = 0;
		}

		public int FlashRate = 125;

		/// <summary>
		/// Flashes lines of text on the screen.
		/// </summary>
		/// <param name="howLong">How many milliseconds to flash for.</param>
		/// <param name="color">The color to flash to.</param>
		/// <param name="lines">Which lines. Don't pass any extra parameters to flash the whole text area.</param>
		public void FlashLines(int howLong, Color color, params int[] lines)
		{
			if (lines == null || lines.Length == 0)
			{
				FlashLines(howLong, color, 0, 1, 2, 3, 4);
				return;
			}

			Stopwatch watch = new Stopwatch();
			watch.Start();

			while (watch.ElapsedMilliseconds < howLong)
			{
				int index = (int)watch.ElapsedMilliseconds % FlashRate / (FlashRate / 2);

				Color clr = color;

				if (index == 1)
					clr = XleCore.FontColor;

				foreach(var line in lines)
				{
					this.lines[line].SetColor(clr);
				}

				XleCore.Redraw();
			}


			foreach (var line in lines)
			{
				this.lines[line].SetColor(XleCore.FontColor);
			}
		}
	}
}
