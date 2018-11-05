using Microsoft.Xna.Framework;

namespace Xle.ScreenModel
{

    public class TextLine
    {
        private readonly ITextArea parent;

        public string Text { get; set; }
        public Color[] Colors { get; set; }

        public TextLine(ITextArea parent)
        {
            this.parent = parent;

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
                for (int i = 0; i < t.Length; i++)
                {
                    if (x + i + colorStartIndex >= Colors.Length)
                        break;

                    Colors[x + i] = newColors[i + colorStartIndex];
                }
            }
            else
            {
                WriteColors(x, t.Length, parent.DefaultColor);
            }
        }

        public void WriteText(int x, string t, Color? color)
        {
            SetTextLength(x);

            Text += t;

            WriteColors(x, t.Length, color ?? parent.DefaultColor);
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

}
