
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
	theCursor = 'A';
	done = false;

	for (i = 0; i < 5; i++)
	{

		bottomColor[i] = new unsigned int[39];
		memset(bottomColor[i], CreateRGB(255,255,255), sizeof(bottomColor));

	}

	myApp = NULL;
	hWnd = NULL;

	pTitleScreen = NULL;

	player.FaceDirection(lotaEast);

	timerSet = false;
	stdDisplay = false;
	quickMenu = false;

	newGraphics = false;

	raftFacing = lotaEast;
	charAnimCount = 0;
	d3dViewport = false;
	ZBufferEnable = true;
	pJoystick = NULL;
	LeftMenuActive = false;
	HPColor = 0x00FFFFFF;
	allowEnter = true;

	disableEncounters = false;

	walkTime = 75;

	invisible = false;
	guard = false;

	titleScreen = true;
	titleState = stNoState;
	titleMenu = 0;

}

/****************************************************************************
 *  Global::~Global()														*
 *																			*
 *  This destructor deletes all dynamically allocated memory for the global	*
 *	class.																	*
 ****************************************************************************/
Global::~Global()
{

	for (int i = 0; i < 5; i++)
	{
		delete [] bottomColor[i];
	}

	Unlock();
	ReleaseFont();

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
		char	tempSpace[40];
		int		i;

		myApp = instance;
		hWnd = window;

		for (i = 0; i < 8; i++)
		{
			LoadString (hInstance(), i + 1, tempSpace, 39);
			weaponName[i] = tempSpace;
		}

		for (i = 0; i < 5; i++)
		{
			LoadString (hInstance(), i + 9, tempSpace, 39);
			armorName[i] = tempSpace;

		}

		for (i = 0; i < 5; i++)
		{
			LoadString (hInstance(), i + 14, tempSpace, 39);
			qualityName[i] = tempSpace;

		}
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
 *  LPDIRECTDRAWSURFACE7 Global::Monsters()									*
 *																			*
 *  This function returns the DirectDraw surface that the tiles bitmap is	*
 *	stored on.																*
 ****************************************************************************/
LPDIRECTDRAWSURFACE7 Global::Monsters()
{
	return pOverlandMonsters;
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
	int	tiles;

	if (locked == false)
	{
		ReleaseFont();

		DDSetColorKey (g_pDDSTemp, RGB(0,0,0));
		myFont = DDLoadBitmap(g_pDD, MAKEINTRESOURCE(LOTA_FONT), 0, 0);

		if (!myFont)
		{
			return false;
		}

		DDSetColorKey (myFont, RGB(255,255,255));
	
		switch (map.TileSet())
		{
		case 0:
			tiles = LOTA_TILES;
			break;
		case 1:
			tiles = LOTA_TOWNTILES;
			break;
		case 2:
			tiles = LOTA_CASTLETILES;
			break;
		default:
			tiles = LOTA_TILES;
			break;

		}

		myTiles = DDLoadBitmap(g_pDD, MAKEINTRESOURCE(tiles), 0, 0);

		if (!myTiles)
		{
			return false;
		}

		myCharacter = DDLoadBitmap(g_pDD, MAKEINTRESOURCE(LOTA_CHARACTER), 0, 0);
		
		if (!myCharacter)
		{
			return false;
		}

		pOverlandMonsters = DDLoadBitmap(g_pDD, MAKEINTRESOURCE(LOTA_OVERLANDMONSTERS), 0, 0);
		
		if (!myCharacter)
		{
			return false;
		}

		pTitleScreen = DDLoadBitmap(g_pDD, MAKEINTRESOURCE(LOTA_TITLE), 0, 0);
		if (!pTitleScreen)
			return false;

		DDSetColorKey (myCharacter, RGB(0,0,0));
		DDSetColorKey (pOverlandMonsters, RGB(0,0,0));

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

	return "";

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
int Global::cursorPos (int change)
{
	int i;
	bool found = false;

	do
	{
		theCursor += change;

		if (theCursor < 'A')
			theCursor = 'Z';
		
		if (theCursor > 'Z')
			theCursor = 'A';

		for (i = 0; i < 14; i++)
		{
			if (menuArray[i][0] == theCursor)
			{
				found = true;
				return i;

			}
		}
	} while (!found && change != 0);

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

void Global::WriteSlow(const String& line, int loc, const unsigned int color)
{
	int i = 0;
	unsigned int colors[40];
	String temp;

	while (i < len(line) && i < 40)
	{
		colors[i++] = color;
		temp = left(line, i);
		
		UpdateBottom(temp, loc, colors);

		wait(50);
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

void Global::UpdateBottom(const String& line, unsigned int color)
{
	unsigned int colors[40];

	for (int i = 0; i < 40; i++)
	{
		colors[i] = color;
	}

	UpdateBottom(line, 0, colors);

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
 *  void Global::ResetTimers()												*
 *																			*
 *  Resets the timers for use with commands									*	
 ****************************************************************************/
void Global::ResetTimers()
{
	DestroyTimers();

	SetTimer (hWnd, commandTimer, waitCommand, NULL);
	SetTimer (hWnd, passTimer, 5000 + 750 * player.Gamespeed(), NULL);

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

void Global::ReleaseFont()
{
	/*
	LPDIRECTDRAWSURFACE7		myFont;				// stores the handle to the font
	LPDIRECTDRAWSURFACE7		myTiles;			// stores the handle to the tiles
	LPDIRECTDRAWSURFACE7		myCharacter;		// stores the handle to the character sprites
	LPDIRECTDRAWSURFACE7		pOverlandMonsters;	// stores the handle to the overland monster sprites

	LPDIRECTDRAWSURFACE7	wallTexture;			// stores the pointer to the wall texture
	LPDIRECTDRAWSURFACE7	floorTexture;			// stores the pointer to the floor texture
	LPDIRECTDRAWSURFACE7	ceilingTexture;			// stores the pointer to the ceiling texture
	LPDIRECTDRAWSURFACE7	floorHoleTexture;		// stores the pointer to the floorhole texture

	LPDIRECTINPUTDEVICE2	pJoystick;				// Joystick

	*/
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
		if (pOverlandMonsters != NULL)
		{
			pOverlandMonsters->Release();
			pOverlandMonsters = NULL;
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
		if (floorHoleTexture != NULL)
		{
			floorHoleTexture->Release();
			floorHoleTexture = NULL;
		}
		if (pTitleScreen != NULL)
		{
			pTitleScreen->Release();
			pTitleScreen = NULL;
		}


	}
}

int Global::AnimFrame(int frame)
{
	static int last = 0;

	if (animating)
	{
		
		if (frame == -1)
		{
			return animFrame;
		}
		else if (last + 150 <= clock())
		{
			
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
			
			last = clock();
			
			return animFrame;
		}
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

// Returns the default font color
unsigned int Global::DefaultColor()
{
	if (map.MapType() == mapDungeon)
	{
		return lotaCyan;
	}
	
	return lotaWhite;

}
String Global::WeaponName(int a)
{
	return weaponName[a - 1];
}
int Global::WeaponCost(int w, int q)
{
	return 	int(9.639302862 + 12.709725901 * w + 7.448174718 * pow(w, 2) + 
			5.552075007 * q + 0.731199405 * pow(q, 2) + 4.290595417 * w * q);

}
String Global::ArmorName(int a)
{
	return armorName[a - 1];
}
int Global::ArmorCost(int a, int q)
{
	return 120 * a + 12 * q + 8 * q * a;
}
String Global::QualityName(int a)
{
	return qualityName[a];
}
