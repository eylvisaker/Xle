using System.Collections.Generic;

using AgateLib.InputLib;

using ERY.Xle.Services.Implementation.Commands;

namespace ERY.Xle.Services
{
    public interface ICommandList : IXleService
    {
        void Prompt();

        void DoCommand(KeyCode keyCode);


        List<Command> Items { get; }

        void ResetCurrentCommand();

        bool IsLeftMenuActive { get; }

        Command CurrentCommand { get; }
    }
}
