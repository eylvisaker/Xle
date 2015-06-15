using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Bootstrap
{
    public class WindsorInitializer
    {
        internal static Assembly MasterAssembly { get; private set; }

        public WindsorContainer BootstrapContainer(Assembly assembly)
        {
            MasterAssembly = assembly;
            var result = new WindsorContainer();

            result.Install(FromAssembly.This());
            result.Install(FromAssembly.Instance(MasterAssembly));
            
            return result;
        }
    }
}
