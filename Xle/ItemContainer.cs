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
		public  void ClearCoins()
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

		FalconFeather = 18,
	}
}
