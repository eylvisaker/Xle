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
			public void WriteText(int x, string t, Color[] newColors, int colorStartIndex = 0)
			{
				SetTextLength(x);

				Text += t;

				if (newColors != null)
				{
					for (int i = colorStartIndex; i < newColors.Length; i++)
					{
						if (x + i >= Colors.Length)
							break;

						Colors[x + i] = newColors[i];
					}
				}
				else
				{
					WriteColors(x, t.Length, XleCore.Renderer.FontColor);
				}
			}
			public void WriteText(int x, string t, Color? color)
			{
				SetTextLength(x);

				Text += t;

				WriteColors(x, t.Length, color ?? XleCore.Renderer.FontColor);
			}

			private void WriteColors(int x, int length, Color newColor)
			{
				for (int i = 0; i < length && x + i < Colors.Length; i++)
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

		public void Draw()
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
			XleCore.Renderer.WriteText(x, y, text, color);
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
		Color[] tempColors;

		public void Print(string text, Color? color)
		{
			if (tempColors == null || tempColors.Length < text.Length)
			{
				tempColors = new Color[text.Length];
			}

			for (int i = 0; i < tempColors.Length; i++)
			{
				tempColors[i] = color ?? XleCore.Renderer.FontColor;
			}

			PrintImpl(text, tempColors);
		}
		public void Print(string text = "", Color[] colors = null)
		{
			PrintImpl(text, colors);
		}

		private void PrintImpl(string text, Color[] colors)
		{
			int startIndex = 0;

			while (startIndex < text.Length)
			{
				CycleIfNeeded();

				int endIndex = text.IndexOf("\n", startIndex);
				var current = lines[cursor.Y];

				if (endIndex == -1)
				{
					current.WriteText(cursor.X, text.Substring(startIndex), colors, startIndex);
					cursor.X += text.Length - startIndex;
					startIndex = text.Length;
				}
				else
				{
					current.WriteText(cursor.X, text.Substring(startIndex, endIndex - startIndex), colors, startIndex);

					startIndex = endIndex + 1;
					cursor.X = Margin;
					cursor.Y++;
				}
			}

			XleCore.Wait(1);
		}

		public void PrintLine(string text, Color color)
		{
			Print(text + "\n", color);
		}
		public void PrintLine(string text = "", Color[] colors = null)
		{
			Print(text + "\n", colors);
		}

		void PrintSlowImpl(string text, Color defaultColor, Color[] colors = null)
		{
			for (int i = 0; i < text.Length; i++)
			{
				if (colors != null)
				{
					Print(text[i].ToString(), colors[i]);
				}
				else
				{
					Print(text[i].ToString(), defaultColor);
				}

				XleCore.Wait(50, true, XleCore.Redraw);

				if (text[i] == '.')
					XleCore.Wait(400);
			}
		}
		public void PrintSlow(string text, Color[] colors = null)
		{
			PrintSlowImpl(text, XleCore.Renderer.FontColor, colors);
		}
		public void PrintSlow(string text, Color color)
		{
			PrintSlowImpl(text, color, null);
		}
		public void PrintLineSlow(string text = "", Color[] colors = null)
		{
			PrintSlow(text, colors);
			PrintLine();
		}
		public void PrintLineSlow(string text, Color color)
		{
			PrintSlow(text, color);
			PrintLine();
		}

		public void Clear(bool cursorAtTop = false)
		{
			foreach (var line in lines)
			{
				line.Text = "";
				line.SetColor(XleCore.Renderer.FontColor);
			}

			cursor.X = margin;
			cursor.Y = 4;

			if (cursorAtTop)
				cursor.Y = 0;
		}

		/// <summary>
		/// Flashes lines of text on the screen.
		/// </summary>
		/// <param name="howLong">How many milliseconds to flash for.</param>
		/// <param name="color">The color to flash to.</param>
		/// <param name="lines">Which lines. Don't pass any extra parameters to flash the whole text area.</param>
		public void FlashLines(int howLong, Color color, int flashRate, params int[] lines)
		{
			if (lines == null || lines.Length == 0)
			{
				FlashLines(howLong, color, flashRate, 0, 1, 2, 3, 4);
				return;
			}
			if (flashRate == 0)
				throw new ArgumentOutOfRangeException("flashRate must be positive.");

			Stopwatch watch = new Stopwatch();
			watch.Start();

			FlashLinesWhile(() => watch.ElapsedMilliseconds < howLong, XleCore.Renderer.FontColor, color, flashRate, lines);
		}

		public void FlashLinesWhile(Func<bool> pred, Color color1, Color color2, int flashRate, params int[] lines)
		{
			if (lines == null || lines.Length == 0)
			{
				FlashLinesWhile(pred, color1, color2, flashRate, 0, 1, 2, 3, 4);
				return;
			}
			if (flashRate == 0)
				throw new ArgumentOutOfRangeException("flashRate must be positive.");

			Stopwatch watch = new Stopwatch();
			watch.Start();

			while (pred())
			{
				int index = (int)watch.ElapsedMilliseconds % flashRate / (flashRate / 2);

				Color clr = color2;

				if (index == 1)
					clr = color1;

				foreach (var line in lines)
				{
					this.lines[line].SetColor(clr);
				}

				XleCore.Redraw();

				if (watch.ElapsedMilliseconds > 10000)
					break;
			}

			foreach (var line in lines)
			{
				this.lines[line].SetColor(color1);
			}
		}


		public void SetLineColor(Color color, params int[] lines)
		{
			if (lines.Length == 0)
				SetLineColor(color, 0, 1, 2, 3, 4);

			foreach (var line in lines)
			{
				this.lines[line].SetColor(color);
			}
		}

		public void SetCharacterColor(int line, int x, Color color)
		{
			lines[line].Colors[x] = color;
		}
	}
}
