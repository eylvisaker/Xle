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
    }
}
