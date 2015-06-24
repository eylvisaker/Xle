using System;

namespace ERY.Xle.Services
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
