using System.Collections.Generic;

using AgateLib.InputLib;

using ERY.Xle.Services.Implementation.Commands;

namespace ERY.Xle.Services
{
    public interface ICommandList : IXleService
    {
        List<Command> Items { get; }

        bool IsLeftMenuActive { get; }

        Command CurrentCommand { get; set; }

        void ResetCurrentCommand();

        Command FindCommand(KeyCode cmd);
    }
}
