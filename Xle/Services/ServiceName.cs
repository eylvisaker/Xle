using System;

namespace Xle.Services
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ServiceNameAttribute : Attribute
    {
        public ServiceNameAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }
    }
}
