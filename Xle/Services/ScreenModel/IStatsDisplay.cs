using Microsoft.Xna.Framework;
using System;

namespace ERY.Xle.Services.ScreenModel
{
    public interface IStatsDisplay : IXleService
    {
        Color HPColor { get; }
        int HP { get; }
        int Gold { get; }
        int Food { get; }

        void FlashHPWhileSound(Color color1, Color? color2 = null);
        void FlashHPWhile(Color color1, Color color2, Func<bool> func);
    }
}
