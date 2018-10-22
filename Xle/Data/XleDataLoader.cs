using AgateLib;
using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Xle.Maps;
using Xle.Maps.XleMapTypes;
using Xle.Maps.XleMapTypes.MuseumDisplays;

namespace Xle.Data
{
    public interface IXleDataLoader
    {
        void LoadGameFile(string filename);
        void LoadDungeonMonsterSurfaces();
    }

    [Transient]
    public class XleDataLoader : IXleDataLoader
    {
        private readonly IContentProvider content;
        private XleData data;

        public XleDataLoader(XleData data, IContentProvider content)
        {
            this.data = data;
            this.content = content;
        }

        public void LoadGameFile(string filename)
        {
            using (var stream = content.Open(filename))
            {
                XDocument doc = XDocument.Load(stream);
                XElement root = doc.Root;

                LoadMapInfo(root.Element("Maps"));
                LoadMagicInfo(root.Element("MagicSpells"));
                LoadEquipmentInfo(root.Element("Weapons"), WeaponList);
                LoadEquipmentInfo(root.Element("Armor"), ArmorList);
                LoadItemInfo(root.Element("Items"));
                LoadQualityInfo(root.Element("Qualities"));
                LoadExhibitInfo(root.Element("Exhibits"));
                Load3DExtraInfo(root.Element("DungeonExtras"));
                LoadDungeonMonsters(root.Element("DungeonMonsters"));
                LoadMonsterInfo(root.Element("OutsideMonsters"));
                LoadFortunes(root.Element("Fortunes"));
            }
        }

        public Size OverworldMonsterSize => data.OverworldMonsterSize;

        private MapList MapList => data.MapList;
        private ItemList ItemList => data.ItemList;
        private EquipmentList WeaponList => data.WeaponList;
        private EquipmentList ArmorList => data.ArmorList;
        private Dictionary<int, string> QualityList => data.QualityList;
        private Dictionary<int, ExhibitInfo> ExhibitInfo => data.ExhibitInfo;
        private Dictionary<int, Map3DExtraInfo> Map3DExtraInfo => data.Map3DExtraInfo;
        private Dictionary<int, MagicSpell> MagicSpells => data.MagicSpells;
        private Dictionary<int, DungeonMonsterData> DungeonMonsters => data.DungeonMonsters;
        private List<MonsterInfo> MonsterInfoList => data.MonsterInfo;
        private IList<string> Fortunes => data.Fortunes;

        private void LoadFortunes(XElement xElement)
        {
            foreach (var node in xElement.Elements("Fortune"))
            {
                Fortunes.Add(node.Value.Trim().Replace("\\n", "\n"));
            }
        }

        private void LoadMonsterInfo(XElement parent)
        {
            data.OverworldMonsterSize = Size.FromString(parent.Attribute("TileSize").Value);

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

                data.MonsterInfo.Add(info);
            }
        }

        private void LoadQualityInfo(XElement element)
        {
            foreach (var node in element.Elements())
            {
                int id = int.Parse(node.Attribute("ID").Value);
                string name = node.Attribute("Name").Value;

                data.QualityList[id] = name;
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

                data.ItemList.Add(id, new ItemInfo(id, name, longName, action)
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

                data.MagicSpells.Add(id, new MagicSpell
                {
                    Name = name,
                    ID = id,
                    BasePrice = basePrice,
                    MaxCarry = maxCarry,
                    ItemID = itemID
                });
            }
        }

        private void LoadEquipmentInfo(XElement element, EquipmentList equipmentList)
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

                    data.MapList.Add(id, name, filename, parent, alias);

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

                data.ExhibitInfo.Add(id, info);
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
                    data.DungeonMonsters.Add(monster.ID, monster);
                }
                else
                {
                    System.Diagnostics.Debug.Print("Could not add monster " + monster.Name);
                }
            }
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

                data.Map3DExtraInfo.Add(id, info);
            }
        }

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
                dm.Value.Image = content.Load<Texture2D>("Images/Dungeon/Monsters/" + dm.Value.ImageFile);
            }
        }

        private string TrimExhibitText(string text)
        {
            var regex = new System.Text.RegularExpressions.Regex("\r\n *");

            return regex.Replace(text.Trim(), "\r\n");
        }


    }
}
