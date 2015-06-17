namespace ERY.Xle.Services
{
	public interface IXleTitleScreen : IXleService
	{
		Player Player { get; }
		void Run();
	}
}
