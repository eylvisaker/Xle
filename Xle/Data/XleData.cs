using AgateLib.Geometry;

using ERY.Xle.Maps;
using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

using ERY.Xle.Services;

namespace ERY.Xle.Data
{
    public class XleData : IXleService
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

        public XleData()
        {

        }

        public Size OverworldMonsterSize { get; private set; }

        public void LoadGameFile(string filename)
        {
            XDocument doc = XDocument.Load(AgateLib.AgateApp.Assets.OpenReadAsync(filename).Result);
            XElement root = doc.Root;

            LoadMapInfo(root.Element("Maps"));
            LoadMagicInfo(root.Element("MagicSpells"));
            LoadEquipmentInfo(root.Element("Weapons"), ref mWeaponList);
            LoadEquipmentInfo(root.Element("Armor"), ref mArmorList);
            LoadItemInfo(root.Element("Items"));
            LoadQualityInfo(root.Element("Qualities"));
            LoadExhibitInfo(root.Element("Exhibits"));
            Load3DExtraInfo(root.Element("DungeonExtras"));
            LoadDungeonMonsters(root.Element("DungeonMonsters"));
            LoadMonsterInfo(root.Element("OutsideMonsters"));
            LoadFortunes(root.Element("Fortunes"));
        }

        private void LoadFortunes(XElement xElement)
        {
            foreach (var node in xElement.Elements("Fortune"))
            {
                mFortunes.Add(node.Value.Trim().Replace("\\n", "\n"));
            }
        }
        private void LoadMonsterInfo(XElement parent)
        {
            OverworldMonsterSize = Size.FromString(parent.Attribute("TileSize").Value);

            foreach (var node in parent.Elements())
            {
                MonsterInfo info = new MonsterInfo
                {
                    ID = int.Parse(node.Attribute("ID").Value),
                    Name = node.Attribute("Name").Value,
                    HP = int.Parse(node.Attribute("HP").Value),
                    Attack = int.Parse(node.Attribute("Attack").Value),
                    Defense = int.Parse(node.Attribute("Defense").Value),
                    Gold = int.Parse(node.Attribute("Gold").Value),
                    Food = int.Parse(node.Attribute("Food").Value),
                    Terrain = (TerrainType)node.GetOptionalAttribute("Terrain", -1),
                    Vulnerability = node.GetOptionalAttribute("Vulnerability", 0),
                    Toxic = node.GetOptionalAttribute("Toxic", false),
                    IntimidateChance = node.GetOptionalAttribute("IntimidateChance", 0),
                    Intelligent = node.GetOptionalAttribute("Intelligent", false)
                };

                mMonsterInfo.Add(info);

            }
        }
        private void LoadQualityInfo(XElement element)
        {
            foreach (var node in element.Elements())
            {
                int id = int.Parse(node.Attribute("ID").Value);
                string name = node.Attribute("Name").Value;

                mQualityList[id] = name;
            }
        }
        private void LoadItemInfo(XElement element)
        {
            foreach (var node in element.Elements())
            {
                int id = int.Parse(node.Attribute("ID").Value);
                string name = node.Attribute("Name").Value;
                string action = node.GetOptionalAttribute("Action", "");
                string longName = node.GetOptionalAttribute("LongName", "");
                bool isKey = node.GetOptionalAttribute("isKey", false);

                mItemList.Add(id, new ItemInfo(id, name, longName, action)
                {
                    IsKey = isKey
                });
            }
        }
        private void LoadMagicInfo(XElement element)
        {
            foreach (var node in element.Elements())
            {
                int id = int.Parse(node.Attribute("ID").Value);
                string name = node.Attribute("Name").Value;
                int basePrice = node.GetOptionalAttribute("BasePrice", 0);
                int maxCarry = node.GetOptionalAttribute("MaxCarry", 10);
                int itemID = int.Parse(node.Attribute("ItemID").Value);

                mMagicSpells.Add(id, new MagicSpell
                {
                    Name = name,
                    ID = id,
                    BasePrice = basePrice,
                    MaxCarry = maxCarry,
                    ItemID = itemID
                });
            }
        }
        private void LoadEquipmentInfo(XElement element, ref EquipmentList equipmentList)
        {
            foreach (var node in element.Elements())
            {
                int id = int.Parse(node.Attribute("ID").Value);
                string name = node.Attribute("Name").Value;
                string prices = "";

                if (node.Attribute("Prices") != null)
                {
                    prices = node.Attribute("Prices").Value;
                }

                var equipment = new EquipmentInfo(id, name, prices);

                if (node.Attribute("Ranged") != null)
                    equipment.Ranged = true;

                equipmentList.Add(equipment.ID, equipment);
            }
        }
        private void LoadMapInfo(XElement element)
        {
            foreach (var node in element.Elements())
            {
                if (node.Name == "Map")
                {
                    int id = int.Parse(node.Attribute("ID").Value);
                    string name = node.Attribute("Name").Value;
                    string filename = node.Attribute("File").Value;
                    int parent = 0;

                    if (node.Attribute("ParentMapID") != null)
                    {
                        parent = int.Parse(node.Attribute("ParentMapID").Value);
                    }

                    string alias = name;

                    if (node.Attribute("Alias") != null)
                    {
                        alias = node.Attribute("Alias").Value;
                    }

                    mMapList.Add(id, name, filename, parent, alias);

                    if (System.IO.File.Exists(@"Maps\" + filename) == false)
                    {
                        System.Diagnostics.Debug.WriteLine("WARNING: File " + filename +
                            " for Map ID = " + id + " does not exist.");
                    }

                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(
                        "Could not understand node Maps." + node.Name);
                }
            }
        }
        private void LoadExhibitInfo(XElement element)
        {
            foreach (var node in element.Elements("Exhibit"))
            {
                int id = int.Parse(node.Attribute("ID").Value);

                var info = new ExhibitInfo();

                if (node.Attribute("Image") != null)
                {
                    info.ImageFile = node.Attribute("Image").Value;
                }

                foreach (XElement child in node.Elements("Text"))
                {
                    int textID = int.Parse(child.Attribute("ID").Value);
                    string text = TrimExhibitText(child.Value);

                    info.Text.Add(textID, text);
                }

                mExhibitInfo.Add(id, info);
            }
        }
        private void LoadDungeonMonsters(XElement element)
        {
            foreach (var node in element.Elements("Monster"))
            {
                var monster = new DungeonMonsterData();

                monster.ID = int.Parse(node.Attribute("ID").Value);
                monster.Name = node.Attribute("Name").Value;
                monster.ImageFile = node.GetOptionalAttribute("ImageFile", "");

                foreach (XElement child in node.Elements("Image"))
                {
                    DungeonMonsterImage image = new DungeonMonsterImage();

                    image.DrawPoint = new Point(
                        int.Parse(child.Attribute("dest_x").Value),
                        int.Parse(child.Attribute("dest_y").Value));

                    foreach (XElement xsource in child.Elements("SourceRect"))
                    {
                        Rectangle rect = new Rectangle(
                            int.Parse(xsource.Attribute("x").Value),
                            int.Parse(xsource.Attribute("y").Value),
                            int.Parse(xsource.Attribute("width").Value),
                            int.Parse(xsource.Attribute("height").Value));

                        image.SourceRects.Add(rect);
                    }

                    monster.Images.Add(image);
                }

                if (monster.IsValid)
                {
                    mDungeonMonsters.Add(monster.ID, monster);
                }
                else
                {
                    System.Diagnostics.Debug.Print("Could not add monster " + monster.Name);
                }
            }
        }

        private string TrimExhibitText(string text)
        {
            var regex = new System.Text.RegularExpressions.Regex("\r\n *");

            return regex.Replace(text.Trim(), "\r\n");
        }
        private void Load3DExtraInfo(XElement element)
        {
            foreach (var node in element.Elements("Extra"))
            {
                int id = int.Parse(node.Attribute("ID").Value);
                string name = node.Attribute("Name").Value;

                var info = new Map3DExtraInfo();

                foreach (XElement child in node.Elements("Image"))
                {
                    int distance = int.Parse(child.Attribute("distance").Value);
                    Rectangle srcRect = ParseRectangle(child.Attribute("srcRect").Value);
                    Rectangle destRect = ParseRectangle(child.Attribute("destRect").Value);

                    var img = new Map3DExtraImage();

                    img.SrcRect = srcRect;
                    img.DestRect = destRect;

                    info.Images[distance] = img;

                    foreach (XElement animNode in child.Elements("Animation"))
                    {
                        var anim = new Map3DExtraAnimation();

                        if (animNode.Attribute("frameTime") != null)
                            anim.FrameTime = double.Parse(animNode.Attribute("frameTime").Value);

                        foreach (XElement frameNode in animNode.Elements("Frame"))
                        {
                            srcRect = ParseRectangle(frameNode.Attribute("srcRect").Value);
                            destRect = ParseRectangle(frameNode.Attribute("destRect").Value);

                            var frame = new Map3DExtraImage();

                            frame.SrcRect = srcRect;
                            frame.DestRect = destRect;

                            anim.Images.Add(frame);
                        }

                        if (anim.Images.Count > 0)
                            img.Animations.Add(anim);
                    }
                }

                mMap3DExtraInfo.Add(id, info);
            }
        }

        public MapList MapList
        {
            get { return mMapList; }
        }
        public ItemList ItemList
        {
            get { return mItemList; }
        }
        public EquipmentList WeaponList
        {
            get { return mWeaponList; }
        }
        public EquipmentList ArmorList
        {
            get { return mArmorList; }
        }
        public Dictionary<int, string> QualityList
        {
            get { return mQualityList; }
        }
        public Dictionary<int, ExhibitInfo> ExhibitInfo
        {
            get { return mExhibitInfo; }
        }
        public Dictionary<int, Map3DExtraInfo> Map3DExtraInfo { get { return mMap3DExtraInfo; } }
        public Dictionary<int, MagicSpell> MagicSpells { get { return mMagicSpells; } }
        public Dictionary<int, DungeonMonsterData> DungeonMonsters { get { return mDungeonMonsters; } }
        public List<MonsterInfo> MonsterInfoList { get { return mMonsterInfo; } }
        public IList<string> Fortunes { get { return mFortunes; } }

        private Rectangle ParseRectangle(string p)
        {
            string[] vals = p.Split(',');

            return new Rectangle(
                int.Parse(vals[0]),
                int.Parse(vals[1]),
                int.Parse(vals[2]),
                int.Parse(vals[3]));
        }

        public void LoadDungeonMonsterSurfaces()
        {
            foreach (var dm in DungeonMonsters)
            {
                dm.Value.Surface = new AgateLib.DisplayLib.Surface("Images/Dungeon/Monsters/" + dm.Value.ImageFile);
            }
        }


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
