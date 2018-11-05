using System.Threading.Tasks;

namespace Xle.Commands.Implementation
{
    public interface IClimb : ICommand { }

    public abstract class Climb : Command, IClimb
    {
        public override string Name
        {
            get { return "Climb"; }
        }

        protected Task FailMessage() => TextArea.PrintLine("\n\nNothing to climb");
    }
}
