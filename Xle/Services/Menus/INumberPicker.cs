using System;

namespace Xle.Services.Menus
{
    public interface INumberPicker : IXleService
    {
        int ChooseNumber(int max);
        int ChooseNumber(Action redraw, int max);
    }
}
