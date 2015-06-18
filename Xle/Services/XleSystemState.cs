using System;

using AgateLib.Geometry;

namespace ERY.Xle.Services
{
    public class XleSystemState : IXleService
    {
        public IXleGameFactory Factory { get; set; }
        public Size WindowBorderSize { get; set; }

        public bool ReturnToTitle { get; set; }

        [Obsolete("Use XleData as a service instead.")]
        public Data.XleData Data { get; set; }
    }
}
