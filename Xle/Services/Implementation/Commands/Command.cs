namespace ERY.Xle.Services.Implementation.Commands
{
    public abstract class Command
    {
        public ITextArea TextArea { get; set; }
        public GameState GameState { get; set; }

        public virtual string Name
        {
            get
            {
                return GetType().Name;
            }
        }

        public abstract void Execute(GameState state);
    }
}
