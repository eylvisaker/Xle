using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle
{
    public interface IXleStartup : IXleService
    {
        void ProcessArguments(string[] args);

        void Run();
    }
}
