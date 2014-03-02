using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Commands
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
