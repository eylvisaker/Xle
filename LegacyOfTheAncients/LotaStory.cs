using AgateLib.Serialization.Xle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA
{
	class LotaStory : IXleSerializable
	{
		void IXleSerializable.WriteData(XleSerializationInfo info)
		{
			info.WritePublicProperties(this);
		}
		void IXleSerializable.ReadData(XleSerializationInfo info)
		{
			info.ReadPublicProperties(this, true);
		}

		public bool Invisible { get; set; }

		public int MuseumEntryPoint { get; set; }
	}
}
