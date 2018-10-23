using AgateLib;

namespace Xle.Services.Commands.Implementation
{
    [Transient]
    public class Pass : Command
    {
        public override void Execute()
        {
            TextArea.PrintLine();
        }
    }
}
