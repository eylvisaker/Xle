using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Services
{
    public interface IXleInput : IXleService
    {
        bool AcceptKey { get; set; }

        void CheckArrowKeys();
    }
}
