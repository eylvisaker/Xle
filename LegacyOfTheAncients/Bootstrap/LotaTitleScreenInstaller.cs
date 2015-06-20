using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.LotA.TitleScreen;
using ERY.Xle.Services;

namespace ERY.Xle.LotA.Bootstrap
{
    public class LotaTitleScreenInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyContaining<LotaTitleScreen>()
                .BasedOn<TitleState>()
                .WithServiceSelf()
                .LifestyleTransient());
        }
    }
}
