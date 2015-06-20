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
using ERY.Xle.Services.Implementation.Commands;
using ERY.Xle.Services.Implementation;

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
			var parameters = new SerialModelParameters(args);

			parameters.ApplicationName = "Legend of Blacksilver";
			parameters.AssetLocations.Path = "LoB";
			parameters.AssetLocations.Sound = "Audio";
			parameters.AssetLocations.Surfaces = "Images";
			parameters.CoordinateSystem = new FixedAspectRatioCoordinates
			{
				MinHeight = 440,
				MaxHeight = 440,
				MinWidth = 680,
				MaxWidth = 680,
				AspectRatio = 680.0 / 440.0,
				Origin = new Point(-20, -20),
			};

			var model = new SerialModel(parameters);

			model.Run(() =>
			{
                var initializer = new WindsorInitializer();
                var container = initializer.BootstrapContainer(typeof(LobProgram).Assembly);

                IXleStartup core = container.Resolve<IXleStartup>();
                core.ProcessArguments(args);
                commandFactory = container.Resolve<ICommandFactory>();

                core.Run();
			});
		}

		public static IEnumerable<Command> CommonLobCommands
		{
			get
			{
                yield return commandFactory.Armor();
                yield return commandFactory.Fight();
				yield return commandFactory.Gamespeed();
				yield return commandFactory.Inventory();
				yield return commandFactory.Pass();
				yield return commandFactory.Use();
				yield return commandFactory.Weapon();
				yield return commandFactory.Xamine();
			}
		}
	}
}
