using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
			using (var setup = new IntegrationTestPlatform())
			{
				setup.InitializeAgateLib();

				var init = new WindsorInitializer();

				container = init.BootstrapContainer(GetType().Assembly);
			}
		}
	}
}
