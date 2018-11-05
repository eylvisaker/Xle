namespace Xle.Game
{
    public interface ITitleScreenFactory : IXleFactory
    {
        IXleTitleScreen CreateTitleScreen();
    }
}
