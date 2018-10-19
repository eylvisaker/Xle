using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.Services.ScreenModel
{
	public class XleDisplay : IDisposable
	{
		private IXleDisplayStack DisplayStack;
		private bool disposed;

		public XleDisplay(IXleDisplayStack displayStack, Action action)
		{
			this.DisplayStack = displayStack;

			this.Action = action;

			displayStack.Push(action);
		}

		public void Dispose()
		{
			if (disposed)
				return;

			DisplayStack.Pop();

			disposed = true;
		}

		public Action Action { get; private set; }
	}
}
