using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.Geometry;

namespace ERY.Xle
{
	public class XleTitleScreen
	{
		public enum TitleScreenState
		{
			NoState = 0,
			Menu1 = 1,
			Menu2 = 2,
			NewGame = 10,
			NewGameMusic = 11,
			NewGameText = 12,
			LoadGame = 20
		};

		Surface titleScreenSurface;			// stores the image of the title screen.

		TitleScreenState titleState = TitleScreenState.NoState;

		int titleMenu;				// current menu location in the title screen

		string[] wnd = new string[25];
		Color[][] color = new Color[25][];
		Color bgColor = XleColor.Black;

		string tempName;
		string[] files = new string[8];
		int page;
		int maxPages = 0;

		Player player;
		bool titleDone = false;

		void ClearTitleText()
		{
			for (int i = 0; i < 25; i++)
			{
				wnd[i] = "";
				color[i] = new Color[40];

				for (int j = 0; j < 40; j++)
				{
					color[i][j] = XleColor.White;
				}
			}
		}

		private void UpdateTitleScreen()
		{
			// update cursor position
			if (titleMenu > 0 && (titleState == TitleScreenState.Menu1
				|| titleState == TitleScreenState.Menu2))
			{
				int loc = 7 + titleMenu * 2;

				for (int i = 9; i < 16; i += 2)
				{
					SetText(i, 4, " ");

					if (i == loc)
					{
						SetText(i, 4, "`");
					}

				}

			}
			else if (titleState == TitleScreenState.LoadGame)
			{
				int loc = 7 + titleMenu;

				if (titleMenu > 0)
					loc++;
				if (titleMenu == 9)
					loc++;

				for (int i = 7; i < 19; i++)
				{
					SetText(i, 5, " ");

					if (i == loc)
					{
						SetText(i, 5, "`");
					}
				}
			}

			switch (titleState)
			{
				case TitleScreenState.NewGameMusic:

					if (SoundMan.IsPlaying(LotaSound.VeryGood) == false)
					{
						SetNewGameText();

						Wait(100);
					}

					break;
			}
		}
		void DisplayTitleScreen()
		{
			Color borderColor, lineColor;

			Display.Clear(bgColor);

			// draw borders & stuff	
			switch (titleState)
			{
				case TitleScreenState.NoState:

					titleScreenSurface.Draw();

					break;

				case TitleScreenState.Menu1:
					borderColor = XleColor.Blue;
					lineColor = XleColor.White;

					XleCore.DrawBorder(borderColor);

					XleCore.DrawLine(0, 20 * 16, 1, XleCore.myWindowWidth, borderColor);
					XleCore.DrawLine(3 * 16 + 8, 0, 0, 7 * 16, borderColor);
					XleCore.DrawLine(35 * 16 + 8, 0, 0, 7 * 16, borderColor);
					XleCore.DrawLine(3 * 16 + 8, 6 * 16 + 4, 1, (35 - 3 + 1) * 16 - 4, borderColor);

					XleCore.DrawInnerBorder(lineColor);

					XleCore.DrawInnerLine(0, 20 * 16, 1, XleCore.myWindowWidth, lineColor);
					XleCore.DrawInnerLine(3 * 16 + 8, 0, 0, 7 * 16, lineColor);
					XleCore.DrawInnerLine(35 * 16 + 8, 0, 0, 7 * 16, lineColor);
					XleCore.DrawInnerLine(3 * 16 + 8, 6 * 16 + 4, 1, (35 - 3 + 1) * 16 - 2, lineColor);

					break;

				case TitleScreenState.Menu2:
					borderColor = XleColor.Brown;
					lineColor = XleColor.Yellow;

					XleCore.DrawBorder(borderColor);

					XleCore.DrawLine(0, 20 * 16, 1, XleCore.myWindowWidth, borderColor);
					XleCore.DrawLine(3 * 16 + 8, 0, 0, 7 * 16, borderColor);
					XleCore.DrawLine(35 * 16 + 8, 0, 0, 7 * 16, borderColor);
					XleCore.DrawLine(3 * 16 + 8, 6 * 16 + 4, 1, (35 - 3 + 1) * 16 - 4, borderColor);

					XleCore.DrawInnerBorder(lineColor);

					XleCore.DrawInnerLine(0, 20 * 16, 1, XleCore.myWindowWidth, lineColor);
					XleCore.DrawInnerLine(3 * 16 + 8, 0, 0, 7 * 16, lineColor);
					XleCore.DrawInnerLine(35 * 16 + 8, 0, 0, 7 * 16, lineColor);
					XleCore.DrawInnerLine(3 * 16 + 8, 6 * 16 + 4, 1, (35 - 3 + 1) * 16 - 2, lineColor);

					break;

				case TitleScreenState.NewGame:

					borderColor = XleColor.LightGray;
					lineColor = XleColor.Yellow;

					XleCore.DrawBorder(borderColor);

					XleCore.DrawLine(10 * 16, 10 * 16, 1, 19 * 16 - 4, borderColor);  // top
					XleCore.DrawLine(10 * 16, 12 * 16, 1, 19 * 16 - 4, borderColor);  // bottom
					XleCore.DrawLine(10 * 16, 10 * 16, 0, 3 * 16 - 4, borderColor);   // left
					XleCore.DrawLine(28 * 16, 10 * 16, 0, 3 * 16 - 4, borderColor);   // right

					XleCore.DrawInnerBorder(lineColor);

					XleCore.DrawInnerLine(10 * 16, 10 * 16, 1, 19 * 16 - 4, XleColor.White);
					XleCore.DrawInnerLine(10 * 16, 12 * 16, 1, 19 * 16 - 2, XleColor.White);
					XleCore.DrawInnerLine(10 * 16, 10 * 16, 0, 3 * 16 - 4, XleColor.White);
					XleCore.DrawInnerLine(28 * 16, 10 * 16, 0, 3 * 16 - 4, XleColor.White);




					wnd[11] = "            " + tempName + "_";

					break;

				case TitleScreenState.NewGameMusic:
				case TitleScreenState.NewGameText:
				case TitleScreenState.LoadGame:

					borderColor = XleColor.LightGray;
					lineColor = XleColor.Yellow;

					XleCore.DrawBorder(borderColor);
					XleCore.DrawInnerBorder(lineColor);

					break;
			}

			for (int i = 0; i < 25; i++)
			{
				XleCore.WriteText(0, i * 16, wnd[i], color[i]);
			}

		}

		private void SetText(int y, int x, string text)
		{
			if (wnd[y] == null)
				wnd[y] = "";

			if (wnd[y].Length < x)
			{
				wnd[y] += new string(' ', x - wnd[y].Length) + text;
			}
			else
			{

				string temp = wnd[y].Substring(0, x);

				temp += text;

				if (wnd[y].Length > x + text.Length)
					temp += wnd[y].Substring(x + text.Length);

				wnd[y] = temp;
			}
		}


		public void Run()
		{
			if (titleScreenSurface == null)
				titleScreenSurface = new Surface("title.png");

			player = null;
			titleDone = false;
			titleState = TitleScreenState.NoState;

			Keyboard.KeyDown += new InputEventHandler(Keyboard_KeyDown);

			while (Display.CurrentWindow.IsClosed == false && !titleDone)
			{
				UpdateTitleScreen();

				Display.BeginFrame();

				DisplayTitleScreen();

				Display.EndFrame();
				Core.KeepAlive();
			}

			Keyboard.KeyDown -= Keyboard_KeyDown;
		}


		void Keyboard_KeyDown(InputEventArgs e)
		{
			const int stdWait = 100;
			KeyCode keyCode = e.KeyCode;

			if (lastTime + waitTime > Timing.TotalMilliseconds)
				return;

			lastTime = Timing.TotalMilliseconds;
			waitTime = 0;

			switch (titleState)
			{
				case TitleScreenState.NoState:

					if (keyCode != KeyCode.None)
					{
						SetMenu1();
						Keyboard.ReleaseAllKeys();
					}

					break;

				case TitleScreenState.Menu1:
				case TitleScreenState.Menu2:

					if (keyCode == KeyCode.Down)
					{
						titleMenu++;

						if (titleMenu > 4)
							titleMenu = 4;

						waitTime = stdWait;
					}
					else if (keyCode == KeyCode.Up)
					{
						titleMenu--;

						if (titleMenu < 1)
							titleMenu = 1;

						waitTime = stdWait;
					}
					else if (keyCode >= KeyCode.D1 && keyCode <= KeyCode.D4)
					{
						titleMenu = keyCode - KeyCode.D0;

						keyCode = KeyCode.Return;

						waitTime = stdWait;
					}

					if (keyCode == KeyCode.Return)
					{
						Keyboard.ReleaseAllKeys();

						if (titleState == TitleScreenState.Menu1)
						{
							if (titleMenu == 1)
							{
								SetMenu2();
							}
							else if (titleMenu == 2) { }
							else if (titleMenu == 3) { }
							else if (titleMenu == 4) { }

						}
						else if (titleState == TitleScreenState.Menu2)
						{
							if (titleMenu == 1) // return to first menu
							{
								SetMenu1();
							}
							else if (titleMenu == 2)
							{
								SetNewGame();
							}
							else if (titleMenu == 3)
							{
								page = 0;
								SetRestoreGame();
							}
							else if (titleMenu == 4)
							{
								SetEraseGame();
							}
						}
					}

					break;

				case TitleScreenState.NewGame:

					if ((keyCode >= KeyCode.A && keyCode <= KeyCode.Z) || keyCode == KeyCode.Space ||
						(keyCode >= KeyCode.D0 && keyCode <= KeyCode.D9))
					{

						if (tempName.Length < 14)
						{
							tempName += e.KeyString;
						}

					}
					else if (keyCode == KeyCode.BackSpace || keyCode == KeyCode.Delete)
					{
						if (tempName.Length > 0)
							tempName = tempName.Substring(0, tempName.Length - 1);
					}
					else if (keyCode == KeyCode.Escape || keyCode == KeyCode.F1)
					{
						SetMenu2();
					}
					else if (keyCode == KeyCode.Enter && tempName.Length > 0)
					{
						if (System.IO.File.Exists(@"Saved\" + tempName + ".chr"))
						{
							SoundMan.PlaySound(LotaSound.Medium);

							for (int i = 13; i < 25; i++)
							{
								wnd[i] = "";
							}

							wnd[16] = "    " + tempName + " has already begun.";

							Wait(2000);

							SetNewGame();
						}
						else
						{

							SoundMan.PlaySound(LotaSound.VeryGood);
							titleState = TitleScreenState.NewGameMusic;

							for (int i = 13; i < 25; i++)
							{
								wnd[i] = "";
							}

							wnd[16] = "    " + tempName + "'s adventures begin";

							player = new Player(tempName);
							player.SavePlayer();
						}
					}

					break;

				case TitleScreenState.LoadGame:
					if (keyCode == KeyCode.Down)
					{
						titleMenu++;

						if (titleMenu >= 9)
							titleMenu = 9;
						else if (files[titleMenu - 1] == "")
						{
							titleMenu = 9;
						}

						waitTime = stdWait;
					}
					else if (keyCode == KeyCode.Up)
					{
						do
						{
							titleMenu--;

							if (titleMenu <= 0)
								break;

						} while (titleMenu > 0 && files[titleMenu - 1] == "");

						if (titleMenu < 0)
							titleMenu = 0;

						waitTime = stdWait;
					}
					else if (keyCode >= KeyCode.D1 && keyCode <= KeyCode.D9)
					{
						titleMenu = keyCode - KeyCode.D0;

						keyCode = KeyCode.Return;

						waitTime = stdWait;
					}

					if (titleMenu == 9 && page == maxPages)
					{
						titleMenu = 8;

						do
						{
							titleMenu--;

							if (titleMenu <= 0)
								break;

						} while (titleMenu > 0 && files[titleMenu - 1] == "");

					}

					if (keyCode == KeyCode.Return)
					{
						waitTime = 1000;
						Wait(500);

						waitTime = stdWait;

						if (titleMenu == 0)
						{
							if (page > 0)
							{
								page--;

								SetRestoreGame();
							}
							else
							{
								SetMenu2();
							}
						}
						else if (titleMenu == 9)
						{
							page++;

							if (page > maxPages)
								page = maxPages;

							SetRestoreGame();
						}
						else
						{
							string file = files[titleMenu-1];
						
							if (string.IsNullOrEmpty(file) == false)
							{
								player = Player.LoadPlayer(file);
								titleDone = true;
							}
						}

					}

					break;

			}


		}

		private void Wait(int milliseconds)
		{
			Keyboard.KeyDown -= Keyboard_KeyDown;
			Timing.StopWatch watch = new Timing.StopWatch();

			while (!g.Done && watch.TotalMilliseconds < milliseconds)
			{
				Display.BeginFrame();

				DisplayTitleScreen();

				Display.EndFrame();
				Core.KeepAlive();
			}

			Keyboard.KeyDown += Keyboard_KeyDown;
		}

		void WriteSlowToString(ref string target, string source)
		{

			int i = 0;
			int length = source.Length;

			while (i <= length && !g.Done)
			{
				target = source.Substring(0, i);

				i++;

				if (Keyboard.AnyKeyPressed == false)
					Wait(62);
				else
				{
					i++;
					Wait(1);
				}
			}

			Wait(1);

		}

		void SetMenu1()
		{
			titleState = TitleScreenState.Menu1;
			titleMenu = 1;

			ClearTitleText();
			bgColor = XleColor.LightBlue;

			wnd[9] = "      1.  Play a game";
			wnd[11] = "      2.  Some Simple Instructions";
			wnd[13] = "      3.  Scenes from Legacy";
			wnd[15] = "      4.  Color Test";
			wnd[18] = "  (Pick option by keyboard or joystick)";
			wnd[22] = "  Copyright 1987 - Quest Software, Inc.";

			for (int i = 0; i < 40; i++)
			{
				color[11][i] = XleColor.LightGray;
				color[13][i] = XleColor.LightGray;
				color[15][i] = XleColor.LightGray;
				color[18][i] = XleColor.Blue;
			}

		}
		void SetMenu2()
		{
			titleState = TitleScreenState.Menu2;
			titleMenu = 1;

			ClearTitleText();
			bgColor = XleColor.Orange;

			wnd[9] = "      1.  Return to first menu";
			wnd[11] = "      2.  Start a new game";
			wnd[13] = "      3.  Restart a game";
			wnd[15] = "      4.  Erase a character";
			wnd[18] = "  (Pick option by keyboard or joystick)";
			wnd[22] = "  Copyright 1987 - Quest Software, Inc.";

			for (int i = 0; i < 40; i++)
			{
				/*color[11][i] = ;
				color[13][i] = lotaLtGray;
				color[15][i] = lotaLtGray;*/
				color[18][i] = XleColor.Yellow;
				color[22][i] = XleColor.Yellow;
			}

		}

		public void SetNewGame()
		{

			titleState = TitleScreenState.NewGame;
			titleMenu = 0;

			tempName = "";

			ClearTitleText();
			bgColor = XleColor.Green;

			wnd[0] = "             Start a new game";
			wnd[4] = "   Type in your new character's name.";
			wnd[6] = "    It may be up to 14 letters long.";

			wnd[11] = "             _";
			wnd[17] = "  ` Press return key when finished. `";
			wnd[20] = "   - Press 'del' key to backspace -";
			wnd[22] = "  - Press 'F1' or Escape to cancel -";


		}

		public void SetRestoreGame()
		{
			titleState = TitleScreenState.LoadGame;
			titleMenu = 0;

			tempName = "";

			ClearTitleText();
			bgColor = XleColor.Brown;

			wnd[0] = "             Restart a game";
			wnd[4] = "        Restart which character?";

			if (page == 0)
			{
				wnd[7] = "          0.  Cancel";
			}
			else
			{
				wnd[7] = "          0.  Previous Page";
			}


			// file system?
			string[] sourceFiles = Directory.GetFiles("Saved");



			for (int i = 0; i < 8; i++)
			{
				if (i + page * 8 < sourceFiles.Length)
					files[i] = sourceFiles[i + page * 8];
				else
					files[i] = "";
			}

			maxPages = (sourceFiles.Length - 1) / 8;


			for (int i = 0; i < 8; i++)
			{
				int len = files[i].Length;

				wnd[9 + i] = "          " + (i + 1).ToString() + ".  ";


				if (string.IsNullOrEmpty(files[i]))
				{
					wnd[9 + i] += "Empty";
				}
				else
					wnd[9 + i] += Path.GetFileNameWithoutExtension(files[i]);

			}

			if (page < maxPages)
				wnd[18] = "          9.  Next Page";

			wnd[21] = "   (Select by joystick or number keys)";

			for (int i = 0; i < 40; i++)
			{
				color[21][i] = XleColor.Yellow;
			}

		}

		void SetNewGameText()
		{
			titleState = TitleScreenState.NewGameText;
			titleMenu = 0;


			ClearTitleText();
			bgColor = XleColor.DarkGray;

			wnd[0] = "             Start a new game";

			for (int i = 0; i < 25; i++)
			{
				wnd[i] = "  ";
			}

			for (int i = 0; i < 40; i++)
			{
				color[24][i] = XleColor.Yellow;
			}

			{
				int i = 4;

				WriteSlowToString(ref wnd[i++], "  You are only a poor peasant on the");
				WriteSlowToString(ref wnd[i++], "  world of Tarmalon, so it's hardly");
				WriteSlowToString(ref wnd[i++], "  surprising that you've never seen");
				WriteSlowToString(ref wnd[i++], "  a dead man before.  His crumpled");
				WriteSlowToString(ref wnd[i++], "  figure lies forlornly by the side");
				WriteSlowToString(ref wnd[i++], "  of the road.");
				WriteSlowToString(ref wnd[i++], "                                   ");

				i++;

				WriteSlowToString(ref wnd[i++], "  Fighting your fear, you kneel by");
				WriteSlowToString(ref wnd[i++], "  the still-warm corpse.  You see a");
				WriteSlowToString(ref wnd[i++], "  a look of panic on his face, a gold");
				WriteSlowToString(ref wnd[i++], "  band around his wrist, and a large");
				WriteSlowToString(ref wnd[i++], "  leather scroll, clutched tightly to");
				WriteSlowToString(ref wnd[i++], "  his chest.");
			}
			wnd[24] = "    (Press key/button to continue)";

			Keyboard.ReleaseAllKeys();
			while (g.Done == false && Keyboard.AnyKeyPressed == false)
			{
				Wait(50);
			}

			ClearTitleText();

			wnd[0] = "             Start a new game";

			for (int i = 0; i < 25; i++)
			{
				wnd[i] = "  ";
			}

			for (int i = 0; i < 40; i++)
			{
				color[24][i] = XleColor.Yellow;
			}
			{
				int i = 4;

				WriteSlowToString(ref wnd[i++], "  You've never been a thief, yet");
				WriteSlowToString(ref wnd[i++], "  something compels you to reach for");
				WriteSlowToString(ref wnd[i++], "  the leather scroll.  Getting the");
				WriteSlowToString(ref wnd[i++], "  armband off is trickier, but you");
				WriteSlowToString(ref wnd[i++], "  manage to snap it around your own");
				WriteSlowToString(ref wnd[i++], "  wrist.  You scoop up two green coins");
				WriteSlowToString(ref wnd[i++], "  lying nearby and hasten on your way.");
				WriteSlowToString(ref wnd[i++], "                                   ");

				i++;

				WriteSlowToString(ref wnd[i++], "  Before you've gone more than a few");
				WriteSlowToString(ref wnd[i++], "  steps, your senses waver and shift.");
				WriteSlowToString(ref wnd[i++], "  Rising from the mists, as though ");
				WriteSlowToString(ref wnd[i++], "  you've never been this way before, ");
				WriteSlowToString(ref wnd[i++], "  is a magnificient structure of ");
				WriteSlowToString(ref wnd[i++], "  polished stone.  A shimmering arch-");
				WriteSlowToString(ref wnd[i++], "  way beckons.");
			}

			wnd[24] = "    (Press key/button to continue)";


			Keyboard.ReleaseAllKeys();
			while (!g.Done && Keyboard.AnyKeyPressed == false)
			{
				Wait(50);
			}

			titleDone = true;
		}
		void SetEraseGame()
		{
			SetMenu2();
		}

		double lastTime = 0;
		int waitTime = 0;

		public Player Player
		{
			get { return player; }
		}

	}
}