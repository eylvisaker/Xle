using ERY.Xle.Services.Game;

namespace ERY.Xle.Services.XleSystem
{
    public interface IXleRunner : IXleService
    {
        void Run(IXleGameFactory gameFactory);
    }
}
