﻿using AgateLib.ApplicationModels;
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

            model.Run(() => GameRunner(args));
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
                yield return commandFactory.Fight();
                yield return commandFactory.Gamespeed();
                yield return commandFactory.Hold();
                yield return commandFactory.Inventory();
                yield return commandFactory.Pass();
                yield return commandFactory.Use(showItemNenu:false);
                yield return commandFactory.Weapon();
            }
        }
    }
}
