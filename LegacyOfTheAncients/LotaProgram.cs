using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ERY.Xle.LotA
{
	static class LotaProgram
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			XleCore core = new XleCore();

			System.IO.Directory.SetCurrentDirectory("LotA");

			core.Run(new LotaFactory());
		}
	}
}
