using AgateLib.Geometry;
using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace ERY.Xle.Data
{
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

		private Data.AgateDataImport mDatabase;

		public Data.AgateDataImport Database
		{
			get { return mDatabase; }
		}

		public void LoadDatabase()
		{
			AgateLib.Data.AgateDatabase _db = AgateLib.Data.AgateDatabase.FromFile("Lota.adb");
			
			mDatabase = new Data.AgateDataImport(_db);
		}

		public void LoadGameFile(XmlDocument doc)
		{
			XmlNode root = doc.ChildNodes[1];

			for (int i = 0; i < root.ChildNodes.Count; i++)
			{
				switch (root.ChildNodes[i].Name)
				{
					case "Maps":
						LoadMapInfo(root.ChildNodes[i]);
						break;

					case "MagicSpells":
						LoadMagicInfo(root.ChildNodes[i]);
						break;

					case "Weapons":
						LoadEquipmentInfo(root.ChildNodes[i], ref mWeaponList);
						break;

					case "Armor":
						LoadEquipmentInfo(root.ChildNodes[i], ref mArmorList);
						break;

					case "Items":
						LoadItemInfo(root.ChildNodes[i]);
						break;

					case "Qualities":
						LoadQualityInfo(root.ChildNodes[i]);
						break;

					case "Exhibits":
						LoadExhibitInfo(root.ChildNodes[i]);
						break;

					case "DungeonExtras":
						Load3DExtraInfo(root.ChildNodes[i]);
						break;

					case "DungeonMonsters":
						LoadDungeonMonsters(root.ChildNodes[i]);
						break;
				}
			}
		}

		private void LoadQualityInfo(XmlNode xmlNode)
		{
			for (int i = 0; i < xmlNode.ChildNodes.Count; i++)
			{
				XmlNode node = xmlNode.ChildNodes[i];

				int id = int.Parse(node.Attributes["ID"].Value);
				string name = node.Attributes["Name"].Value;

				mQualityList[id] = name;
			}
		}
		private void LoadItemInfo(XmlNode xmlNode)
		{
			for (int i = 0; i < xmlNode.ChildNodes.Count; i++)
			{
				XmlNode node = xmlNode.ChildNodes[i];

				int id = int.Parse(node.Attributes["ID"].Value);
				string name = node.Attributes["Name"].Value;
				string action = GetOptionalAttribute(node, "Action", "");
				string longName = GetOptionalAttribute(node, "LongName", "");
				bool isKey = GetOptionalAttribute(node, "isKey", false);

				mItemList.Add(id, new ItemInfo(id, name, longName, action)
				{
					IsKey = isKey
				});
			}
		}
		private void LoadMagicInfo(XmlNode xmlNode)
		{
			for (int i = 0; i < xmlNode.ChildNodes.Count; i++)
			{
				XmlNode node = xmlNode.ChildNodes[i];

				int id = int.Parse(node.Attributes["ID"].Value);
				string name = node.Attributes["Name"].Value;
				int basePrice = int.Parse(GetOptionalAttribute(node, "BasePrice", "0"));

				mMagicSpells.Add(id, new MagicSpell { Name = name, BasePrice = basePrice });
			}
		}
		private void LoadEquipmentInfo(XmlNode xmlNode, ref EquipmentList equipmentList)
		{
			for (int i = 0; i < xmlNode.ChildNodes.Count; i++)
			{
				XmlNode node = xmlNode.ChildNodes[i];

				int id = int.Parse(node.Attributes["ID"].Value);
				string name = node.Attributes["Name"].Value;
				string prices = "";

				if (node.Attributes["Prices"] != null)
				{
					prices = node.Attributes["Prices"].Value;
				}

				equipmentList.Add(id, name, prices);
			}
		}
		private void LoadMapInfo(XmlNode mapNode)
		{
			for (int i = 0; i < mapNode.ChildNodes.Count; i++)
			{
				XmlNode node = mapNode.ChildNodes[i];

				if (node.Name == "Map")
				{
					int id = int.Parse(node.Attributes["ID"].Value);
					string name = node.Attributes["Name"].Value;
					string filename = node.Attributes["File"].Value;
					int parent = 0;

					if (node.Attributes["ParentMapID"] != null)
					{
						parent = int.Parse(node.Attributes["ParentMapID"].Value);
					}

					string alias = name;

					if (node.Attributes["Alias"] != null)
					{
						alias = node.Attributes["Alias"].Value;
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
		private void LoadExhibitInfo(XmlNode mapNode)
		{
			for (int i = 0; i < mapNode.ChildNodes.Count; i++)
			{
				XmlNode node = mapNode.ChildNodes[i];

				if (node.Name == "Exhibit")
				{
					int id = int.Parse(node.Attributes["ID"].Value);

					var info = new ExhibitInfo();

					if (node.Attributes["Image"] != null)
					{
						info.ImageFile = node.Attributes["Image"].Value;
					}

					foreach (XmlNode child in node.ChildNodes)
					{
						if (child.Name == "Text")
						{
							int textID = int.Parse(child.Attributes["ID"].Value);
							string text = TrimExhibitText(child.InnerText);

							info.Text.Add(textID, text);
						}
					}

					mExhibitInfo.Add(id, info);
				}
			}
		}
		private void LoadDungeonMonsters(XmlNode xmlNode)
		{
			for (int i = 0; i < xmlNode.ChildNodes.Count; i++)
			{
				XmlNode node = xmlNode.ChildNodes[i];

				if (node.Name == "Extra")
				{
					int id = int.Parse(node.Attributes["ID"].Value);
					string name = node.Attributes["Name"].Value;

					var info = new Map3DExtraInfo();

					foreach (XmlNode child in node.ChildNodes)
					{
						if (child.Name == "Image")
						{
							int distance = int.Parse(child.Attributes["distance"].Value);
							Rectangle srcRect = ParseRectangle(child.Attributes["srcRect"].Value);
							Rectangle destRect = ParseRectangle(child.Attributes["destRect"].Value);

							var img = new Map3DExtraImage();

							img.SrcRect = srcRect;
							img.DestRect = destRect;

							info.Images[distance] = img;

							foreach (XmlNode animNode in child.ChildNodes)
							{
								if (animNode.Name != "Animation")
									continue;

								var anim = new Map3DExtraAnimation();

								if (animNode.Attributes["frameTime"] != null)
									anim.FrameTime = double.Parse(animNode.Attributes["frameTime"].Value);

								foreach (XmlNode frameNode in animNode.ChildNodes)
								{
									if (frameNode.Name != "Frame")
										continue;

									srcRect = ParseRectangle(frameNode.Attributes["srcRect"].Value);
									destRect = ParseRectangle(frameNode.Attributes["destRect"].Value);

									var frame = new Map3DExtraImage();

									frame.SrcRect = srcRect;
									frame.DestRect = destRect;

									anim.Images.Add(frame);
								}

								if (anim.Images.Count > 0)
									img.Animations.Add(anim);
							}
						}
					}

					mMap3DExtraInfo.Add(id, info);
				}
			}
		}

		private string TrimExhibitText(string text)
		{
			var regex = new System.Text.RegularExpressions.Regex("\r\n *");

			return regex.Replace(text.Trim(), "\r\n");
		}
		private void Load3DExtraInfo(XmlNode xmlNode)
		{
			for (int i = 0; i < xmlNode.ChildNodes.Count; i++)
			{
				XmlNode node = xmlNode.ChildNodes[i];

				if (node.Name == "Extra")
				{
					int id = int.Parse(node.Attributes["ID"].Value);
					string name = node.Attributes["Name"].Value;

					var info = new Map3DExtraInfo();

					foreach (XmlNode child in node.ChildNodes)
					{
						if (child.Name == "Image")
						{
							int distance = int.Parse(child.Attributes["distance"].Value);
							Rectangle srcRect = ParseRectangle(child.Attributes["srcRect"].Value);
							Rectangle destRect = ParseRectangle(child.Attributes["destRect"].Value);

							var img = new Map3DExtraImage();

							img.SrcRect = srcRect;
							img.DestRect = destRect;

							info.Images[distance] = img;

							foreach (XmlNode animNode in child.ChildNodes)
							{
								if (animNode.Name != "Animation")
									continue;

								var anim = new Map3DExtraAnimation();

								if (animNode.Attributes["frameTime"] != null)
									anim.FrameTime = double.Parse(animNode.Attributes["frameTime"].Value);

								foreach (XmlNode frameNode in animNode.ChildNodes)
								{
									if (frameNode.Name != "Frame")
										continue;

									srcRect = ParseRectangle(frameNode.Attributes["srcRect"].Value);
									destRect = ParseRectangle(frameNode.Attributes["destRect"].Value);

									var frame = new Map3DExtraImage();

									frame.SrcRect = srcRect;
									frame.DestRect = destRect;

									anim.Images.Add(frame);
								}

								if (anim.Images.Count > 0)
									img.Animations.Add(anim);
							}
						}
					}

					mMap3DExtraInfo.Add(id, info);
				}
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


		private static T GetOptionalAttribute<T>(XmlNode node, string attrib, T defaultValue)
		{
			if (node.Attributes[attrib] != null)
				return (T)Convert.ChangeType(node.Attributes[attrib].Value, typeof(T));
			else
				return defaultValue;
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

	}
}
