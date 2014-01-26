using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERY.Xle.LoB
{
	static class LoBProgram
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			XleCore core = new XleCore();

			System.IO.Directory.SetCurrentDirectory("LoB");

			core.Run();
			
		}
	}
}
