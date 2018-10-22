namespace Xle.Services.XleSystem
{
    public interface IXleStartup : IXleService
    {
        void ProcessArguments(string[] args);
    }
}
