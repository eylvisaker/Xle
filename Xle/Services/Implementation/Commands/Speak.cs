namespace ERY.Xle.Services.Implementation.Commands
{
    public class Speak : Command
    {
        public override void Execute()
        {
            if (GameState.MapExtender.PlayerSpeak(GameState) == false)
            {
                TextArea.PrintLine();
                TextArea.PrintLine();
                TextArea.PrintLine("No response.");
            }
        }
    }
}
