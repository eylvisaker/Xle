using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using ERY.Xle.Commands;
using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using ERY.Xle.Rendering;
using ERY.Xle.XleEventTypes;
using ERY.Xle.XleEventTypes.Stores;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml;

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

		private XleGameFactory mFactory;

		private MapList mMapList = new MapList();
		private ItemList mItemList = new ItemList();
		private EquipmentList mWeaponList = new EquipmentList();
		private EquipmentList mArmorList = new EquipmentList();
		private Dictionary<int, string> mQualityList = new Dictionary<int, string>();
		private Dictionary<int, ExhibitInfo> mExhibitInfo = new Dictionary<int, ExhibitInfo>();
		private Dictionary<int, Map3DExtraInfo> mMap3DExtras = new Dictionary<int, Map3DExtraInfo>();
		private Dictionary<int, MagicSpell> mMagicSpells = new Dictionary<int, MagicSpell>();

		private Data.AgateDataImport mDatabase;

		
		public static TextArea TextArea { get; private set; }

		public XleCore()
		{
			inst = this;
			Renderer = new XleRenderer();

			Renderer.PlayerColor = XleColor.White;

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

				DisplayWindow wind;
				
				if (EnableDebugMode)
				{
					Size windowSize = new Size(
						640 + windowBorderSize.Width * 2, 
						400 + windowBorderSize.Height * 2);

					wind = DisplayWindow.CreateWindowed(
						mFactory.GameTitle, windowSize.Width, windowSize.Height);
				}
				else
				{
					windowBorderSize.Width = 80;
					windowBorderSize.Height = 100;

					wind = DisplayWindow.CreateFullScreen(
						mFactory.GameTitle, 800, 600);
				}

				SoundMan.Load();

				mFactory.LoadSurfaces();

				IXleTitleScreen titleScreen;

				do
				{
					GameState = null;

					titleScreen = mFactory.CreateTitleScreen();
					titleScreen.Run();
					returnToTitle = false;

					RunGame(titleScreen.Player);

				} while (titleScreen.Player != null);
			}
		}

		static Size windowBorderSize = new Size(20, 20);

		public static void SetOrthoProjection(Color clearColor)
		{
			AgateLib.DisplayLib.Shaders.Basic2DShader shader = new AgateLib.DisplayLib.Shaders.Basic2DShader();
			
			shader.CoordinateSystem = new Rectangle(
				-windowBorderSize.Width, 
				-windowBorderSize.Height, 
				640 + windowBorderSize.Width * 2, 
				400 + windowBorderSize.Height * 2);

			shader.Activate();

			Display.Clear(clearColor);
		}
		public static void SetProjectionAndBackColors(ColorScheme cs)
		{
			SetOrthoProjection(cs.BorderColor);
			
			Display.FillRect(new Rectangle(0, 0, 640, 400), cs.BackColor);
			Display.FillRect(0, 296, 640, 104, cs.TextAreaBackColor);
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
			if (GameState.Map.HasGuards == false)
			{
				throw new InvalidOperationException("There are no guards on this map.");
			}

			GameState.Map.Guards.Clear();
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
		public static XleRenderer Renderer { get; set; }

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
		public static Dictionary<int, ExhibitInfo> ExhibitInfo
		{
			get { return inst.mExhibitInfo; }
		}
		public static Dictionary<int, Map3DExtraInfo> Map3DExtraInfo { get { return inst.mMap3DExtras; } }
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
			GameState.Commands = new CommandList(GameState);

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
			GameState.Commands.Items.Clear();

			GameState.Map.SetCommands(GameState.Commands);
			GameState.Commands.ResetCommands();

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
			XleCore.SetProjectionAndBackColors(GameState.Map.ColorScheme);

			Renderer.Draw();

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

			try
			{
				AcceptKey = false;

				if (Keyboard.Keys[KeyCode.Down]) GameState.Commands.DoCommand(KeyCode.Down);
				else if (Keyboard.Keys[KeyCode.Left]) GameState.Commands.DoCommand(KeyCode.Left);
				else if (Keyboard.Keys[KeyCode.Up]) GameState.Commands.DoCommand(KeyCode.Up);
				else if (Keyboard.Keys[KeyCode.Right]) GameState.Commands.DoCommand(KeyCode.Right);
			}
			finally
			{
				AcceptKey = true;
			}
		}

		void Keyboard_KeyDown(InputEventArgs e)
		{
			if (AcceptKey == false)
				return;

			try
			{
				AcceptKey = false;
				GameState.Commands.DoCommand(e.KeyCode);
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

		public static void FlashHPWhileSound(Color clr)
		{
			FlashHPWhileSound(clr, Renderer.FontColor);
		}
		public static void FlashHPWhileSound(Color clr, Color clr2)
		{
			FlashHPWhile(clr, clr2, () => SoundMan.IsAnyPlaying());

		}
		public static void FlashHPWhile(Color clr, Color clr2, Func<bool> pred)
		{
			Renderer.FlashHPWhile(clr, clr2, pred);

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
			SubMenu menu = new SubMenu();

			menu.title = title;
			menu.value = choice;
			menu.theList = items;

			return RunSubMenu(menu);
		}

		private static int RunSubMenu(SubMenu menu)
		{
			for (int i = 0; i < menu.theList.Count; i++)
			{
				if (menu.theList[i].Length + 6 > menu.width)
				{
					menu.width = menu.theList[i].Length + 6;
				}

			}

			string displayTitle = "Choose " + menu.title;

			if (displayTitle.Length + 2 > menu.width)
			{
				menu.width = displayTitle.Length + 2;
			}

			Action redraw = () =>
			{
				inst.UpdateAnim();

				Display.BeginFrame();
				XleCore.SetProjectionAndBackColors(GameState.Map.ColorScheme);

				Renderer.Draw();
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
					if (menu.value >= menu.theList.Count)
						menu.value = menu.theList.Count - 1;
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

					if (v < menu.theList.Count)
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

			InputEventHandler keyhandler = e => key = e.KeyCode;

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
			Color fontColor = GameState.Map.DefaultColor;

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

			Renderer.WriteText(xx + (int)((624 - xx) / 32) * 16 - (int)(thestring.Length / 2) * 16,
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

				Renderer.WriteText(xx, yy, thestring);

				if (i == menu.value)
				{
					int xx1;

					xx1 = xx + thestring.Length * 16;
					Renderer.WriteText(xx1, yy, "`");
				}


			}


		}

		/// <summary>
		/// Gives the player a yes/no choice, returning 0 if the player chose yes and
		/// 1 if the player chose no.
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
			return QuickMenu(items, spaces, 0, XleCore.Renderer.FontColor, XleCore.Renderer.FontColor);
		}
		public static int QuickMenu(MenuItemList items, int spaces, int value = 0)
		{
			return QuickMenu(items, spaces, value, XleCore.Renderer.FontColor, XleCore.Renderer.FontColor);
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
			return QuickMenu(redraw, items, spaces, 0, XleCore.Renderer.FontColor, XleCore.Renderer.FontColor);
		}
		public static int QuickMenu(Action redraw, MenuItemList items, int spaces, int value)
		{
			return QuickMenu(redraw, items, spaces, value, XleCore.Renderer.FontColor, XleCore.Renderer.FontColor);
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

			XleCore.TextArea.PrintLine(tempLine, clrInit);
			XleCore.TextArea.PrintLine();

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

			XleCore.TextArea.PrintLine();

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

			XleCore.TextArea.PrintLine();

			ColorStringBuilder builder = new ColorStringBuilder();

			builder.AddText("Enter number by ", XleColor.White);
			builder.AddText("keyboard", XleColor.Yellow);
			builder.AddText(" or ", XleColor.White);
			builder.AddText("joystick", XleColor.Cyan);

			g.AddBottom(builder);
			XleCore.TextArea.PrintLine();

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

							XleCore.TextArea.PrintLine("Use joystick - press button when done");
							XleCore.TextArea.PrintLine();
							XleCore.TextArea.PrintLine("  Horizontal - Slow change", XleColor.Cyan);
							XleCore.TextArea.PrintLine("  Vertical   - Fast change", XleColor.Cyan);
							XleCore.TextArea.PrintLine("                          - 0 -");

							method = 2;

							break;
						default:
							XleCore.TextArea.PrintLine("Keyboard entry-press return when done", XleColor.Yellow);
							XleCore.TextArea.PrintLine();
							XleCore.TextArea.PrintLine();
							XleCore.TextArea.PrintLine("                          - 0 -");
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

		public static void LoadTiles(string tileset)
		{
			if (tileset.EndsWith(".png") == false)
				tileset += ".png";

			Renderer.Tiles = new Surface(tileset);
		}

		public static void ChangeMap(Player player, int mMapID, int targetEntryPoint)
		{
			ChangeMapImpl(player, mMapID, targetEntryPoint, 0, 0);
		}
		[Obsolete]
		public static void ChangeMap(Player player, int mMapID, int targetEntryPoint, int targetX, int targetY)
		{
			ChangeMapImpl(player, mMapID, targetEntryPoint, targetX, targetY);
		}
		static void ChangeMapImpl(Player player, int mMapID, int targetEntryPoint, int targetX, int targetY)
		{
			if (GameState.Map == null)
			{
				player.MapID = mMapID;
				return;
			}

			if (GameState.Map is Outside)
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

				GameState.Map.GameState = GameState;

				if (mMapID != 0)
				{
					GameState.Map.OnLoad(player);
				}

				SetTilesAndCommands();
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.Print(e.Message);
				System.Diagnostics.Debug.Print(e.StackTrace);

				player.MapID = saveMap.MapID;
				GameState.Map = saveMap;
				player.X = saveX;
				player.Y = saveY;

				throw;
			}

			GameState.Map.OnAfterEntry(GameState);
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


		public void ProcessArguments(string[] args)
		{
			for(int i = 0; i < args.Length; i++)
			{
				switch(args[i])
				{
					case "-debug":
						EnableDebugMode = true;
						break;
				}
			}
		}

		public static bool EnableDebugMode { get; set; }
	}
}