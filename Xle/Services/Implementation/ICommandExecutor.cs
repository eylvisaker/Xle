using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Services.Implementation
{
    public interface ICommandExecutor : IXleService
    {
        void Prompt();

        void DoCommand(AgateLib.InputLib.KeyCode keyCode);

        void ResetCurrentCommand();
    }
}
