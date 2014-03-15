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
		// action window functions
		/// <summary>
		/// This function adds a line to the bottom of the action window and	
		/// scrolls the remaining lines up one each.
		/// </summary>
		/// <param name="?"></param>
		[Obsolete("Use XleCore.TextArea instead.")]
		static public void AddBottom(ColorStringBuilder builder)
		{
			XleCore.TextArea.PrintLine(builder.Text, builder.Colors);
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

		static public bool disableEncounters;		// used to disable overworld encounters
	}
}