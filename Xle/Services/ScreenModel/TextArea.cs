using AgateLib;
using ERY.Xle.Services.Game;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;

namespace ERY.Xle.Services.ScreenModel
{
    public interface ITextArea
    {
        Point CursorLocation { get; }

        void Clear(bool setCursorAtTop = false);

        void PrintLine(string text, Color color);
        void PrintLine(string text = "", Color[] colors = null);

        void Print(string text, Color? color);
        void Print(string text = "", Color[] colors = null);

        void PrintSlow(string text, Color color);
        void PrintSlow(string text, Color[] colors = null);

        void RewriteLine(int line, string text, Color? color = null);

        void PrintLineSlow(string text, Color color);
        void PrintLineSlow(string text = "", Color[] colors = null);

        void FlashLines(int howLong, Color color, int flashRate, params int[] lines);
        void FlashLinesWhile(Func<bool> pred, Color color1, Color color2, int flashRate, params int[] lines);

        void SetLineColor(Color color, params int[] lines);

        void SetCharacterColor(int p1, int p2, Color color);

        int Margin { get; set; }
        Color DefaultColor { get; }

        void PrintLineCentered(string p, Color color);

        TextLine GetLine(int i);
    }

    [Singleton]
    public class TextArea : ITextArea
    {
        private TextLine[] lines = new TextLine[5];
        private Point cursor = new Point(1, 5);
        private int margin = 1;
        private Color[] tempColors;

        private IXleScreen screen;
        private GameState gameState;
        private IXleGameControl gameControl;

        public TextArea(
            IXleGameControl gameControl,
            IXleScreen screen,
            GameState gameState)
        {
            this.gameState = gameState;
            this.screen = screen;
            this.gameControl = gameControl;

            for (int i = 0; i < lines.Length; i++)
                lines[i] = new TextLine(this);
        }

        public int Margin { get { return margin; } set { margin = value; } }

        public Color DefaultColor
        {
            get { return gameState.Map.ColorScheme.TextColor; }
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

            gameControl.Wait(1);
        }

        private void PrintSlowImpl(string text, Color defaultColor, Color[] colors = null)
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

                gameControl.Wait(50, keyBreak: true);

                if (text[i] == '.' || text[i] == '!')
                    gameControl.Wait(500);
                if (text[i] == ',')
                    gameControl.Wait(350);
            }
        }

        public void Print(string text, Color? color)
        {
            if (tempColors == null || tempColors.Length < text.Length)
            {
                tempColors = new Color[text.Length];
            }

            for (int i = 0; i < tempColors.Length; i++)
            {
                tempColors[i] = color ?? DefaultColor;
            }

            PrintImpl(text, tempColors);
        }
        public void Print(string text = "", Color[] colors = null)
        {
            PrintImpl(text, colors);
        }

        public void PrintLine(string text, Color color)
        {
            Print(text + "\n", color);
        }
        public void PrintLine(string text = "", Color[] colors = null)
        {
            Print(text + "\n", colors);
        }

        public void PrintSlow(string text, Color[] colors = null)
        {
            PrintSlowImpl(text, screen.FontColor, colors);
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
                line.SetColor(screen.FontColor);
            }

            cursor.X = margin;
            cursor.Y = 4;

            if (cursorAtTop)
                cursor.Y = 0;
        }

        public void RewriteLine(int line, string text, Color? color = null)
        {
            cursor.X = margin;
            cursor.Y = line;

            Print(text, color);
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
                throw new ArgumentOutOfRangeException("flashRate", "flashRate must be positive.");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            FlashLinesWhile(() => watch.ElapsedMilliseconds < howLong, screen.FontColor, color, flashRate, lines);
        }

        public void FlashLinesWhile(Func<bool> pred, Color color1, Color color2, int flashRate, params int[] lines)
        {
            throw new NotImplementedException();
            //if (lines == null || lines.Length == 0)
            //{
            //    FlashLinesWhile(pred, color1, color2, flashRate, 0, 1, 2, 3, 4);
            //    return;
            //}
            //if (flashRate == 0)
            //    throw new ArgumentOutOfRangeException("flashRate", "flashRate must be positive.");

            //Stopwatch watch = new Stopwatch();
            //watch.Start();

            //while (pred())
            //{
            //    int index = (int)watch.ElapsedMilliseconds % flashRate / (flashRate / 2);

            //    Color clr = color2;

            //    if (index == 1)
            //        clr = color1;

            //    foreach (var line in lines)
            //    {
            //        this.lines[line].SetColor(clr);
            //    }

            //    gameControl.Redraw(time);

            //    if (watch.ElapsedMilliseconds > 10000)
            //        break;
            //}

            //foreach (var line in lines)
            //{
            //    this.lines[line].SetColor(color1);
            //}
        }

        public Point CursorLocation { get { return cursor; } }

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

        public void PrintLineCentered(string text, Color color)
        {
            text = new string(' ', 19 - text.Length / 2) + text;

            PrintLine(text, color);
        }


        public TextLine GetLine(int i)
        {
            return lines[i];
        }
    }
}
