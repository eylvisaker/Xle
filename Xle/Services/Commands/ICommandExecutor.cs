using AgateLib.InputLib;

namespace ERY.Xle.Services.Commands
{
    public interface ICommandExecutor : IXleService
    {
        void Prompt();

        void DoCommand(KeyCode keyCode);

        void ResetCurrentCommand();
    }
}
