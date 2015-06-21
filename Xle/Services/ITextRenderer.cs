using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Services
{
    public interface ITextRenderer : IXleService
    {
        void WriteText(int x, int y, string text, AgateLib.Geometry.Color[] color);

        void WriteText(int px, int py, string theText);

        void WriteText(int px, int py, string theText, AgateLib.Geometry.Color c);
    }
}
