// LotA.cpp : Defines the entry point for the application.
//

#include "lota.h"
#include <time.h>

const looping = 1,
	  redraw = 2;

Global g;

int iFoo = 0;

int APIENTRY WinMain(HINSTANCE hInstance,
                     HINSTANCE hPrevInstance,
                     LPSTR     lpCmdLine,
                     int       iCmdShow)
{
	static TCHAR	szAppName[] = TEXT("Legacy of the Ancients");
	HWND			hwnd;
	MSG				msg;
	WNDCLASS		wndclass;
	int cx, cy;

	wndclass.style			= CS_HREDRAW | CS_VREDRAW;
	wndclass.lpfnWndProc	= WndProc;
	wndclass.cbClsExtra		= 0;
	wndclass.cbWndExtra		= 0;
	wndclass.hInstance		= hInstance;
	wndclass.hIcon			= LoadIcon (NULL, IDI_APPLICATION);
	wndclass.hCursor		= LoadCursor (NULL, IDC_ARROW);
	wndclass.hbrBackground	= (HBRUSH) GetStockObject(WHITE_BRUSH);
	wndclass.lpszMenuName	= NULL;
	wndclass.lpszClassName	= szAppName;

	if (!RegisterClass(&wndclass))
	{
		MessageBox (NULL, TEXT ("This program requires Windows 98!"), szAppName, MB_ICONERROR);

		return 0;
	}

	// Get system window metrics so that our client area is exactly the size we want.
    cx = actualWindowWidth + GetSystemMetrics(SM_CXSIZEFRAME)*2;
    cy = actualWindowHeight + GetSystemMetrics(SM_CYSIZEFRAME)*2 + GetSystemMetrics(SM_CYMENU);

	// Create the window
	hwnd = CreateWindow (szAppName,						// window class name
						 TEXT("Legacy of the Ancients"),// window caption
						 WS_OVERLAPPEDWINDOW,			// window style
						 CW_USEDEFAULT,					// initial x position
						 CW_USEDEFAULT,					// initial y position
						 cx,							// initial width
						 cy,							// initial height
						 NULL,							// parent window handle
						 NULL,							// window menu handle
						 hInstance,						// program instance handle
						 NULL);							// creation parameters
	
	ShowWindow (hwnd, iCmdShow);
	UpdateWindow(hwnd);

	g.SetHInstance(hInstance, hwnd);

	g.LoadFont();

	g.Lock();

	while (GetMessage (&msg, NULL, 0, 0) && !g.done)
	{
		TranslateMessage(&msg);
		DispatchMessage (&msg);

		HeartBeat(looping);
	}

	DDDestroySurfaces();

	return msg.wParam;
	
}

LRESULT CALLBACK WndProc (HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	static int first = 0;
	int i = 0;
	HDC			hdc;
	PAINTSTRUCT	ps;
	//RECT		rect;
	long		returnValue = 0;

	
	switch (message)
	{
	case WM_CREATE:
		
		if (!DDInit(hwnd))
		{
			MessageBox(NULL, TEXT("Couldn't initialize DirectDraw!"), TEXT("Failed"), 0);
			PostQuitMessage(1);
			return 1;
		}

		if (!DDCreateSurfaces(false, actualWindowWidth, actualWindowHeight, 16))
		{
			MessageBox(NULL, TEXT("Couldn't create DirectDraw surfaces!"), TEXT("Failed"), 0);
			PostQuitMessage(1);
			return 1;
		}
		

		//SetTimer(hwnd, passTimer, 1000, NULL);
		//SetTimer(hwnd, commandTimer, 1000, NULL);
		//SetTimer(hwnd, charAnimTimer, 1000, NULL);

		g.ResetTimers();

		g.WriteMenu(i++, "Armor");
		g.WriteMenu(i++, "Disembark");
		g.WriteMenu(i++, "End");
		g.WriteMenu(i++, "Fight");
		g.WriteMenu(i++, "Gamespeed");
		g.WriteMenu(i++, "Hold");
		g.WriteMenu(i++, "Inventory");
		g.WriteMenu(i++, "Magic");
		g.WriteMenu(i++, "Pass");
		g.WriteMenu(i++, "Speak");
		g.WriteMenu(i++, "Use");
		g.WriteMenu(i++, "Weapon");
		g.WriteMenu(i++, "Xamine");
		g.WriteMenu(i++, "");
		g.WriteMenu(i++, "H.P. 300");
		g.WriteMenu(i++, "FOOD 200");
		g.WriteMenu(i++, "GOLD 900");

		g.map.LoadMap(1);

		g.commandMode = 5;
		DoCommand(0);

		return 0;

	case WM_PAINT:
		
		hdc = BeginPaint (hwnd, &ps);

		EndPaint (hwnd, &ps);

		HeartBeat(redraw);
		
		return 0;

	case WM_KEYDOWN:

		if ((wParam >= 65 && wParam <= 91) || wParam == VK_RIGHT || 
			wParam == VK_UP || wParam == VK_LEFT || wParam == VK_DOWN)
		{
			DoCommand(wParam);
		}

		break;

	case WM_TIMER:
		
		switch (wParam)
		{
		case passTimer:
			DoCommand('Z');
			break;

		case commandTimer:
			DoCommand(0);
			break;

		case charAnimTimer:
			CheckAnim();
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
 *  void HeartBeat(short mode)												*
 *																			*
 *  This is the main function that runs the game.  It does all the drawing	*
 *	in the window right now.												*	
 ****************************************************************************/
void HeartBeat(short mode)
{
	if (g.waitCommand < 100)
	{
		DoCommand(0);
	}

	int i = 0;
	int boxWidth = 12;
	int innerOffsetH = 8;
	int innerOffsetV = 2;
	DWORD boxColor;
	DWORD innerColor;

	int innerWidth = 2;

	int vertLine = 16 * 16;
	int horizLine = 18 * 16;
	

	switch (g.map.MapType())
	{
	case mapTown:

		boxColor = lotaOrange;
		innerColor = lotaYellow;
		break;

	case mapMuseum:

		boxColor = lotaDkGray;
		innerColor = lotaYellow;

		break;

	case mapDungeon:

		boxColor = lotaDkGray;
		innerColor = lotaLtGreen;

		break;

	case mapOutside:
	default:
		boxColor = CreateRGB(159,112,64);
		innerColor = lotaYellow;

		break;

	}

	// Check for lost surfaces
	CheckSurfaces();
	
	// Clear the back buffer
	DDPutBox (g_pDDSBack, 0, 0, myWindowWidth, myWindowHeight, CreateRGB(0,0,0));


	// Draw borders
	DDPutBox (g_pDDSBack, 0, 0, boxWidth, myWindowHeight, boxColor);
	DDPutBox (g_pDDSBack, myWindowWidth - boxWidth, 0, boxWidth, myWindowHeight, boxColor);
	DDPutBox (g_pDDSBack, 0, 0, myWindowWidth, boxWidth, boxColor);
	DDPutBox (g_pDDSBack, 0, myWindowHeight - boxWidth, myWindowWidth, boxWidth, boxColor);

	DDPutBox (g_pDDSBack, 0, horizLine, myWindowWidth, boxWidth, boxColor); 
	DDPutBox (g_pDDSBack, vertLine, 0, boxWidth, horizLine, boxColor); 

	// Draw inner lines
	DDPutBox (g_pDDSBack, innerOffsetH, 
						  innerOffsetV, 
						  myWindowWidth - boxWidth, 
						  innerWidth, 
						  innerColor);

	DDPutBox (g_pDDSBack, innerOffsetH, 
						  innerOffsetV, 
						  innerWidth, 
						  myWindowHeight - boxWidth + innerOffsetV, 
						  innerColor);

	DDPutBox (g_pDDSBack, myWindowWidth - boxWidth + innerOffsetH, 
						  innerOffsetV, 
						  innerWidth, 
						  myWindowHeight - boxWidth + innerWidth,	// innerWidth is included here
						  innerColor);								// to eliminate the blank space

	DDPutBox (g_pDDSBack, innerOffsetH, 
						  myWindowHeight - boxWidth + innerOffsetV, 
						  myWindowWidth - boxWidth, 
						  innerWidth, 
						  innerColor);

	DDPutBox (g_pDDSBack, innerOffsetH, 
						  horizLine + innerOffsetV, 
						  myWindowWidth - boxWidth, 
						  innerWidth, 
						  innerColor);

	DDPutBox (g_pDDSBack, vertLine + innerOffsetH, 
						  innerOffsetV, 
						  innerWidth, 
						  horizLine, 
						  innerColor);

	int vert = 1;


	for (i = 0; i < 13; i++)
	{	
		WriteText (g_pDDSBack, 48, 16 * (i + 1), g.Menu(i));
	}

	for (i = 14; i < 17; i++)
	{	
		WriteText (g_pDDSBack, 48, 16 * (i + 1), g.Menu(i));
	}

	WriteText (g_pDDSBack, 32, 16 * (g.cursorPos () + 1), "`");

	for (i = 0; i < 5; i++)
	{
		WriteText (g_pDDSBack, 32, 368 - 16 * i, g.Bottom(i), g.BottomColor(i));
	}
	
	g.map.Draw(g_pDDSBack, g.GetY(), g.GetX());

	DrawCharacter(g_pDDSBack, g.AnimFrame());
	
	// Blit the back buffer to the front buffer
	DDFlip();

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

	WriteText ( pDDS, px, py, theText, coloring);

	delete [] coloring;

}

void WriteText ( LPDIRECTDRAWSURFACE7 pDDS, int px, int py, const char *theText, const unsigned int *coloring)
{
	int i;
	int fx, fy, c;
	int len = strlen(theText);
	RECT fontRect;
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

		fx = c % 16 * 16;
		fy = int(c / 16) * 16 ;
		
		SetRect(&fontRect, fx, fy, fx + 16, fy + 16);
		SetRect(&destRect, px, py, px + 16, py + 16);
		
		DDPutBox (pDDS, px, py, 16, 16, color);
		
		pDDS->Blt(&destRect, g.Font(), &fontRect,  DDBLT_WAIT | DDBLT_KEYSRC, NULL);

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

	tx = tile % 16 * 16;
	ty = int(tile / 16) * 16 ;
	
	SetRect(&tileRect, tx, ty, tx + 16, ty + 16);
	SetRect(&destRect, px, py, px + 16, py + 16);
		
	pDDS->Blt(&destRect, g.Tiles(), &tileRect,  DDBLT_WAIT, NULL);


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
	int px = 448, py = 144;

	RECT charRect;
	RECT destRect;

	tx = anim * 32;
	ty = (g.FaceDirection() - 1) * 32 ;
	
	SetRect(&charRect, tx, ty, tx + 32, ty + 32);
	SetRect(&destRect, px, py, px + 32, py + 32);
		
	pDDS->Blt(&destRect, g.Character(), &charRect,  DDBLT_WAIT | DDBLT_KEYSRC, NULL);


}


/****************************************************************************
 *  void DoCommand (char cmd)												*
 *																			*
 *  This function will process user input and execute their commands.		*
 ****************************************************************************/
void DoCommand (char cmd)
{
	char		theCommand[40] = "Enter Command: ";
	int			last = g.cursor();
	char		cursorKeys = 0;
	char		command[40] = "";
	char		originalCmd = cmd;
	int			terrain = 0;

	cmd = toupper(cmd);

	switch (g.commandMode)
	{
	case 0:
	
		break;
	case 5:
		
		g.AddBottom("");
		g.AddBottom("Enter command:  ");

		g.commandMode = 10;

		break;
	case 10:
		if (cmd != 0)
		{

			cmd = g.cursor(cmd);
			
			if (cmd == -1)
			{
				cmd = originalCmd;
			}

			// Test for cursor movement so we can get the right command first
			switch (originalCmd)
			{
			case VK_RIGHT:
				cursorKeys = 1;

				break;

			case VK_UP:
				cursorKeys = 2;

				break;

			case VK_LEFT:
				cursorKeys = 3;

				break;

			case VK_DOWN:
				cursorKeys = 4;

				break;
				
			}

			if ((cmd >= 65 && cmd <= 91) || cursorKeys > 0 || originalCmd == 'Z')
			{
				
				char *mcmd = g.Menu(cmd);
				
				if (mcmd)
				{
					lstrcpy(command, mcmd);
				}

				if (cursorKeys)
				{
					g.waitCommand = 200;

					if (!g.Animating())
					{
						g.Animating(true);
						g.AnimFrame(animInc);
					}

					if (g.map.MapType() == mapDungeon)
					{
						g.waitCommand = 500;
					}


				}
				else
				{
					g.waitCommand = 700;
				}


				switch (cursorKeys)
				{
				case lotaEast:
					g.FaceDirection(lotaEast);

					switch (g.map.MapType())
					{
					case mapOutside:
						lstrcpy(command, "Move East");

						terrain = g.SetPos(g.GetX() + 1, g.GetY());
					
						break;

					case mapTown:
						lstrcpy(command, "Walk East");

						terrain = g.SetPos(g.GetX() + 1, g.GetY());

						break;

					case mapDungeon:
					case mapMuseum:


						lstrcpy(command, "Turn Right");

						break;

					}
					break;

				case lotaNorth:
					g.FaceDirection(lotaNorth);

					switch (g.map.MapType())
					{
					case mapOutside:
						lstrcpy(command, "Move North");

						terrain = g.SetPos(g.GetX(), g.GetY() - 1);
						break;

					case mapTown:
						lstrcpy(command, "Walk North");

						terrain = g.SetPos(g.GetX(), g.GetY() - 1);
						break;

					case mapDungeon:
					case mapMuseum:
						lstrcpy(command, "Move Forward");
						break;

					}

					break;

				case lotaWest:
					g.FaceDirection(lotaWest);

					switch (g.map.MapType())
					{
					case mapOutside:
						lstrcpy(command, "Move West");

						terrain = g.SetPos(g.GetX() - 1, g.GetY());
						break;

					case mapTown:
						lstrcpy(command, "Walk West");

						terrain = g.SetPos(g.GetX() - 1, g.GetY());
						break;

					case mapDungeon:
					case mapMuseum:
						lstrcpy(command, "Turn Left");
						break;

					}

					break;

				case lotaSouth:
					g.FaceDirection(lotaSouth);

					switch (g.map.MapType())
					{
					case mapOutside:
						lstrcpy(command, "Move South");

						terrain = g.SetPos(g.GetX(), g.GetY() + 1);
						break;

					case mapTown:
						lstrcpy(command, "Walk South");

						terrain = g.SetPos(g.GetX(), g.GetY() + 1);
						break;

					case mapDungeon:
					case mapMuseum:
						lstrcpy(command, "Move Backward");
						break;

					}

					break;

				}

				if (originalCmd == 'Z')
				{
					lstrcpy(command, "Pass");
				}

				lstrcat(theCommand, command);
				g.UpdateBottom(theCommand);
				g.currentCommand = cmd;

				if (terrain == 1)
				{
					unsigned int coloring[39];

					for (int count = 0; count < 39; count ++)
					{
						coloring[count] = lotaCyan;
					}

					g.AddBottom("");
					g.AddBottom("There is too much water for travel.", (unsigned int *)coloring);
				}
				else if (terrain == 2)
				{
					
					g.AddBottom("");
					g.AddBottom("You are not equipped to");
					g.AddBottom("cross the mountains.");

				}


				switch (cmd)
				{
				case 'A':
					ChangeScreenMode();

					break;
				case 'E':
					PostQuitMessage(0);

					break;

				case 'G':
					g.Gamespeed(g.Gamespeed() - 1);

					break;

				case 'I':
					if (cmd < 4)
					{
						cmd = 4;
					}

					break;

				case 'P':
				case 'Z':
					break;

				}
				
				MSG			msg;
				int			clock1;
				int			clock2;

				clock1 = clock();

				g.ResetTimers();
				g.commandMode = 0;

				int foo = 0;

				do 
				{
					foo++;

					clock2 = clock();

					if (!GetMessage (&msg, NULL, 0, 0))
					{
						g.done = true;
						return;
					}

					TranslateMessage(&msg);
					DispatchMessage (&msg);

					CheckAnim();

					HeartBeat(looping);

				} while (clock2 - clock1 < 200);

				g.AddBottom("");
				g.AddBottom("Enter Command: ");

				g.commandMode = 10;

				g.ResetTimers();
			}
		}

		break;

	}

}


void ChangeScreenMode()
{
	g.Unlock();

	g.ReleaseFont();

	DDDestroySurfaces();

	if (g_bFullScreen)
	{
		if (!DDCreateSurfaces(false, actualWindowWidth, actualWindowHeight, 16))
		{
			MessageBox(NULL, TEXT("Couldn't create DirectDraw surfaces!"), TEXT("Failed"), 0);

			return;
		}
	}
	else
	{
		if (!DDCreateSurfaces(true, actualWindowWidth, actualWindowHeight, 16))
		{
			MessageBox(NULL, TEXT("Couldn't create DirectDraw surfaces!"), TEXT("Failed"), 0);

			return;
		}
	}

	g.LoadFont();

	g.Lock();

	g.ResetTimers();

}

void CheckAnim()
{
	static last = 0;

	if (g.Animating() && last + 100 <= clock())
	{
		last = clock();

		static int foo = 0;

		g.AnimFrame(animInc);

		foo ++;

		if (foo > 8)
		{
			foo = 0;
			g.Animating(false);
		}

	}
}