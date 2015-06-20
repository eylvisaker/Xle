namespace ERY.Xle.Services.Implementation.Commands
{
    public class Hold : Command
    {
        private Use use;

        public Hold(Use use)
        {
            this.use = use;
        }

        public override void Execute(GameState state)
        {
            use.ChooseHeldItem(state);
        }
    }
}
