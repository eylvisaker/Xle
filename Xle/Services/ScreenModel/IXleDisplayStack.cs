using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Services.ScreenModel
{
	public interface IXleDisplayStack : IXleService
	{
		Action TopDisplay { get; }

		void Push(Action action);

		void Pop();
	}
}
