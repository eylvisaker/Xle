using AgateLib.Geometry;
using AgateLib.Geometry.CoordinateSystems;
using AgateLib.Platform.WinForms;
using ERY.Xle.Bootstrap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgateLib;
using AgateLib.DisplayLib;
using ERY.Xle.Services;
using ERY.Xle.Services.Commands;
using ERY.Xle.Services.XleSystem;

namespace ERY.Xle.LoB
{
	static class LobProgram
	{
		private static ICommandFactory commandFactory;
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			using (AgateWinForms.Initialize(args))
			using (new DisplayWindowBuilder(args)
				.BackbufferSize(680, 440)
				.Title("Legend of Blacksilver")
				.Build())
			{
				AgateApp.SetAssetPath("LoB");

				var initializer = new WindsorInitializer();
				var container = initializer.BootstrapContainer(typeof(LobProgram).Assembly);

				IXleStartup core = container.Resolve<IXleStartup>();
				core.ProcessArguments(args);
				commandFactory = container.Resolve<ICommandFactory>();

				core.Run();
			}
		}

		public static IEnumerable<Command> CommonLobCommands
		{
			get
			{
				yield return commandFactory.Armor();
				yield return commandFactory.Gamespeed();
				yield return commandFactory.Inventory();
				yield return commandFactory.Pass();
				yield return commandFactory.Weapon();
			}
		}
	}
}
