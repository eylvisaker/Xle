using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgateLib.ApplicationModels;
using AgateLib.Drivers;
using AgateLib.Platform.IntegrationTest;
using Castle.MicroKernel;
using Castle.Windsor;
using Castle.Windsor.Installer;
using ERY.Xle.Bootstrap;
using ERY.Xle.Services.Game;
using ERY.Xle.Services.Menus;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ERY.XleTests.ServiceTests
{
	[TestClass]
	public class ContainerTests
	{
		private IWindsorContainer container;

		public ContainerTests()
		{
			var appDirPath = Path.GetDirectoryName(Assembly.GetAssembly(typeof(ContainerTests)).Location);
			var parameters = new SerialModelParameters();

			IntegrationTestPlatform.Initialize(parameters, appDirPath);

			var init = new WindsorInitializer();

			container = init.BootstrapContainer(GetType().Assembly);
		}
	}
}
