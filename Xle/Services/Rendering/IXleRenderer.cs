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
        Action ReplacementDrawMethod { get; set; }

        void Draw();

        void UpdateAnim();

        void LoadTiles(string tileset);

        void DrawFrame(Color color);

        void DrawFrameHighlight(Color color);

        void DrawInnerFrameHighlight(int p1, int p2, int p3, int p4, Color color);

        void DrawFrameLine(int p1, int p2, int p3, int p4, Color color);

        void DrawObject(TextWindow wind);

        void SetProjectionAndBackColors(ColorScheme colorScheme);

        void DrawObject(ColorScheme mColorScheme);

        void FlashHPWhile(Color color1, Color color2, Func<bool> func);

        void FlashHPWhileSound(Color clr, Color? clr2 = null);

    }
}
