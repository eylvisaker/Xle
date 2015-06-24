using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Services.Implementation.Commands.MapSpecific
{
    public class ServiceNameAttribute : Attribute
    {
        public ServiceNameAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }
    }
}
