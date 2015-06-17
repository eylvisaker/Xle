namespace ERY.Xle.Services.Implementation.Commands
{
	public abstract class Command
	{
		public virtual string Name
		{
			get
			{
				return GetType().Name;
			}
		}

		public abstract void Execute(GameState state);
	}
}
