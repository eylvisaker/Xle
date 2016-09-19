using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Services.ScreenModel.Implementation
{
	class XleDisplayStack : IXleDisplayStack
	{
		private Stack<Action> displays = new Stack<Action>();
		 
		public Action TopDisplay => displays.Peek();

		public void Push(Action action)
		{
			displays.Push(action);
		}

		public void Pop()
		{
			displays.Pop();
		}
	}
}
