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
using Xunit;

namespace ERY.XleTests.ServiceTests
{
	public class ContainerTests
	{
		private IWindsorContainer container;

		public ContainerTests()
		{
			using (var platform = new AgateIntegrationTestPlatform())
			{
				var init = new WindsorInitializer();

				container = init.BootstrapContainer(GetType().Assembly);
			}
		}
	}
}
