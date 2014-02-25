﻿using AgateLib.Serialization.Xle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA
{
	public class LotaStory : IXleSerializable
	{
		public LotaStory()
		{
			Museum = new int[16];
		}

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

		public int[] Museum { get; set; }

		public bool BeenInDungeon { get; set; }

		public bool ArmakComplete { get; set; }
		public bool PirateComplete { get; set; }
		public bool FourJewelsComplete { get; set; }

		public bool FoundGuardianLeader { get; set; }

		public bool HasGuardianPassword { get; set; }

		public bool HasGuardianMark { get; set; }
	}
}
