using System;
using System.Collections.Generic;
using Xle.Services.Commands;

namespace Xle.LoB
{
    internal static class LobProgram
    {
        //		private static ICommandFactory commandFactory;
        //		/// <summary>
        //		/// The main entry point for the application.
        //		/// </summary>
        //		[STAThread]
        //		static void Main(string[] args)
        //		{
        //			using (new AgateWinForms(args)
        //				.AssetPath("LoB")
        //				.Initialize())
        //			using (new DisplayWindowBuilder(args)
        //				.BackbufferSize(680, 440)
        //				.Title("Legend of Blacksilver")
        //				.WithCoordinates(new FixedCoordinateSystem(new Rectangle(-20, -20, 680, 440)))
        //				.AllowResize()
        //				.Build())
        //			{
        //				var initializer = new WindsorInitializer();
        //				var container = initializer.BootstrapContainer(typeof(LobProgram).Assembly);

        //				IXleStartup core = container.Resolve<IXleStartup>();
        //				core.ProcessArguments(args);
        //				commandFactory = container.Resolve<ICommandFactory>();

        //				core.Run();
        //			}
        //		}

        public static IEnumerable<Command> CommonLobCommands
        {
            get
            {
                throw new NotImplementedException();
                //yield return commandFactory.Armor();
                //yield return commandFactory.Gamespeed();
                //yield return commandFactory.Inventory();
                //yield return commandFactory.Pass();
                //yield return commandFactory.Weapon();
            }
        }
    }
}
