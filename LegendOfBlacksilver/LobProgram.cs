using AgateLib.ApplicationModels;
using AgateLib.Geometry;
using AgateLib.Geometry.CoordinateSystems;
using AgateLib.Platform.WinForms;
using AgateLib.Platform.WinForms.ApplicationModels;
using ERY.Xle.Bootstrap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
			using (var setup = new AgateSetupWinForms(args))
			{
				setup.ApplicationName = "Legend of Blacksilver";
				setup.AssetLocations.Path = "LoB";
				setup.AssetLocations.Sound = "Audio";
				setup.AssetLocations.Surfaces = "Images";
				setup.DesiredDisplayWindowResolution = new Size(680, 440);
				setup.DisplayWindowExpansionType = AgateLib.Configuration.WindowExpansionType.Scale;

				setup.InitializeAgateLib();

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
