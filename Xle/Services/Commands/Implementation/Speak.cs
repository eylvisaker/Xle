namespace ERY.Xle.Services.Commands.Implementation
{
    public class Speak : Command
    {
        public override void Execute()
        {
            if (GameState.MapExtender.PlayerSpeak() == false)
            {
                TextArea.PrintLine();
                TextArea.PrintLine();
                TextArea.PrintLine("No response.");
            }
        }
    }
}
