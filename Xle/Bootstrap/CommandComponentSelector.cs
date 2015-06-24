using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Castle.Facilities.TypedFactory;
using Castle.MicroKernel;
using System.Reflection;

namespace ERY.Xle.Bootstrap
{
    public class CommandComponentSelector : DefaultTypedFactoryComponentSelector
    {
        protected override string GetComponentName(MethodInfo method, object[] arguments)
        {
            var ps = method.GetParameters().ToList();
            var p = ps.Where(x => x.Name.Equals("name", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            if (p == null)
                return null;

            var index = ps.IndexOf(p);

            return (string)arguments[index];
        }
    }
}
