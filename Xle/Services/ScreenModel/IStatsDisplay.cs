using AgateLib.Geometry;

namespace ERY.Xle.Services.ScreenModel
{
    public interface IStatsDisplay : IXleService
    {
        void FlashHPWhileSound(Color color1, Color? color2 = null);
    }
}
