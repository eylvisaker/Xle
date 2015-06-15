using System;

namespace ERY.Xle
{
	public interface IXleTitleScreen : IXleService
	{
		Player Player { get; }
		void Run();
	}
}
