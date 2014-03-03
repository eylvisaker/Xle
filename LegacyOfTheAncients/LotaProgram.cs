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

		public static IEnumerable<Commands.Command> CommonLotaCommands
		{
			get
			{
				yield return new Commands.Armor();
				yield return new Commands.Fight();
				yield return new Commands.Gamespeed();
				yield return new Commands.Hold();
				yield return new Commands.Inventory();
				yield return new Commands.Pass();
				yield return new Commands.Use { ShowItemMenu = false };
				yield return new Commands.Weapon();
				yield return new Commands.Xamine();
			}
		}
	}
}
