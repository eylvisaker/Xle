using AgateLib;
using System.Threading.Tasks;

namespace Xle.Services.Commands.Implementation
{
    public interface IXamine : ICommand { }

    [Transient("Xamine")]
    public class Xamine : Command, IXamine
    {
        public override string Name
        {
            get { return "Xamine"; }
        }

        public override async Task Execute()
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine();
            await TextArea.PrintLine("You are in " + GameState.Map.MapName + ".");
            await TextArea.PrintLine("Look about to see more.");
        }
    }
}
