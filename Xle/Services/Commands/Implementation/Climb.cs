namespace Xle.Services.Commands.Implementation
{
    public abstract class Climb : Command
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
