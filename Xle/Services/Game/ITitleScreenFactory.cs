namespace ERY.Xle.Services.Game
{
    public interface ITitleScreenFactory : IXleFactory
    {
        IXleTitleScreen CreateTitleScreen();
    }
}
