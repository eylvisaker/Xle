using AgateLib.Geometry;
using AgateLib.Geometry.CoordinateSystems;
using AgateLib.Platform.WinForms;
using AgateLib.Utility;
using Castle.Windsor;
using Castle.Windsor.Installer;
using ERY.Xle.Bootstrap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using ERY.Xle.Services;
using ERY.Xle.Services.Commands;
using ERY.Xle.Services.XleSystem;

namespace ERY.Xle.LotA
{
	static class LotaProgram
	{
		private static ICommandFactory commandFactory;
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			RunGame(args);
		}

		private static void RunGame(string[] args)
		{
			using (var setup = new AgateSetup(args))
			{
				setup.ApplicationName = "Legacy of the Ancients";
				setup.AssetLocations.Path = "LotA";
				setup.AssetLocations.Sound = "Audio";
				setup.AssetLocations.Surfaces = "Images";
				setup.DesiredDisplayWindowResolution = new Size(680, 440);
				setup.DisplayWindowExpansionType = AgateLib.Configuration.WindowExpansionType.Scale;
				setup.InitializeAgateLib();

				GameRunner(args);
			}
		}

		private static void GameRunner(string[] args)
		{
			var initializer = new WindsorInitializer();
			var container = initializer.BootstrapContainer(typeof(LotaProgram).Assembly);

			IXleStartup core = container.Resolve<IXleStartup>();
			core.ProcessArguments(args);
			commandFactory = container.Resolve<ICommandFactory>();

			core.Run();
		}

		public static IEnumerable<Command> CommonLotaCommands
		{
			get
			{
				yield return commandFactory.Armor();
				yield return commandFactory.Gamespeed();
				yield return commandFactory.Hold();
				yield return commandFactory.Inventory();
				yield return commandFactory.Pass();
				yield return commandFactory.Weapon();
			}
		}
	}
}
