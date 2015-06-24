namespace ERY.Xle.Services.Implementation.Commands
{
    public class Climb : Command
    {
        public override string Name
        {
            get { return "Climb"; }
        }

        public override void Execute()
        {
            if (GameState.MapExtender.PlayerClimb(GameState) == false)
            {
                FailMessage();
            }
        }

        protected void FailMessage()
        {
            TextArea.PrintLine("\n\nNothing to climb");
        }
    }
}
