namespace ERY.Xle.Services
{
    public interface IXleStartup : IXleService
    {
        void ProcessArguments(string[] args);

        void Run();
    }
}
