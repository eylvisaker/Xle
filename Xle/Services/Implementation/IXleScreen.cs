using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Services.Implementation
{
    public interface IXleScreen : IXleService
    {
        void RunRedrawLoop();
    }
}
