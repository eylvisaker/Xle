using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle
{
	public class ItemContainer
	{
		int[] mItems = new int[50];

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
		RopeAndPulley = 16,
		SilverCoin = 17,
		FalconFeather = 18,
		SingingCrystal = 19,
		GlassOrb = 20,
		OwlGrail = 21,
		Statuette = 22,
		Lute = 23,
		Staff = 24,
		SignetRing = 25,
		BlackWand = 26,
		FlaxtonIncense = 27,
		SteelHammer = 28,
		Blacksilver = 29,
		DragonTear = 30,
		BlueGem = 31,
		RedGarnet = 32,
		AmethystGem = 33,
		Emerald = 34,
		YellowDiamond = 35,
		WhiteDiamond = 36,
		Opal = 37,
	}
}
