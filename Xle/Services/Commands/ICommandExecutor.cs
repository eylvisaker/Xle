using Microsoft.Xna.Framework.Input;

namespace Xle.Services.Commands
{
    public interface ICommandExecutor : IXleService
    {
        void Prompt();

        void DoCommand(Keys Keys);

        void ResetCurrentCommand();
    }
}
