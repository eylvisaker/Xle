using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle
{
    public class XleSystemState :IXleService
    {
        public IXleGameFactory Factory { get; set; }
        public Size WindowBorderSize { get; set; }

        public bool ReturnToTitle { get; set; }

        public Data.XleData Data { get; set; }
    }
}
