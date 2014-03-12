using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.Xle
{
	[Serializable]
	public class Monster
	{
		public string Name { get { return mData.Name; } }
		public TerrainType Terrain { get { return (TerrainType)mData.Terrain; } }
		public int HP { get; set; }
		public int Attack { get { return mData.Attack; } }
		public int Defense { get { return mData.Defense; } }
		public int Gold { get { return mData.Gold; } }
		public int Food { get { return mData.Food; } }
		public int Weapon { get { return mData.Weapon; } }
		public int Image { get { return mData.Image; } }
		public bool Talks { get { return mData.Talks; } }
		public int Friendly { get { return mData.Friendly; } }
		private Data.Monster mData;

		public Monster(Monster m) { copyFrom(m); }

		public Monster(Data.Monster monster)
		{
			this.mData = monster;

			HP = monster.HP;
		}

		public void copyFrom(Monster m)
		{
			mData = m.mData;
			HP = m.HP;
		}
	}
}