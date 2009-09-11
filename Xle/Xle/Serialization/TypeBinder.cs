using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ERY.Xle.Serialization
{
	class TypeBinder : ITypeBinder
	{
		public List<Assembly> SearchAssemblies = new List<Assembly>();

		public Type GetType(string typename)
		{
			if (Type.GetType(typename) != null)
				return Type.GetType(typename);

			for (int i = 0; i < SearchAssemblies.Count; i++)
			{
				if (SearchAssemblies[i].GetType(typename) != null)
				{
					return SearchAssemblies[i].GetType(typename);
				}
			}

			return null;
		}
	}
}
