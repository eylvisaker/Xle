namespace Xle.Game
{
	public interface IXleTitleScreen : IXleService
	{
		Player Player { get; }

		void Run();
	}
}
