// LotA.cpp : Defines the entry point for the application.
//

#include "lota.h"

const int sndFlags = SND_ASYNC | SND_RESOURCE;

#define ShowCoordinates
#define StartFullScreen false

Global g;

bool runningTitle = false;

/****************************************************************************
 *  int WinMain (HINSTANCE, HINSTANCE, LPSTR, int)							*
 *																			*
 *  This function starts the application.									*
 *																			*
 *	Parameters:	standard WinMain parameters									*
 *  Returns:	0 if no error.												*
 ****************************************************************************/
int APIENTRY WinMain(HINSTANCE hInstance,
                     HINSTANCE hPrevInstance,
                     LPSTR     lpCmdLine,
                     int       iCmdShow)
{
	static TCHAR	szAppName[] = TEXT("Legacy of the Ancients");
	static TCHAR    menuName[] = TEXT("LOTA_MENU");
	HWND			hwnd;
	MSG				msg;
	HMENU			hMenu;
	WNDCLASS		wndclass;
	int				cx, cy;

	wndclass.style			= CS_HREDRAW | CS_VREDRAW;
	wndclass.lpfnWndProc	= WndProc;
	wndclass.cbClsExtra		= 0;
	wndclass.cbWndExtra		= 0;
	wndclass.hInstance		= hInstance;
	wndclass.hIcon			= LoadIcon (hInstance, MAKEINTRESOURCE(ICON_LOTA));
	wndclass.hCursor		= LoadCursor (NULL, IDC_ARROW);
	wndclass.hbrBackground	= (HBRUSH) GetStockObject(WHITE_BRUSH);
	wndclass.lpszMenuName	= NULL;
	wndclass.lpszClassName	= szAppName;


	if (!RegisterClass(&wndclass))
	{
		MessageBox (NULL, TEXT ("This program requires Windows 98!"), szAppName, MB_ICONERROR);

		return 0;
	}

	hMenu = LoadMenu(hInstance, MAKEINTRESOURCE(LOTA_MENU));

	// Seed the random number generator
	srand(time(0));
	rnd(0,0);

	// Get system window metrics so that our client area is exactly the size we want.
    cx = actualWindowWidth + GetSystemMetrics(SM_CXSIZEFRAME)*2;
    cy = actualWindowHeight + GetSystemMetrics(SM_CYSIZEFRAME)*2 + GetSystemMetrics(SM_CYMENU) + GetSystemMetrics(SM_CYCAPTION);

	// Create the window
	hwnd = CreateWindow (szAppName,						// window class name
						 TEXT("Legacy of the Ancients"),// window caption
						 WS_OVERLAPPEDWINDOW,			// window style
						 CW_USEDEFAULT,					// initial x position
						 CW_USEDEFAULT,					// initial y position
						 cx,							// initial width
						 cy,							// initial height
						 NULL,							// parent window handle
						 hMenu,							// window menu handle
						 hInstance,						// program instance handle
						 NULL);							// creation parameters

	g.Unlock();
	
	g.SetHInstance(hInstance, hwnd);

	InitDSound();
	InitDirectInput(hwnd);

	ShowWindow (hwnd, iCmdShow);
	UpdateWindow(hwnd);


	g.ReleaseFont();
	g.LoadFont();

	g.Lock();

	//SetTimer(hwnd, 1001, 1, NULL);

	while (GetMessage (&msg, NULL, 0, 0) && !g.done)
	{
		TranslateMessage(&msg);
		DispatchMessage (&msg);

		CheckJoystick();

		if (!g.done)
		{
			HeartBeat(looping);
			//CreateThread(NULL, 0, HeartBeatThread, LPVOID(looping), 0, (ULONG*)&cx);
		}

	}

	//KillTimer(hwnd, 1001);
	g.done = true;
	Sleep(10);

	g.Unlock();
	g.ReleaseFont();

	FreeDirectInput();
	DDDestroySurfaces();
	DDDone();
	ShutDownDSound();

	return msg.wParam;
	
}

unsigned long __stdcall HeartBeatThread (void *param)
{
	while (!g.done)
	{
		HeartBeat((short)param);
		Sleep(0);
	}

	return 0;
}

/****************************************************************************
 *	LRESULT CALLBACK WndProc   (HWND hwnd, UINT message,					*
 *								WPARAM wParam, LPARAM lParam)				*
 *																			*
 *  This function handles messages for the main window.						*
 *																			*
 *	Parameters:	standard WndProc parameters									*
 *  Returns:	0 if no error												*
 ****************************************************************************/
LRESULT CALLBACK WndProc (HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	static int first = 0;
	int i = 0;
	HDC			hdc;
	PAINTSTRUCT	ps;
	long		returnValue = 0;
	
	if (first == 0 && message != WM_CREATE)
	{
		return DefWindowProc (hwnd, message, wParam, lParam);
	}

	switch (message)
	{

	case WM_CREATE:

		if (!DDInit(hwnd))
		{
			MessageBox(NULL, TEXT("Couldn't initialize DirectDraw!"), TEXT("Failed"), 0);
			g.done = true;
			PostQuitMessage(1);
			return 1;
		}

		if (!DDCreateSurfaces(StartFullScreen, actualWindowWidth, actualWindowHeight, 16))
		{
			MessageBox(NULL, TEXT("Couldn't create DirectDraw surfaces!"), TEXT("Failed"), 0);
			g.done = true;
			PostQuitMessage(1);
			return 1;
		}
				
		g.ResetTimers();

		g.map.LoadMap(g.player.Map());
		g.map.MapMenu();

		g.commandMode = cmdPrompt;
		DoCommand(0);

		first = 1;

		//CreateThread(NULL, 0, HeartBeatThread, LPVOID(looping), 0, (ULONG*)&i);

		return 0;

	case WM_PAINT:
		
		hdc = BeginPaint (hwnd, &ps);

		EndPaint (hwnd, &ps);

		if (!g.done)
		{
			HeartBeat(redraw);
		}
		return 0;

	case WM_COMMAND:

		switch (LOWORD (wParam))
		{
		case ID_FILE_NEWGAME:

			SetNewGame();
			RunTitleScreen();

			return 0;

		case ID_FILE_RESTARTGAME:
			
			SetRestoreGame();
			RunTitleScreen();

			return 0;

		case ID_FILE_SAVEGAME:
			g.player.SaveGame();

			return 0;

		case ID_FILE_QUIT:
			PostQuitMessage(0);

			return 0;

		case ID_OPTIONS_TOGGLEFULLSCREEN:
			ChangeScreenMode();

			return 0;


		case ID_OPTIONS_PREFERENCES:
			g.commandMode = cmdBad;
			

			//while (GetMessage (&msg, NULL, 0, 0))
			{
			//	TranslateMessage(&msg);
			//	DispatchMessage(&msg);

				if (DialogBox (g.hInstance(), MAKEINTRESOURCE(dlgOptions), hwnd, OptionsDlgProc))
					int i = 0;


			}

			g.commandMode = cmdEnterCommand;

			return 0;

		case ID_HELP_ABOUT:

			g.commandMode = cmdBad;
			
			DialogBox (g.hInstance(), MAKEINTRESOURCE(dlgAboutBox), g.hwnd(), OptionsDlgProc);

			g.commandMode = cmdEnterCommand;
			return 0;
		}

		break;
	case WM_ACTIVATE:
		switch (LOWORD(wParam))
		{
		case WA_ACTIVE:
		case WA_CLICKACTIVE:
			
			g.ReleaseFont();

			g.Unlock();
			g.LoadFont();
			g.Lock();


			if (g.pJoystick)
				g.pJoystick->Acquire();

			g.ResetTimers();
			break;
		case WA_INACTIVE:


			g.DestroyTimers();
			break;
		}
		
		break;

	case WM_KEYDOWN:

		if (wParam == VK_F4)
		{
			ChangeScreenMode();
		}

		if (wParam == VK_F3)
		{
			g.newGraphics = !g.newGraphics;
		}


		g.menuKey = wParam;
		/*
		if (g.quickMenu == true)
		{
			g.menuKey = wParam;
		}
		else
		{
			//if ((wParam >= 65 && wParam <= 91) || wParam == VK_RIGHT || 
			//	wParam == VK_UP || wParam == VK_LEFT || wParam == VK_DOWN)
			if (wParam > 0)
			{
				DoCommand(wParam);
			}
		}
		*/

		break;

	case WM_TIMER:
		
		if (g.titleScreen)
			break;

		switch (wParam)
		{
		case passTimer:
			DoCommand('Z');
			break;

		case commandTimer:
			DoCommand(0);
			break;

		case charAnimTimer:
			//CheckAnim();

			break;
		}

		break;
	case WM_DESTROY:
		PostQuitMessage (0);
		
		KillTimer (hwnd, 1);

		break;
    
	default:
		break;
	}

	return DefWindowProc (hwnd, message, wParam, lParam);

}

/****************************************************************************
 *	BOOL CALLBACK OptionsDlgProc   (HWND hwnd, UINT message,				*
 *									WPARAM wParam, LPARAM lParam)			*
 *																			*
 *  This function handles messages for the preferences dialog box.			*
 *																			*
 *	Parameters:	standard WndProc parameters									*
 *  Returns:	true if the event was handled								*
 ****************************************************************************/
BOOL CALLBACK OptionsDlgProc (HWND hdlg, UINT message, WPARAM wParam, LPARAM lParam)
{
	static bool enable3D;
	static bool zBuffer;
	static bool newGfx;
	static bool disableEnc;

	switch (message)
	{
	case WM_INITDIALOG:
		enable3D = 0;
		zBuffer = g.ZBufferEnable;
		newGfx = g.newGraphics;
		disableEnc = g.disableEncounters;

		CheckDlgButton (hdlg, chk3DAccel, enable3D);
		CheckDlgButton (hdlg, chkZBuffer, zBuffer);
		CheckDlgButton (hdlg, chkNewGraphics, newGfx);
		CheckDlgButton (hdlg, chkDisableEncounter, disableEnc);

		return false;

	case WM_COMMAND:
		switch (LOWORD (wParam))
		{
		case IDOK:

			g.ZBufferEnable = IsDlgButtonChecked(hdlg, chkZBuffer) != 0 ? true : false;
			g.newGraphics = IsDlgButtonChecked(hdlg, chkNewGraphics) != 0 ? true : false;
			g.disableEncounters = IsDlgButtonChecked(hdlg, chkDisableEncounter) != 0 ? true : false;

		case IDABOUTOK:
			EndDialog (hdlg, true);
			
			return true;
			
		case IDCANCEL:
			EndDialog (hdlg, false);

			return true;

		}

		break;

	}

	return false;
}

/****************************************************************************
 *  void HeartBeat(short mode)												*
 *																			*
 *  This is the main function that runs the game.  It does all the drawing	*
 *	in the window right now.												*
 *																			*
 *	Parameters:	short mode, which i don't think is used anymore.			*
 *  Returns:	void														*
 ****************************************************************************/
void HeartBeat(short mode)
{
	static updating = false;

	while (updating)
		Sleep(0);

	updating = true;

	int i = 0, j = 0;
	DWORD boxColor;
	DWORD innerColor;
	int horizLine = 18 * 16;
	static int lastRaftAnim = 0;
	static int lastCharAnim = 0;
	static int lastOceanSound = 0;
	unsigned long fontColor = lotaWhite;
	unsigned long menuColor;

	// FPS counter variables
	int			 theClock = clock();
	static int   timer = theClock;
	static double frames = 0;
	static double fps;

	if (g.titleScreen)
	{
		CheckSurfaces();

		updating = false;

		DisplayTitleScreen(g_pDDSBack);
		DDFlip();

		return;
	}

	if (g.stdDisplay)
		g.commandMode = cmdBad;
	
	// count the number of frames and divide by the number of seconds that have passed
	frames++;
	if (theClock - timer > 1000)
	{
		fps = frames / ((theClock - timer) / 1000.0);
		frames = 0;
		timer = theClock;
	}

#ifdef Disable3D
	if (g.commandMode == 3)
		g.commandMode = cmdEnterCommand;
#endif

	//if (lastRaftAnim + 100 <= clock())
	{
		RaftAnim();
		lastRaftAnim = clock();
	}

	//if (lastCharAnim + 150 <= clock())
	{
		CheckAnim();
		lastCharAnim = clock();
	}

	g.map.AnimateGuards();

	// Check for lost surfaces
	CheckSurfaces();

	if (g.stdDisplay != 0)
	{

		DrawSpecial(g_pDDSBack);

	}

	else
	{
		switch (g.map.MapType())
		{
		case mapCastle:

		case mapTown:

			boxColor = lotaOrange;
			innerColor = lotaYellow;
			g.vertLine = 13 * 16;
			break;

		case mapMuseum:

			boxColor = lotaMdGray;
			innerColor = lotaYellow;
			g.vertLine = 15 * 16;

			break;

		case mapDungeon:

			boxColor = lotaMdGray;
			innerColor = lotaLtGreen;
			fontColor = lotaCyan;
			g.vertLine = 15 * 16;

			break;

		case mapOutside:
		default:
			boxColor = CreateRGB(159,112,64);
			innerColor = lotaYellow;	
			g.vertLine = 15 * 16;


			break;

		}

		//g.vertLine = 16;

		menuColor = fontColor;

	//	if (g.HPColor == lotaWhite || g.HPColor == lotaCyan)
		if (g.HPColor == lotaBlack)
		{
			g.HPColor = fontColor;
		}

		if (g.LeftMenuActive)
		{
			menuColor = lotaYellow;
		}

		int vertLine = g.vertLine;

		// Clear the back buffer
		DDPutBox (g_pDDSBack, 0, 0, myWindowWidth, myWindowHeight, CreateRGB(0,0,0));

		DrawBorder(g_pDDSBack, boxColor);

		DrawLine(g_pDDSBack, vertLine, 0, 0, horizLine + 12, boxColor);
		DrawLine(g_pDDSBack, 0, horizLine, 1, myWindowWidth, boxColor);

		DrawInnerBorder(g_pDDSBack, innerColor);

		DrawInnerLine(g_pDDSBack, vertLine, 0, 0, horizLine + 12, innerColor);
		DrawInnerLine(g_pDDSBack, 0, horizLine, 1, myWindowWidth, innerColor);

		int vert = 1;

		String tempLine = (String)"H.P. " + g.player.HP();
		g.WriteMenu(14, tempLine);

		tempLine = (String)"Food " + g.player.Food();
		g.WriteMenu(15, tempLine);

		tempLine = (String)"Gold " + g.player.Gold();

		g.WriteMenu(16, tempLine);

	#ifdef ShowCoordinates
		// Show coordinates & framerate at top
		//DDPutBox (g_pDDSBack, 0, 0, 640, 16, lotaBlack);
		
		String		 coor;
		coor = (String)"X: " + g.player.X();

		DDPutBox (g_pDDSBack, 256, 0, 128, 16, lotaBlack);
		WriteText (g_pDDSBack, 272, 0, coor);

		coor = (String)"Y: " + g.player.Y();

		DDPutBox (g_pDDSBack, 400, 0, 128, 16, lotaBlack);
		WriteText (g_pDDSBack, 416, 0, coor);

		coor = (String)"F: " + g.player.FaceDirection();

		DDPutBox (g_pDDSBack, 544, 0, 96, 16, lotaBlack);
		WriteText (g_pDDSBack, 560, 0, coor);	

		coor = (String)"FPS: " + int(fps) + "." + int(fps * 10) % 10;

		DDPutBox (g_pDDSBack, 0, 0, 160, 16, lotaBlack);
		WriteText (g_pDDSBack, 0, 0, coor);

	#endif

		for (i = 0; i < 13; i++)
		{	
			WriteText (g_pDDSBack, 48, 16 * (i + 1), g.Menu(i), menuColor);
		}


		WriteText (g_pDDSBack, 32, 16 * (g.cursorPos () + 1), "`", menuColor);

		for (i = 14; i < 17; i++)
		{	
			WriteText (g_pDDSBack, 48, 16 * (i + 1), g.Menu(i), g.HPColor);
		}

		DrawBottomText(g_pDDSBack);

		// check to see if we are in a dungeon or the museum and set the d3d viewport
		if (g.map.MapType() == mapDungeon || g.map.MapType() == mapMuseum)
		{
	#ifndef Disable3D
			Set3DViewPort();
			Store3DMap();
			g.map.Draw(g_pDDSBack, g.player.Y(), g.player.X());
	#endif
		}
		else
		{
			g.map.Draw(g_pDDSBack, g.player.Y(), g.player.X());

			if (g.map.MapType() == mapOutside)
			{
				DrawRafts(g_pDDSBack);
			}

			if (!g.player.OnRaft())
			{
				DrawCharacter(g_pDDSBack, g.AnimFrame());
			}
		}


		// check to see if a submenu is active
		if (g.subMenu.onScreen)
		{
			DrawMenu(g_pDDSBack);
		}
		
		/////////////////////////////////////////////////////////////////////////
		// Check sounds
		//
		CheckFade();

		if (g.player.OnRaft())
		{
			LotaPlaySound(snd_Raft1, DSBPLAY_LOOPING, false);
			LotaStopSound(snd_Ocean1);
			LotaStopSound(snd_Ocean2);

		}
		else
		{
			LotaStopSound(snd_Raft1);
			int ocean = 0;

			if (g.map.MapType() == mapOutside)
			{

				for (i = -1; i <= 2 && ocean == 0; i++)
				{
					for (j = -1; j <= 2 && ocean == 0; j++)
					{
						if (sqrt(pow(i, 2) + pow (j, 2)) <= 5)
						{
							if (g.map.M(g.player.Y() + j, g.player.X() + i) < 16)
							{
								ocean = 1;
							}
						}
					}
				}
				
				//  If we're not near the ocean, fade the sound out
				if (ocean == 0)
				{
					if (LotaGetSoundStatus(snd_Ocean1) & (DSBSTATUS_LOOPING | DSBSTATUS_PLAYING))
					{
						//LotaPlaySound(snd_Ocean1, 0, false);
						LotaFadeSound(snd_Ocean1, -2);
					}
					if (LotaGetSoundStatus(snd_Ocean2) & (DSBSTATUS_LOOPING | DSBSTATUS_PLAYING))
					{
						//LotaPlaySound(snd_Ocean2, 0, false);
						LotaFadeSound(snd_Ocean2, -2);
					}

				}
				//  we are near the ocean, so check to see if we need to play the next
				//  sound (at 1 second intervals)
				else
				{
					if (lastOceanSound + 1000 < clock())
					{
						if (rnd(1, 2) == 1)
						{
							LotaPlaySound(snd_Ocean1, 0, false);
						}
						else
						{
							LotaPlaySound(snd_Ocean2, 0, false);
						}

						lastOceanSound = clock();
					}
				}
				
				//  Play mountain sounds...
				if (g.player.Terrain() == mapMountain)
				{
					if (!(LotaGetSoundStatus(snd_Mountains) & DSBSTATUS_PLAYING))
					{
						LotaPlaySound(snd_Mountains, DSBPLAY_LOOPING, true);
						//LotaFadeSound(snd_Mountains, 2, DSBPLAY_LOOPING);
					}
				}
				else if (LotaGetSoundStatus(snd_Mountains) & (DSBSTATUS_LOOPING | DSBSTATUS_PLAYING))
				{
					//if (LotaGetSoundStatus(snd_Mountains) & DSBSTATUS_PLAYING)
					{
						LotaFadeSound(snd_Mountains, -1, 0);
						//LotaStopSound(snd_Mountains);
					}

				}

			}
		}
		//
		// End sounds
		/////////////////////////////////////////////////////////////////////////

	}

	// Blit the back buffer to the front buffer
	DDFlip();

	updating = false;

	if (g.menuKey && mode == looping)
		DoCommand(g.menuKey);

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
void DrawBottomText ( LPDIRECTDRAWSURFACE7 pDDS )
{
	for (int i = 0; i < 5; i++)
	{
		WriteText (g_pDDSBack, 32, 368 - 16 * i, g.Bottom(i), g.BottomColor(i));
	}
}	

/****************************************************************************
 *	void DrawBorder( LPDIRECTDRAWSURFACE7 pDDS, unsigned int boxColor)		*
 *																			*
 *  This function draws the border around the screen.						*
 *																			*
 *	Parameters:	the direct draw surface to draw to, and the color to draw	*
 *  Returns:	void														*
 ****************************************************************************/
void DrawBorder( LPDIRECTDRAWSURFACE7 pDDS, unsigned int boxColor)
{

	DrawLine(pDDS, 0, 0, 1, myWindowWidth, boxColor);
	DrawLine(pDDS, 0, 0, 0, myWindowHeight, boxColor);
	DrawLine(pDDS, 0, myWindowHeight - 12, 1, myWindowWidth, boxColor);
	DrawLine(pDDS, myWindowWidth - 12, 0, 0, myWindowHeight, boxColor);
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
void DrawInnerBorder( LPDIRECTDRAWSURFACE7 pDDS, unsigned int innerColor)
{
	DrawInnerLine(pDDS, 0, 0, 1, myWindowWidth, innerColor);
	DrawInnerLine(pDDS, 0, 0, 0, myWindowHeight, innerColor);
	DrawInnerLine(pDDS, 0, myWindowHeight - 12, 1, myWindowWidth + 2, innerColor);
	DrawInnerLine(pDDS, myWindowWidth - 12, 0, 0, myWindowHeight, innerColor);

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
void DrawLine(LPDIRECTDRAWSURFACE7 pDDS, int left, int top, int direction, 
			  int length, unsigned int boxColor)
{
	int boxWidth = 12;
	const int innerOffsetH = 8;
	const int innerOffsetV = 2;
	const int innerWidth = 2;

	top += 2;

	if (direction == 1)
	{
		boxWidth -= 2;
		DDPutBox (g_pDDSBack, left, top, length, 
						  boxWidth, boxColor);

	}
	else
	{
		length -= 4;

		DDPutBox (g_pDDSBack, left, top, boxWidth, length, 
						  boxColor);

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
void DrawInnerLine(LPDIRECTDRAWSURFACE7 pDDS, int left, int top, int direction, 
			  int length, unsigned int innerColor)
{
	int boxWidth = 12;
	int innerOffsetH = 8;
	int innerOffsetV = 2;
	int innerWidth = 2;

	top += 2;

	if (direction == 1)
	{
		//boxWidth -= 4;
		DDPutBox (pDDS, left + innerOffsetH, 
						  top + innerOffsetV, 
						  length - boxWidth, 
						  innerWidth, 
						  innerColor);
	}
	else
	{
		//length -= 2;

		DDPutBox (pDDS, left + innerOffsetH, 
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
 *  This function is the text driver that writes to our direct draw surface	*
 *	the text we want at the x,y point given in our font.					*
 *	The color is is overloaded so an array of coloring can be passed, or	*
 *	just a single color.													*
 ****************************************************************************/
void WriteText ( LPDIRECTDRAWSURFACE7 pDDS, int px, int py, const char *theText, unsigned long c)
{
	int i, len = lstrlen(theText) + 1;
	unsigned int* coloring = new unsigned int[len];
	
	for (i = 0; i < len; i++)
	{
		coloring[i] = c;
	}

	WriteText ( pDDS, px, py, theText, coloring );

	delete [] coloring;

}

/****************************************************************************
 *  void WriteText ( LPDIRECTDRAWSURFACE7 pDDS, int px, int py,				*
 *					 const char *theText, const unsigned int* coloring)		*
 *																			*
 *  This function is the text driver that writes to our direct draw surface	*
 *	the text we want at the x,y point given in our font.					*
 *	The color is is overloaded so an array of coloring can be passed, or	*
 *	just a single color.													*
 ****************************************************************************/
void WriteText ( LPDIRECTDRAWSURFACE7 pDDS, int px, int py, const char *theText, const unsigned int *coloring)
{
	int i;
	int fx, fy, c;
	int len = strlen(theText);
	RECT fontRect;
	RECT tempRect;
	RECT destRect;
	int color;
	
	for (i = 0; i < len; i++, px += 16) 
	{
		c = toupper(theText[i]);
		if (coloring != NULL)
		{
			color = coloring[i];
		}
		else
		{
			color = CreateRGB(255, 255, 255);
		}

		///  removed the new graphics because colored text looks like crap.  I need to 
		///	 antialias it some other way
		fx = c % 16 * 16 ;//+ 256 * g.newGraphics;
		fy = int(c / 16) * 16 ;
		

		SetRect(&fontRect, fx, fy, fx + 16, fy + 16);
		SetRect(&tempRect, 0, 0, 16, 16);
		SetRect(&destRect, px, py, px + 16, py + 16);
		
		DDPutBox (g_pDDSTemp, 0, 0, 16, 16, color);

		//g_pDDSTemp->Blt (&tempRect, g.Font(), &fontRect, DDBLT_WAIT | DDBLT_KEYSRC, NULL);
		g_pDDSTemp->BltFast (tempRect.left, tempRect.top, g.Font(), &fontRect, DDBLTFAST_SRCCOLORKEY);

		//pDDS->Blt(&destRect, g_pDDSTemp, &tempRect,  DDBLT_WAIT | DDBLT_KEYSRC, NULL);
		pDDS->BltFast (destRect.left, destRect.top, g_pDDSTemp, &tempRect, DDBLTFAST_SRCCOLORKEY);
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
void DrawTile( LPDIRECTDRAWSURFACE7 pDDS, int px, int py, int tile)
{
	int tx, ty;

	RECT tileRect;
	RECT destRect;

	tx = tile % 16 * 16 + 256 * g.newGraphics;
	ty = int(tile / 16) * 16 ;
	
	SetRect(&tileRect, tx, ty, tx + 16, ty + 16);
	SetRect(&destRect, px, py, px + 16, py + 16);
	
	//pDDS->Blt(&destRect, g.Tiles(), &tileRect,  DDBLT_WAIT, NULL);
	pDDS->BltFast(destRect.left, destRect.top, g.Tiles(), &tileRect, DDBLTFAST_WAIT);

}

/****************************************************************************
 *  DrawMonster( LPDIRECTDRAWSURFACE7 pDDS, int px, int py, int monst)		*											*
 *																			*
 *  This function drives monsters when they are displayed					*
 ****************************************************************************/
void DrawMonster( LPDIRECTDRAWSURFACE7 pDDS, int px, int py, int monst)
{
	int tx, ty;

	RECT monstRect;
	RECT destRect;

	tx = (monst % 8) * 64;
	ty = (monst / 8) * 64;
	
	SetRect(&monstRect, tx, ty, tx + 64, ty + 64);
	SetRect(&destRect, px, py, px + 64, py + 64);
		
	//pDDS->Blt(&destRect, g.Character(), &charRect,  DDBLT_WAIT | DDBLT_KEYSRC, NULL);
	pDDS->BltFast(destRect.left, destRect.top, g.Monsters(), &monstRect, DDBLTFAST_SRCCOLORKEY);

}

/****************************************************************************
 *  void DrawCharacter ( LPDIRECTDRAWSURFACE7 pDDS )						*
 *																			*
 *  This function displays the character sprite in the middle of the map	*
 *	subscreen.																*
 ****************************************************************************/
void DrawCharacter( LPDIRECTDRAWSURFACE7 pDDS, int anim)
{
	int tx, ty;
	int px = g.vertLine + 16;
	int width = (624 - px) / 16;
	int py = 144;

	RECT charRect;
	RECT destRect;

	px += int(width / 2) * 16;
	
	tx = anim * 32 + g.newGraphics * 96;
	ty = (g.player.FaceDirection() - 1) * 32 ;
	
	if (g.invisible)
	{
		ty += 11 * 32;
	}
	else if (g.guard)
	{
		ty += 4 * 32;
	}

	SetRect(&charRect, tx, ty, tx + 32, ty + 32);
	SetRect(&destRect, px, py, px + 32, py + 32);
		
	//pDDS->Blt(&destRect, g.Character(), &charRect,  DDBLT_WAIT | DDBLT_KEYSRC, NULL);
	pDDS->BltFast(destRect.left, destRect.top, g.Character(), &charRect, DDBLTFAST_SRCCOLORKEY);

}

/****************************************************************************
 *  void DrawRafts ( LPDIRECTDRAWSURFACE7 pDDS )							*
 *																			*
 *  This function draws all the rafts that are on screen.					*
 ****************************************************************************/
void DrawRafts( LPDIRECTDRAWSURFACE7 pDDS)
{
	int tx, ty;
	int lx = g.vertLine + 16;
	int width = (624 - lx) / 16;
	int px = lx + int(width / 2) * 16;
	int py = 144;
	int i;
	int rx, ry;
	RECT charRect;
	RECT destRect;
	
	tx = g.raftAnim * 32 + g.newGraphics * 96;
	ty = 256;
	
	SetRect(&charRect, tx, ty, tx + 32, ty + 32);
	
	for (i = 1; i < 32; i++)
	{
		if (g.map.MapNumber() != g.player.RaftMap(i))
			continue;

		rx = px - (g.player.X() - g.player.Raft(i).x) * 16;
		ry = py - (g.player.Y() - g.player.Raft(i).y) * 16;
		
		if (i == g.player.OnRaft())
		{
			if (g.raftFacing == lotaWest)
			{
				SetRect(&charRect, tx, ty + 64, tx + 32, ty + 96);
			}
			else
			{
				SetRect(&charRect, tx, ty + 32, tx + 32, ty + 64);
			}
		}
		else
		{
			SetRect(&charRect, tx, ty , tx + 32, ty + 32);
		}

		if (rx >= lx && ry >= 16 && rx <= 592 && ry < 272)
		{

			SetRect(&destRect, rx, ry, rx + 32, ry + 32);
			//pDDS->Blt(&destRect, g.Character(), &charRect,  DDBLT_WAIT | DDBLT_KEYSRC, NULL);
			pDDS->BltFast(destRect.left, destRect.top, g.Character(), &charRect, DDBLTFAST_SRCCOLORKEY);
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
void ChangeScreenMode()
{
	RECT rect;

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
		MoveWindow (g.hwnd(), g.screenLeft, g.screenTop, 
			actualWindowWidth + GetSystemMetrics(SM_CXSIZEFRAME)*2, 
			actualWindowHeight + GetSystemMetrics(SM_CYSIZEFRAME)*2 + GetSystemMetrics(SM_CYMENU) + GetSystemMetrics(SM_CYCAPTION), true);

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
		MoveWindow (g.hwnd(), -GetSystemMetrics(SM_CXSIZEFRAME), -GetSystemMetrics(SM_CYCAPTION) - GetSystemMetrics(SM_CYSIZEFRAME), 
			actualWindowWidth + GetSystemMetrics(SM_CXSIZEFRAME)*2,
			actualWindowHeight + GetSystemMetrics(SM_CYSIZEFRAME)*2 + GetSystemMetrics(SM_CYMENU) + GetSystemMetrics(SM_CYCAPTION), true);

	}

	g.LoadFont();
	g.Lock();
	g.ResetTimers();		// reset the animation timers

}

/****************************************************************************
 *	void CheckAnim()														*
 *																			*
 *  This function animates the main character.  It should be called in the	*
 *	main drawing loop.														*
 *																			*
 *	Parameters:	none.														*
 *  Returns:	void														*
 ****************************************************************************/
void CheckAnim()
{
	static last = 0;

	if (g.Animating() && last + 100 <= clock())
	{
		last = clock();

		g.AnimFrame(animInc);

		g.charAnimCount++;

		if (g.charAnimCount > 6)
		{
			g.charAnimCount = 0;
			g.Animating(false);
		}

	}
}

/****************************************************************************
 *	void wait(int howLong)													*
 *																			*
 *  This function is a message-friendly wait that will continue screen-		*
 *	drawing and animation.  It allows the code to pause for a specified		*
 *	amount of time.															*
 *																			*
 *	Parameters:	the number of milliseconds to wait							*
 *  Returns:	void														*
 ****************************************************************************/
void wait (int howLong, bool keyBreak)
{
	MSG			msg;
	int			clock1;
	int			clock2;
	int			key = g.menuKey;
	CmdMode		old = g.commandMode;


	g.commandMode = cmdBad;
	g.menuKey = 0;

	if (howLong > 0)
	{
		clock1 = clock();

		do 
		{


			if (!GetMessage (&msg, NULL, 0, 0))
			{
				g.done = true;
				g.commandMode = old;

				return;
			}

			TranslateMessage(&msg);
			DispatchMessage (&msg);

			CheckJoystick();

			HeartBeat(looping);

			clock2 = clock();

			if (keyBreak && g.menuKey != 0)
				break;

		} while (clock2 - clock1 < howLong);
	}

	g.commandMode = old;
	
	// don't store the menu key if we are just breaking the wait
	// this may not be necessary, because chances are this function will be called
	// again before the command mode is set to prompt or enter command.
	if (keyBreak)
		g.menuKey = key;

}

/****************************************************************************
 * void WaitKey()															*
 *																			*
 *  Waits for a key or joystick press.										*
 ****************************************************************************/
void WaitKey()
{
	CmdMode oldMode = g.commandMode;
	int key = g.menuKey;

	g.menuKey = 0;
	g.commandMode = cmdBad;

	while (g.menuKey == 0)
		wait(1);

	g.menuKey = key;
	g.commandMode = oldMode;

}

/****************************************************************************
 *	void CheckSurfaces()													*
 *																			*
 *  This function checks the ddraw surfaces to make sure they haven't been	*
 *	lost.  It currently does not restore the contents of the surface.		*
 *																			*
 *	Parameters:	none														*
 *  Returns:	void														*
 ****************************************************************************/
void CheckSurfaces()
{
	// Check the font surface
	if (g.Font())
	{
		if (g.Font()->IsLost() == DDERR_SURFACELOST)
			g.Font()->Restore();
	}
	// Check the tiles surface
	if (g.Tiles())
	{
		if (g.Tiles()->IsLost() == DDERR_SURFACELOST)
			g.Tiles()->Restore();
	}
	// Check the character surface
	if (g.Character())
	{
		if (g.Character()->IsLost() == DDERR_SURFACELOST)
			g.Character()->Restore();
	}
	// Check the D3D surfaces
	if (g.floorTexture)
	{
		if (g.floorTexture->IsLost() == DDERR_SURFACELOST)
			g.floorTexture->Restore();
	}

	DDCheckSurfaces();


}

/****************************************************************************
 *	int rnd(int lo, int hi)													*
 *																			*
 *  This function returns a randomly generated number between lo and hi,	*
 *	inclusive.																*
 *																			*
 *	Parameters:	range of numbers to be returned.							*
 *  Returns:	the number													*
 ****************************************************************************/
int rnd(int lo, int hi)
{
	return int((rand() / 32768.0) * (hi - lo + 1)) + lo;
}

double frnd(double lo, double hi)
{
	return ((rand() / 32768.0) * (hi - lo)) + lo;
}

/****************************************************************************
 *	int SubMenu(MenuItemList items)											*
 *																			*
 *  This function creates a sub menu in the top of the map section and		*
 *	forces the player to chose an option from the list provided.			*
 *																			*
 *	Parameters:	a MenuItemList collection of menu items.					*
 *  Returns:	the choice the user made.									*
 ****************************************************************************/
int SubMenu(const MenuItemList &items)
{
	String stBuffer;

	g.subMenu.onScreen = true;
	g.subMenu.theList = items;
	g.subMenu.width = 0;

	for (int i = 0; i < g.subMenu.theList.TotalItems(); i++)
	{
		stBuffer = g.subMenu.theList.GetItem(i);

		if (len(stBuffer) + 6 > g.subMenu.width)
		{
			g.subMenu.width = len(stBuffer) + 6;
		}

	}

	stBuffer = "Choose " + g.subMenu.title;

	if (len(stBuffer) + 2 > g.subMenu.width)
	{
		g.subMenu.width = len(stBuffer) + 2;
	}

	g.commandMode = cmdBad;
	g.quickMenu = true;

	do
	{
		g.menuKey = 0;
		wait(1);

		if (g.menuKey == VK_UP)
		{
			g.subMenu.value--;
			if (g.subMenu.value < 0)
				g.subMenu.value = 0;
		}
		if (g.menuKey == VK_DOWN)
		{
			g.subMenu.value ++;
			if (g.subMenu.value >= items.TotalItems())
				g.subMenu.value = items.TotalItems() - 1;
		}
		else if (g.menuKey >= '0')
		{
			char vv[2];
			int v;

			vv[0] = (char)g.menuKey;
			vv[1] = 0;

			if (g.menuKey > '9')
			{
				v = char(g.menuKey) - 55;
			}
			else
			{
				v = atoi(vv);
			}

			if (v < items.TotalItems())
			{
				g.subMenu.value = v;
				g.menuKey = VK_RETURN;
			}

		
		}


	} while (g.menuKey != VK_RETURN && !g.done);

	wait(300);

	g.quickMenu = false;
	g.subMenu.onScreen = false;
	g.commandMode = cmdEnterCommand;

	return g.subMenu.value;

}

/****************************************************************************
 *	void DrawMenu( LPDIRECTDRAWSURFACE7 pDDS )								*
 *																			*
 *  This function draws the submenu created by SubMenu() onto the direct	*
 *	draw surface.															*
 *																			*
 *	Parameters:	The directdraw surface to write to.							*
 *  Returns:	void														*
 ****************************************************************************/
void DrawMenu( LPDIRECTDRAWSURFACE7 pDDS )
{
	String theString;
	int xx, yy, i = 0, height;
	String buffer;
	unsigned long fontColor = lotaWhite;

	if (g.map.MapType() == mapDungeon)
	{
		fontColor = lotaCyan;
	}

	xx = 624 - g.subMenu.width * 16;
	yy = 16;
	height = (g.subMenu.theList.TotalItems() + 3) * 16;

	if ( xx < g.vertLine + 16 )
	{
		xx = g.vertLine + 16;
		i = 1;
	}

	DDPutBox(pDDS, xx, yy, 624 - xx, height, lotaBlack);

	if (i == 0)
	{
		xx += 16;
	}

	theString = g.subMenu.title;

	WriteText(pDDS, xx + int((624 - xx) / 32) * 16 - int(len(theString) / 2) * 16, yy, theString, fontColor);
	yy += 16;

	for (i = 0; i < g.subMenu.theList.TotalItems(); i++)
	{
		yy += 16;
		buffer = g.subMenu.theList.GetItem(i);

		if (i > 9)
			theString = char(i + 'A' - 10);
		else
			theString = i;

		theString += ". " + buffer;

		WriteText(pDDS, xx, yy, theString);

		if (i == g.subMenu.value)
		{
			int xx1;

			xx1 = xx + len(theString) * 16;
			WriteText (pDDS, xx1, yy, "`");
		}


	}


}

/****************************************************************************
 *	int QuickMenu(MenuItemList items, int spaces, int value)				*
 *																			*
 *  This function creates a quick menu at the bottow of the screen,			*
 *	allowing the player to pick from a few choices.							*
 *																			*
 *	Parameters:	The MenuItemList, the amount of spaces between the items	*
 *		in the list, and the default value, and the initial and changed		*
 *		colors.																*
 *  Returns:	the player's choice.										*
 ****************************************************************************/
int QuickMenu(const MenuItemList &items, int spaces, int value, unsigned int clrInit, unsigned int clrChanged)
{
	int	 spacing[10] = {0,0,0,0,0,0,0,0,0,0};
	int  last = 0;
	CmdMode  oldCmd;
	String tempLine("Choose: ");
	String topLine;
	char tempItem[40];
	unsigned int colors[40];

	if (clrInit == 0)
		clrInit = lotaWhite;
	if (clrChanged == 0)
		clrChanged = clrInit;

	for (int i = 0; i < 40; i++)
		colors[i] = clrChanged;
	
	
	spacing[0] = 8;

	// Construct the temporray line
	for (int i = 0; i < items.TotalItems(); i++)
	{
		items.GetItem(i, tempItem);

		tempLine += tempItem + space(spaces);

		spacing[i] += last + lstrlen(tempItem) - 1;
		last = spacing[i]  + spaces + 1;
	}

	g.AddBottom(tempLine, clrInit);
	g.AddBottom("");

	g.quickMenu = true;

	topLine = tempLine;
	tempLine = space(spacing[value]) + "`";

	g.UpdateBottom(tempLine, clrInit);
	oldCmd = g.commandMode;
	g.commandMode = cmdBad;

	do
	{
		g.menuKey = 0;
		wait(1);


		if (g.menuKey == VK_LEFT)
		{
			value--;
			if (value < 0)
				value = 0;
		}
		if (g.menuKey == VK_RIGHT)
		{
			value ++;
			if (value >= items.TotalItems())
				value = items.TotalItems() - 1;
		}
		else if (g.menuKey >= '0')
		{
			for (i = 0; i < items.TotalItems(); i++)
			{
				items.GetItem(i, tempItem);

				if (g.menuKey == tempItem[0])
				{
					value = i;
					g.menuKey = VK_RETURN;
				}
			}
		}

		tempLine = space(spacing[value]) + "`";

		if (g.menuKey)
		{
			g.UpdateBottom(topLine, 1, colors); 
			g.UpdateBottom(tempLine, clrChanged);
		}


	} while (g.menuKey != VK_RETURN && !g.done);

	wait(100);

	g.quickMenu = false;
	g.commandMode = oldCmd;

	g.AddBottom("");

	return value;

}

/****************************************************************************
 *	String space(int num)													*
 *																			*
 *  This function returns a character string that contains the specified	*
 *	number of spaces.														*
 *																			*
 *	Parameters:	the number of spaces.										*
 *  Returns:	The string containing the spaces.							*
 ****************************************************************************/
String space(int num)
{
	String tempString;

	for (int i = 0; i < num; i++)
	{
		tempString += ' ';
	}

	return tempString;
}

/****************************************************************************
 *	void RaftAnim()															*
 *																			*
 *  This function animates the raft											*
 *																			*
 *	Parameters:	none														*
 *  Returns:	void														*
 ****************************************************************************/
void RaftAnim()
{
	static int last = 0;

	if (last + 100 < clock())
	{

		g.raftAnim++;

		if (g.raftAnim == 3)
			g.raftAnim = 0;

		last = clock();
	}

}

/****************************************************************************
 *	void CheckJoystick()													*
 *																			*
 *  This function checks the current state of the joystick, and generates	*
 *	a key event if there is any action happening.  It also handles the		*
 *	menu when the button is held.											*
 *																			*
 *	Parameters:	none														*
 *  Returns:	void														*
 ****************************************************************************/
void CheckJoystick()
{
    HRESULT     hr;
    DIJOYSTATE  js;					// DInput joystick state 
	static		buttonTime = 0;		// time when the button was held down
	static		buttonHeld = 0;		// are they holding the button down?
	int			key = 0;
	static int	lastMove = 0;

    if( g.pJoystick ) 
    {
        do
		{
			// Poll the device to read the current state
			hr = g.pJoystick->Poll();

			// Get the input's device state
			hr = g.pJoystick->GetDeviceState( sizeof(DIJOYSTATE), &js );

			if( hr == DIERR_INPUTLOST || hr == DIERR_NOTACQUIRED )
			{
				// DInput is telling us that the input stream has been
				// interrupted. We aren't tracking any state between polls, so
				// we don't have any special reset that needs to be done. We
				// just re-acquire and try again.
				hr = g.pJoystick->Acquire();
				if( FAILED(hr) )  
					return;
			}
		}        
        while( DIERR_INPUTLOST == hr );

        if( FAILED(hr) )  
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
			OutputDebugString("Held button.\n");

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

}

/****************************************************************************
 *	int ChooseNumber(int max)												*
 *																			*
 *  This function asks the user to choose a number, either by the joystick	*
 *	or the keyboard.														*
 *																			*
 *	Parameters:	the maximum value the user is allowed to select.			*
 *  Returns:	the number that the user selected.							*
 ****************************************************************************/
int ChooseNumber(int max)
{
	int i;
	unsigned int color[40];
	int method = 0;
	int amount = 0;
	CmdMode tempCommand;
	String tempString;

	g.AddBottom("");

	tempString = "Enter number by ";
	for (i = 0; i < len(tempString); i++)
	{
		color[i] = lotaWhite;
	}

	tempString += "keyboard";
	for (; i < len(tempString); i++)
	{
		color[i] = lotaYellow;
	}

	tempString += " or ";
	for (; i < len(tempString); i++)
	{
		color[i] = lotaWhite;
	}

	tempString += "joystick";
	for (; i < len(tempString); i++)
	{
		color[i] = lotaCyan;
	}

	g.AddBottom(tempString, color);
	g.AddBottom("");

	g.quickMenu = true;
	tempCommand = g.commandMode;
	g.commandMode = cmdBad;

	do
	{

		g.menuKey = 0;
		wait(1);
		
		if (method == 0)
		{
			switch(g.menuKey)
			{
			case 0:
				break;

			case VK_RIGHT:
			case VK_UP:
			case VK_LEFT:
			case VK_DOWN:

				g.AddBottom("Use joystick - press button when done");
				g.AddBottom("");
				g.AddBottom("  Horizontal - Slow change", lotaCyan);
				g.AddBottom("  Vertical   - Fast change", lotaCyan);
				g.AddBottom("                          - 0 -");

				method = 2;

				break;
			default:
				g.AddBottom("Keyboard entry-press return when done", lotaYellow);
				g.AddBottom("");
				g.AddBottom("");
				g.AddBottom("                          - 0 -");
				method = 1;

				break;
			}

		}

		if (method == 1)
		{
			if (g.menuKey >= '0' && g.menuKey <= '9')
				amount = 10 * amount + g.menuKey - 0x30;

			if (g.menuKey == VK_BACK)
				amount /= 10;

			if (amount > max)
				amount = max;

			if (amount < 0)
				amount = 0;


			tempString = "                          - " + String(amount) + " -";

			g.UpdateBottom(tempString);
		
		}
		else if (method == 2)
		{
			switch (g.menuKey)
			{
			case VK_RIGHT:
				amount ++;
				break;
			case VK_UP:
				amount += 20;
				break;
			case VK_LEFT:
				amount --;
				break;
			case VK_DOWN:
				amount -= 20;
				break;
			}

			if (amount > max)
				amount = max;

			if (amount < 0)
				amount = 0;

			tempString = "                          - " + String(amount) + " -";

			g.UpdateBottom(tempString);
		}

		
	} while (g.menuKey != VK_RETURN && !g.done );
	
	g.quickMenu = false;
	g.commandMode = tempCommand;

	return amount;

}









//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//		Title Screen Functions
//
//
//

char wnd[25][100];
unsigned int color[25][40];
unsigned int bgColor = lotaBlack;

String tempName;
String files[8];
int page;
int maxPages = 0;

void ClearTitleText()
{
	for (int i = 0; i < 25; i++)
	{
		wnd[i][0] = 0;
		memset(color[i], 0xFF, sizeof(color[i]));
	}
}

void DisplayTitleScreen(LPDIRECTDRAWSURFACE7 pDDS)
{
	int i;
	RECT destRect;
	unsigned int borderColor, lineColor;

	if (!runningTitle)
	{
		ClearTitleText();
		RunTitleScreen();

		return;
	}

	DDPutBox (g_pDDSBack, 0, 0, 640, 400, bgColor);

	if (g.titleMenu > 0 && (g.titleState == stMenu1 || g.titleState == stMenu2))
	{
		int loc = 7 + g.titleMenu * 2;

		for (i = 9; i < 16; i += 2)
		{
			wnd[i][4] = ' ';

			if (i == loc)
			{
				wnd[i][4] = '`';
			}

		}

	}
	else if (g.titleState == stLoadGame)
	{
		int loc = 7 + g.titleMenu;

		if (g.titleMenu > 0)
			loc ++;
		if (g.titleMenu == 9)
			loc ++;

		for (i = 7; i < 19; i++)
		{
			wnd[i][5] = ' ';

			if (i == loc)
			{
				wnd[i][5] = '`';
			}
		}
	}

	// draw borders & stuff	
	switch (g.titleState)
	{
		case stNoState: 

			SetRect(&destRect, 0, 0, 640, 400);
			pDDS->BltFast(destRect.left, destRect.top, g.pTitleScreen, &destRect, DDBLTFAST_NOCOLORKEY );

			break;

		case stMenu1:
			borderColor = lotaBlue;
			lineColor = lotaWhite;

			DrawBorder(pDDS, borderColor);

			DrawLine(pDDS, 0, 20 * 16, 1, myWindowWidth, borderColor);
 			DrawLine(pDDS, 3 * 16 + 8, 0, 0, 7 * 16, borderColor);
  			DrawLine(pDDS, 35 * 16 + 8, 0, 0, 7 * 16, borderColor);
 			DrawLine(pDDS, 3 * 16 + 8, 6 * 16+4, 1, (35-3+1) * 16 -4, borderColor);

			DrawInnerBorder(pDDS, lineColor);

			DrawInnerLine(pDDS, 0, 20 * 16, 1, myWindowWidth, lineColor);
			DrawInnerLine(pDDS, 3 * 16 + 8, 0, 0, 7 * 16, lineColor);
			DrawInnerLine(pDDS, 35 * 16 + 8, 0, 0, 7 * 16, lineColor);
			DrawInnerLine(pDDS, 3 * 16 + 8, 6 * 16+4, 1, (35-3+1) * 16-2, lineColor);

			break;

		case stMenu2:
			borderColor = lotaBrown;
			lineColor = lotaYellow;

			DrawBorder(pDDS, borderColor);

			DrawLine(pDDS, 0, 20 * 16, 1, myWindowWidth, borderColor);
 			DrawLine(pDDS, 3 * 16 + 8, 0, 0, 7 * 16, borderColor);
  			DrawLine(pDDS, 35 * 16 + 8, 0, 0, 7 * 16, borderColor);
 			DrawLine(pDDS, 3 * 16 + 8, 6 * 16+4, 1, (35-3+1) * 16 -4, borderColor);

			DrawInnerBorder(pDDS, lineColor);

			DrawInnerLine(pDDS, 0, 20 * 16, 1, myWindowWidth, lineColor);
			DrawInnerLine(pDDS, 3 * 16 + 8, 0, 0, 7 * 16, lineColor);
			DrawInnerLine(pDDS, 35 * 16 + 8, 0, 0, 7 * 16, lineColor);
			DrawInnerLine(pDDS, 3 * 16 + 8, 6 * 16+4, 1, (35-3+1) * 16-2, lineColor);

			break;

		case stNewGame:

			borderColor = lotaLtGray;
			lineColor = lotaYellow;

			DrawBorder(pDDS, borderColor);
			
  			DrawLine(pDDS, 10 * 16, 10 * 16, 1, 19 * 16-4, borderColor);  // top
 			DrawLine(pDDS, 10 * 16, 12 * 16, 1, 19 * 16-4, borderColor);  // bottom
			DrawLine(pDDS, 10 * 16, 10 * 16, 0, 3 * 16-4, borderColor);   // left
			DrawLine(pDDS, 28 * 16, 10 * 16, 0, 3 * 16-4, borderColor);   // right

			DrawInnerBorder(pDDS, lineColor);
 
 			DrawInnerLine(pDDS, 10 * 16, 10 * 16, 1, 19 * 16-4, lotaWhite);
 			DrawInnerLine(pDDS, 10 * 16, 12 * 16, 1, 19 * 16-2, lotaWhite);
			DrawInnerLine(pDDS, 10 * 16, 10 * 16, 0, 3 * 16-4, lotaWhite);
 			DrawInnerLine(pDDS, 28 * 16, 10 * 16, 0, 3 * 16-4, lotaWhite);




			strcpy(wnd[11], "            " + tempName + "_");

			break;

		case stNewGameMusic:
			
			if (!(LotaGetSoundStatus(snd_VeryGood) && DSBSTATUS_PLAYING))
			{
				g.player.NewPlayer(tempName);

				SetNewGameText();

				wait(100);
				g.titleScreen = false;
			}

		case stNewGameText:

			borderColor = lotaLtGray;
			lineColor = lotaYellow;

			DrawBorder(pDDS, borderColor);
			DrawInnerBorder(pDDS, lineColor);

		case stLoadGame:

			borderColor = lotaLtGray;
			lineColor = lotaYellow;

			DrawBorder(pDDS, borderColor);
			DrawInnerBorder(pDDS, lineColor);

			break;
	}

	for (i = 0; i < 25; i++)
	{
		WriteText(pDDS, 0, i * 16, wnd[i], color[i]);
	}

}

void RunTitleScreen()
{
	runningTitle = true;
	g.titleScreen = true;

	while (g.titleScreen && !g.done)
	{
		wait(10);

		TitleKey();

	}

	runningTitle = false;
}

void WriteSlowToString(char *target, const char* source)
{
	
	int i = 0;
	int length = lstrlen(source);
	int key;

	while (i < length && !g.done)
	{
		target[i] = source[i];
		target[i+1] = 0;

		i++;

		if (!g.menuKey)
			wait(62);
	}

	key = g.menuKey;
	wait(1);
	g.menuKey = key;

}


void SetMenu1()
{
	g.titleState = stMenu1;
	g.titleMenu = 1;

	ClearTitleText();
	bgColor = lotaLtBlue;

	strcpy(wnd[9],  "      1.  Play a game");
	strcpy(wnd[11], "      2.  Some Simple Instructions");
	strcpy(wnd[13], "      3.  Scenes from Legacy");
	strcpy(wnd[15], "      4.  Color Test");
	strcpy(wnd[18], "  (Pick option by keyboard or joystick)");
	strcpy(wnd[22], "  Copyright 1987 - Quest Software, Inc.");

	for (int i = 0; i < 40; i++)
	{
		color[11][i] = lotaLtGray;
		color[13][i] = lotaLtGray;
		color[15][i] = lotaLtGray;
		color[18][i] = lotaBlue;
	}

}

void SetMenu2()
{
	g.titleState = stMenu2;
	g.titleMenu = 1;

	ClearTitleText();
	bgColor = lotaOrange;

	strcpy(wnd[9],  "      1.  Return to first menu");
	strcpy(wnd[11], "      2.  Start a new game");
	strcpy(wnd[13], "      3.  Restart a game");
	strcpy(wnd[15], "      4.  Erase a character");
	strcpy(wnd[18], "  (Pick option by keyboard or joystick)");
	strcpy(wnd[22], "  Copyright 1987 - Quest Software, Inc.");

	for (int i = 0; i < 40; i++)
	{
		/*color[11][i] = ;
		color[13][i] = lotaLtGray;
		color[15][i] = lotaLtGray;*/
		color[18][i] = lotaYellow;
		color[22][i] = lotaYellow;
	}

}

void SetNewGame()
{

	g.titleState = stNewGame;
	g.titleMenu = 0;
	
	tempName = "";

	ClearTitleText();
	bgColor = lotaGreen;

	strcpy(wnd[0],  "             Start a new game");
	strcpy(wnd[4],  "   Type in your new character's name.");
	strcpy(wnd[6],  "    It may be up to 14 letters long.");

	strcpy(wnd[11], "             _");
	strcpy(wnd[17], "  ` Press return key when finished. `");
	strcpy(wnd[20], "   - Press 'del' key to backspace -");
	strcpy(wnd[22], "  - Press 'F1' or Escape to cancel -");


}

void SetNewGameText()
{
	g.titleState = stNewGameText;
	g.titleMenu = 0;
	
	ClearTitleText();
	bgColor = lotaDkGray;

	strcpy(wnd[0],  "             Start a new game");

	for (int i = 0; i < 25; i++)
	{
		lstrcpy(wnd[i], "  ");
	}
	
	for (i = 0; i < 40; i++)
	{
		color[24][i] = lotaYellow;
	}

	i = 4;

	WriteSlowToString(wnd[i++] + 2, "You are only a poor peasant on the");
	WriteSlowToString(wnd[i++] + 2, "world of Tarmalon, so it's hardly");
	WriteSlowToString(wnd[i++] + 2, "surprising that you've never seen");
	WriteSlowToString(wnd[i++] + 2, "a dead man before.  His crumpled");
	WriteSlowToString(wnd[i++] + 2, "figure lies forlornly by the side");
	WriteSlowToString(wnd[i++] + 2, "of the road.");
	WriteSlowToString(wnd[i++] + 2, "                                 ");
	
	i++;

	WriteSlowToString(wnd[i++] + 2, "Fighting your fear, you kneel by");
	WriteSlowToString(wnd[i++] + 2, "the still-warm corpse.  You see a");
	WriteSlowToString(wnd[i++] + 2, "a look of panic on his face, a gold");
	WriteSlowToString(wnd[i++] + 2, "band around his wrist, and a large");
	WriteSlowToString(wnd[i++] + 2, "leather scroll, clutched tightly to");
	WriteSlowToString(wnd[i++] + 2, "his chest.");
	
	lstrcpy(wnd[24], "    (Press key/button to continue)");

	g.menuKey = 0;
	while (g.menuKey == 0 && !g.done)
	{
		wait(50);
	}
	g.menuKey = 0;

	ClearTitleText();

	strcpy(wnd[0],  "             Start a new game");

	for (int i = 0; i < 25; i++)
	{
		lstrcpy(wnd[i], "  ");
	}

	for (i = 0; i < 40; i++)
	{
		color[24][i] = lotaYellow;
	}

	i = 4;

	WriteSlowToString(wnd[i++] + 2, "You've never been a thief, yet");
	WriteSlowToString(wnd[i++] + 2, "something compels you to reach for");
	WriteSlowToString(wnd[i++] + 2, "the leather scroll.  Getting the");
	WriteSlowToString(wnd[i++] + 2, "armband off is trickier, but you");
	WriteSlowToString(wnd[i++] + 2, "manage to snap it around your own");
	WriteSlowToString(wnd[i++] + 2, "wrist.  You scoop up two green coins");
	WriteSlowToString(wnd[i++] + 2, "lying nearby and hasten on your way.");
	WriteSlowToString(wnd[i++] + 2, "                                 ");
	
	i ++;

	WriteSlowToString(wnd[i++] + 2, "Before you've gone more than a few");
	WriteSlowToString(wnd[i++] + 2, "steps, your senses waver and shift.");
	WriteSlowToString(wnd[i++] + 2, "Rising from the mists, as though ");
	WriteSlowToString(wnd[i++] + 2, "you've never been this way before, ");
	WriteSlowToString(wnd[i++] + 2, "is a magnificient structure of ");
	WriteSlowToString(wnd[i++] + 2, "polished stone.  A shimmering arch-");
	WriteSlowToString(wnd[i++] + 2, "way beckons.");
	
	lstrcpy(wnd[24], "    (Press key/button to continue)");

	g.menuKey = 0;
	while (g.menuKey == 0 && !g.done)
	{
		wait(1);
	}
	g.menuKey = 0;

}

void SetRestoreGame()
{
	g.titleState = stLoadGame;
	g.titleMenu = 0;
	
	tempName = "";

	ClearTitleText();
	bgColor = lotaBrown;

	strcpy(wnd[0],  "             Restart a game");
	strcpy(wnd[4],  "        Restart which character?");

	if (page == 0)
	{
		strcpy(wnd[7],  "          0.  Cancel");
	}
	else
	{
		strcpy(wnd[7],  "          0.  Previous Page");
	}

	// file system?
	WIN32_FIND_DATA FileData; 
	HANDLE hSearch; 
	//char szDirPath[] = "c:\\TEXTRO\\"; 
	//char szNewPath[MAX_PATH]; 
	//char szHome[MAX_PATH]; 
	
	 
	BOOL fFinished = FALSE; 

	 
	// Start searching for .TXT files in the current directory. 
	 
	hSearch = FindFirstFile("*.chr", &FileData); 
	if (hSearch == INVALID_HANDLE_VALUE) 
	{ 
		fFinished = true;
	} 
	 
	for (int i = 0; i < 8; i++)
	{
		files[i] = "";
	}

	maxPages = 0;
	i = 0;
	while (!fFinished) 
	{ 
		if (i / 8 == page)
		{
			files[i % 8] = FileData.cFileName;
		}

		i++;

		if (!FindNextFile(hSearch, &FileData)) 
		{
			if (GetLastError() == ERROR_NO_MORE_FILES) 
			{ 
				/*MessageBox(hwnd, "No more .TXT files.", 
					"Search completed.", MB_OK); */
				fFinished = TRUE; 
			} 
			else 
			{ 
				//ErrorHandler("Couldn't find next file."); 
			} 
		}
	} 
	 
	maxPages = (i - 1) / 8;

	// Close the search handle. 
	if (!FindClose(hSearch)) 
	{ 
		//ErrorHandler("Couldn't close search handle."); 
	} 

	for (i = 0; i < 8; i++)
	{
		strcpy(wnd[9+i],  "          " + String(i+1) + ".  " + left(files[i], len(files[i]) - 4));

		if (len(files[i]) == 0)
		{
			strcat(wnd[9+i], "Empty");
		}

	}

	if (page < maxPages)
 		strcpy(wnd[18], "          9.  Next Page");

	strcpy(wnd[21], "   (Select by joystick or number keys)");
	
	for (i = 0; i < 40; i++)
	{
		color[21][i] = lotaYellow;
	}
}

void SetEraseGame()
{
	SetMenu2();
}

void TitleKey()
{
	static int lastTime = 0;
	static int waitTime = 0;
	const int stdWait = 100;

	if (lastTime + waitTime > clock())
		return;

	lastTime = clock();
	waitTime = 0;

	switch(g.titleState)
	{
		case stNoState:

			if (g.menuKey)
			{
				SetMenu1();
				waitTime = 300;
			}

			break;

		case stMenu1:
		case stMenu2:

			if (g.menuKey == VK_DOWN)
			{
				g.titleMenu++;

				if (g.titleMenu > 4)
					g.titleMenu = 4;

				waitTime = stdWait;
			}
			else if (g.menuKey == VK_UP)
			{
				g.titleMenu--;

				if (g.titleMenu < 1)
					g.titleMenu = 1;

				waitTime = stdWait;
			}
			else if (g.menuKey >= '1' && g.menuKey <= '4')
			{
				g.titleMenu = g.menuKey - 0x30;
				
				g.menuKey = VK_RETURN;

				waitTime = stdWait;
			}

			if (g.menuKey == VK_RETURN)
			{
				waitTime = 1000;
				wait(500);

				waitTime = stdWait;

				if (g.titleState == stMenu1)
				{
					if (g.titleMenu == 1)
					{
						SetMenu2();
					}
					else if (g.titleMenu == 2) {}
					else if (g.titleMenu == 3) {}
					else if (g.titleMenu == 4) {}

				}
				else if (g.titleState == stMenu2)
					if (g.titleMenu == 1) // return to first menu
					{
						SetMenu1();
					}
					else if (g.titleMenu == 2) 
					{
						SetNewGame();
					}
					else if (g.titleMenu == 3) 
					{
						page = 0;
						SetRestoreGame();
					}
					else if (g.titleMenu == 4) 
					{
						SetEraseGame();
					}

			}

			break;

		case stNewGame:

			if ((g.menuKey >= 'A' && g.menuKey <= 'Z') || g.menuKey == ' ' ||
				(g.menuKey >= '0' && g.menuKey <= '9') )
			{
			
				if (len(tempName) < 14)
				{
					char k[2];

					k[0] = g.menuKey;
					k[1] = 0;

					tempName += k;
				}

 			}
			else if (g.menuKey == VK_BACK || g.menuKey == VK_DELETE)
			{
				tempName = left(tempName, len(tempName) - 1);
			}
			else if (g.menuKey == VK_ESCAPE || g.menuKey == VK_F1)
			{
				SetMenu2();
			}
			else if (g.menuKey == VK_RETURN && len(tempName) > 0)
			{
				LotaPlaySound(snd_VeryGood);
				g.titleState = stNewGameMusic;
				
				for (int i = 13; i < 25; i++)
				{
					wnd[i][0] = 0;
				}
				
				strcpy(wnd[16],  "    " + tempName + "'s adventures begin");

			}

			break;

		case stLoadGame:
			if (g.menuKey == VK_DOWN)
			{
				g.titleMenu++;

				if (g.titleMenu >= 9)
					g.titleMenu = 9;
				else if (files[g.titleMenu-1] == "")
				{
					g.titleMenu = 9;
				}

				waitTime = stdWait;
			}
			else if (g.menuKey == VK_UP)
			{
				do
				{
					g.titleMenu--;

					if (g.titleMenu <= 0)
						break;

				} while (g.titleMenu > 0 && files[g.titleMenu - 1] == "");
				
				if (g.titleMenu < 0)
					g.titleMenu = 0;

				waitTime = stdWait;
			}
			else if (g.menuKey >= '1' && g.menuKey <= '9')
			{
				g.titleMenu = g.menuKey - 0x30;
				
				g.menuKey = VK_RETURN;

				waitTime = stdWait;
			}

			if (g.titleMenu == 9 && page == maxPages)
			{
				g.titleMenu = 8;

				do
				{
					g.titleMenu--;

					if (g.titleMenu <= 0)
						break;

				} while (g.titleMenu > 0 && files[g.titleMenu - 1] == "");
				
			}

			if (g.menuKey == VK_RETURN)
			{
				waitTime = 1000;
				wait(500);

				waitTime = stdWait;

				if (g.titleMenu == 0)
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
				else if (g.titleMenu == 9)
				{
					page++;

					if (page > maxPages)
						page = maxPages;

					SetRestoreGame();
				}
				else
				{
					g.titleScreen = false;
					g.player.LoadGame(files[g.titleMenu-1]);
				}

			}

			break;

	}

}



