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
	}


	public static class g
	{
		static int animFrame;

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

			invisible = false;
			guard = false;

		}

		// character functions
		static Timing.StopWatch animWatch = new Timing.StopWatch();
		const int frameTime = 150;

		static public int AnimFrame
		{
			get
			{
				int oldAnim = animFrame;

				if (animWatch.IsPaused == false)
					animFrame = (((int)animWatch.TotalMilliseconds) / frameTime);

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
		[Obsolete("Use XleCore.TextArea instead.")]
		static public void AddBottom(ColorStringBuilder builder)
		{
			AddBottom(builder.Text, builder.Colors);
		}

		[Obsolete("Use XleCore.TextArea instead.")]
		static public void AddBottom()
		{
			AddBottom("");
		}
		[Obsolete("Use XleCore.TextArea instead.")]
		static public void AddBottom(string line, int margin = 1)
		{
			AddBottom(line, null);
		}
		[Obsolete("Use XleCore.TextArea instead.")]
		static public void AddBottom(string line, Color color)
		{
			Color[] colors = new Color[40];

			for (int i = 0; i < 40; i++)
			{
				colors[i] = color;
			}

			AddBottom(line, colors);
		}

		[Obsolete("Use XleCore.TextArea instead.")]
		public static void AddBottomCentered(string p)
		{
			BottomCenterString(ref p);
			AddBottom(p);
		}
		[Obsolete("Use XleCore.TextArea instead.")]
		public static void AddBottomCentered(string p, Color color)
		{
			BottomCenterString(ref p);
			AddBottom(p, color);
		}
		[Obsolete("Use XleCore.TextArea instead.")]
		public static void AddBottomCentered(string p, Color[] colors)
		{
			BottomCenterString(ref p);
			AddBottom(p, colors);
		}

		[Obsolete("Use XleCore.TextArea instead.")]
		private static void BottomCenterString(ref string p)
		{
			int spaces = 36 - p.Length;
			spaces /= 2;
			if (spaces < 0) spaces = 0;

			p = new string(' ', spaces) + p;
		}
		// adds a line to the bottom of the action window
		[Obsolete("Use XleCore.TextArea instead.")]
		static public void AddBottom(string line, Color[] colors)
		{
			XleCore.TextArea.WriteLine(line, colors);
			/*
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
			*/
		}
		[Obsolete("Use XleCore.TextArea instead.")]
		static public void UpdateBottom(string line)
		{
			UpdateBottom(line, 0, null);
		}
		[Obsolete("Use XleCore.TextArea instead.")]
		static public void UpdateBottom(string line, int loc)
		{
			UpdateBottom(line, loc, null);
		}
		[Obsolete("Use XleCore.TextArea instead.")]
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
		[Obsolete("Use XleCore.TextArea instead.")]
		static public void UpdateBottom(ColorStringBuilder builder, int lineNumber)
		{
			UpdateBottom(builder.Text, lineNumber, builder.Colors);
		}
		/// <summary>
		/// This function updates a line in the action window.
		/// </summary>
		/// <param name="lineNumber">The number of the line to update.  0 is the bottom line, 4 is the top.</param>
		[Obsolete("Use XleCore.TextArea instead.")]
		static public void UpdateBottom(string line, int lineNumber, Color[] colors)
		{
			XleCore.TextArea.UpdateLine(4 - lineNumber, 0, line, colors);
			/*
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
			 * */
		}
		[Obsolete("Use XleCore.TextArea instead.")]
		static public void UpdateBottom(string line, Color color)
		{
			Color[] colors = new Color[40];

			for (int i = 0; i < 40; i++)
			{
				colors[i] = color;
			}

			UpdateBottom(line, 0, colors);

		}

		[Obsolete("Use XleCore.TextArea instead.")]
		static public string Bottom(int line)
		{
			return XleCore.TextArea.GetTextLine(4 - line);
			/*
			string tempspace;

			if (line >= 0 && line <= 4)
			{
				tempspace = bottom[line];

				return tempspace;
			}
			else
			{
				return null;
			}*/

		}


		[Obsolete("Use XleCore.TextArea.Clear instead.")]
		static public void ClearBottom()
		{
			for (int i = 0; i < 5; i++)
			{
				AddBottom("");
			}
		}

		[Obsolete("Use XleCore.TextArea instead.")]
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

				XleCore.Wait(50);
			}
		}
		
		// other commonly used variables that don't need accessors
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
	}
}