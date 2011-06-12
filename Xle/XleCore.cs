#define Disable3D
#define ShowCoordinates
//#define StartFullScreen 

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;

namespace ERY.Xle
{
	public class XleCore
	{
		#region --- Static Members ---

		/// <summary>
		/// Entry point.
		/// </summary>
		/// <param name="args"></param>
		[STAThread]
		public static void Main(string[] args)
		{
			PromptToContinueOnWait = true;

			new XleCore().Run(args);
		}

		public const int myWindowWidth = 640;
		public const int myWindowHeight = 400;

		public static Random random = new Random();

		private static XleMap map;
		private static Player player;
		private static Commands commands;

		private static bool AcceptKey = true;

		private static XleCore inst;

		private static bool returnToTitle = false;

		public static XleMap Map
		{
			get { return XleCore.map; }
			set
			{
				XleCore.map = value;

				inst.menuArray = map.MapMenu();
				g.LoadTiles(map.TileSet);
			}
		}

		private static void ParseArgs(string[] args)
		{
			string gamePath = "Game";

			for (int i = 0; i < args.Length; i++)
			{
				if (args[i].StartsWith("-") == false)
				{
					System.Diagnostics.Debug.WriteLine("Could not understand argument " + args[i]);
					continue;
				}

				if (args[i].ToLower() == "-game")
					gamePath = args[++i];
			}

			System.IO.Directory.SetCurrentDirectory(gamePath);
		}

		#endregion

		private char theCursor = 'P';			// location of the menu dot
		private string[] menuArray = new string[17];		// keeps the menu portion of the screen

		private MapList mMapList = new MapList();
		private ItemList mItemList = new ItemList();
		private EquipmentList mWeaponList = new EquipmentList();
		private EquipmentList mArmorList = new EquipmentList();
		private Dictionary<int, string> mQualityList = new Dictionary<int, string>();
		private Dictionary<int, XleMapTypes.MuseumDisplays.ExhibitInfo> mExhibitInfo = new Dictionary<int,XleMapTypes.MuseumDisplays.ExhibitInfo>();

		public XleCore()
		{
			inst = this;
			for (int i = 0; i < 17; i++)
			{
				menuArray[i] = "";
			}
		}
		public void Run(string[] args)
		{
			ParseArgs(args);

			AgateLib.AgateFileProvider.Images.AddPath("Images");
			AgateLib.AgateFileProvider.Sounds.AddPath("Audio");
			AgateLib.Core.ErrorReporting.CrossPlatformDebugLevel = CrossPlatformDebugLevel.Exception;

			LoadGameFile();

			using (AgateSetup setup = new AgateSetup())
			{
				setup.InitializeAll();
				if (setup.WasCanceled)
					return;

				DisplayWindow wind = DisplayWindow.CreateWindowed("Legacy of the Ancients", 640, 400);

				SoundMan.Load();

				g.LoadSurfaces();

				XleTitleScreen titleScreen;

				do
				{
					titleScreen = new XleTitleScreen();
					titleScreen.Run();
					returnToTitle = false;

					RunGame(titleScreen.Player);

				} while (titleScreen.Player != null);

			}

		}

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
				}
			}
		}

		private static string GetOptionalAttribute(XmlNode node, string attrib, string defaultValue)
		{
			if (node.Attributes[attrib] != null)
				return node.Attributes[attrib].Value;
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

				mItemList.Add(id, name, action);
			}
		}
		private void LoadEquipmentInfo(XmlNode xmlNode, ref EquipmentList equipmentList)
		{
			for (int i = 0; i < xmlNode.ChildNodes.Count; i++)
			{
				XmlNode node = xmlNode.ChildNodes[i];

				int id = int.Parse(node.Attributes["ID"].Value);
				int basePrice = int.Parse(GetOptionalAttribute(node, "BasePrice", "0"));
				string name = node.Attributes["Name"].Value;

				equipmentList.Add(id, name, basePrice);
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

					mMapList.Add(id, name, filename);

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

					foreach (XmlNode child in node.ChildNodes)
					{
						if (child.Name == "Text")
						{
							int textID = int.Parse(child.Attributes["ID"].Value);
							string text = child.InnerText;

							info.Text.Add(textID, text);
						}
					}

					mExhibitInfo.Add(id, info);
				}
			}
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

		public static string GetWeaponName(int weaponID, int qualityID)
		{
			return inst.mQualityList[qualityID] + " " + inst.mWeaponList[weaponID].Name;
		}
		public static string GetArmorName(int armorID, int qualityID)
		{
			return inst.mQualityList[qualityID] + " " + inst.mArmorList[armorID].Name;
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

			player = thePlayer;
			player.MapChanged += new EventHandler(player_MapChanged);

			commands = new Commands(player);

			Map = LoadMap(player.Map);
			Map.OnLoad(player);

			Keyboard.KeyDown += new InputEventHandler(Keyboard_KeyDown);

			while (Display.CurrentWindow.IsClosed == false && returnToTitle == false)
			{
				Redraw();
			}

			Keyboard.KeyDown -= Keyboard_KeyDown;
		}

		void player_MapChanged(object sender, EventArgs e)
		{
			Map = LoadMap(player.Map);
		}

		public static void Redraw()
		{
			inst.RedrawImpl();
		}
		private void RedrawImpl()
		{
			UpdateAnim();

			Display.BeginFrame();

			map.UpdateAnim();

			Draw();

			Display.EndFrame();
			Core.KeepAlive();

			CheckArrowKeys();
		}
		private void CheckArrowKeys()
		{
			if (AcceptKey == false)
				return;

			AcceptKey = false;

			if (Keyboard.Keys[KeyCode.Down]) commands.DoCommand(KeyCode.Down);
			else if (Keyboard.Keys[KeyCode.Left]) commands.DoCommand(KeyCode.Left);
			else if (Keyboard.Keys[KeyCode.Up]) commands.DoCommand(KeyCode.Up);
			else if (Keyboard.Keys[KeyCode.Right]) commands.DoCommand(KeyCode.Right);

			AcceptKey = true;
		}

		void Keyboard_KeyDown(InputEventArgs e)
		{
			if (AcceptKey == false)
				return;

			AcceptKey = false;
			commands.DoCommand(e.KeyCode);

			AcceptKey = true;
		}

		// TODO: Which of these are obsolete?
		static bool updating = false;
		static double lastRaftAnim = 0;
		static double lastCharAnim = 0;
		static int lastOceanSound = 0;
		static double timer;
		static double frames = 0;
		static double fps;

		public void UpdateAnim()
		{
			RaftAnim();
			CheckAnim();

			map.UpdateAnim();
		}

		public void Draw()
		{
			int i = 0, j = 0;
			Color boxColor;
			Color innerColor;
			Color fontColor;
			Color menuColor;
			int horizLine = 18 * 16;
			int vertLine;

			map.GetBoxColors(out boxColor, out innerColor, out fontColor, out vertLine);

			menuColor = fontColor;

			if (g.HPColor == XleColor.Black)
			{
				g.HPColor = fontColor;
			}

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


#if ShowCoordinates
			// Show coordinates & framerate at top
			Display.FillRect(0, 0, 160, 16, XleColor.Black);
			Display.FillRect(256, 0, 128, 16, XleColor.Black);
			Display.FillRect(400, 0, 128, 16, XleColor.Black);
			Display.FillRect(544, 0, 96, 16, XleColor.Black);

			WriteText(272, 0, "X: " + player.X.ToString());
			WriteText(416, 0, "Y: " + player.Y.ToString());
			WriteText(560, 0, "F: " + player.FaceDirection.ToString()[0].ToString());

			WriteText(0, 0, "FPS: " + ((int)(Display.FramesPerSecond + 0.5)).ToString()
				+ "." + ((int)(Display.FramesPerSecond * 10) % 10).ToString());

#endif

			for (i = 0; i < menuArray.Length; i++)
			{
				WriteText(48, 16 * (i + 1), menuArray[i], menuColor);
			}

			WriteText(32, 16 * (CursorPos + 1), "`", menuColor);

			WriteText(48, 16 * 15, "H.P. " + player.HP.ToString(), g.HPColor);
			WriteText(48, 16 * 16, "Food " + player.Food.ToString(), g.HPColor);
			WriteText(48, 16 * 17, "Gold " + player.Gold.ToString(), g.HPColor);

			DrawBottomText();

			if (map.AutoDrawPlayer)
			{
				DrawRafts(mapRect);
				
				if (player.OnRaft == 0)
					DrawCharacter(g.AnimFrame, vertLine);
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

		public static void FlashHPWhileSound(Color clr)
		{
			Color oldClr = g.HPColor;
			Color lastColor = g.HPColor;

			while (SoundMan.IsAnyPlaying())
			{
				if (lastColor == clr)
					lastColor = oldClr;
				else
					lastColor = clr;

				g.HPColor = lastColor;

				XleCore.wait(80);
			}

			g.HPColor = oldClr;
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
		public static void DrawBottomText()
		{

			for (int i = 0; i < 5; i++)
			{
				int x = 16 + 16 * g.BottomMargin(i);

				WriteText(x, 368 - 16 * i, g.Bottom(i), g.BottomColor(i));
			}
		}

		public static int BottomTextMargin = 1;

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
			DrawLine(0, 0, 0, myWindowHeight, boxColor);
			DrawLine(0, myWindowHeight - 12, 1, myWindowWidth, boxColor);
			DrawLine(myWindowWidth - 12, 0, 0, myWindowHeight, boxColor);
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
			DrawInnerLine(0, 0, 0, myWindowHeight, innerColor);
			DrawInnerLine(0, myWindowHeight - 12, 1, myWindowWidth + 2, innerColor);
			DrawInnerLine(myWindowWidth - 12, 0, 0, myWindowHeight, innerColor);

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
			const int innerOffsetH = 8;
			const int innerOffsetV = 2;
			const int innerWidth = 2;

			top += 2;

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
			int innerOffsetV = 2;
			int innerWidth = 2;

			top += 2;

			if (direction == 1)
			{
				//boxWidth -= 4;
				//DDPutBox(pDDS, left + innerOffsetH,
				//                  top + innerOffsetV,
				//                  length - boxWidth,
				//                  innerWidth,
				//                  innerColor);

				Display.FillRect(left + innerOffsetH,
					top + innerOffsetV,
					length - boxWidth,
					innerWidth,
					innerColor);
			}
			else
			{
				//length -= 2;

				//DDPutBox(pDDS, left + innerOffsetH,
				//                  top + innerOffsetV,
				//                  innerWidth,
				//                  length - boxWidth,
				//                  innerColor);

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

				g.Font.Color = color;

				g.Font.DrawText(px, py, c.ToString());

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

			g.Tiles.Draw(tileRect, destRect);

		}

		/****************************************************************************
		 *  DrawMonster( LPDIRECTDRAWSURFACE7 pDDS, int px, int py, int monst)		*											*
		 *																			*
		 *  This function drives monsters when they are displayed					*
		 ****************************************************************************/
		/// <summary>
		/// Draws monsters on the outside maps.
		/// </summary>
		/// <param name="px"></param>
		/// <param name="py"></param>
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

			//pDDS->Blt(&destRect, g.Character(), &charRect,  DDBLT_WAIT | DDBLT_KEYSRC, NULL);
			//pDDS->BltFast(destRect.left, destRect.top, g.Monsters(), &monstRect, DDBLTFAST_SRCCOLORKEY);
			g.Monsters.Draw(monstRect, destRect);

		}

		/// <summary>
		/// Draws the player character.
		/// </summary>
		/// <param name="anim"></param>
		/// <param name="vertLine"></param>
		static void DrawCharacter(int anim, int vertLine)
		{
			if (g.invisible)
				DrawCharacter(anim, vertLine, XleColor.Gray);
			else if (g.guard)
				DrawCharacter(anim, vertLine, XleColor.Yellow);
			else
				DrawCharacter(anim, vertLine, XleColor.White);

		}
		static void DrawCharacter(int anim, int vertLine, Color clr)
		{
			int tx, ty;
			int px = vertLine + 16;
			int width = (624 - px) / 16;
			int py = 144;

			Rectangle charRect;
			Rectangle destRect;

			px += (int)(width / 2) * 16;

			tx = anim * 32;// +g.newGraphics * 96;
			ty = ((int)player.FaceDirection - 1) * 32;

			charRect = new Rectangle(tx, ty, 32, 32);
			destRect = new Rectangle(px, py, 32, 32);

			g.Character.Color = clr;
			g.Character.Draw(charRect, destRect);

			// todo: remove this
			g.Character.Color = XleColor.White;
		}

		/// <summary>
		/// Draws the rafts that should be on the screen.
		/// </summary>
		/// <param name="inRect"></param>
		static void DrawRafts(Rectangle inRect)
		{
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

				if (map.MapID != raft.MapNumber)
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
					//pDDS->Blt(&destRect, g.Character(), &charRect,  DDBLT_WAIT | DDBLT_KEYSRC, NULL);
					//pDDS->BltFast(destRect.left, destRect.top, g.Character(), &charRect, DDBLTFAST_SRCCOLORKEY);

					g.Character.Draw(charRect, destRect);
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


		static double lastCheckAnim = 0;

		/// <summary>
		/// This function animates the main character.  It should be called in the
		/// main drawing loop. However it doesn't seem to do anything right now and
		/// may be obsolete.
		/// </summary>
		[Obsolete]
		static void CheckAnim()
		{

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
		public static void wait(int howLong)
		{
			wait(Redraw, howLong, false);
		}
		public static void wait(RedrawDelegate redraw, int howLong)
		{
			wait(redraw, howLong, false);
		}
		public static void wait(RedrawDelegate redraw, int howLong, bool keyBreak)
		{
			Timing.StopWatch watch = new Timing.StopWatch();

			do
			{
				inst.UpdateAnim();

				redraw();
				Core.KeepAlive();

				if (keyBreak && Keyboard.AnyKeyPressed)
					break;

			} while (watch.TotalMilliseconds < howLong && g.Done == false);
		}

		/// <summary>
		/// Waits for a key or joystick press.
		/// </summary>
		[Obsolete]
		static void WaitKey()
		{
			Keyboard.ReleaseAllKeys();

			while (Keyboard.AnyKeyPressed == false)
				wait(1);

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

			RedrawDelegate redraw =
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

			wait(300);


			return menu.value;

		}

		public delegate void RedrawDelegate();

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
		public static KeyCode WaitForKey(RedrawDelegate redraw, params KeyCode[] keys)
		{
			KeyCode key = KeyCode.None;
			bool done = false;

			InputEventHandler keyhandler =
				delegate(InputEventArgs e)
				{
					key = e.KeyCode;
				};

			PromptToContinue = true;

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

			} while (!done);


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
		/// This function creates a quick menu at the bottow of the screen,
		/// allowing the player to pick from a few choices.	
		/// </summary>
		/// <param name="items">The items in the list.</param>
		/// <param name="spaces"></param>
		/// <returns></returns>
		public static int QuickMenu(MenuItemList items, int spaces)
		{
			return QuickMenu(items, spaces, 0, XleColor.White, XleColor.White);
		}
		public static int QuickMenu(MenuItemList items, int spaces, int value = 0)
		{
			return QuickMenu(items, spaces, value, XleColor.White, XleColor.White);
		}
		public static int QuickMenu(MenuItemList items, int spaces, int value, Color clrInit)
		{
			return QuickMenu(items, spaces, value, clrInit, clrInit);
		}
		public static int QuickMenu(MenuItemList items, int spaces, int value, Color clrInit, Color clrChanged)
		{
			return QuickMenu(Redraw, items, spaces, value, clrInit, clrChanged);
		}

		public static int QuickMenu(RedrawDelegate redraw, MenuItemList items, int spaces)
		{
			return QuickMenu(redraw, items, spaces, 0, XleColor.White, XleColor.White);
		}
		public static int QuickMenu(RedrawDelegate redraw, MenuItemList items, int spaces, int value)
		{
			return QuickMenu(redraw, items, spaces, value, XleColor.White, XleColor.White);
		}
		public static int QuickMenu(RedrawDelegate redraw, MenuItemList items, int spaces, int value, Color clrInit)
		{
			return QuickMenu(redraw, items, spaces, value, clrInit, clrInit);
		}

		public static int QuickMenu(RedrawDelegate redraw, MenuItemList items, int spaces, int value, Color clrInit, Color clrChanged)
		{
			int[] spacing = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
			int last = 0;
			string tempLine = "Choose: ";
			string topLine;
			string tempItem;
			Color[] colors = new Color[40];


			for (int i = 0; i < 40; i++)
				colors[i] = clrChanged;


			spacing[0] = 8;

			// Construct the temporray line
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

			wait(redraw, 100);

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
		static int buttonTime = 0;		// time when the button was held down
		static int buttonHeld = 0;		// are they holding the button down?
		static int lastMove = 0;

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
		public static int ChooseNumber(RedrawDelegate redraw, int max)
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
	}
}