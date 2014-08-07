using AgateLib.ApplicationModels.CoordinateSystems;
using AgateLib.Geometry;
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
			var parameters = new SerialModelParameters(args);

			parameters.AssetPath = "LoB";
			parameters.AssetLocations.Sound = "Audio";
			parameters.AssetLocations.Surfaces = "Images";
			parameters.CoordinateSystem = new FixedAreaCoordinates
			{
				MinHeight = 440,
				MaxHeight = 440,
				MinWidth = 680,
				MaxWidth = 680,
				Origin = new Point(-20, -20),
			};

			var model = new SerialModel(parameters);

			model.Run(() =>
			{
				XleCore core = new XleCore();
				core.ProcessArguments(args);

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
