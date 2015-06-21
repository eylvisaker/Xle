using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Services
{
    public interface IStatsDisplay : IXleService
    {
        void FlashHPWhileSound(Color color1, Color? color2 = null);
    }
}
