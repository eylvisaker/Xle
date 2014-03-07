using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERY.Xle.LoB
{
	static class LobProgram
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			XleCore core = new XleCore();
			core.ProcessArguments(args);

			System.IO.Directory.SetCurrentDirectory("LoB");

			core.Run(new LobFactory());
		}

		public static IEnumerable<Commands.Command> CommonLobCommands
		{
			get
			{
				yield return new Commands.Armor();
				yield return new Commands.Fight();
				yield return new Commands.Gamespeed();
				yield return new Commands.Inventory();
				yield return new Commands.Pass();
				yield return new Commands.Use();
				yield return new Commands.Weapon();
				yield return new Commands.Xamine();
			}
		}
	}
}
