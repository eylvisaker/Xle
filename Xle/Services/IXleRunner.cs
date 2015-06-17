namespace ERY.Xle.Services
{
    public interface IXleRunner : IXleService
    {
        void Run(IXleGameFactory gameFactory);
    }
}
