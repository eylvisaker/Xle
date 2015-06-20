namespace ERY.Xle.Services.Implementation.Commands
{
    public class Pass : Command
    {
        public override void Execute(GameState state)
        {
            TextArea.PrintLine();
        }
    }
}
