using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders
{
	class ExtenderDictionary
	{
		Dictionary<string, IEventExtender> extenders = new Dictionary<string,IEventExtender>();

		class StringComparer : IComparer<string>
		{
			public int Compare(string x, string y)
			{
				return string.Compare(x, y, true);
			}
		}
		public bool ThrowExceptionIfNotFound { get;set;}

		public void Add(string key, IEventExtender value)
		{
			extenders.Add(key, value);
		}

		public IEventExtender Find(string key)
		{
			if (string.IsNullOrWhiteSpace(key))
				return null;

			if (extenders.ContainsKey(key))
				return extenders[key];

			if (ThrowExceptionIfNotFound)
				throw new KeyNotFoundException(key);

			return null;
		}
	}
}
