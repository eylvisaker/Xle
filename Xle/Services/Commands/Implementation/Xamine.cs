namespace Xle.Services.Commands.Implementation
{
    public class Xamine : Command
    {
        public override string Name
        {
            get { return "Xamine"; }
        }

        public override void Execute()
        {
            TextArea.PrintLine();
            TextArea.PrintLine();
            TextArea.PrintLine("You are in " + GameState.Map.MapName + ".");
            TextArea.PrintLine("Look about to see more.");
        }
    }
}
