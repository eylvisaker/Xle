// LotA.cpp : Defines the entry point for the application.
//

#include "lota.h"

const int sndFlags = SND_ASYNC | SND_RESOURCE;

#define ShowCoordinates

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
	int				cx, cy;

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

	srand(clock());

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

		if (!g.done)
		{
			HeartBeat(looping);
		}
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
	long		returnValue = 0;

	
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

		if (!DDCreateSurfaces(false, actualWindowWidth, actualWindowHeight, 16))
		{
			MessageBox(NULL, TEXT("Couldn't create DirectDraw surfaces!"), TEXT("Failed"), 0);
			g.done = true;
			PostQuitMessage(1);
			return 1;
		}
		
		g.ResetTimers();

		g.map.MapMenu();

		g.map.LoadMap(1);

		g.commandMode = 5;
		DoCommand(0);

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
		case ID_OPTIONS_TOGGLEFULLSCREEN:
			ChangeScreenMode();

			break;

		case ID_FILE_QUIT:
			PostQuitMessage(0);

			break;

		}

	case WM_KEYDOWN:

		if (wParam == VK_F4)
		{
			ChangeScreenMode();
		}

		if (g.quickMenu == true)
		{
			g.menuKey = wParam;
		}
		else
		{
			if ((wParam >= 65 && wParam <= 91) || wParam == VK_RIGHT || 
				wParam == VK_UP || wParam == VK_LEFT || wParam == VK_DOWN)
			{
				DoCommand(wParam);
			}
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
 *  void HeartBeat(short mode)												*
 *																			*
 *  This is the main function that runs the game.  It does all the drawing	*
 *	in the window right now.												*	
 ****************************************************************************/
void HeartBeat(short mode)
{
	static int lastRaftAnim = 0;
	static int lastCharAnim = 0;
	unsigned long fontColor = lotaWhite;

	if (lastRaftAnim + 100 <= clock())
	{
		RaftAnim();
		lastRaftAnim = clock();
	}

	if (lastCharAnim + 125 <= clock())
	{
		CheckAnim();
		lastCharAnim = clock();
	}

	// Check for lost surfaces
	CheckSurfaces();

	if (g.waitCommand < 100)
	{
		DoCommand(0);
	}

	if (g.stdDisplay == false)
	{

		DrawSpecial(g_pDDSBack);

		// Blit the back buffer to the front buffer
		DDFlip();

		return;
	}

	int i = 0;
	DWORD boxColor;
	DWORD innerColor;


	int horizLine = 18 * 16;

	switch (g.map.MapType())
	{
	case mapTown:

		boxColor = lotaOrange;
		innerColor = lotaYellow;
		break;

	case mapMuseum:

		boxColor = lotaMdGray;
		innerColor = lotaYellow;

		break;

	case mapDungeon:

		boxColor = lotaMdGray;
		innerColor = lotaLtGreen;
		fontColor = lotaCyan;

		break;

	case mapOutside:
	default:
		boxColor = CreateRGB(159,112,64);
		innerColor = lotaYellow;	
		g.vertLine = 15 * 16;


		break;

	}

	int vertLine = g.vertLine;

	// Clear the back buffer
	DDPutBox (g_pDDSBack, 0, 0, myWindowWidth, myWindowHeight, CreateRGB(0,0,0));

	DrawBorder(g_pDDSBack, boxColor);

	DrawLine(g_pDDSBack, vertLine, 0, 0, horizLine, boxColor);
	DrawLine(g_pDDSBack, 0, horizLine, 1, myWindowWidth, boxColor);

	DrawInnerBorder(g_pDDSBack, innerColor);

	DrawInnerLine(g_pDDSBack, vertLine, 0, 0, horizLine + 12, innerColor);
	DrawInnerLine(g_pDDSBack, 0, horizLine, 1, myWindowWidth, innerColor);

	int vert = 1;

	String tempLine("H.P. ");

	tempLine += g.player.hp;

	g.WriteMenu(14, tempLine);

	tempLine = "Food ";
	tempLine += g.player.food;

	g.WriteMenu(15, tempLine);

	tempLine = "Gold ";
	tempLine += g.player.gold;

	g.WriteMenu(16, tempLine);


	for (i = 0; i < 13; i++)
	{	
		WriteText (g_pDDSBack, 48, 16 * (i + 1), g.Menu(i), fontColor);
	}

	for (i = 14; i < 17; i++)
	{	
		WriteText (g_pDDSBack, 48, 16 * (i + 1), g.Menu(i), fontColor);
	}

	WriteText (g_pDDSBack, 32, 16 * (g.cursorPos () + 1), "`", fontColor);

	for (i = 0; i < 5; i++)
	{
		WriteText (g_pDDSBack, 32, 368 - 16 * i, g.Bottom(i), g.BottomColor(i));
	}
	
	// check to see if we are in a dungeon or the museum and set the d3d viewport
	if (g.map.MapType() == mapDungeon || g.map.MapType() == mapMuseum)
	{
		Set3DViewPort();
		Store3DMap();
		Render3DMap();

	}
	else
	{
		g.map.Draw(g_pDDSBack, g.GetY(), g.GetX());
		DrawRafts(g_pDDSBack);

		if (!g.player.onRaft)
		{
			DrawCharacter(g_pDDSBack, g.AnimFrame());
		}
	}
	
	// check to see if a submenu is active
	if (g.subMenu.onScreen)
	{
		DrawMenu(g_pDDSBack);
	}


#ifdef ShowCoordinates
	// Show coordinates at top
	String		coor;

	coor = "X: ";
	coor += g.GetX();

	DDPutBox (g_pDDSBack, 256, 0, 128, 16, lotaBlack);
	WriteText (g_pDDSBack, 272, 0, coor);

	coor = "Y: ";
	coor += g.GetY();

	DDPutBox (g_pDDSBack, 400, 0, 128, 16, lotaBlack);
	WriteText (g_pDDSBack, 416, 0, coor);

	coor = "F: ";
	coor += g.player.faceDirection;

	DDPutBox (g_pDDSBack, 544, 0, 96, 16, lotaBlack);
	WriteText (g_pDDSBack, 560, 0, coor);	

#endif

	// Blit the back buffer to the front buffer
	DDFlip();

}

void DrawBorder( LPDIRECTDRAWSURFACE7 pDDS, unsigned int boxColor)
{

	DrawLine(pDDS, 0, 0, 1, myWindowWidth, boxColor);
	DrawLine(pDDS, 0, 0, 0, myWindowHeight, boxColor);
	DrawLine(pDDS, 0, myWindowHeight - 12, 1, myWindowWidth, boxColor);
	DrawLine(pDDS, myWindowWidth - 12, 0, 0, myWindowHeight, boxColor);
}

void DrawInnerBorder( LPDIRECTDRAWSURFACE7 pDDS, unsigned int innerColor)
{
	DrawInnerLine(pDDS, 0, 0, 1, myWindowWidth, innerColor);
	DrawInnerLine(pDDS, 0, 0, 0, myWindowHeight, innerColor);
	DrawInnerLine(pDDS, 0, myWindowHeight - 12, 1, myWindowWidth + 2, innerColor);
	DrawInnerLine(pDDS, myWindowWidth - 12, 0, 0, myWindowHeight, innerColor);

}

void DrawLine(LPDIRECTDRAWSURFACE7 pDDS, int left, int top, int direction, 
			  int length, unsigned int boxColor)
{
	int boxWidth = 12;
	int innerOffsetH = 8;
	int innerOffsetV = 2;
	int innerWidth = 2;

	if (direction == 1)
	{
		DDPutBox (g_pDDSBack, left, top, length, 
						  boxWidth, boxColor);

	}
	else
	{
		DDPutBox (g_pDDSBack, left, top, boxWidth, length, 
						  boxColor);

	}

}

void DrawInnerLine(LPDIRECTDRAWSURFACE7 pDDS, int left, int top, int direction, 
			  int length, unsigned int innerColor)
{
	int boxWidth = 12;
	int innerOffsetH = 8;
	int innerOffsetV = 2;
	int innerWidth = 2;

	if (direction == 1)
	{

		DDPutBox (pDDS, left + innerOffsetH, 
						  top + innerOffsetV, 
						  length - boxWidth, 
						  innerWidth, 
						  innerColor);
	}
	else
	{

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

	WriteText ( pDDS, px, py, theText, coloring);

	delete [] coloring;

}

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

		fx = c % 16 * 16;
		fy = int(c / 16) * 16 ;
		

		SetRect(&fontRect, fx, fy, fx + 16, fy + 16);
		SetRect(&tempRect, 0, 0, 16, 16);
		SetRect(&destRect, px, py, px + 16, py + 16);
		
		DDPutBox (g_pDDSTemp, 0, 0, 16, 16, color);

		g_pDDSTemp->Blt (&tempRect, g.Font(), &fontRect, DDBLT_WAIT | DDBLT_KEYSRC, NULL);

		pDDS->Blt(&destRect, g_pDDSTemp, &tempRect,  DDBLT_WAIT | DDBLT_KEYSRC, NULL);

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
	
	/*if (ty == 0 && (tx == 0 || tx == 16))
	{
		if (rnd(1, 100) < 5)
		{
			tx = 16;
		}
		else
		{
			tx = 0;
		}

	}*/

	if (ty > 80)
	{
		int klsdjf = 0;
	}

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
	int px = g.vertLine + 16;
	int width = (624 - px) / 16;
	int py = 144;

	RECT charRect;
	RECT destRect;

	px += int(width / 2) * 16;
	
	tx = anim * 32;
	ty = (g.FaceDirection() - 1) * 32 ;
	
	SetRect(&charRect, tx, ty, tx + 32, ty + 32);
	SetRect(&destRect, px, py, px + 32, py + 32);
		
	pDDS->Blt(&destRect, g.Character(), &charRect,  DDBLT_WAIT | DDBLT_KEYSRC, NULL);


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
	
	tx = g.raftAnim * 32;
	ty = 256;
	
	SetRect(&charRect, tx, ty, tx + 32, ty + 32);
	
	for (i = 1; i < 16; i++)
	{
		rx = px - (g.player.x - g.player.raft[i].x) * 16;
		ry = py - (g.player.y - g.player.raft[i].y) * 16;
		
		if (i == g.player.onRaft)
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

		if (rx >= lx && ry >= 16 && rx <= 624 && ry < 18*16)
		{

			SetRect(&destRect, rx, ry, rx + 32, ry + 32);
			pDDS->Blt(&destRect, g.Character(), &charRect,  DDBLT_WAIT | DDBLT_KEYSRC, NULL);

		}
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

		g.AnimFrame(animInc);

		g.charAnimCount++;

		if (g.charAnimCount > 6)
		{
			g.charAnimCount = 0;
			g.Animating(false);
		}

	}
}

void wait (int howLong)
{
	MSG			msg;
	int			clock1;
	int			clock2;

	if (howLong > 0)
	{
		clock1 = clock();

		do 
		{

			clock2 = clock();

			if (!GetMessage (&msg, NULL, 0, 0))
			{
				g.done = true;
				return;
			}

			TranslateMessage(&msg);
			DispatchMessage (&msg);

			//CheckAnim();

			HeartBeat(looping);

		} while (clock2 - clock1 < howLong);
	}
}

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
	DDCheckSurfaces();


}

int rnd(int lo, int hi)
{
	return int((rand() / 32767.0) * (hi - lo + 1)) + lo;
}

int SubMenu(MenuItemList items)
{
	char buffer[40];
	String stBuffer;

	g.subMenu.onScreen = true;
	g.subMenu.theList = items;
	g.subMenu.width = 0;

	for (int i = 0; i < g.subMenu.theList.TotalItems(); i++)
	{
		g.subMenu.theList.GetItem(i, buffer);
		stBuffer = buffer;

		if (stBuffer.len() + 6 > g.subMenu.width)
		{
			g.subMenu.width = stBuffer.len() + 6;
		}

	}

	stBuffer = "Choose ";
	stBuffer += g.subMenu.title;

	if (stBuffer.len() + 2 > g.subMenu.width)
	{
		g.subMenu.width = stBuffer.len() + 2;
	}

	g.commandMode = 0;
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
			vv[0] = (char)g.menuKey;
			vv[1] = 0;

			int v = atoi(vv);

			if (v < items.TotalItems())
			{
				g.subMenu.value = v;
			}

			g.menuKey = VK_RETURN;
		}


	} while (g.menuKey != VK_RETURN && !g.done);

	wait(1);

	g.quickMenu = false;
	g.subMenu.onScreen = false;
	g.commandMode = 10;

	return g.subMenu.value;

}

void DrawMenu( LPDIRECTDRAWSURFACE7 pDDS )
{
	String theString;
	int xx, yy, i = 0, height;
	char buffer[40];
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

	WriteText(pDDS, xx + int((624 - xx) / 32) * 16 - int(theString.len() / 2) * 16, yy, theString, fontColor);
	yy += 16;

	for (i = 0; i < g.subMenu.theList.TotalItems(); i++)
	{
		yy += 16;
		g.subMenu.theList.GetItem(i, buffer);

		theString = i;
		theString += ". ";
		theString += buffer;

		WriteText(pDDS, xx, yy, theString);

		if (i == g.subMenu.value)
		{
			int xx1;

			xx1 = xx + theString.len() * 16;
			WriteText (pDDS, xx1, yy, "`");
		}


	}


}

int QuickMenu(MenuItemList items, int spaces, int value)
{
	int	 spacing[10] = {0,0,0,0,0,0,0,0,0,0};
	int  last = 0;
	String tempLine("Choose: ");
	char tempItem[40];
	
	spacing[0] = 8;

	for (int i = 0; i < items.TotalItems(); i++)
	{
		items.GetItem(i, tempItem);

		tempLine += tempItem;
		tempLine += space(spaces);

		spacing[i] += last + lstrlen(tempItem) - 1;
		last = spacing[i]  + spaces + 1;
	}

	g.AddBottom(tempLine);
	g.AddBottom("");

	g.quickMenu = true;
	
	tempLine = space(spacing[value]);
	tempLine += "`";

	g.UpdateBottom(tempLine);
	g.commandMode = 0;

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

		tempLine = space(spacing[value]);
		tempLine += "`";

		g.UpdateBottom(tempLine);


	} while (g.menuKey != VK_RETURN && !g.done);


	g.quickMenu = false;
	g.commandMode = 10;

	g.AddBottom("");

	return value;

}

__inline char* space(int num)
{
	static char temp[40];

	for (int i = 0; i < num; i++)
	{
		temp[i] = ' ';
	}

	temp[num] = 0;

	return temp;
}

void LotaPlaySound(int snd)
{
	PlaySound (NULL, NULL, NULL);
	PlaySound (MAKEINTRESOURCE(snd), g.hInstance(), SND_ASYNC | SND_RESOURCE);
}

void RaftAnim()
{
	g.raftAnim++;

	if (g.raftAnim == 3)
		g.raftAnim = 0;

}
