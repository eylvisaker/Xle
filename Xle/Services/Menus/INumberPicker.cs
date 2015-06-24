using System;

namespace ERY.Xle.Services.Menus
{
    public interface INumberPicker : IXleService
    {
        int ChooseNumber(int max);
        int ChooseNumber(Action redraw, int max);
    }
}
