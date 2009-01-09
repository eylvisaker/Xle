
#include "lota.h"


/****************************************************************************
 *  Global::Global()														*
 *																			*
 *  This constructor initializes all the values in the global				*
 *	class to their default values.											*
 ****************************************************************************/
Global::Global()
{
	int i;
	
	locked = false;
	//memset(menuArray, 32, sizeof(menuArray));
	theCursor = 'A';
	done = false;

	for (i = 0; i < 5; i++)
	{
		//bottom[i] = new char[39];
		//memset(bottom[i], 32, 38);
		//bottom[i][38] = 0;

		bottomColor[i] = new unsigned int[39];
		memset(bottomColor[i], CreateRGB(255,255,255), sizeof(bottomColor));

	}

	myApp = NULL;
	hWnd = NULL;

	player.faceDirection = lotaEast;

	//player.x = 50;
	//player.y = 50;

	timerSet = false;
	stdDisplay = true;
	quickMenu = false;
	newGraphics = false;
	raftFacing = lotaEast;
	charAnimCount = 0;
	d3dViewport = false;

}

/****************************************************************************
 *  Global::~Global()														*
 *																			*
 *  This destructor deletes all dynamically allocated memory for the global	*
 *	class.																	*
 ****************************************************************************/
Global::~Global()
{

	//int i;

	/*
	for (i = 0; i < 5; i++)
	{
		delete [] bottom[i];
	}*/

}

/****************************************************************************
 *  LPDIRECTDRAWSURFACE7 Global::Font()										*
 *																			*
 *  This function returns the DirectDraw surface that the font bitmap is	*
 *	stored on.																*
 ****************************************************************************/
LPDIRECTDRAWSURFACE7 Global::Font()
{
	return myFont;
}


/****************************************************************************
 *  LPDIRECTDRAWSURFACE7 Global::Tiles()									*
 *																			*
 *  This function returns the DirectDraw surface that the tiles bitmap is	*
 *	stored on.																*
 ****************************************************************************/
LPDIRECTDRAWSURFACE7 Global::Tiles()
{
	return myTiles;
}

/****************************************************************************
 *  LPDIRECTDRAWSURFACE7 Global::Character()								*
 *																			*
 *  This function returns the DirectDraw surface that the tiles bitmap is	*
 *	stored on.																*
 ****************************************************************************/
LPDIRECTDRAWSURFACE7 Global::Character()
{
	return myCharacter;
}

/****************************************************************************
 *  bool Global::LoadFont()													*
 *																			*
 *  This function loads the font from our file (our resource, when that's	*
 *	properly implemented) and returns a value as to whether or not the font	*
 *	was successfully loaded.												*
 ****************************************************************************/
bool Global::LoadFont()
{
	DDSetColorKey (g_pDDSTemp, RGB(0,0,0));

	if (locked == false)
	{
//		myFont = DDLoadBitmap(g_pDD, TEXT("font.bmp"), 0, 0);
		myFont = DDLoadBitmap(g_pDD, MAKEINTRESOURCE(LOTA_FONT), 0, 0);

		if (!myFont)
		{
			return false;
		}

		DDSetColorKey (myFont, RGB(255,255,255));
		myTiles = DDLoadBitmap(g_pDD, MAKEINTRESOURCE(LOTA_TILES), 0, 0);

		if (!myTiles)
		{
			return false;
		}

		myCharacter = DDLoadBitmap(g_pDD, MAKEINTRESOURCE(LOTA_CHARACTER), 0, 0);
		
		if (!myCharacter)
		{
			return false;
		}

		DDSetColorKey (myCharacter, RGB(0,0,0));

		return true;
	}
	else
	{
		return false;
	}
}

/****************************************************************************
 *  HINSTANCE Global::hInstance()											*
 *																			*
 *  This function returns the handle to the program instance that was		*
 *	passed to the program when it was first run.							*
 ****************************************************************************/
HINSTANCE Global::hInstance()
{
	return myApp;
}

/****************************************************************************
 *	HWND Global::hwnd()														*
 *																			*
 *  This function returns the handle to the window.							*
 ****************************************************************************/
HWND Global::hwnd()
{
	return hWnd;
}


/****************************************************************************
 *  void Global::SetHInstance(HINSTANCE instance)							*
 *																			*
 *  Here the hInstance value is stored for hInstance to return it when		*
 *	called.																	*
 ****************************************************************************/
void Global::SetHInstance(HINSTANCE instance, HWND window)
{
	if (locked == false)
	{
		myApp = instance;
		hWnd = window;
	}
}

/****************************************************************************
 *  void Global::Lock()			and			void Global::Unlock()			*
 *																			*
 *  These two functions toggle locked, which is a value that allows the		*
 *	program to modify key values, like hInstance and myFont.  Once the		*
 *	values are set, the program calls lock and then those values may not	*
 *	modified until unlock has been called.									*
 ****************************************************************************/
void Global::Lock()
{
	locked = true;
}

void Global::Unlock()
{
	locked = false;
}

/****************************************************************************
 *  char Global::Menu(int mx, int my)										*
 *																			*
 *  This function will return a single character value at an (x, y) point	*
 *	in the menu at the left.												*
 ****************************************************************************/
char Global::Menu(int mx, int my)
{
	return menuArray[my][mx];
}

/****************************************************************************
 *  char Global::Menu(int my)												*
 *																			*
 *  This function returns a pointer to an entire line from the menu at the 	*
 *	left.																	*
 ****************************************************************************/
String Global::Menu(int my)
{
	String line = menuArray[my];

	return line;
}

/****************************************************************************
 *  String Global::Menu(char)												*
 *																			*
 *  This returns an entire line from the menu that matches the first		*
 *	character of the command selected.  If it's passed						*
 *  a value that's not in the menu, it returns NULL							*
 ****************************************************************************/
String Global::Menu(char newCursor)
{

	int i;
	String csr;

	newCursor = toupper(newCursor);

	for (i = 0; i < 14; i++)
	{
		if (menuArray[i][0] == newCursor)
		{
			csr = menuArray[i];

			return csr;

		}

	}

	return NULL;

}
/****************************************************************************
 *  void Global::WriteMenu(int loc, const String line)						*
 *																			*
 *  This function allows the program to set a line in the menu.				*
 ****************************************************************************/
void Global::WriteMenu(int loc, const String line)
{
	if (loc < 17)
		menuArray[loc] = line;
}

/****************************************************************************
 *  char Global::cursor()													*
 *																			*
 *  This will return the first character of the last command entered.		*
 ****************************************************************************/
char Global::cursor()
{
	return theCursor;
}

/****************************************************************************
 *  char Global::cursor(char newCursor)										*
 *																			*
 *  This will return the first character of the command selected.  It also	*
 *	allows setting the command with the argument.  If a matching command is *
 *  found, then it will set the cursor at that command and return that		*
 *  value.  If not, it will return the last selected value.	 If it's passed	*
 *  a value that's not in the menu, it returns -1							*
 ****************************************************************************/
char Global::cursor (char newCursor)
{
	int i;

	newCursor = toupper(newCursor);

	if (newCursor != 0)
	{

		for (i = 0; i < 14; i++)
		{
			if (menuArray[i][0] == newCursor)
			{
				theCursor = newCursor;
				return theCursor;

			}

		}
	}

	return -1;

}

/****************************************************************************
 *  int Global::cursorPos ()												*
 *																			*
 *  This will return the numerical position of the cursor so that it may 	*
 *	be plotted on the screen.												*	
 ****************************************************************************/
int Global::cursorPos ()
{
	int i;

	for (i = 0; i < 14; i++)
	{
		if (menuArray[i][0] == theCursor)
		{
			return i;
		}
	}

	return 0;

}

/****************************************************************************
 *  void Global::AddBottom(const char *line, const int *colors)				*
 *																			*
 *  This function adds a line to the bottom of the action window and		*
 *	scrolls the remaining lines up one each.								*	
 ****************************************************************************/
void Global::AddBottom(const String& line, const unsigned int color)
{
	unsigned int colors[40];

	for (int i = 0; i < 40; i++)
	{
		colors[i] = color;
	}

	AddBottom(line, colors);
}

void Global::AddBottom(const String& line, const unsigned int *colors)
{
	int i;

	bottom[4] = "";
	delete [] bottomColor[4];

	for (i = 3; i >= 0; i--)
	{
		bottom[i + 1] = bottom[i];
		bottomColor[i + 1] = bottomColor[i];
	}


	//bottom[0] = new char[39];
	bottomColor[0] = new unsigned int[39];

	bottom[0] = line;

	if (colors)
	{
		for (i = 0; i < 39; i++)
		{
			bottomColor[0][i] = colors[i];
		}
	}
	else
	{
		unsigned int fontColor = lotaWhite;

		if (map.MapType() == mapDungeon)
		{
			fontColor = lotaCyan;
		}
		for (i = 0; i < 39; i++)
		{
			bottomColor[0][i] = fontColor;
		}
	}

}

/****************************************************************************
 *  void Global::UpdateBottom(const char *line, int loc = 0)				*
 *																			*
 *  This function updates a line in the action window.						*
 ****************************************************************************/
void Global::UpdateBottom(const String& line, int loc, const unsigned int * colors)
{

	bottom[loc] = line;

	if (colors)
	{
		for (int i = 0; i < 39; i++)
		{
			bottomColor[loc][i] = colors[i];
		}

	}

	
}

/****************************************************************************
 *  char* Global::Bottom(int line)											*
 *																			*
 *  This function returns a line from the bottom of the action window.		*
 *	Returns null if the line was out of range.								*	
 ****************************************************************************/
String Global::Bottom(int line)
{
	String tempspace;

	if (line >= 0 && line <=4 )
	{

		tempspace = bottom[line];

		return tempspace;
	}
	else
	{
		return NULL;
	}

}

/****************************************************************************
 *  int * Global::BottomColor(int line)										*
 *																			*
 *  This function returns a line from the bottom of the action window.		*
 *	Returns null if the line was out of range.								*	
 ****************************************************************************/
unsigned int * Global::BottomColor(int line)
{
	static unsigned int tempspace[39];
	int i;

	if (line >= 0 && line <=4 )
	{

		for (i = 0; i < 39; i++)
		{
			tempspace[i] = bottomColor[line][i];
		}

		return tempspace;
	}
	else
	{
		return NULL;
	}

}

void Global::ClearBottom()
{
	int i;

	for (i = 0; i < 5; i++)
	{
		AddBottom("");
	}

}

/****************************************************************************
 *  int Global::Gamespeed(int g)											*
 *																			*
 *  This function sets or returns the current gamespeed, and updates the	*
 *	timer values.															*	
 ****************************************************************************/
int Global::Gamespeed(int gs)
{
	if (gs > 0 && gs < 6)
	{
		player.gamespeed = gs;	
	}

	ResetTimers();

	return player.gamespeed;
}

/****************************************************************************
 *  void Global::ResetTimers()												*
 *																			*
 *  Resets the timers for use with commands									*	
 ****************************************************************************/
void Global::ResetTimers()
{
	DestroyTimers();

	SetTimer (hWnd, commandTimer, waitCommand, NULL);
	SetTimer (hWnd, passTimer, 5000 + 750 * player.gamespeed, NULL);

	timerSet = true;
}

/****************************************************************************
 *  void Global::ResetTimers()												*
 *																			*
 *  Destroys the timers for use with commands so the window proc won't be	*
 *	interrupted.															*
 ****************************************************************************/
void Global::DestroyTimers()
{
	if (timerSet == true)
	{
		KillTimer (hWnd, passTimer);
		KillTimer (hWnd, commandTimer);
	}

	timerSet = false;
}

/****************************************************************************
 *  int Global::SetPos(int x, int y)										*
 *																			*
 *  Sets the positions of the player on the current map.  Returns a nonzero	*	
 *  value if they can't move that direction, or be in that position.		*
 ****************************************************************************/
int Global::SetPos(int xx, int yy)
{
	int i, j;
	int t = 0;

	int oldx = player.x;
	int oldy = player.y;

	int test;

	player.x = xx;
	player.y = yy;

	if (map.MapType() == mapOutside)
	{
		test = player.Terrain();

		if (test == 0)
		{
			t = 1;
		}
		else if (test == 1)
		{
			t = 2;
		}
	}

	switch (map.MapType())
	{
	case mapOutside:
	case mapTown:
	case mapCastle:

		for (j = 0; j < 2; j++)
		{
			for (i = 0; i < 2; i++)
			{
				if (map.MapType() == mapOutside)
				{
					test = int(map.M(yy + j, xx + i) / 16) * 16;

					if (test == 0)
					{
						if (!player.onRaft)
							t = 1;

					}
					else if (player.onRaft)
					{
						t = 50;
					}
				}
			}
		}

		break;

	case mapDungeon:
	case mapMuseum:
		if (map.M(yy, xx) == 0x11)
		{
			t = 3;
		}
		break;
	}

	if (t == 1)
	{
		for (i = 1; i < 16; i++)
		{
			if (sqrt(pow(abs(player.raft[i].x - player.x), 2) + pow(abs(player.raft[i].y - player.y), 2)) <= sqrt(2))
			{
				t = 0;
			}

			if (player.raft[i].x == player.x && player.raft[i].y == player.y && player.onRaft == 0)
			{
				player.onRaft = i;				
			}

		}

	}


	if (t == 2 && player.hold == 2)
	{
		t = 0;
	}

	if (t > 0)
	{
		player.x = oldx;
		player.y = oldy;
		return t;
	}

	player.x = xx;
	player.y = yy;
	
	if (player.onRaft)
	{
		player.raft[player.onRaft].x = xx;
		player.raft[player.onRaft].y = yy;

	}
	return 0;
}

int Global::GetX()
{
	return player.x;
}

int Global::GetY()
{
	return player.y;
}

void Global::ReleaseFont()
{
	if (locked == false)
	{
		if (myFont != NULL)
		{
			myFont->Release();
			myFont = NULL;
		}
		if (myTiles != NULL)
		{
			myTiles->Release();
			myTiles = NULL;
		}
		if (myCharacter != NULL)
		{
			myCharacter->Release();
			myCharacter = NULL;
		}
		if (wallTexture != NULL)
		{
			wallTexture->Release();
			wallTexture = NULL;
		}
		if (floorTexture != NULL)
		{
			floorTexture->Release();
			floorTexture = NULL;
		}
		if (ceilingTexture != NULL)
		{
			ceilingTexture->Release();
			ceilingTexture = NULL;
		}

	}
}

int Global::FaceDirection(int dir)
{
	if (dir >= lotaEast && dir <= lotaSouth)
	{
		player.faceDirection = dir;
	}

	return player.faceDirection;
}

int Global::AnimFrame(int frame)
{
	if (animating)
	{
		if (frame == -1)
		{
			return animFrame;
		}

		if (frame == animInc)
		{
			animFrame++;

			if (animFrame > 2)
			{
				animFrame = 0;
			}
		} 
		else if (frame == animDec)
		{
			animFrame--;

			if (animFrame < 0)
			{
				animFrame = 2;
			}
		}
		else if (frame >= 0 && frame <= 2)
		{
			animFrame = frame;
		}

		return animFrame;
	}

	return 0;

}

bool Global::Animating()
{
	if (animating == false)
	{
		animFrame = 0;
	}

	return animating;

}

bool Global::Animating(bool an)
{

	animating = an;

	if (an == false)
	{
		animFrame = 0;
	}
	else
	{
	}

	return animating;
}
