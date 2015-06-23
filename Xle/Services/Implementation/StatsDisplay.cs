using AgateLib.Geometry;
using ERY.Xle.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Services.Implementation
{
    public class StatsDisplay : IStatsDisplay
    {
        private IXleRenderer renderer;

        public StatsDisplay(IXleRenderer renderer)
        {
            this.renderer = renderer;
        }

        public void FlashHPWhileSound(Color color1, Color? color2 = null)
        {
            renderer.FlashHPWhileSound(color1, color2);
        }
    }
}
