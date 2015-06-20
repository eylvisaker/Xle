namespace ERY.Xle.Services.Implementation.Commands
{
    public class Speak : Command
    {
        public override void Execute(GameState state)
        {
            if (GameState.MapExtender.PlayerSpeak(state) == false)
            {
                TextArea.PrintLine();
                TextArea.PrintLine();
                TextArea.PrintLine("No response.");
            }
        }
    }
}
