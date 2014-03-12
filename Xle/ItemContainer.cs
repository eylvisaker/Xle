using AgateLib.Serialization.Xle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle
{
	public class ItemContainer : IXleSerializable
	{
		int[] mItems = new int[50];


		void IXleSerializable.WriteData(XleSerializationInfo info)
		{
			info.Write("Items", mItems);
		}

		void IXleSerializable.ReadData(XleSerializationInfo info)
		{
			mItems = info.ReadInt32Array("Items");
		}
		[Obsolete]
		public int[] ItemArray { get { return mItems; } set { mItems = value; } }

		public int this[int index]
		{
			get { return mItems[index]; }
			set { mItems[index] = value; }
		}

		public int this[LotaItem index]
		{
			get { return mItems[(int)index]; }
			set { mItems[(int)index] = value; }
		}
		public int this[LobItem index]
		{
			get { return mItems[(int)index]; }
			set { mItems[(int)index] = value; }
		}

		public void ClearStoryItems()
		{
			Array.Clear(mItems, 4, 5);
			Array.Clear(mItems, 10, 7);
		}
		public void ClearCoins()
		{
			Array.Clear(mItems, 17, 7);
		}
	}

	public enum LotaItem
	{
		Nothing = 0,
		GoldArmband = 1,
		ClimbingGear = 2,
		HealingHerb = 3,
		IronKey = 4,
		CopperKey = 5,
		BrassKey = 6,
		StoneKey = 7,
		MagicSeed = 8,
		Mail = 9,
		Tulip = 10,
		Compass = 11,
		MagicIce = 12,
		Scepter = 13,
		GuardJewel = 14,
		Compendium = 15,
		Crown = 16,
		JadeCoin = 17,
		TopazCoin = 18,
		AmethystCoin = 19,
		SapphireCoin = 20,
		TurquoiseCoin = 21,
		RubyCoin = 22,
		DiamondCoin = 23,
		MagicFlame = 24,
		Firebolt = 25,
		Befuddle = 26,
		PsychoStrength = 27,
		KillFlash = 28,
		SeekSpell = 29,
	}
	public enum LobItem
	{
		Nothing = 0,
		CrystalRing = 1,
		LifeElixir = 2,
		ClimbingGear = 3,
		LaggardVapors = 4,
		SmallKey = 5,
		WoodenKey = 6,
		GoldKey = 7,
		BlackKey = 8,
		IronKey = 9,
		QuartzKey = 10,
		LargeKey = 11,
		AmberKey = 12,
		SkeletonKey = 13,
		Lodestone = 14,
		RustyKey = 15,
		Package = 16,
		RopeAndPulley = 17,
		SilverCoin = 18,
		FalconFeather = 19,
		SingingCrystal = 20,
		GlassOrb = 21,
		OwlGrail = 22,
		Statuette = 23,
		Lute = 24,
		Staff = 25,
		SignetRing = 26,
		BlackWand = 28,
		FlaxtonIncense = 29,
		SteelHammer = 30,
		Blacksilver = 31,
		DragonTear = 32,
		BlueGem = 33,
		RedGarnet = 34,
		AmethystGem = 35,
		Emerald = 36,
		YellowDiamond = 37,
		WhiteDiamond = 38,
		Opal = 39,
	}
}
