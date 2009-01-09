
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
	memset(menuArray, 32, sizeof(menuArray));
	theCursor = 'A';
	done = false;

	for (i = 0; i < 5; i++)
	{
		bottom[i] = new char[39];
		memset(bottom[i], 32, 38);
		bottom[i][38] = 0;

		bottomColor[i] = new unsigned int[39];
		memset(bottomColor[i], CreateRGB(255,255,255), sizeof(bottomColor));

	}

	myApp = NULL;
	hWnd = NULL;
	mapType = mapOutside;

	faceDirection = lotaEast;

	x = 50;
	y = 50;

	timerSet = false;

}

/****************************************************************************
 *  Global::~Global()														*
 *																			*
 *  This destructor deletes all dynamically allocated memory for the global	*
 *	class.																	*
 ****************************************************************************/
Global::~Global()
{

	int i;

	for (i = 0; i < 5; i++)
	{
		delete [] bottom[i];
	}

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
char* Global::Menu(int my)
{
	static char		line[15];

	lstrcpyn(line, menuArray[my], 14);
	line[15] = 0;

	return line;
}

/****************************************************************************
 *  char* Global::Menu(char)												*
 *																			*
 *  This returns an entire line from the menu that matches the first		*
 *	character of the command selected.  If it's passed						*
 *  a value that's not in the menu, it returns NULL							*
 ****************************************************************************/
char* Global::Menu(char newCursor)
{

	int i;
	static char csr[40];

	newCursor = toupper(newCursor);

	for (i = 0; i < 14; i++)
	{
		if (menuArray[i][0] == newCursor)
		{
			lstrcpyn(csr, menuArray[i], 39);

			return csr;

		}

	}

	return NULL;

}
/****************************************************************************
 *  void Global::WriteMenu(int loc, const char * line)						*
 *																			*
 *  This function allows the program to set a line in the menu.				*
 ****************************************************************************/
void Global::WriteMenu(int loc, const char * line)
{
	if (loc < 17)
		lstrcpyn(menuArray[loc], line, 14);
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
void Global::AddBottom(const char *line, const unsigned int *colors)
{
	int i;

	delete [] bottom[4];
	delete [] bottomColor[4];

	for (i = 3; i >= 0; i--)
	{
		bottom[i + 1] = bottom[i];
		bottomColor[i + 1] = bottomColor[i];
	}


	bottom[0] = new char[39];
	bottomColor[0] = new unsigned int[39];

	lstrcpyn(bottom[0], line, 39);

	if (colors)
	{
		for (i = 0; i < 39; i++)
		{
			bottomColor[0][i] = colors[i];
		}
	}
	else
	{
		for (i = 0; i < 39; i++)
		{
			bottomColor[0][i] = CreateRGB(255,255,255);
		}
	}

}

/****************************************************************************
 *  void Global::UpdateBottom(const char *line, int loc = 0)				*
 *																			*
 *  This function updates a line in the action window.						*
 ****************************************************************************/
void Global::UpdateBottom(const char *line, int loc)
{

	lstrcpy(bottom[loc], line);
	
}

/****************************************************************************
 *  char* Global::Bottom(int line)											*
 *																			*
 *  This function returns a line from the bottom of the action window.		*
 *	Returns null if the line was out of range.								*	
 ****************************************************************************/
char* Global::Bottom(int line)
{
	static char	tempspace[39];

	if (line >= 0 && line <=4 )
	{

		lstrcpyn (tempspace, bottom[line], 38);
		tempspace[38] = 0;

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
		gamespeed = gs;	
	}

	ResetTimers();

	return gamespeed;
}

/****************************************************************************
 *  void Global::ResetTimers()												*
 *																			*
 *  Resets the timers for use with commands									*	
 ****************************************************************************/
void Global::ResetTimers()
{
	if (timerSet == true)
	{
		KillTimer (hWnd, passTimer);
		KillTimer (hWnd, commandTimer);
		KillTimer (hWnd, charAnimTimer);
	}

	SetTimer (hWnd, commandTimer, waitCommand, NULL);
	SetTimer (hWnd, passTimer, 5000 + 750 * gamespeed, NULL);
	SetTimer (hWnd, charAnimTimer, 100, NULL);

	timerSet = true;
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
	int test;

	for (j = 0; j < 2; j++)
	{
		for (i = 0; i < 2; i++)
		{
			test = int(map.M(yy + j, xx + i) / 16) * 16;

			if (test == 0)
			{
				t = 1;
			}
			else if (test == 16)
			{
				t = 2;
			}
		}
	}

	if (t > 0)
	{
		return t;
	}

	x = xx;
	y = yy;

	return 0;
}

int Global::GetX()
{
	return x;
}

int Global::GetY()
{
	return y;
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
	}
}

int Global::FaceDirection(int dir)
{
	if (dir >= lotaEast && dir <= lotaSouth)
	{
		faceDirection = dir;
	}

	return faceDirection;
}

int Global::AnimFrame(int frame)
{
	
	if (frame == -1)
	{
		return animFrame;
	}

	if (frame == animInc)
	{
		animFrame ++;

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

bool Global::Animating()
{
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
