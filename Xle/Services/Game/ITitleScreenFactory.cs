namespace Xle.Services.Game
{
    public interface ITitleScreenFactory : IXleFactory
    {
        IXleTitleScreen CreateTitleScreen();
    }
}
