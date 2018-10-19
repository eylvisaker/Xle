using Microsoft.Xna.Framework.Input;
using System;

namespace Xle.Services.XleSystem
{
    public interface IXleInput : IXleService
    {
        bool AcceptKey { get; set; }

        Keys WaitForKey(params Keys[] keys);
        Keys WaitForKey(Action redraw, params Keys[] keys);

        bool PromptToContinueOnWait { get; set; }

        event EventHandler<CommandEventArgs> DoCommand;
    }
}
