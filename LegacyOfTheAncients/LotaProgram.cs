using AgateLib.ApplicationModels;
using AgateLib.ApplicationModels.CoordinateSystems;
using AgateLib.Geometry;
using AgateLib.Platform.WindowsForms;
using AgateLib.Platform.WindowsForms.ApplicationModels;
using AgateLib.Utility;
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
		static void Main(string[] args)
		{
			RunGame(args);
		}

		private static void RunGame(string[] args)
		{
			var parameters = new SerialModelParameters(args);

			parameters.AssetPath = "LotA";
			parameters.AssetLocations.Sound = "Audio";
			parameters.AssetLocations.Surfaces = "Images";
			parameters.CoordinateSystem = new FixedAspectRatioCoordinates
			{
				MinHeight = 440,
				MaxHeight = 440,
				MinWidth = 680,
				MaxWidth = 680,
				AspectRatio = 680.0/440.0,
				Origin = new Point(-20, -20),
			};

			var model = new SerialModel(parameters);

			model.Run(() =>
			{
				XleCore core = new XleCore();
				core.ProcessArguments(args);

				core.Run(new LotaFactory());
			});
		}

		public static IEnumerable<Commands.Command> CommonLotaCommands
		{
			get
			{
				yield return new Commands.ArmorCommand();
				yield return new Commands.Fight();
				yield return new Commands.Gamespeed();
				yield return new Commands.Hold();
				yield return new Commands.Inventory();
				yield return new Commands.Pass();
				yield return new Commands.Use { ShowItemMenu = false };
				yield return new Commands.WeaponCommand();
				yield return new Commands.Xamine();
			}
		}
	}
}
