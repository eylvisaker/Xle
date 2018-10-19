using Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.Ancients.MapExtenders
{
    [Obsolete]
	class ExtenderDictionary
	{
		Dictionary<string, EventExtender> extenders = new Dictionary<string,EventExtender>();

		class StringComparer : IComparer<string>
		{
			public int Compare(string x, string y)
			{
				return string.Compare(x, y, true);
			}
		}
		public bool ThrowExceptionIfNotFound { get;set;}

		public void Add(string key, EventExtender value)
		{
			extenders.Add(key, value);
		}

		public EventExtender Find(string key)
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
