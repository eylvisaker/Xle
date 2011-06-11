using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.Xle
{
	[Serializable]
	public class Monster
	{
		public string mName;
		public TerrainType mTerrain;
		public int mHP;
		public int mAttack;
		public int mDefense;
		public int mGold;
		public int mFood;
		public int mWeapon;
		public int mImage;
		public bool mTalks;
		public int mFriendly;

		public Monster()
		{ }

		public Monster(Monster m) { copyFrom(m); }

		public void copyFrom(Monster m)
		{
			mName = m.mName;
			mTerrain = m.mTerrain;
			mHP = m.mHP;
			mAttack = m.mAttack;
			mDefense = m.mDefense;
			mGold = m.mGold;
			mFood = m.mFood;
			mWeapon = m.mWeapon;
			mImage = m.mImage;
			mTalks = m.mTalks;
			mFriendly = m.mFriendly;

		}
	}
}