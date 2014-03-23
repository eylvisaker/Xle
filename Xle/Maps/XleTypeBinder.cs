using ERY.Xle.XleEventTypes.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ERY.Xle.Maps
{
	class XleTypeBinder : AgateLib.Serialization.Xle.ITypeBinder
	{
		Dictionary<string, Type> typemap = new Dictionary<string, Type>();
		private AgateLib.Serialization.Xle.ITypeBinder typeBinder;
		
		XleTypeBinder()
		{
			Assembly ass = Assembly.GetExecutingAssembly();

			typemap.Add("ERY.Xle.Roof", typeof(Roof));
			typemap.Add("ERY.Xle.Guard", typeof(Guard));
		}

		public XleTypeBinder(AgateLib.Serialization.Xle.ITypeBinder typeBinder)
			: this()
		{
			this.typeBinder = typeBinder;
		}
		public Type GetType(string typename)
		{
			if (typemap.ContainsKey(typename))
				return typemap[typename];
			else
				return typeBinder.GetType(typename);
		}
	}
}
