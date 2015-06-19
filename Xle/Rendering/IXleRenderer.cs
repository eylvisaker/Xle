using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgateLib.Geometry;

using ERY.Xle.Services;

namespace ERY.Xle.Rendering
{
    public interface IXleRenderer : IXleService
    {
        Color FontColor { get; set; }

        Rectangle Coordinates { get; }
        Action ReplacementDrawMethod { get; set; }

        void Draw();

        void UpdateAnim();

        void WriteText(int x, int y, string text, Color[] color);
        void WriteText(int x, int y, string text, Color textColor);

        void LoadTiles(string tileset);


        void DrawFrame(Color color);

        void DrawFrameHighlight(Color color);

        void WriteText(int p1, int p2, string p3);

        void DrawInnerFrameHighlight(int p1, int p2, int p3, int p4, Color color);

        void DrawFrameLine(int p1, int p2, int p3, int p4, Color color);

        void DrawObject(TextWindow wind);
    }
}
