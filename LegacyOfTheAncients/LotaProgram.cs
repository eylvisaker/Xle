using AgateLib.ApplicationModels;
using AgateLib.Geometry;
using AgateLib.Geometry.CoordinateSystems;
using AgateLib.Platform.WinForms;
using AgateLib.Platform.WinForms.ApplicationModels;
using AgateLib.Utility;
using Castle.Windsor;
using Castle.Windsor.Installer;
using ERY.Xle.Bootstrap;
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

            parameters.ApplicationName = "Legacy of the Ancients";
            parameters.AssetLocations.Path = "LotA";
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
                var container = initializer.BootstrapContainer(typeof(LotaProgram).Assembly);

                IXleStartup core = container.Resolve<IXleStartup>();
                core.ProcessArguments(args);

                core.Run();
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
