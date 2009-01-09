using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ERY.Xle
{
    public static class XleFactory
    {
        static List<Assembly> assemblies = new List<Assembly>();

        static XleFactory()
        {
            assemblies.Add(Assembly.GetAssembly(typeof(XleFactory)));
        }
        public static IEnumerable<Type> MapTypes
        {
            get
            {
                for (int i = 0; i < assemblies.Count; i++)
                {
                    foreach (Type t in assemblies[i].GetTypes())
                    {
                        if (typeof(XleMap).IsAssignableFrom(t) == false)
                            continue;

                        if (t.IsAbstract)
                            continue;

                        yield return t;
                    }
                }
            }
        }
        public static IEnumerable<Type> EventTypes
        {
            get
            {

                for (int i = 0; i < assemblies.Count; i++)
                {
                    foreach (Type t in assemblies[i].GetTypes())
                    {
                        if (typeof(XleEvent).IsAssignableFrom(t) == false)
                            continue;

                        if (t.IsAbstract)
                            continue;

                        yield return t;
                    }
                }
            }
        }
    }
}
