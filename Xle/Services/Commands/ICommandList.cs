using System.Collections.Generic;

using AgateLib.InputLib;

namespace ERY.Xle.Services.Commands
{
    public interface ICommandList : IXleService
    {
        List<ICommand> Items { get; }

        bool IsLeftMenuActive { get; }

        ICommand CurrentCommand { get; set; }

        void ResetCurrentCommand();

        ICommand FindCommand(KeyCode cmd);
    }
}
