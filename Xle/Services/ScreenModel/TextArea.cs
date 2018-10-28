using AgateLib;
using Xle.Services.Game;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Xle.Services.ScreenModel
{
    public interface ITextArea
    {
        Point CursorLocation { get; }

        void Clear(bool setCursorAtTop = false);

        Task PrintLine(string text, Color color);
        Task PrintLine(string text = "", Color[] colors = null);

        Task Print(string text, Color? color);
        Task Print(string text = "", Color[] colors = null);

        Task PrintSlow(string text, Color color);
        Task PrintSlow(string text, Color[] colors = null);

        Task RewriteLine(int line, string text, Color? color = null);

        Task PrintLineSlow(string text, Color color);
        Task PrintLineSlow(string text = "", Color[] colors = null);

        Task FlashLines(int howLong, Color color, int flashRate, params int[] lines);
        Task FlashLinesWhile(Func<bool> pred, Color color1, Color color2, int flashRate, params int[] lines);

        void SetLineColor(Color color, params int[] lines);

        void SetCharacterColor(int p1, int p2, Color color);

        int Margin { get; set; }
        Color DefaultColor { get; }

        Func<int, bool, IRenderer, Task> Waiter { get; set; }

        Task PrintLineCentered(string p, Color color);

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

        public TextArea(
            IXleScreen screen,
            GameState gameState)
        {
            this.gameState = gameState;
            this.screen = screen;

            for (int i = 0; i < lines.Length; i++)
                lines[i] = new TextLine(this);
        }

        public Func<int, bool, IRenderer, Task> Waiter { get; set; }

        public int Margin { get { return margin; } set { margin = value; } }

        public Color DefaultColor
        {
            get { return gameState.Map.ColorScheme.TextColor; }
        }

        private Task WaitAsync(int howLong, bool keyBreak = false, IRenderer redraw = null) => Waiter?.Invoke(howLong, keyBreak, redraw);

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

        private async Task PrintImpl(string text, Color[] colors)
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

            await WaitAsync(1);
        }

        private async Task PrintSlowImpl(string text, Color defaultColor, Color[] colors = null)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (colors != null)
                {
                    await Print(text[i].ToString(), colors[i]);
                }
                else
                {
                    await Print(text[i].ToString(), defaultColor);
                }

                await WaitAsync(50, keyBreak: true);

                if (text[i] == '.' || text[i] == '!')
                    await WaitAsync(500);
                if (text[i] == ',')
                    await WaitAsync(350);
            }
        }

        public async Task Print(string text, Color? color)
        {
            if (tempColors == null || tempColors.Length < text.Length)
            {
                tempColors = new Color[text.Length];
            }

            for (int i = 0; i < tempColors.Length; i++)
            {
                tempColors[i] = color ?? DefaultColor;
            }

            await PrintImpl(text, tempColors);
        }

        public async Task Print(string text = "", Color[] colors = null)
        {
            await PrintImpl(text, colors);
        }

        public async Task PrintLine(string text, Color color)
        {
            await Print(text + "\n", color);
        }
        public async Task PrintLine(string text = "", Color[] colors = null)
        {
            await Print(text + "\n", colors);
        }

        public Task PrintSlow(string text, Color[] colors = null)
        {
            return PrintSlowImpl(text, screen.FontColor, colors);
        }

        public Task PrintSlow(string text, Color color)
        {
            return PrintSlowImpl(text, color, null);
        }

        public async Task PrintLineSlow(string text = "", Color[] colors = null)
        {
            await PrintSlow(text, colors);
            await PrintLine();
        }

        public async Task PrintLineSlow(string text, Color color)
        {
            await PrintSlow(text, color);
            await PrintLine();
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

        public async Task RewriteLine(int line, string text, Color? color = null)
        {
            cursor.X = margin;
            cursor.Y = line;

            await Print(text, color);
        }

        /// <summary>
        /// Flashes lines of text on the screen.
        /// </summary>
        /// <param name="howLong">How many milliseconds to flash for.</param>
        /// <param name="color">The color to flash to.</param>
        /// <param name="lines">Which lines. Don't pass any extra parameters to flash the whole text area.</param>
        public async Task FlashLines(int howLong, Color color, int flashRate, params int[] lines)
        {
            if (lines == null || lines.Length == 0)
            {
                await FlashLines(howLong, color, flashRate, 0, 1, 2, 3, 4);
                return;
            }
            if (flashRate == 0)
                throw new ArgumentOutOfRangeException("flashRate", "flashRate must be positive.");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            await FlashLinesWhile(() => watch.ElapsedMilliseconds < howLong, screen.FontColor, color, flashRate, lines);
        }

        public async Task FlashLinesWhile(Func<bool> pred, Color color1, Color color2, int flashRate, params int[] lines)
        {
            if (lines == null || lines.Length == 0)
            {
               await FlashLinesWhile(pred, color1, color2, flashRate, 0, 1, 2, 3, 4);
                return;
            }

            if (flashRate == 0)
                throw new ArgumentOutOfRangeException("flashRate", "flashRate must be positive.");

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

                await Task.Yield();

                if (watch.ElapsedMilliseconds > 10000)
                    break;
            }

            foreach (var line in lines)
            {
                this.lines[line].SetColor(color1);
            }
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

        public async Task PrintLineCentered(string text, Color color)
        {
            text = new string(' ', 19 - text.Length / 2) + text;

            await PrintLine(text, color);
        }


        public TextLine GetLine(int i)
        {
            return lines[i];
        }
    }
}
