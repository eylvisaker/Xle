using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace ERY.Xle.Services.Commands
{
    public interface ICommandList : IXleService
    {
        List<ICommand> Items { get; }

        bool IsLeftMenuActive { get; }

        ICommand CurrentCommand { get; set; }

        void ResetCurrentCommand();

        ICommand FindCommand(Keys cmd);
    }
}
