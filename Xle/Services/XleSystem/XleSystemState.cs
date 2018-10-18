using AgateLib;
using AgateLib.Mathematics.Geometry;
using ERY.Xle.Services.Game;
using System;

namespace ERY.Xle.Services.XleSystem
{
    [Singleton]
    public class XleSystemState
    {
        public IXleGameFactory Factory { get; set; }
        public Size WindowBorderSize { get; set; }

        public bool ReturnToTitle { get; set; }

        [Obsolete("Use XleData as a service instead.")]
        public Data.XleData Data { get; set; }
    }
}
