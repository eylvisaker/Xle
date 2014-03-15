using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Rendering
{
	public class XleRenderer
	{
		public Size WindowSize { get; set; }

		Size GameAreaSize { get { return new Size(640, 400); } }

		public Color FontColor { get; set; }
		public Surface Tiles { get; set; }

		bool mOverrideHPColor;
		Color mHPColor;

		/****************************************************************************
		 *	void DrawBorder( LPDIRECTDRAWSURFACE7 pDDS, unsigned int boxColor)		*
		 *																			*
		 *  This function draws the border around the screen.						*
		 *																			*
		 *	Parameters:	the direct draw surface to draw to, and the color to draw	*
		 *  Returns:	void														*
		 ****************************************************************************/
		public void DrawFrame(Color boxColor)
		{
			DrawFrameLine(0, 0, 1, GameAreaSize.Width, boxColor);
			DrawFrameLine(0, 0, 0, GameAreaSize.Height - 2, boxColor);
			DrawFrameLine(0, GameAreaSize.Height - 16, 1, GameAreaSize.Width, boxColor);
			DrawFrameLine(GameAreaSize.Width - 12, 0, 0, GameAreaSize.Height - 2, boxColor);
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
		public void DrawFrameHighlight(Color innerColor)
		{
			DrawInnerFrameHighlight(0, 0, 1, GameAreaSize.Width, innerColor);
			DrawInnerFrameHighlight(0, 0, 0, GameAreaSize.Height - 2, innerColor);
			DrawInnerFrameHighlight(0, GameAreaSize.Height - 16, 1, GameAreaSize.Width + 2, innerColor);
			DrawInnerFrameHighlight(GameAreaSize.Width - 12, 0, 0, GameAreaSize.Height - 2, innerColor);

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
		public void DrawFrameLine(int left, int top, int direction,
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
		public void DrawInnerFrameHighlight(int left, int top, int direction,
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
					length - boxWidth + 2,
					innerWidth,
					innerColor);
			}
			else
			{

				Display.FillRect(left + innerOffsetH,
					top + innerOffsetV,
					innerWidth + 2,
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
		public void WriteText(int px, int py, string theText)
		{
			WriteText(px, py, theText, XleColor.White);
		}
		public void WriteText(int px, int py, string theText, Color c)
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
		public void WriteText(int px, int py, string theText, Color[] coloring)
		{
			if (string.IsNullOrEmpty(theText))
				return;

			int i;
			int fx, fy;
			int startx = px;

			int len = theText.Length;
			Color color;

			for (i = 0; i < len; i++, px += 16)
			{
				char c = theText[i];

				if (c == '\n')
				{
					px = startx - 16;
					py += 16;
				}

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

				XleCore.Factory.Font.Color = color;

				XleCore.Factory.Font.DrawText(px, py, c.ToString());
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
		public void DrawTile(int px, int py, int tile)
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
		public void DrawMonster(int px, int py, int monst)
		{
			int tx, ty;

			Rectangle monstRect;
			Rectangle destRect;

			tx = (monst % 8) * 64;
			ty = (monst / 8) * 64;

			monstRect = new Rectangle(tx, ty, 64, 64);
			destRect = new Rectangle(px, py, 64, 64);

			if (XleCore.Factory.Monsters != null)
				XleCore.Factory.Monsters.Draw(monstRect, destRect);

		}

		public Color PlayerColor { get; set; }
		/// <summary>
		/// Draws the player character.
		/// </summary>
		/// <param name="animFrame"></param>
		/// <param name="vertLine"></param>
		void DrawCharacter(bool animating, int animFrame, int vertLine)
		{
			DrawCharacter(animating, animFrame, vertLine, PlayerColor);
		}
		void DrawCharacter(bool animating, int animFrame, int vertLine, Color clr)
		{
			int px = vertLine + 16;
			int py = 16 + 7 * 16;
			int width = (624 - px) / 16;

			px += 11 * 16;

			DrawCharacterSprite(px, py, XleCore.GameState.Player.FaceDirection, animating, animFrame, true, clr);

			CharRect = new Rectangle(px, py, 32, 32);
		}

		public void DrawCharacterSprite(int destx, int desty, Direction facing, bool animating, int animFrame, bool allowPingPong, Color clr)
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

			XleCore.Factory.Character.Color = clr;
			XleCore.Factory.Character.Draw(charRect, destRect);
		}

		public Rectangle CharRect { get; private set; }

		/// <summary>
		/// Draws the rafts that should be on the screen.
		/// </summary>
		/// <param name="inRect"></param>
		void DrawRafts(Rectangle inRect)
		{
			Player player = XleCore.GameState.Player;
			int tx, ty;
			int lx = inRect.Left;
			int width = inRect.Width;
			int px = lx + (int)((width / 16) / 2) * 16;
			int py = 128;
			int rx, ry;
			Rectangle charRect;
			Rectangle destRect;


			foreach (var raft in player.Rafts)
			{
				if (raft.RaftImage > 0)
					raftAnim %= 4;
				else
					raftAnim = 1 + raftAnim % 3;

				int sourceX = raftAnim * 32;
				int sourceY = 256;

				tx = sourceX;
				ty = sourceY;

				if (XleCore.GameState.Map.MapID != raft.MapNumber)
					continue;
				if (raft.RaftImage > 0)
				{
					tx += 32 * 4;
				}

				rx = px - (player.X - raft.X) * 16;
				ry = py - (player.Y - raft.Y) * 16;

				if (raft == player.BoardedRaft)
				{
					if (player.RaftFaceDirection == Direction.West)
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

					XleCore.Factory.Character.Draw(charRect, destRect);
				}
			}
		}

		public Action OnRedraw { get; set; }

		public void Draw()
		{
			if (OnRedraw != null)
			{
				OnRedraw();
				return;
			}

			Player player = XleCore.GameState.Player;
			XleMap map = XleCore.GameState.Map;

			int i = 0;
			Color boxColor = map.ColorScheme.FrameColor;
			Color innerColor = map.ColorScheme.FrameHighlightColor;
			int horizLine = 18 * 16;
			int vertLine = (38 - map.ColorScheme.MapAreaWidth) * 16;

			FontColor = map.ColorScheme.TextColor;
			Color menuColor = map.ColorScheme.TextColor;

			if (XleCore.GameState.Commands.IsLeftMenuActive)
			{
				menuColor = XleColor.Yellow;
			}

			DrawFrame(boxColor);

			DrawFrameLine(vertLine, 0, 0, horizLine + 12, boxColor);
			DrawFrameLine(0, horizLine, 1, GameAreaSize.Width, boxColor);

			DrawFrameHighlight(innerColor);

			DrawInnerFrameHighlight(vertLine, 0, 0, horizLine + 12, innerColor);
			DrawInnerFrameHighlight(0, horizLine, 1, GameAreaSize.Width, innerColor);

			Rectangle mapRect = Rectangle.FromLTRB
				(vertLine + 16, 16, GameAreaSize.Width - 16, horizLine);

			map.Draw(player.X, player.Y, player.FaceDirection, mapRect);

			i = 0;
			int cursorPos = 0;
			foreach (var cmd in XleCore.GameState.Commands.Items)
			{
				WriteText(48, 16 * (i + 1), cmd.Name, menuColor);

				if (cmd == XleCore.GameState.Commands.CurrentCommand)
					cursorPos = i;

				i++;
			}

			WriteText(32, 16 * (cursorPos + 1), "`", menuColor);

			Color hpColor = map.ColorScheme.TextColor;
			if (mOverrideHPColor)
				hpColor = mHPColor;

			WriteText(48, 16 * 15, "H.P. " + player.HP.ToString(), hpColor);
			WriteText(48, 16 * 16, "Food " + ((int)player.Food).ToString(), hpColor);
			WriteText(48, 16 * 17, "Gold " + player.Gold.ToString(), hpColor);

			XleCore.TextArea.Draw();

			if (map.AutoDrawPlayer)
			{
				DrawRafts(mapRect);

				if (player.IsOnRaft == false)
					DrawCharacter(Animating, AnimFrame, vertLine);
			}

			if (XleCore.PromptToContinue)
			{
				Display.FillRect(192, 384, 17 * 16, 16, XleColor.Black);
				WriteText(208, 384, "(Press to Cont)", XleColor.Yellow);
			}
		}


		public void FlashHPWhile(Color clr, Color clr2, Func<bool> pred)
		{
			Color oldClr = mHPColor;
			Color lastColor = clr2;

			mOverrideHPColor = true;
			int count = 0;

			while (pred())
			{
				if (lastColor == clr)
					lastColor = clr2;
				else
					lastColor = clr;

				mHPColor = lastColor;

				XleCore.Wait(80);

				count++;

				if (count > 10000 / 80)
					break;
			}

			mOverrideHPColor = false;
			mHPColor = oldClr;

		}

		public void AnimateStep()
		{
			if (Animating == false)
			{
				Animating = true;
				AnimFrame = 0;
			}

			charAnimCount = 0;
		}

		/// <summary>
		/// sets or returns whether or not the character is animating
		/// </summary>
		/// <returns></returns>
		bool Animating
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

		// character functions
		static Timing.StopWatch animWatch = new Timing.StopWatch();
		const int frameTime = 150;

		static int animFrame;

		int AnimFrame
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
		int charAnimCount;			// animation count for the player

		/// <summary>
		/// Animates the rafts.
		/// </summary>
		void RaftAnim()
		{
			if (lastRaftAnim + 100 < Timing.TotalMilliseconds)
			{
				raftAnim++;

				lastRaftAnim = Timing.TotalMilliseconds;
			}
		}
		
		public int raftAnim;				// raft animation frame

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
	}
}
