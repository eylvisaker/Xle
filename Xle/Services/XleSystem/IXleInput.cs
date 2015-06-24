using System;

using AgateLib.InputLib;

namespace ERY.Xle.Services.XleSystem
{
    public interface IXleInput : IXleService
    {
        bool AcceptKey { get; set; }

        KeyCode WaitForKey(params KeyCode[] keys);
        KeyCode WaitForKey(Action redraw, params KeyCode[] keys);

        bool PromptToContinueOnWait { get; set; }
    }
}
