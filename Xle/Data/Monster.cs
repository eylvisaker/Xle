namespace Xle.Data
{
    public class Monster
    {
        public string Name { get { return mData.Name; } }
        public TerrainType Terrain { get { return (TerrainType)mData.Terrain; } }
        public int HP { get; set; }
        public int Attack { get { return mData.Attack; } }
        public int Defense { get { return mData.Defense; } }
        public int Gold { get { return mData.Gold; } }
        public int Food { get { return mData.Food; } }
        public int Vulnerability { get { return mData.Vulnerability; } }
        public int Image { get { return mData.ID; } }
        public bool Intelligent { get { return mData.Intelligent; } }

        private Data.MonsterInfo mData;

        public Monster(Monster m) { copyFrom(m); }

        public Monster(Data.MonsterInfo monster)
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