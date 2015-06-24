using AgateLib.Geometry;

using ERY.Xle.Services.Rendering;

namespace ERY.Xle.Services.ScreenModel.Implementation
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
