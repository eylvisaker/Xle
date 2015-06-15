using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle
{
    public interface ITitleScreenFactory : IXleFactory
    {
        IXleTitleScreen CreateTitleScreen();
    }
}
