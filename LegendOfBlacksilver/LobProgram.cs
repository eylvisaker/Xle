using AgateLib.Platform.WindowsForms;
using AgateLib.Platform.WindowsForms.ApplicationModels;
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
			PassiveModel.Run(args, () =>
			{
				XleCore core = new XleCore();
				core.ProcessArguments(args);

				System.IO.Directory.SetCurrentDirectory("LoB");

				Configuration.Images.AddPath("Images");
				Configuration.Sounds.AddPath("Audio");

				core.Run(new LobFactory());
			});
		}

		public static IEnumerable<Commands.Command> CommonLobCommands
		{
			get
			{
				yield return new Commands.ArmorCommand();
				yield return new Commands.Fight();
				yield return new Commands.Gamespeed();
				yield return new Commands.Inventory();
				yield return new Commands.Pass();
				yield return new Commands.Use();
				yield return new Commands.WeaponCommand();
				yield return new Commands.Xamine();
			}
		}
	}
}
