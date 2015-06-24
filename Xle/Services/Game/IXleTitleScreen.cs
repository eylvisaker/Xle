namespace ERY.Xle.Services.Game
{
	public interface IXleTitleScreen : IXleService
	{
		Player Player { get; }
		void Run();
	}
}
