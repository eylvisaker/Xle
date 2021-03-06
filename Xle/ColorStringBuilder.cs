using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Xle
{
    public class ColorStringBuilder
    {
        private string text;
        private List<Color> colors = new List<Color>();

        public void AddText(string text, Color color)
        {
            if (string.IsNullOrEmpty(text))
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
            set
            {
                text = value;

                while (colors.Count < text.Length)
                {
                    colors.Add(XleColor.White);
                }
                while (colors.Count > text.Length)
                {
                    colors.RemoveAt(colors.Count - 1);
                }
            }
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
