using System;
using System.Collections.Generic;
using System.Text;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.Geometry;

namespace ERY.Xle
{

	// Directional definitions
	public enum Direction
	{
		None = 0,
		East = 1,
		North,
		West,
		South
	}

	// Terrain Definitions
	public enum TerrainType
	{
		All = -1,
		Water,
		Mountain,
		Grass,
		Forest,
		Desert,
		Swamp,
		Mixed,
		Foothills,
		Scrubland,
	}


	public static class g
	{
		static int animFrame;

		static FontSurface myFont { get { return XleCore.Factory.Font; }}	// stores the handle to the font
		static Surface myTiles { get { return XleCore.Tiles;}}		// stores the handle to the tiles
		static Surface myCharacter;		// stores the handle to the character sprites
		static Surface pOverlandMonsters;	// stores the handle to the overland monster sprites

		static string[] bottom = new string[5];			// keeps the bottom portion of the screen
		static int[] bottommargin = new int[5];
		static Color[][] bottomColor = new Color[5][];	// keeps the bottom colors on the screen


		// constructors and destructor:
		static g()
		{

			done = false;

			for (int i = 0; i < 5; i++)
			{
				bottomColor[i] = new Color[40];

				for (int j = 0; j < bottomColor[i].Length; j++)
					bottomColor[i][j] = XleColor.White;

			}

			raftFacing = Direction.East;
			charAnimCount = 0;
			LeftMenuActive = false;

			disableEncounters = false;

			walkTime = 150;

			invisible = false;
			guard = false;

		}

		// Surfaces
		static public FontSurface Font { get { return XleCore.Factory.Font;}}					// returns the handle to the font resource
		public static Surface Tiles { get { return XleCore.Tiles; } }				// returns the handle to the tiles resource
		public static Surface Character { get { return XleCore.Factory.Character; } }		// returns the handle to the character resource
		public static Surface Monsters { get { return XleCore.Factory.Monsters; } }				// returns the handle to the monsters resource

		public static Surface MuseumBackdrop { get { return XleCore.Factory.MuseumBackdrop; } }
		public static Surface MuseumWall { get { return XleCore.Factory.MuseumWall; } }			// stores the pointer to the wall texture
		public static Surface MuseumSidePassage { get { return XleCore.Factory.MuseumSidePassage; } }
		public static Surface MuseumDoor { get { return XleCore.Factory.MuseumDoor; } }
		public static Surface MuseumExhibitFrame { get { return XleCore.Factory.MuseumExhibitFrame; } }
		public static Surface MuseumExhibitStatic { get { return XleCore.Factory.MuseumExhibitStatic; } }
		public static Surface MuseumExtras { get { return XleCore.Factory.MuseumExtras; } }
		public static Surface MuseumCloseup { get { return XleCore.Factory.MuseumCloseup; } }

		public static Surface DungeonBlueBackdrop { get { return XleCore.Factory.DungeonBlueBackdrop; } }
		public static Surface DungeonBlueWall { get { return XleCore.Factory.DungeonBlueWall; } }
		public static Surface DungeonBlueSidePassage { get { return XleCore.Factory.DungeonBlueSidePassage; } }
		public static Surface DungeonBlueExtras { get { return XleCore.Factory.DungeonBlueExtras; } }

		// character functions


		static Timing.StopWatch animWatch = new Timing.StopWatch();
		const int frameTime = 150;

		static public int AnimFrame
		{
			get
			{
				int oldAnim = animFrame;

				if (animWatch.IsPaused == false)
					animFrame = (((int)animWatch.TotalMilliseconds) / frameTime) % 3;

				if (oldAnim != animFrame)
				{
					charAnimCount++;

					if (charAnimCount > 6)
					{
						animFrame = 0;
						charAnimCount = 0;
						Animating = false;
					}
				}

				return animFrame;
			}
			set
			{
				animFrame = value;

				while (animFrame < 0)
					animFrame += 3;

				if (animFrame >= 3)
					animFrame = value % 3;

				animWatch.Reset();
			}
		}
		/// <summary>
		/// sets or returns whether or not the character is animating
		/// </summary>
		/// <returns></returns>
		static public bool Animating
		{
			get
			{
				if (animWatch.IsPaused == true)
				{
					animFrame = 0;
				}

				return animWatch.IsPaused == false;
			}
			set
			{
				if (value == false)
				{
					animFrame = 0;
					charAnimCount = 0;

					if (animWatch.IsPaused == false)
						animWatch.Pause();
				}
				else
					animWatch.Resume();

			}
		}

		// menu functions

		static public SubMenu subMenu;					// the submenu
		static public bool LeftMenuActive;				// is the left menu active?

		// action window functions
		/// <summary>
		/// This function adds a line to the bottom of the action window and	
		/// scrolls the remaining lines up one each.
		/// </summary>
		/// <param name="?"></param>
		static public void AddBottom(ColorStringBuilder builder)
		{
			AddBottom(builder.Text, builder.Colors);
		}

		static public void AddBottom()
		{
			AddBottom("");
		}
		static public void AddBottom(string line, int margin = 1)
		{
			AddBottom(line, null);
		}
		static public void AddBottom(string line, Color color)
		{
			Color[] colors = new Color[40];

			for (int i = 0; i < 40; i++)
			{
				colors[i] = color;
			}

			AddBottom(line, colors);
		}

		public static void AddBottomCentered(string p)
		{
			BottomCenterString(ref p);
			AddBottom(p);
		}
		public static void AddBottomCentered(string p, Color color)
		{
			BottomCenterString(ref p);
			AddBottom(p, color);
		}
		public static void AddBottomCentered(string p, Color[] colors)
		{
			BottomCenterString(ref p);
			AddBottom(p, colors);
		}

		private static void BottomCenterString(ref string p)
		{
			int spaces = 36 - p.Length;
			spaces /= 2;
			if (spaces < 0) spaces = 0;

			p = new string(' ', spaces) + p;
		}
		// adds a line to the bottom of the action window
		static public void AddBottom(string line, Color[] colors)
		{
			int i;

			bottom[4] = "";

			for (i = 3; i >= 0; i--)
			{
				bottom[i + 1] = bottom[i];
				bottomColor[i + 1] = bottomColor[i];
				bottommargin[i + 1] = bottommargin[i];
			}

			bottomColor[0] = new Color[39];

			bottom[0] = line;
			bottommargin[0] = XleCore.BottomTextMargin;

			if (colors != null)
			{
				for (i = 0; i < 39; i++)
				{
					Color clr;

					if (i >= colors.Length)
						clr = colors[colors.Length - 1];
					else
						clr = colors[i];

					bottomColor[0][i] = clr;
				}
			}
			else
			{
				for (i = 0; i < 39; i++)
				{
					bottomColor[0][i] = XleCore.FontColor;
				}
			}

		}
		static public void UpdateBottom(string line)
		{
			UpdateBottom(line, 0, null);
		}
		static public void UpdateBottom(string line, int loc)
		{
			UpdateBottom(line, loc, null);
		}
		static public void UpdateBottom(string line, int loc, Color color)
		{
			Color[] colors = new Color[40];

			for (int i = 0; i < 40; i++)
			{
				colors[i] = color;
			}

			UpdateBottom(line, loc, colors);
		}
		/// <summary>
		/// This function updates a line in the action window.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="lineNumber">The number of the line to update.  0 is the bottom line, 4 is the top.</param>
		static public void UpdateBottom(ColorStringBuilder builder, int lineNumber)
		{
			UpdateBottom(builder.Text, lineNumber, builder.Colors);
		}
		/// <summary>
		/// This function updates a line in the action window.
		/// </summary>
		/// <param name="lineNumber">The number of the line to update.  0 is the bottom line, 4 is the top.</param>
		static public void UpdateBottom(string line, int lineNumber, Color[] colors)
		{
			if (lineNumber < 0 || lineNumber > 4) throw new ArgumentOutOfRangeException("lineNumber", "The line number must be between 0 and 4.");

			bottom[lineNumber] = line;

			if (colors != null)
			{
				for (int i = 0; i < 39; i++)
				{
					Color clr;

					if (i < colors.Length)
						clr = colors[i];
					else
						clr = colors[colors.Length - 1];

					bottomColor[lineNumber][i] = clr;
				}

			}
		}
		static public void UpdateBottom(string line, Color color)
		{
			Color[] colors = new Color[40];

			for (int i = 0; i < 40; i++)
			{
				colors[i] = color;
			}

			UpdateBottom(line, 0, colors);

		}

		static public string Bottom(int line)
		{
			string tempspace;

			if (line >= 0 && line <= 4)
			{
				tempspace = bottom[line];

				return tempspace;
			}
			else
			{
				return null;
			}

		}
		public static int BottomMargin(int line)
		{
			return bottommargin[line];
		}

		static public Color[] BottomColor(int line)
		{
			Color[] tempspace = new Color[40];

			if (line >= 0 && line <= 4)
			{
				bottomColor[line].CopyTo(tempspace, 0);

				return tempspace;
			}
			else
			{
				return null;
			}

		}

		static public void ClearBottom()
		{
			for (int i = 0; i < 5; i++)
			{
				AddBottom("");
			}
		}

		static public void WriteSlow(string line, int loc, Color color)
		{
			int i = 0;
			Color[] colors = new Color[40];
			string temp;

			while (i < line.Length && i < 40)
			{
				colors[i++] = color;
				temp = line.Substring(0, i);

				UpdateBottom(temp, loc, colors);

				XleCore.wait(50);
			}
		}

		[Obsolete]
		static public bool LoadTiles(string tileset)
		{
			XleCore.LoadTiles(tileset);

			return true;
		}
		
		// other commonly used variables that don't need accessors
		static public int walkTime;				// time to wait between steps

		//		full screen mode
		static private bool done;

		public static bool Done
		{
			get
			{
				if (Display.CurrentWindow.IsClosed)
					return true;
				else
					return g.done;
			}
			set { g.done = value; }
		}

		static public int vertLine;				// Vertical line dividing menu and map
		static public int raftAnim;				// raft animation frame
		static public Direction raftFacing;		// direction the raft is facing (lotaEast or lotaWest)
		static public int charAnimCount;			// animation count for the player

		static public bool invisible;				// is the player invisible?
		static public bool guard;					// is the player in guard colors?

		static public bool disableEncounters;		// used to disable overworld encounters


		internal static bool HasCommand(KeyCode cmd)
		{
			return true;
		}

	}
}