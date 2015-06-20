using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Services
{
    public interface INumberPicker : IXleService
    {
        int ChooseNumber(int max);
        int ChooseNumber(Action redraw, int max);
    }
}
