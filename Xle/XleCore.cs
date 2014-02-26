using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using System.ComponentModel;
using ERY.Xle.XleMapTypes.MuseumDisplays;
using ERY.Xle.XleEventTypes;

namespace ERY.Xle
{
	public class XleCore
	{
		#region --- Static Members ---


		public const int myWindowWidth = 640;
		public const int myWindowHeight = 400;

		public static Random random = new Random();

		private static bool AcceptKey = true;

		private static XleCore inst;

		private static bool returnToTitle = false;

		[Obsolete("Use GameState.Map instead.")]
		public static XleMap Map
		{
			get { return GameState.Map; }
			set
			{
				GameState.Map = value;
			}
		}

		public static XleGameFactory Factory
		{
			get
			{
				if (inst == null)
					return null;

				return inst.mFactory;
			}
		}

		#endregion

		private char theCursor = 'P';			// location of the menu dot
		private string[] menuArray = new string[17];		// keeps the menu portion of the screen

		private XleGameFactory mFactory;

		private MapList mMapList = new MapList();
		private ItemList mItemList = new ItemList();
		private EquipmentList mWeaponList = new EquipmentList();
		private EquipmentList mArmorList = new EquipmentList();
		private Dictionary<int, string> mQualityList = new Dictionary<int, string>();
		private Dictionary<int, XleMapTypes.MuseumDisplays.ExhibitInfo> mExhibitInfo = new Dictionary<int, XleMapTypes.MuseumDisplays.ExhibitInfo>();
		private Dictionary<int, XleMapTypes.Map3DExtraInfo> mMap3DExtras = new Dictionary<int, XleMapTypes.Map3DExtraInfo>();
		private Dictionary<int, MagicSpell> mMagicSpells = new Dictionary<int, MagicSpell>();

		private Data.AgateDataImport mDatabase;

		public static Color FontColor { get; private set; }

		public static Surface Tiles { get; private set; }

		public static TextArea TextArea { get; private set; }

		public XleCore()
		{
			inst = this;
			PlayerColor = XleColor.White;

			for (int i = 0; i < 17; i++)
			{
				menuArray[i] = "";
			}

			TextArea = new TextArea();
		}
		public void Run(XleGameFactory factory)
		{
			if (factory == null) throw new ArgumentNullException();

			mFactory = factory;

			AgateLib.AgateFileProvider.Images.AddPath("Images");
			AgateLib.AgateFileProvider.Sounds.AddPath("Audio");
			AgateLib.Core.ErrorReporting.CrossPlatformDebugLevel = CrossPlatformDebugLevel.Exception;

			LoadGameFile();
			LoadDatabase();

			InitializeConsole();

			using (AgateSetup setup = new AgateSetup())
			{
				setup.InitializeAll();
				if (setup.WasCanceled)
					return;

				DisplayWindow wind = DisplayWindow.CreateWindowed(mFactory.GameTitle, 640, 400);

				SoundMan.Load();

				mFactory.LoadSurfaces();

				IXleTitleScreen titleScreen;

				do
				{
					titleScreen = mFactory.CreateTitleScreen();
					titleScreen.Run();
					returnToTitle = false;

					RunGame(titleScreen.Player);

				} while (titleScreen.Player != null);
			}
		}

		#region --- Console Commands ---

		private void InitializeConsole()
		{
			AgateConsole.Initialize();

			AgateConsole.Instance.CommandProcessor.DescribeCommand += CommandProcessor_DescribeCommand;

			AgateConsole.Commands.Add("gold", new Action<int>(CheatGiveGold));
			AgateConsole.Commands.Add("food", new Action<int>(CheatGiveFood));
			AgateConsole.Commands.Add("level", new Action<int>(CheatLevel));
			AgateConsole.Commands.Add("goto", new Action<string>(CheatGoto));
			AgateConsole.Commands.Add("enter", new Action<string, int>(CheatEnter));
			AgateConsole.Commands.Add("move", new Action<int, int, int>(CheatMove));
			AgateConsole.Commands.Add("godmode", new Action(CheatGod));
			AgateConsole.Commands.Add("killall", new Action(CheatKillAll));
		}

		[Description("Kills all the guards on the map.")]
		private void CheatKillAll()
		{
			if (Map is IHasGuards == false)
			{
				throw new InvalidOperationException("There are no guards on this map.");
			}

			var mp = Map as IHasGuards;

			mp.Guards.Clear();
		}

		string CommandProcessor_DescribeCommand(string command)
		{
			StringBuilder b = new StringBuilder();

			switch (command)
			{
				case "goto":
					b.AppendLine("Jumps to the entrance of the specified map. Allowed map values are: ");

					ConsolePrintMapList(b);

					break;

				case "enter":
					b.AppendLine("Jumps to the specified map. You can specify the map name and optionally and entry point.");

					ConsolePrintMapList(b);

					break;
			}

			return b.ToString();
		}

		private void ConsolePrintMapList(StringBuilder b)
		{
			bool comma = false;
			int count = 0;

			foreach (var map in mMapList)
			{
				if (comma)
					b.Append(", ");
				if (count == 0)
					b.Append("    ");

				b.Append(map.Value.Alias);
				comma = true;

				count++;
				if (count > 4)
				{
					b.AppendLine();
					count = 0;
				}
			}
		}


		[Description("Gives gold")]
		void CheatGiveGold(int amount = 1000)
		{
			GameState.Player.Gold += amount;
		}
		[Description("Gives food")]
		void CheatGiveFood(int amount = 500)
		{
			GameState.Player.Food += amount;
		}
		[Description("Sets the players level. Gives items and sets story variables to be consistent with the level chosen. Does not affect weapons or armor.")]
		public static void CheatLevel(int level)
		{
			Factory.CheatLevel(GameState.Player, level);
		}

		[Description("Moves to a specified place on the current map. Pass no arguments to see the current position. Pass two arguments to set x,y. Pass three arguments to set x,y,level for dungeons.")]
		private void CheatMove(int x = -1, int y = -1, int level = -1)
		{
			Player player = GameState.Player;

			if (x == -1)
			{
				if (Map.IsMultiLevelMap)
				{
					AgateConsole.WriteLine("Current Position: {0}, {1}, level: {2}", player.X, player.Y, player.DungeonLevel + 1);
				}
				else
				{
					AgateConsole.WriteLine("Current Position: {0}, {1}", player.X, player.Y);
				}
				return;
			}
			else if (y == -1)
				throw new Exception("You must pass x and y to move.");

			if (x < 0) throw new Exception("x cannot be less than zero.");
			if (y < 0) throw new Exception("y cannot be less than zero.");
			if (x >= Map.Width) throw new Exception(string.Format("x cannot be {0} or greater.", Map.Width));
			if (y >= Map.Height) throw new Exception(string.Format("y cannot be {0} or greater.", Map.Height));

			if (level == -1)
			{
				player.X = x;
				player.Y = y;
			}
			else
			{
				if (GameState.Map.IsMultiLevelMap == false)
					AgateConsole.WriteLine("Cannot pass level on a map without levels.");
				else
				{
					if (level < 1 || level > GameState.Map.Levels)
						throw new Exception(string.Format("level cannot be less than 1 or greater than {0}", GameState.Map.Levels));

					player.X = x;
					player.Y = y;
					player.DungeonLevel = level - 1;
				}
			}
		}


		private static void CheatEnter(string mapName, int entryPoint = 0)
		{
			MapInfo mapInfo = FindMapByPartialName(mapName);

			if (mapInfo == null)
				return;

			ChangeMap(GameState.Player, mapInfo.ID, entryPoint, 0, 0);
		}
		public static void CheatGoto(string mapName)
		{
			Player player = GameState.Player;

			MapInfo mapInfo = FindMapByPartialName(mapName);

			if (mapInfo == null)
				return;

			var map = LoadMap(mapInfo.ParentMapID);
			int targetX = 0, targetY = 0;

			foreach (ChangeMapEvent evt in from evt in map.Events
										   where evt is ChangeMapEvent
										   select (ChangeMapEvent)evt)
			{
				if (evt.MapID == mapInfo.ID)
				{
					targetX = evt.X;
					targetY = evt.Y;
				}
			}

			if (map.CanPlayerStepInto(player, targetX + 2, targetY))
				targetX += 2;
			else if (map.CanPlayerStepInto(player, targetX - 2, targetY))
				targetX -= 2;
			else if (map.CanPlayerStepInto(player, targetX, targetY + 2))
				targetY += 2;
			else if (map.CanPlayerStepInto(player, targetX, targetY - 2))
				targetY -= 2;

			ChangeMap(player, map.MapID, -1, targetX, targetY);
		}

		private static MapInfo FindMapByPartialName(string mapName)
		{
			IEnumerable<MapInfo> matches = from m in MapList.Values
										   where m.Alias.ToUpperInvariant().Contains(mapName.ToUpperInvariant())
										   select m;

			MapInfo exactMatch = matches.FirstOrDefault(x => x.Alias.ToUpperInvariant() == mapName.ToUpperInvariant());

			if (matches.Count() == 0)
			{
				AgateConsole.WriteLine("Map name not found.");
				return null;
			}
			else if (matches.Count() > 1 && exactMatch == null)
			{
				AgateConsole.WriteLine("Found multiple matches:");

				foreach (var m in matches)
				{
					AgateConsole.WriteLine("    {0}", m.Alias);
				}

				return null;
			}

			return exactMatch ?? matches.First();
		}

		[Description("Makes you super powerful.")]
		public static void CheatGod()
		{
			Player player = GameState.Player;

			player.Gold = 99999;
			player.GoldInBank = 999999;
			player.Food = 99999;
			player.Attribute[Attributes.strength] = 300;
			player.Attribute[Attributes.dexterity] = 300;
			player.Attribute[Attributes.intelligence] = 300;
			player.Attribute[Attributes.charm] = 300;
			player.Attribute[Attributes.endurance] = 300;
		}
		#endregion

		public static bool ReturnToTitle
		{
			get { return XleCore.returnToTitle; }
			set { XleCore.returnToTitle = value; }
		}

		private void LoadGameFile()
		{
			XmlDocument doc = new XmlDocument();
			doc.Load("Game.xml");

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
				}
			}
		}

		public static GameState GameState { get; set; }

		private static void LoadDatabase()
		{
			AgateLib.Data.AgateDatabase _db = AgateLib.Data.AgateDatabase.FromFile("Lota.adb");
			inst.mDatabase = new Data.AgateDataImport(_db);
		}

		private static T GetOptionalAttribute<T>(XmlNode node, string attrib, T defaultValue)
		{
			if (node.Attributes[attrib] != null)
				return (T)Convert.ChangeType(node.Attributes[attrib].Value, typeof(T));
			else
				return defaultValue;
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

					var info = new XleMapTypes.MuseumDisplays.ExhibitInfo();

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

					var info = new XleMapTypes.Map3DExtraInfo();

					foreach (XmlNode child in node.ChildNodes)
					{
						if (child.Name == "Image")
						{
							int distance = int.Parse(child.Attributes["distance"].Value);
							Rectangle srcRect = ParseRectangle(child.Attributes["srcRect"].Value);
							Rectangle destRect = ParseRectangle(child.Attributes["destRect"].Value);

							var img = new XleMapTypes.Map3DExtraImage();

							img.SrcRect = srcRect;
							img.DestRect = destRect;

							info.Images[distance] = img;

							foreach (XmlNode animNode in child.ChildNodes)
							{
								if (animNode.Name != "Animation")
									continue;

								var anim = new XleMapTypes.Map3DExtraAnimation();

								if (animNode.Attributes["frameTime"] != null)
									anim.FrameTime = double.Parse(animNode.Attributes["frameTime"].Value);

								foreach (XmlNode frameNode in animNode.ChildNodes)
								{
									if (frameNode.Name != "Frame")
										continue;

									srcRect = ParseRectangle(frameNode.Attributes["srcRect"].Value);
									destRect = ParseRectangle(frameNode.Attributes["destRect"].Value);

									var frame = new XleMapTypes.Map3DExtraImage();

									frame.SrcRect = srcRect;
									frame.DestRect = destRect;

									anim.Images.Add(frame);
								}

								if (anim.Images.Count > 0)
									img.Animations.Add(anim);
							}
						}
					}

					mMap3DExtras.Add(id, info);
				}
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

		public static string GetMapName(int id)
		{
			return inst.mMapList[id].Name;
		}
		public static MapList MapList
		{
			get { return inst.mMapList; }
		}
		public static ItemList ItemList
		{
			get { return inst.mItemList; }
		}
		public static EquipmentList WeaponList
		{
			get { return inst.mWeaponList; }
		}
		public static EquipmentList ArmorList
		{
			get { return inst.mArmorList; }
		}
		public static Dictionary<int, string> QualityList
		{
			get { return inst.mQualityList; }
		}
		public static Dictionary<int, XleMapTypes.MuseumDisplays.ExhibitInfo> ExhibitInfo
		{
			get { return inst.mExhibitInfo; }
		}
		public static Dictionary<int, XleMapTypes.Map3DExtraInfo> Map3DExtraInfo { get { return inst.mMap3DExtras; } }
		public static Dictionary<int, MagicSpell> MagicSpells { get { return inst.mMagicSpells; } }

		public static Data.AgateDataImport Database
		{
			get { return inst.mDatabase; }
		}

		public static string GetWeaponName(int weaponID, int qualityID)
		{
			return inst.mQualityList[qualityID] + " " + inst.mWeaponList[weaponID].Name;
		}
		public static string GetArmorName(int armorID, int qualityID)
		{
			return inst.mQualityList[qualityID] + " " + inst.mArmorList[armorID].Name;
		}

		public static int WeaponCost(int item, int quality)
		{
			return WeaponList[item].Prices[quality];
		}
		public static int ArmorCost(int item, int quality)
		{
			return ArmorList[item].Prices[quality];
		}

		public static XleMap LoadMap(int mapID)
		{
			string file = System.IO.Path.Combine("Maps", inst.mMapList[mapID].Filename);

			return XleMap.LoadMap(file, mapID);
		}

		private void RunGame(Player thePlayer)
		{
			if (thePlayer == null)
				return;
			
			GameState = new Xle.GameState();

			GameState.Player = thePlayer;
			
			GameState.commands = new Commands(GameState.Player);

			GameState.Map = LoadMap(GameState.Player.MapID);
			GameState.Map.OnLoad(GameState.Player);

			Factory.SetGameSpeed(GameState, thePlayer.Gamespeed);

			SetTilesAndCommands();

			Keyboard.KeyDown += new InputEventHandler(Keyboard_KeyDown);

			while (Display.CurrentWindow.IsClosed == false && returnToTitle == false)
			{
				Redraw();
			}

			Keyboard.KeyDown -= Keyboard_KeyDown;
		}

		private static void SetTilesAndCommands()
		{
			inst.menuArray = GameState.Map.MapMenu();
			XleCore.LoadTiles(GameState.Map.TileImage);
		}

		void player_MapChanged(object sender, EventArgs e)
		{
		}

		public static void Redraw()
		{
			inst.RedrawImpl();
		}
		private void RedrawImpl()
		{
			UpdateAnim();

			Display.BeginFrame();

			Draw();

			Display.EndFrame();
			Core.KeepAlive();

			if (AgateConsole.IsVisible == false)
			{
				CheckArrowKeys();
			}
		}
		private void CheckArrowKeys()
		{
			if (AcceptKey == false)
				return;

			AcceptKey = false;

			if (Keyboard.Keys[KeyCode.Down]) GameState.commands.DoCommand(KeyCode.Down);
			else if (Keyboard.Keys[KeyCode.Left]) GameState.commands.DoCommand(KeyCode.Left);
			else if (Keyboard.Keys[KeyCode.Up]) GameState.commands.DoCommand(KeyCode.Up);
			else if (Keyboard.Keys[KeyCode.Right]) GameState.commands.DoCommand(KeyCode.Right);

			AcceptKey = true;
		}

		void Keyboard_KeyDown(InputEventArgs e)
		{
			if (AcceptKey == false)
				return;

			try
			{
				AcceptKey = false;
				GameState.commands.DoCommand(e.KeyCode);
			}
			finally
			{
				AcceptKey = true;
			}
		}

		// TODO: Which of these are obsolete?
		//static bool updating = false;
		static double lastRaftAnim = 0;
		//static double lastCharAnim = 0;
		//static int lastOceanSound = 0;
		//static double timer;
		//static double frames = 0;
		//static double fps;

		public void UpdateAnim()
		{
			RaftAnim();
			//CheckAnim();
		}

		public void Draw()
		{
			Player player = GameState.Player;
			XleMap map = GameState.Map;

			int i = 0;
			Color boxColor;
			Color innerColor;
			Color fontColor;
			Color menuColor;
			int horizLine = 18 * 16;
			int vertLine;

			map.GetBoxColors(out boxColor, out innerColor, out fontColor, out vertLine);
			FontColor = fontColor;

			menuColor = fontColor;

			if (g.LeftMenuActive)
			{
				menuColor = XleColor.Yellow;
			}

			// Clear the back buffer
			Display.Clear();

			DrawBorder(boxColor);

			DrawLine(vertLine, 0, 0, horizLine + 12, boxColor);
			DrawLine(0, horizLine, 1, myWindowWidth, boxColor);

			DrawInnerBorder(innerColor);

			DrawInnerLine(vertLine, 0, 0, horizLine + 12, innerColor);
			DrawInnerLine(0, horizLine, 1, myWindowWidth, innerColor);

			Rectangle mapRect = Rectangle.FromLTRB
				(vertLine + 16, 16, XleCore.myWindowWidth - 16, horizLine);

			map.Draw(player.X, player.Y, player.FaceDirection, mapRect);

			for (i = 0; i < menuArray.Length; i++)
			{
				WriteText(48, 16 * (i + 1), menuArray[i], menuColor);
			}

			WriteText(32, 16 * (CursorPos + 1), "`", menuColor);

			Color hpColor = fontColor;
			if (mOverrideHPColor)
				hpColor = mHPColor;

			WriteText(48, 16 * 15, "H.P. " + player.HP.ToString(), hpColor);
			WriteText(48, 16 * 16, "Food " + player.Food.ToString(), hpColor);
			WriteText(48, 16 * 17, "Gold " + player.Gold.ToString(), hpColor);

			DrawBottomText();

			if (map.AutoDrawPlayer)
			{
				DrawRafts(mapRect);

				if (player.OnRaft == 0)
					DrawCharacter(g.Animating, g.AnimFrame, vertLine);
			}

			if (PromptToContinue)
			{
				Display.FillRect(192, 384, 17 * 16, 16, XleColor.Black);
				WriteText(208, 384, "(Press to Cont)", XleColor.Yellow);
			}

			/////////////////////////////////////////////////////////////////////////
			// Check sounds
			//
			//CheckFade();

			Map.CheckSounds(player);

			//
			// End sounds
			/////////////////////////////////////////////////////////////////////////
		}

		bool mOverrideHPColor;

		private static Color mHPColor;

		public static void FlashHPWhileSound(Color clr)
		{
			FlashHPWhileSound(clr, FontColor);
		}
		public static void FlashHPWhileSound(Color clr, Color clr2)
		{
			Color oldClr = mHPColor;
			Color lastColor = clr2;

			inst.mOverrideHPColor = true;

			while (SoundMan.IsAnyPlaying())
			{
				if (lastColor == clr)
					lastColor = clr2;
				else
					lastColor = clr;

				mHPColor = lastColor;

				XleCore.Wait(80);
			}

			inst.mOverrideHPColor = false;
			mHPColor = oldClr;
		}

		/****************************************************************************
		 *	void DrawBottomText ( LPDIRECTDRAWSURFACE7 pDDS )						*
		 *																			*
		 *  This function handles draws the action history at the bottom of the		*
		 *	main window for heartbeat												*
		 *																			*
		 *	Parameters:	the direct draw surface to draw to							*
		 *  Returns:	void														*
		 ****************************************************************************/
		/// <summary>
		/// This function handles draws the action history at the bottom of the
		/// main window for heartbeat		
		/// </summary>
		[Obsolete("Use TextArea.DrawArea instead.")]
		public static void DrawBottomText()
		{
			TextArea.DrawArea();

		}

		[Obsolete("Use TextArea.Margin instead.")]
		public static int BottomTextMargin { get { return TextArea.Margin; } set { TextArea.Margin = value; } }

		/****************************************************************************
		 *	void DrawBorder( LPDIRECTDRAWSURFACE7 pDDS, unsigned int boxColor)		*
		 *																			*
		 *  This function draws the border around the screen.						*
		 *																			*
		 *	Parameters:	the direct draw surface to draw to, and the color to draw	*
		 *  Returns:	void														*
		 ****************************************************************************/
		public static void DrawBorder(Color boxColor)
		{
			DrawLine(0, 0, 1, myWindowWidth, boxColor);
			DrawLine(0, 0, 0, myWindowHeight - 2, boxColor);
			DrawLine(0, myWindowHeight - 16, 1, myWindowWidth, boxColor);
			DrawLine(myWindowWidth - 12, 0, 0, myWindowHeight - 2, boxColor);
		}

		/****************************************************************************
		 *	void DrawInnerBorder( LPDIRECTDRAWSURFACE7 pDDS,						*
		 *						  unsigned int innerColor)							*
		 *																			*
		 *  This function draws the colored lines inside the border					*
		 *																			*
		 *	Parameters:	the direct draw surface to draw to, and the color to draw	*
		 *  Returns:	void														*
		 ****************************************************************************/
		public static void DrawInnerBorder(Color innerColor)
		{
			DrawInnerLine(0, 0, 1, myWindowWidth, innerColor);
			DrawInnerLine(0, 0, 0, myWindowHeight - 2, innerColor);
			DrawInnerLine(0, myWindowHeight - 16, 1, myWindowWidth + 2, innerColor);
			DrawInnerLine(myWindowWidth - 12, 0, 0, myWindowHeight - 2, innerColor);

		}

		/****************************************************************************
		 *	void DrawLine(LPDIRECTDRAWSURFACE7 pDDS, int left, int top,				*
		 *				int direction, int length, unsigned int boxColor)			*
		 *																			*
		 *																			*
		 *  This function draws a single colored line at the point specified.		*
		 *																			*
		 *	Parameters:	the direct draw surface to draw to, the left and top		*
		 *		coordinates, direction = 1 for drawing to the right, or 0 for down,	*
		 *		the length of the line, and the color to draw.						*
		 *  Returns:	void														*
		 ****************************************************************************/
		public static void DrawLine(int left, int top, int direction,
					  int length, Color boxColor)
		{
			int boxWidth = 12;

			top += 4;

			if (direction == 1)
			{
				boxWidth -= 2;

				Display.FillRect(left, top, length, boxWidth, boxColor);

			}
			else
			{
				length -= 4;

				Display.FillRect(left, top, boxWidth, length, boxColor);
			}

		}

		/****************************************************************************
		 *	void DrawInnerLine(LPDIRECTDRAWSURFACE7 pDDS, int left, int top,		*
		 *				int direction,  int length, unsigned int innerColor)		*
		 *																			*
		 *																			*
		 *  This function draws the inner border at the location specified.			*
		 *																			*
		 *	Parameters:	the direct draw surface to draw to, the left and top		*
		 *		coordinates, direction = 1 for drawing to the right, or 0 for down,	*
		 *		the length of the line, and the color to draw.						*
		 *  Returns:	void														*
		 ****************************************************************************/
		public static void DrawInnerLine(int left, int top, int direction,
					  int length, Color innerColor)
		{
			int boxWidth = 12;
			int innerOffsetH = 8;
			int innerOffsetV = 4;
			int innerWidth = 2;

			top += 2;

			if (direction == 1)
			{
				Display.FillRect(left + innerOffsetH,
					top + innerOffsetV,
					length - boxWidth,
					innerWidth,
					innerColor);
			}
			else
			{

				Display.FillRect(left + innerOffsetH,
					top + innerOffsetV,
					innerWidth,
					length - boxWidth,
					innerColor);

			}

		}

		/****************************************************************************
		 *  void WriteText ( LPDIRECTDRAWSURFACE7 pDDS, int px, int py,				*
		 *					 const char *theText, const unsigned int* coloring)		*
		 *																			*
		 *  This function is the message driver that writes to our direct draw surface	*
		 *	the message we want at the x,y point given in our font.					*
		 *	The color is is overloaded so an array of coloring can be passed, or	*
		 *	just a single color.													*
		 ****************************************************************************/
		public static void WriteText(int px, int py, string theText)
		{
			WriteText(px, py, theText, XleColor.White);
		}
		public static void WriteText(int px, int py, string theText, Color c)
		{
			if (string.IsNullOrEmpty(theText)) return;

			int i, len = theText.Length + 1;
			Color[] coloring = new Color[len];

			for (i = 0; i < len; i++)
			{
				coloring[i] = c;
			}

			WriteText(px, py, theText, coloring);

		}

		/// <summary>
		/// Writes out the specified text to the back buffer.
		/// </summary>
		/// <param name="px"></param>
		/// <param name="py"></param>
		/// <param name="theText"></param>
		/// <param name="coloring"></param>
		public static void WriteText(int px, int py, string theText, Color[] coloring)
		{
			if (string.IsNullOrEmpty(theText))
				return;

			int i;
			int fx, fy;

			int len = theText.Length;
			Color color;

			for (i = 0; i < len; i++, px += 16)
			{
				char c = theText[i];

				if (coloring != null)
				{
					if (i < coloring.Length)
						color = coloring[i];
					else
					{
						System.Diagnostics.Debug.WriteLine("Warning: coloring array was too short.");
						color = coloring[coloring.Length - 1];
					}
				}
				else
				{
					color = XleColor.White;
				}

				///  removed the new graphics because colored message looks like crap.  I need to 
				///	 antialias it some other way
				fx = c % 16 * 16;//+ 256 * g.newGraphics;
				fy = (int)(c / 16) * 16;

				Factory.Font.Color = color;

				Factory.Font.DrawText(px, py, c.ToString());
			}
		}

		/****************************************************************************
		 *  void DrawTile ( LPDIRECTDRAWSURFACE7 pDDS, int px, int py,				*
		 *					 int tile)												*
		 *																			*
		 *  This function drives the tiles that are printed on the screen for the	*
		 *	maps.  It takes an x and y coordinate and a tile number, then prints	*
		 *	it on the screen.														*
		 ****************************************************************************/
		public static void DrawTile(int px, int py, int tile)
		{
			int tx, ty;

			Rectangle tileRect;
			Rectangle destRect;

			tx = tile % 16 * 16;
			ty = (int)(tile / 16) * 16;

			tileRect = new Rectangle(tx, ty, 16, 16);
			destRect = new Rectangle(px, py, 16, 16);

			Tiles.Draw(tileRect, destRect);
		}

		/****************************************************************************
		 *  DrawMonster( LPDIRECTDRAWSURFACE7 pDDS, int px, int py, int monst)		*											*
		 *																			*
		 *  This function drives monsters when they are displayed					*
		 ****************************************************************************/
		/// <summary>
		/// Draws monsters on the outside maps.
		/// </summary>
		/// <param name="px">The x position of the monster, in screen coordinates.</param>
		/// <param name="py">The y position of the monster, in screen coordinates.</param>
		/// <param name="monst"></param>
		public static void DrawMonster(int px, int py, int monst)
		{
			int tx, ty;

			Rectangle monstRect;
			Rectangle destRect;

			tx = (monst % 8) * 64;
			ty = (monst / 8) * 64;

			monstRect = new Rectangle(tx, ty, 64, 64);
			destRect = new Rectangle(px, py, 64, 64);

			if (Factory.Monsters != null)
				Factory.Monsters.Draw(monstRect, destRect);

		}

		public static Color PlayerColor { get; set; }
		/// <summary>
		/// Draws the player character.
		/// </summary>
		/// <param name="animFrame"></param>
		/// <param name="vertLine"></param>
		static void DrawCharacter(bool animating, int animFrame, int vertLine)
		{
			DrawCharacter(animating, animFrame, vertLine, PlayerColor);
		}
		static void DrawCharacter(bool animating, int animFrame, int vertLine, Color clr)
		{
			int px = vertLine + 16;
			int py = 16 + 7 * 16;
			int width = (624 - px) / 16;

			px += 11 * 16;

			DrawCharacterSprite(px, py, GameState.Player.FaceDirection, animating, animFrame, true, clr);

			CharRect = new Rectangle(px, py, 32, 32);
		}

		public static void DrawCharacterSprite(int destx, int desty, Direction facing, bool animating, int animFrame, bool allowPingPong, Color clr)
		{
			int tx = 0, ty;

			Rectangle charRect;
			Rectangle destRect;

			if (allowPingPong && (facing == Direction.North || facing == Direction.South))
			{
				animFrame %= 4;

				// ping-pong animation
				if (animFrame == 3)
					animFrame = 1;
			}
			else
			{
				animFrame %= 3;
			}

			if (animating)
				tx = (1 + animFrame) * 32;

			ty = ((int)facing - 1) * 32;

			charRect = new Rectangle(tx, ty, 32, 32);
			destRect = new Rectangle(destx, desty, 32, 32);

			Factory.Character.Color = clr;
			Factory.Character.Draw(charRect, destRect);
		}

		public static Rectangle CharRect { get; private set; }

		/// <summary>
		/// Draws the rafts that should be on the screen.
		/// </summary>
		/// <param name="inRect"></param>
		static void DrawRafts(Rectangle inRect)
		{
			Player player = GameState.Player; 
			int tx, ty;
			int lx = inRect.Left;
			int width = inRect.Width;
			int px = lx + (int)((width / 16) / 2) * 16;
			int py = 144;
			int rx, ry;
			Rectangle charRect;
			Rectangle destRect;

			tx = g.raftAnim * 32;
			ty = 256;

			charRect = new Rectangle(tx, ty, 32, 32);

			for (int i = 1; i < player.Rafts.Count; i++)
			{
				RaftData raft = player.Rafts[i];

				if (GameState.Map.MapID != raft.MapNumber)
					continue;

				rx = px - (player.X - raft.X) * 16;
				ry = py - (player.Y - raft.Y) * 16;

				if (i == player.OnRaft)
				{
					if (g.raftFacing == Direction.West)
					{
						charRect = new Rectangle(tx, ty + 64, 32, 32);
					}
					else
					{
						charRect = new Rectangle(tx, ty + 32, 32, 32);
					}
				}
				else
				{
					charRect = new Rectangle(tx, ty, 32, 32);
				}

				if (rx >= lx && ry >= 16 && rx <= 592 && ry < 272)
				{
					destRect = new Rectangle(rx, ry, 32, 32);

					Factory.Character.Draw(charRect, destRect);
				}
			}
		}

		/****************************************************************************
		 *	void ChangeScreenMode()													*
		 *																			*
		 *																			*
		 *  This function toggles between full screen and windowed.	 It currently	*
		 *	will end the program if it fails.										*
		 *																			*
		 *	Parameters:	none.														*
		 *  Returns:	void														*
		 ****************************************************************************/
		public static void ChangeScreenMode()
		{
			//Rectangle rect;
			/*
			//  Release all the directdraw surfaces so we can change modes.
			g.Unlock();
			g.ReleaseFont();
			DDDestroySurfaces();

			if (g_bFullScreen)
			{
				// Go to windowed
				if (!DDCreateSurfaces(false, actualWindowWidth, actualWindowHeight, g_iBpp))
				{
					MessageBox(NULL, TEXT("Couldn't create DirectDraw surfaces!"), TEXT("Failed"), 0);

					return;
				}

				// Restore the window size and position
				MoveWindow(g.hwnd(), g.screenLeft, g.screenTop,
					actualWindowWidth + GetSystemMetrics(SM_CXSIZEFRAME) * 2,
					actualWindowHeight + GetSystemMetrics(SM_CYSIZEFRAME) * 2 + GetSystemMetrics(SM_CYMENU) + GetSystemMetrics(SM_CYCAPTION), true);

			}
			else
			{
				// Save the window size and position
				GetWindowRect(g.hwnd(), &rect);

				g.screenLeft = rect.left;
				g.screenTop = rect.top;

				// Go to fullscreen mode
				if (!DDCreateSurfaces(true, actualWindowWidth, actualWindowHeight, g_iBpp))
				{
					MessageBox(NULL, TEXT("Couldn't create DirectDraw surfaces!"), TEXT("Failed"), 0);

					return;
				}

				// Cover up the top left section of the screen
				MoveWindow(g.hwnd(), -GetSystemMetrics(SM_CXSIZEFRAME), -GetSystemMetrics(SM_CYCAPTION) - GetSystemMetrics(SM_CYSIZEFRAME),
					actualWindowWidth + GetSystemMetrics(SM_CXSIZEFRAME) * 2,
					actualWindowHeight + GetSystemMetrics(SM_CYSIZEFRAME) * 2 + GetSystemMetrics(SM_CYMENU) + GetSystemMetrics(SM_CYCAPTION), true);

			}

			g.LoadFont();
			g.Lock();
			g.ResetTimers();		// reset the animation timers
			*/
		}

		/****************************************************************************
		 *	void wait(int howLong)													*
		 *																			*
		 *  														*
		 *																			*
		 *	Parameters:	the number of milliseconds to wait							*
		 *  Returns:	void														*
		 ****************************************************************************/
		/// <summary>
		/// This function is a message-friendly wait that will continue screen
		/// drawing and animation.  It allows the code to pause for a specified
		/// amount of time.	
		/// </summary>
		/// <param name="howLong"></param>
		public static void Wait(int howLong)
		{
			Wait(howLong, false, Redraw);
		}
		public static void Wait(int howLong, Action redraw)
		{
			Wait(howLong, false, redraw);
		}
		public static void Wait(int howLong, bool keyBreak, Action redraw)
		{
			Timing.StopWatch watch = new Timing.StopWatch();

			if (Display.CurrentWindow.IsClosed)
				return;

			do
			{
				inst.UpdateAnim();

				redraw();
				Core.KeepAlive();

				if (keyBreak && Keyboard.AnyKeyPressed)
					break;

			} while (watch.TotalMilliseconds < howLong && g.Done == false && Display.CurrentWindow.IsClosed == false);
		}

		/// <summary>
		/// This function creates a sub menu in the top of the map section and
		/// forces the player to chose an option from the list provided.	
		/// </summary>
		/// <param name="title"></param>
		/// <param name="choice"></param>
		/// <param name="items">A MenuItemList collection of menu items</param>
		/// <returns>The choice the user made.</returns>
		public static int SubMenu(string title, int choice, MenuItemList items)
		{
			string displayTitle;
			SubMenu menu = new SubMenu();

			menu.title = title;
			menu.value = choice;
			menu.theList = items;
			menu.width = 0;

			for (int i = 0; i < menu.theList.Count; i++)
			{
				if (menu.theList[i].Length + 6 > menu.width)
				{
					menu.width = menu.theList[i].Length + 6;
				}

			}

			displayTitle = "Choose " + menu.title;

			if (displayTitle.Length + 2 > menu.width)
			{
				menu.width = displayTitle.Length + 2;
			}

			Action redraw =
				delegate()
				{
					inst.UpdateAnim();

					Display.BeginFrame();

					inst.Draw();
					DrawMenu(menu);

					Display.EndFrame();
					Core.KeepAlive();
				};

			KeyCode key;

			do
			{
				key = WaitForKey(redraw);

				if (key == KeyCode.Up)
				{
					menu.value--;
					if (menu.value < 0)
						menu.value = 0;
				}
				if (key == KeyCode.Down)
				{
					menu.value++;
					if (menu.value >= items.Count)
						menu.value = items.Count - 1;
				}
				else if (key >= KeyCode.D0)
				{
					int v;

					if (key >= KeyCode.A)
					{
						v = (int)(key) - (int)(KeyCode.A);
					}
					else
					{
						v = key - KeyCode.D0;
					}

					if (v < items.Count)
					{
						menu.value = v;
						key = KeyCode.Return;
					}


				}


			} while (key != KeyCode.Return && !g.Done);

			Wait(300);


			return menu.value;

		}

		/// <summary>
		/// Waits for one of the specified keys, while redrawing the screen.
		/// </summary>
		/// <param name="keys">A list of keys which will break out of the wait. 
		/// Pass none for any key to break out.</param>
		/// <returns></returns>
		public static KeyCode WaitForKey(params KeyCode[] keys)
		{
			return WaitForKey(Redraw, keys);
		}
		/// <summary>
		/// Waits for one of the specified keys, while calling the delegate
		/// to redraw the screen.
		/// </summary>
		/// <param name="redraw"></param>
		/// <param name="keys">A list of keys which will break out of the wait. 
		/// Pass none for any key to break out.</param>
		/// <returns></returns>
		public static KeyCode WaitForKey(Action redraw, params KeyCode[] keys)
		{
			KeyCode key = KeyCode.None;
			bool done = false;

			InputEventHandler keyhandler =
				delegate(InputEventArgs e)
				{
					key = e.KeyCode;
				};

			PromptToContinue = PromptToContinueOnWait;

			Keyboard.ReleaseAllKeys();
			Keyboard.KeyDown += keyhandler;

			do
			{
				redraw();

				if (Display.CurrentWindow.IsClosed == true)
				{
					if (keys.Length > 0)
						key = keys[0];
					else
						key = KeyCode.Escape;

					break;
				}

				if ((keys == null || keys.Length == 0) && key != KeyCode.None)
					break;

				for (int i = 0; i < keys.Length; i++)
				{
					if (keys[i] == key)
					{
						done = true;
						break;
					}
				}

			} while (!done && Display.CurrentWindow.IsClosed == false);


			Keyboard.KeyDown -= keyhandler;

			PromptToContinue = false;
			PromptToContinueOnWait = true;

			return key;
		}

		/// <summary>
		/// Set to false to have WaitForKey not display a prompt 
		/// with the standard drawing method.
		/// </summary>
		public static bool PromptToContinueOnWait { get; set; }
		/// <summary>
		/// Set to true to show the (press to cont) prompt.
		/// </summary>
		public static bool PromptToContinue { get; set; }

		/// <summary>
		/// Draws the submenu created by SubMenu.
		/// </summary>
		/// <param name="menu"></param>
		static void DrawMenu(SubMenu menu)
		{
			string thestring;
			int xx, yy, i = 0, height;
			string buffer;
			Color fontColor = Map.DefaultColor;

			xx = 624 - menu.width * 16;
			yy = 16;
			height = (menu.theList.Count + 3) * 16;

			if (xx < g.vertLine + 16)
			{
				xx = g.vertLine + 16;
				i = 1;
			}

			Display.FillRect(xx, yy, 624 - xx, height, XleColor.Black);


			if (i == 0)
			{
				xx += 16;
			}

			thestring = menu.title;

			WriteText(xx + (int)((624 - xx) / 32) * 16 - (int)(thestring.Length / 2) * 16,
					   yy, thestring, fontColor);

			yy += 16;

			for (i = 0; i < menu.theList.Count; i++)
			{
				yy += 16;
				buffer = menu.theList[i];

				if (i > 9)
					thestring = ((char)(i + 'A' - 10)).ToString();
				else
					thestring = i.ToString();

				thestring += ". " + buffer;

				WriteText(xx, yy, thestring);

				if (i == menu.value)
				{
					int xx1;

					xx1 = xx + thestring.Length * 16;
					WriteText(xx1, yy, "`");
				}


			}


		}

		/// <summary>
		/// Gives the player a yes/no choice.
		/// </summary>
		/// <returns>Returns 0 if the player chose yes, 1 if the player chose no.</returns>
		public static int QuickMenuYesNo()
		{
			return XleCore.QuickMenu(new MenuItemList("Yes", "No"), 3);
		}
		/// <summary>
		/// This function creates a quick menu at the bottow of the screen,
		/// allowing the player to pick from a few choices.	
		/// </summary>
		/// <param name="items">The items in the list.</param>
		/// <param name="spaces"></param>
		/// <returns></returns>
		public static int QuickMenu(MenuItemList items, int spaces)
		{
			return QuickMenu(items, spaces, 0, XleCore.FontColor, XleCore.FontColor);
		}
		public static int QuickMenu(MenuItemList items, int spaces, int value = 0)
		{
			return QuickMenu(items, spaces, value, XleCore.FontColor, XleCore.FontColor);
		}
		public static int QuickMenu(MenuItemList items, int spaces, int value, Color clrInit)
		{
			return QuickMenu(items, spaces, value, clrInit, clrInit);
		}
		public static int QuickMenu(MenuItemList items, int spaces, int value, Color clrInit, Color clrChanged)
		{
			return QuickMenu(Redraw, items, spaces, value, clrInit, clrChanged);
		}

		public static int QuickMenu(Action redraw, MenuItemList items, int spaces)
		{
			return QuickMenu(redraw, items, spaces, 0, XleCore.FontColor, XleCore.FontColor);
		}
		public static int QuickMenu(Action redraw, MenuItemList items, int spaces, int value)
		{
			return QuickMenu(redraw, items, spaces, value, XleCore.FontColor, XleCore.FontColor);
		}
		public static int QuickMenu(Action redraw, MenuItemList items, int spaces, int value, Color clrInit)
		{
			return QuickMenu(redraw, items, spaces, value, clrInit, clrInit);
		}

		public static int QuickMenu(Action redraw, MenuItemList items, int spaces, int value, Color clrInit, Color clrChanged)
		{
			int[] spacing = new int[18];
			int last = 0;
			string tempLine = "Choose: ";
			string topLine;
			string tempItem;
			Color[] colors = new Color[40];


			for (int i = 0; i < 40; i++)
				colors[i] = clrChanged;


			spacing[0] = 8;

			// Construct the temporary line
			for (int i = 0; i < items.Count; i++)
			{
				tempItem = items[i];

				tempLine += tempItem + new string(' ', spaces);

				spacing[i] += last + tempItem.Length - 1;
				last = spacing[i] + spaces + 1;
			}

			g.AddBottom(tempLine, clrInit);
			g.AddBottom("");

			topLine = tempLine;
			tempLine = new string(' ', spacing[value]) + "`";

			g.UpdateBottom(tempLine, clrInit);

			KeyCode key;

			do
			{
				key = WaitForKey(redraw);

				if (key == KeyCode.Left)
				{
					value--;
					if (value < 0)
						value = 0;
				}
				if (key == KeyCode.Right)
				{
					value++;
					if (value >= items.Count)
						value = items.Count - 1;
				}
				else if (key >= KeyCode.D0)
				{
					for (int i = 0; i < items.Count; i++)
					{
						tempItem = items[i];

						if (key - KeyCode.A ==
							char.ToUpperInvariant(tempItem[0]) - 'A')
						{
							value = i;
							key = KeyCode.Return;
						}
					}
				}

				tempLine = new string(' ', spacing[value]) + "`";

				if (key != KeyCode.None)
				{
					g.UpdateBottom(topLine, 1, colors);
					g.UpdateBottom(tempLine, clrChanged);
				}


			} while (key != KeyCode.Return && Display.CurrentWindow.IsClosed == false);

			Wait(100, redraw);

			g.AddBottom("");

			return value;

		}

		/// <summary>
		/// Animates the rafts.
		/// </summary>
		static void RaftAnim()
		{
			if (lastRaftAnim + 100 < Timing.TotalMilliseconds)
			{
				g.raftAnim++;

				if (g.raftAnim == 3)
					g.raftAnim = 0;

				lastRaftAnim = Timing.TotalMilliseconds;
			}
		}

		/****************************************************************************
		 *	void CheckJoystick()													*
		 *																			*
		 *  This function checks the current state of the joystick, and generates	*
		 *	a key event if there is any action happeninGlobal.  It also handles the		*
		 *	menu when the button is held.											*
		 *																			*
		 *	Parameters:	none														*
		 *  Returns:	void														*
		 ****************************************************************************/
		//static int buttonTime = 0;		// time when the button was held down
		//static int buttonHeld = 0;		// are they holding the button down?
		//static int lastMove = 0;

		static void CheckJoystick()
		{
			return;
			/*
			HRESULT hr;
			DIJOYSTATE js;					// DInput joystick state 
			int key = 0;

			if (g.pJoystick)
			{
				do
				{
					// Poll the device to read the current state
					hr = g.pJoystick->Poll();

					// Get the input's device state
					hr = g.pJoystick->GetDeviceState(sizeof(DIJOYSTATE), &js);

					if (hr == DIERR_INPUTLOST || hr == DIERR_NOTACQUIRED)
					{
						// DInput is telling us that the input stream has been
						// interrupted. We aren't tracking any state between polls, so
						// we don't have any special reset that needs to be done. We
						// just re-acquire and try again.
						hr = g.pJoystick->Acquire();
						if (FAILED(hr))
							return;
					}
				}
				while (DIERR_INPUTLOST == hr);

				if (FAILED(hr))
					return;

				if (js.lX < -500)
				{
					key = VK_LEFT;
				}
				else if (js.lX > 500)
				{
					key = VK_RIGHT;
				}
				else if (js.lY < -500)
				{
					key = VK_UP;
				}
				else if (js.lY > 500)
				{
					key = VK_DOWN;
				}
				else if (js.rgbButtons[0] & 0x80 && buttonHeld == 0)
				{
					key = VK_RETURN;
				}

				if (g.quickMenu == true && key > 0 && buttonHeld == 0)
				{
					if (lastMove + 200 < clock())
					{
						lastMove = clock();
						g.menuKey = key;
					}
				}
				else if (key > 0 && (key != VK_RETURN || g.commandMode != 10))
				{
					lastMove = clock();
					g.menuKey = key;
				}


				if (js.rgbButtons[0] & 0x80 && buttonHeld == 0 && g.commandMode == 10)
				{
					buttonHeld = 1;
					buttonTime = clock();
					OutputDebugstring("Held button.\n");

					//g.commandMode = (CmdMode)10;

				}
				else if (js.rgbButtons[0] & 0x80 && g.commandMode == 20 && buttonHeld == 0)
				{

					g.commandMode = cmdEnterCommand;
					buttonHeld = 2;

					g.menuKey = VK_RETURN;

					g.LeftMenuActive = false;
					lastMove = clock();

				}
				else if (js.rgbButtons[0] & 0x80 && buttonHeld == 1)
				{
					if (g.quickMenu == true)
					{
						g.menuKey = VK_RETURN;
						buttonHeld = 2;
					}
					else if (clock() - buttonTime > 700 && g.commandMode == cmdEnterCommand)
					{
						// TODO:  wtf???
						g.commandMode = (CmdMode)20;
						buttonHeld = 2;

						g.LeftMenuActive = true;
						lastMove = clock();

					}
				}
				else if (js.rgbButtons[0] & 0x80)
				{
					buttonHeld = 2;

				}
				else if (buttonHeld == 2 && !(js.rgbButtons[0] & 0x80))
				{
					buttonHeld = 0;
				}
				else if (buttonHeld == 1)
				{
					if (g.commandMode == (CmdMode)20)
						g.commandMode = cmdEnterCommand;

					buttonHeld = 2;

					g.menuKey = VK_RETURN;
				}

			}
			*/
		}

		/// <summary>
		/// Asks the user to choose a number.
		/// </summary>
		/// <param name="max">The maximum value the user is allowed to select.</param>
		/// <returns></returns>
		public static int ChooseNumber(int max)
		{
			return ChooseNumber(Redraw, max);
		}
		public static int ChooseNumber(Action redraw, int max)
		{
			int method = 0;
			int amount = 0;

			g.AddBottom("");

			ColorStringBuilder builder = new ColorStringBuilder();

			builder.AddText("Enter number by ", XleColor.White);
			builder.AddText("keyboard", XleColor.Yellow);
			builder.AddText(" or ", XleColor.White);
			builder.AddText("joystick", XleColor.Cyan);

			g.AddBottom(builder);
			g.AddBottom();

			KeyCode key;


			do
			{
				XleCore.PromptToContinueOnWait = false;

				key = WaitForKey(redraw);

				if (method == 0)
				{
					switch (key)
					{
						case KeyCode.None:
							break;

						case KeyCode.Right:
						case KeyCode.Up:
						case KeyCode.Left:
						case KeyCode.Down:

							g.AddBottom("Use joystick - press button when done");
							g.AddBottom("");
							g.AddBottom("  Horizontal - Slow change", XleColor.Cyan);
							g.AddBottom("  Vertical   - Fast change", XleColor.Cyan);
							g.AddBottom("                          - 0 -");

							method = 2;

							break;
						default:
							g.AddBottom("Keyboard entry-press return when done", XleColor.Yellow);
							g.AddBottom("");
							g.AddBottom("");
							g.AddBottom("                          - 0 -");
							method = 1;

							break;
					}

				}

				if (method == 1)
				{
					if (key >= KeyCode.D0 && key <= KeyCode.D9)
						amount = 10 * amount + key - KeyCode.D0;

					if (key == KeyCode.BackSpace)
						amount /= 10;

					if (amount > max)
						amount = max;

					if (amount < 0)
						amount = 0;

					g.UpdateBottom("                          - " + amount.ToString() + " -");

				}
				else if (method == 2)
				{
					switch (key)
					{
						case KeyCode.Right:
							amount++;
							break;
						case KeyCode.Up:
							amount += 20;
							break;
						case KeyCode.Left:
							amount--;
							break;
						case KeyCode.Down:
							amount -= 20;
							break;
					}

					if (amount > max)
						amount = max;

					if (amount < 0)
						amount = 0;

					g.UpdateBottom("                          - " + amount.ToString() + " -");
				}


			} while (key != KeyCode.Return && !g.Done);

			XleCore.PromptToContinueOnWait = true;

			return amount;
		}

		/// <summary>
		/// The getter will return the first character of the command selected.  
		/// The setter allows setting the command.  If a matching command is  
		/// found, then it will set the cursor at that command.
		/// If it's passed a value that's not in the menu, nothing changes.
		/// </summary>
		public char cursor
		{
			get
			{
				return theCursor;
			}
			set
			{
				char newCursor = char.ToUpperInvariant(value);

				if (newCursor != 0)
				{
					for (int i = 0; i < 14; i++)
					{
						if (string.IsNullOrEmpty(menuArray[i]))
							continue;

						if (menuArray[i][0] == newCursor)
						{
							theCursor = newCursor;
						}
					}
				}
			}
		}

		/// <summary>
		/// This will return the numerical position of the cursor so that it may 	
		/// be plotted on the screen.	
		/// </summary>
		/// <returns></returns>
		[Obsolete]
		int CursorPos
		{
			get
			{
				for (int i = 0; i < 14; i++)
				{
					if (menuArray[i][0] == theCursor)
						return i;
				}

				return 0;
			}
			set
			{
				bool found = false;
				int change = value - theCursor;

				do
				{
					theCursor += (char)change;

					if (theCursor < 'A')
						theCursor = 'Z';

					if (theCursor > 'Z')
						theCursor = 'A';

					for (int i = 0; i < 14; i++)
					{

						if (menuArray[i][0] == theCursor)
						{
							found = true;
							return;
						}
					}
				} while (!found && change != 0);

			}
		}

		/// <summary>
		/// This returns an entire line from the menu that matches the first
		/// character of the command selected.  If it's passed
		/// a value that's not in the menu, it returns an empty string.
		/// </summary>
		/// <param name="newCursor"></param>
		/// <returns></returns>
		public static string Menu(KeyCode newCursor)
		{
			return inst.MenuImpl(newCursor);
		}
		private string MenuImpl(KeyCode newCursor)
		{
			char keychar = (char)('A' + (newCursor - KeyCode.A));

			for (int i = 0; i < menuArray.Length; i++)
			{
				if (menuArray[i].Length == 0)
					continue;

				if (menuArray[i][0] == keychar)
				{
					return menuArray[i];
				}

			}

			return "";
		}

		[Obsolete("Use GameState.Player instead.")]
		public static void SetPlayer(Player thePlayer)
		{
			GameState.Player = thePlayer;
		}

		public static void LoadTiles(string tileset)
		{
			if (tileset.EndsWith(".png") == false)
				tileset += ".png";

			Tiles = new Surface(tileset);
		}

		public static void ChangeMap(Player player, int mMapID, int targetEntryPoint, int targetX, int targetY)
		{
			if (GameState.Map == null)
			{
				player.MapID = mMapID;
				return;
			}

			if (GameState.Map is XleMapTypes.Outside)
			{
				player.SetReturnLocation(player.MapID, player.X, player.Y, Direction.South);
			}

			var saveMap = GameState.Map;
			var saveX = player.X;
			var saveY = player.Y;

			try
			{
				if (mMapID != 0 && saveMap != null && mMapID != saveMap.MapID)
				{
					GameState.Map = LoadMap(mMapID);
					player.MapID = mMapID;

					TextArea.Clear();
				}

				if (targetEntryPoint < 0 || targetEntryPoint >= Map.EntryPoints.Count)
				{
					player.X = targetX;
					player.Y = targetY;

					if (targetEntryPoint >= 0)
					{
						TextArea.PrintLine("Failed to find entry point " + targetEntryPoint.ToString(), XleColor.Yellow);
						TextArea.PrintLine();
					}
				}
				else
				{
					GameState.Map.BeforeEntry(GameState, ref targetEntryPoint);

					var ep = GameState.Map.EntryPoints[targetEntryPoint];

					player.X = ep.Location.X;
					player.Y = ep.Location.Y;
					player.DungeonLevel = ep.DungeonLevel;

					if (ep.Facing != Direction.None)
					{
						player.FaceDirection = ep.Facing;
					}
				}

				if (mMapID != 0)
				{
					GameState.Map.OnLoad(player);
				}

				GameState.Map.GameState = GameState;
				SetTilesAndCommands();
			}
			catch (Exception e)
			{
				player.MapID = saveMap.MapID;
				GameState.Map = saveMap;
				player.X = saveX;
				player.Y = saveY;

				throw e;
			}

			CheckLoan(player);
		}

		static void CheckLoan(Player player)
		{
			if (XleCore.Map.Events.Any(x => x is StoreLending))
			{
				if (player.loan > 0 && player.dueDate - player.TimeDays <= 0)
				{
					TextArea.PrintLine("This is your friendly lender.");
					TextArea.PrintLine("You owe me money!");

					XleCore.Wait(1000);
				}
			}
		}
	}
}