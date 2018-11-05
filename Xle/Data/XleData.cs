using AgateLib;
using AgateLib.Mathematics.Geometry;
using Xle.Maps;
using Xle.Maps.XleMapTypes;
using Xle.Maps.XleMapTypes.MuseumDisplays;
using Xle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Xle.Data
{
    [Singleton]
    public class XleData
    {
        private MapList mMapList = new MapList();
        private ItemList mItemList = new ItemList();
        private EquipmentList mWeaponList = new EquipmentList();
        private EquipmentList mArmorList = new EquipmentList();
        private Dictionary<int, string> mQualityList = new Dictionary<int, string>();
        private Dictionary<int, ExhibitInfo> mExhibitInfo = new Dictionary<int, ExhibitInfo>();
        private Dictionary<int, Map3DExtraInfo> mMap3DExtraInfo = new Dictionary<int, Map3DExtraInfo>();
        private Dictionary<int, MagicSpell> mMagicSpells = new Dictionary<int, MagicSpell>();
        private Dictionary<int, DungeonMonsterData> mDungeonMonsters = new Dictionary<int, DungeonMonsterData>();
        private List<MonsterInfo> mMonsterInfo = new List<MonsterInfo>();
        private List<string> mFortunes = new List<string>();

        public Size OverworldMonsterSize { get; set; }

        public MapList MapList => mMapList;
        public ItemList ItemList => mItemList;
        public EquipmentList WeaponList => mWeaponList;
        public EquipmentList ArmorList => mArmorList;
        public Dictionary<int, string> QualityList => mQualityList;
        public Dictionary<int, ExhibitInfo> ExhibitInfo => mExhibitInfo;
        public Dictionary<int, Map3DExtraInfo> Map3DExtraInfo => mMap3DExtraInfo;
        public Dictionary<int, MagicSpell> MagicSpells => mMagicSpells;
        public Dictionary<int, DungeonMonsterData> DungeonMonsters => mDungeonMonsters;
        public List<MonsterInfo> MonsterInfo => mMonsterInfo;
        public IList<string> Fortunes => mFortunes;

        public string GetWeaponName(int weaponID, int qualityID)
        {
            return QualityList[qualityID] + " " + WeaponList[weaponID].Name;
        }
        public string GetArmorName(int armorID, int qualityID)
        {
            return QualityList[qualityID] + " " + ArmorList[armorID].Name;
        }

        public int WeaponCost(int item, int quality)
        {
            return WeaponList[item].Prices[quality];
        }
        public int ArmorCost(int item, int quality)
        {
            return ArmorList[item].Prices[quality];
        }
    }
}
