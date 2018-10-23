namespace Xle.Services.Commands.Implementation
{
    public interface IClimb : ICommand { }

    public abstract class Climb : Command, IClimb
    {
        public override string Name
        {
            get { return "Climb"; }
        }

        protected void FailMessage()
        {
            TextArea.PrintLine("\n\nNothing to climb");
        }
    }
}
