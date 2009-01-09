
#ifndef __LOTA__
#define __LOTA__

// Window size definitions
#define myWindowWidth 640
#define myWindowHeight 400
#define actualWindowWidth 640
#define actualWindowHeight 400

// Timer definitions
#define passTimer 1
#define commandTimer 2
#define charAnimTimer 3
#define raftAnimTimer 4

// Map Type definitions
#define mapMuseum 1
#define mapOutside 2
#define mapTown 3
#define mapDungeon 4
#define mapCastle 5

// Map definitions
#define lotaOutside 1
#define lotaMuseum 31
#define lotaThornberry 11
#define lotaPirates 51

// Color definitions
#define lotaBlack (DWORD)CreateRGB(0,0,0)
#define lotaWhite (DWORD)CreateRGB(255,255,255)
#define lotaRed (DWORD)CreateRGB(223,63,63)
#define lotaCyan (DWORD)CreateRGB(96,252,252)
#define lotaPurple (DWORD)CreateRGB(223,95,223)
#define lotaGreen (DWORD)CreateRGB(63,223,63)
#define lotaBlue (DWORD)CreateRGB(63,63,223)
#define lotaYellow (DWORD)CreateRGB(255,255,63)
#define lotaOrange (DWORD)CreateRGB(223,159,63)
#define lotaBrown (DWORD)CreateRGB(156, 116, 72)
#define lotaPink (DWORD)CreateRGB(255,159,159)
#define lotaDkGray (DWORD)CreateRGB(80,80,80)
#define lotaMdGray (DWORD)CreateRGB(128,128,128)
#define lotaLtGreen (DWORD)CreateRGB(159,255,159)
#define lotaLtBlue (DWORD)CreateRGB(159,159,255)
#define lotaLtGray (DWORD)CreateRGB(191,191,191)

// Directional definitions
#define lotaEast 1
#define lotaNorth 2
#define lotaWest 3
#define lotaSouth 4

// Animation definitions
#define animInc -2
#define animDec -3

// Terrain Definitions
#define mapWater 0
#define mapMountain 1
#define mapGrass 2
#define mapForest 3
#define mapDesert 4
#define mapSwamp 5
#define mapMixed 6
#define mapFoothills 7

// system headers
#include <windows.h>
#include <string.h>
#include <fstream.h>
#include <math.h>
#include <time.h>

// my headers
#include "resource.h"
#include "string.h"
#include "dd.h"
#include "ddutil.h"
#include "map.h"
#include "player.h"
#include "menulist.h"

const looping = 1,
	  redraw = 2;

class Global 
{
private:
	int							userControl;
	int							mode;
	int							animFrame;
	bool						animating;

	LPDIRECTDRAWSURFACE7		myFont;				// stores the handle to the font
	LPDIRECTDRAWSURFACE7		myTiles;			// stores the handle to the tiles
	LPDIRECTDRAWSURFACE7		myCharacter;		// stores the handle to the character sprites
	HINSTANCE					myApp;				// stores the instance handle to the app
	HWND						hWnd;				// stores the handle to the window

	bool						locked;				// stores whether or not key values are locked
	String						menuArray[17];		// keeps the menu portion of the screen
	String 						bottom[5];			// keeps the bottom portion of the screen
	unsigned int				*bottomColor[5];	// keeps the bottom colors on the screen
	char						theCursor;			// location of the menu dot
	bool						timerSet;			// stores whether or not the timer was set at first
	LotaMap						theMap;				// stores all the values for the current map

public:
	// constructors and destructor:
	Global();
	~Global();

	// accessor functions
	LPDIRECTDRAWSURFACE7	Font();					// returns the handle to the font resource
	LPDIRECTDRAWSURFACE7	Tiles();				// returns the handle to the tiles resource
	LPDIRECTDRAWSURFACE7	Character();			// returns the handle to the character resource
	HINSTANCE				hInstance();			// returns the handle to the hInstance
	HWND					hwnd();					// returns the handle to the window

	// character functions
	Player					player;					// stores the Player class
	int						FaceDirection(int = 0);	// sets or returns the direction that the 
													//		character is facing
	int						AnimFrame(int = -1);	// sets or returns the animation frame
	bool					Animating();			// sets or returns whether or not the character
	bool					Animating(bool);		//		is animating

	// menu functions
	char					Menu(int mx, int my);	// returns a character cell in the menu
	String					Menu(int my);			// returns an entire line
	void					WriteMenu(int, const String);	// Writes a line to the menu
	char					cursor();					// returns the location of the menu dot
	char					cursor(char newCursor);		// sets the location of the menu dot
	int						cursorPos();				// returns the cursor position
	String					Menu(char);					// returns the line that starts with passed parameter
	SubMenu					subMenu;					// the submenu

	// action window functions
	void					AddBottom(const String&, const unsigned int);
														// adds a line to the bottom of the action window
	void					AddBottom(const String&, const unsigned int * = NULL);
														// adds a line to the bottom of the action window
	void					UpdateBottom(const String&,  // updates a line in the action string
										 int = 0, const unsigned int * = NULL);
	String					Bottom(int);				// returns a line from the bottom
	unsigned int *			BottomColor(int);			// returns the color index for a line at the bottom
	int						Gamespeed(int = 0);			// sets or returns the gamespeed
	void					ClearBottom();				// clears the bottom

	// lock functions:
	// these functions will set locked, which keeps key values like myFont and myApp from 
	// being changed.
	void			Lock();							// locks key values
	void			Unlock();						// unlocks those values (necessary?)

	// others:
	bool			LoadFont();						// Loads the Font	
	void			SetHInstance(HINSTANCE, HWND);	// Stores the HINSTANCE & hwnd
	void			ResetTimers ();					// Resets the timers
	void			DestroyTimers ();				// Kills the timers
	void			ReleaseFont();					// Releases the font direct draw surfaces

	// map:
	LotaMap			map;							// the map class
	int				SetPos(int x, int y);			// sets the position on the map for the PC
	int				GetX();
	int				GetY();

	// other commonly used variables that don't need accessors
	int				commandMode;
	char			currentCommand;
	int				waitCommand;			// holds the time in msec to wait to give Enter Command
	int				screenLeft;				// these two values store the original position
	int				screenTop;				//		of the window so we can restore it upon exiting 
											//		full screen mode
	bool			stdDisplay;				// heartbeat has control of the display
	bool			done;					
	bool			waterReset;				// reset the water dots
	bool			quickMenu;				// stores whether or not a small menu has control
	int				menuKey;				// stores the key pressed inside the menu
	int				vertLine;				// Vertical line dividing menu and map
	bool			newGraphics;			// are we displaying the new graphics?
	int				raftAnim;				// raft animation frame
	int				raftFacing;				// direction the raft is facing (lotaEast or lotaWest)
	int				charAnimCount;			// animation count for the player

};


// LOTA.CPP
LRESULT CALLBACK WndProc (HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam);
void HeartBeat (short mode);
void WriteText ( LPDIRECTDRAWSURFACE7 pDDS, int px, int py, const char *theText, const unsigned int *coloring = NULL);
void WriteText ( LPDIRECTDRAWSURFACE7 pDDS, int px, int py, const char *theText, unsigned long c);
void DoCommand (char cmd);
void DrawTile( LPDIRECTDRAWSURFACE7 pDDS, int px, int py, int tile);
void DrawCharacter( LPDIRECTDRAWSURFACE7 pDDS, int anim );
void ChangeScreenMode();
void CheckAnim();
void wait (int howLong);
void DrawBorder( LPDIRECTDRAWSURFACE7 pDDS, unsigned int boxColor);
void DrawInnerBorder( LPDIRECTDRAWSURFACE7 pDDS, unsigned int innerColor);
void DrawLine(LPDIRECTDRAWSURFACE7 pDDS, int left, int top, int direction, 
			  int length, unsigned int boxColor);
void DrawInnerLine(LPDIRECTDRAWSURFACE7 pDDS, int left, int top, int direction, 
			  int length, unsigned int innerColor);
void CheckSurfaces();
int rnd(int lo, int hi);
int SubMenu(MenuItemList items);
int QuickMenu(MenuItemList items, int spacing, int value = 0);
__inline char *space(int);
void DrawMenu( LPDIRECTDRAWSURFACE7 pDDS );
void LotaPlaySound(int snd);
void DrawRafts( LPDIRECTDRAWSURFACE7 pDDS );
void RaftAnim();



// COMMANDS.CPP
void Armor();
void Disembark();
void End();
void Fight();
void GameSpeed();
void Hold();
void Inventory (int mode = 0);
void Leave();
void Magic();
void Open();
void Pass();
void Rob();
void Speak();
void Take();
void Weapon();
void Use();
void Xamine();

void DrawSpecial( LPDIRECTDRAWSURFACE7 pDDS);


#endif
