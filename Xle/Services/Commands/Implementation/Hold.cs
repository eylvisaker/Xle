namespace ERY.Xle.Services.Commands.Implementation
{
    public class Hold : Command
    {
        private Use use;

        public Hold(Use use)
        {
            this.use = use;
        }

        public override void Execute()
        {
            use.ChooseHeldItem();
        }
    }
}
