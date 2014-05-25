using AgateLib.Serialization.Xle;
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
			CastleGroundChests = new int[40];
			CastleUpperChests = new int[24];
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
		public int[] CastleGroundChests { get; set; }
		public int[] CastleUpperChests { get; set; }

		public bool BeenInDungeon { get; set; }

		public bool ArmakComplete { get; set; }
		public bool PirateComplete { get; set; }
		public bool FourJewelsComplete { get; set; }

		public bool FoundGuardianLeader { get; set; }

		public bool HasGuardianPassword { get; set; }

		public bool HasGuardianMark { get; set; }

		public bool EatenJutonFruit { get; set; }

		public bool PurchasedHerbs { get; set; }

		public bool ReturnedTulip { get; set; }

		public bool SearchingForTulip { get; set; }

		public bool FortressComplete { get; set; }

		public bool VisitedCasandra { get; set; }

		public bool BoughtPotion { get; set; }

		public int BefuddleTurns { get; set; }

		public int BackfiredBefuddleTurns { get; set; }

		public int NextFortune { get; set; }
	}
}
