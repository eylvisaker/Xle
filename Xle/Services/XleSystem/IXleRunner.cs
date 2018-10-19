using Xle.Services.Game;

namespace Xle.Services.XleSystem
{
    public interface IXleRunner : IXleService
    {
        void Run(IXleGameFactory gameFactory);
    }
}
