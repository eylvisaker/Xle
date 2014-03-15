using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle
{
	[Serializable]
	public class MainWindowClosedException : Exception
	{
		public MainWindowClosedException() { }
		public MainWindowClosedException(string message) : base(message) { }
		public MainWindowClosedException(string message, Exception inner) : base(message, inner) { }
		protected MainWindowClosedException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}
