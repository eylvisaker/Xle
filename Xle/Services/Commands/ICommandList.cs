using System.Collections.Generic;

using AgateLib.InputLib;

namespace ERY.Xle.Services.Commands
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
