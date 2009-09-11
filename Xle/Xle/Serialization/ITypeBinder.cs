using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Serialization
{
	public interface ITypeBinder
	{
		Type GetType(string typename);
	}
}
