
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
enum MapTypes {
	mapMuseum = 1,
	mapOutside,
	mapTown,
	mapDungeon,
	mapCastle
};

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
enum Direction {
	lotaNone = 0,
	lotaEast = 1,
	lotaNorth,
	lotaWest,
	lotaSouth
};

// Animation definitions
#define animInc -2
#define animDec -3

// Terrain Definitions
enum TerrainType {
	mapWater,
	mapMountain,
	mapGrass,
	mapForest,
	mapDesert,
	mapSwamp,
	mapMixed,
	mapFoothills
};

enum CmdMode {
	cmdBad = 0,
	cmd2,
	cmd3,
	cmd4,
	cmdPrompt = 5,
	cmdEnterCommand = 10
};

// Direct3D Definitions
#define STRICT
#define D3D_OVERLOADS
//#define Disable3D
#define DIRECTINPUT_VERSION 0x0700

// system headers
#include "afxwin.h"
#ifdef _DEBUG
//#define new DEBUG_NEW
#endif

#include <windows.h>
#include <mmsystem.h>
#include <d3d.h>
#include <dsound.h>
#include <dinput.h>
#include <ddraw.h>
#include <string.h>
#include <fstream>
#include <math.h>
#include <time.h>


#include <ErikUtils.h>

// my headers
#include "resource.h"
#include "dd.h"
#include "ddutil.h"
#include "map.h"
#include "player.h"
#include "menulist.h"
#include "render.h"
#include "d3dutil.h"
#include "lotasound.h"
#include "dsutil3d.h"
#include "joystick.h"
#include "town.h"
#include "story.h"

// Math Definitions
#define pi 3.1415926535f

const looping = 1,
	  redraw = 2;

enum TitleScreenState
{
	stNoState = 0,
	stMenu1 = 1,
	stMenu2 = 2,
	stNewGame = 10,
	stNewGameMusic = 11,
	stNewGameText = 12,
	stLoadGame = 20
};

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
	LPDIRECTDRAWSURFACE7		pOverlandMonsters;	// stores the handle to the overland monster sprites
	HINSTANCE					myApp;				// stores the instance handle to the app
	HWND						hWnd;				// stores the handle to the window

	bool						locked;				// stores whether or not key values are locked
	String						menuArray[17];		// keeps the menu portion of the screen
	String 						bottom[5];			// keeps the bottom portion of the screen
	unsigned int				*bottomColor[5];	// keeps the bottom colors on the screen
	char						theCursor;			// location of the menu dot
	bool						timerSet;			// stores whether or not the timer was set at first
	LotaMap						theMap;				// stores all the values for the current map
	String						weaponName[20];		// stores weapon names
	String						armorName[20];		// stores armor names
	String						qualityName[6];		// stores quality names

public:
	// constructors and destructor:
	Global();
	~Global();

	// DirectX functions and data members
	LPDIRECTDRAWSURFACE7	Font();					// returns the handle to the font resource
	LPDIRECTDRAWSURFACE7	Tiles();				// returns the handle to the tiles resource
	LPDIRECTDRAWSURFACE7	Character();			// returns the handle to the character resource
	LPDIRECTDRAWSURFACE7	Monsters();				// returns the handle to the monsters resource
	LPDIRECTDRAWSURFACE7	wallTexture;			// stores the pointer to the wall texture
	LPDIRECTDRAWSURFACE7	floorTexture;			// stores the pointer to the floor texture
	LPDIRECTDRAWSURFACE7	ceilingTexture;			// stores the pointer to the ceiling texture
	LPDIRECTDRAWSURFACE7	floorHoleTexture;		// stores the pointer to the floorhole texture
	LPDIRECTDRAWSURFACE7	pTitleScreen;			// stores the image of the title screen.

	LPDIRECTINPUTDEVICE2	pJoystick;				// Joystick

	//	Important stuff
	HINSTANCE				hInstance();			// returns the handle to the hInstance
	HWND					hwnd();					// returns the handle to the window

	// character functions
	Player					player;					// stores the Player class
	int						AnimFrame(int = -1);	// sets or returns the animation frame
	bool					Animating();			// sets or returns whether or not the character
	bool					Animating(bool);		//		is animating

	// menu functions
	char					Menu(int mx, int my);	// returns a character cell in the menu
	String					Menu(int my);			// returns an entire line
	void					WriteMenu(int, const String);	// Writes a line to the menu
	char					cursor();					// returns the location of the menu dot
	char					cursor(char newCursor);		// sets the location of the menu dot
	int						cursorPos(int = 0);			// returns the cursor position or changes it
	String					Menu(char);					// returns the line that starts with passed parameter
	SubMenu					subMenu;					// the submenu
	bool					LeftMenuActive;				// is the left menu active?
	unsigned int			HPColor;					// color of left status display

	// action window functions
	void					AddBottom(const String&, const unsigned int);
														// adds a line to the bottom of the action window
	void					AddBottom(const String&, const unsigned int * = NULL);
														// adds a line to the bottom of the action window
	void					UpdateBottom(const String&,  // updates a line in the action string
										 int = 0, const unsigned int * = NULL);
	void					UpdateBottom(const String&, unsigned int color);
										
	String					Bottom(int);				// returns a line from the bottom
	unsigned int *			BottomColor(int);			// returns the color index for a line at the bottom
	void					ClearBottom();				// clears the bottom
	void					WriteSlow(const String&, int, const unsigned int);

	// lock functions:
	// these functions will set locked, which keeps key values like myFont and myApp from 
	// being changed.
	void			Lock();							// locks key values
	void			Unlock();						// unlocks those values

	// others:
	bool			LoadFont();						// Loads the Font	
	void			SetHInstance(HINSTANCE, HWND);	// Stores the HINSTANCE & hwnd
	void			ResetTimers ();					// Resets the timers
	void			DestroyTimers ();				// Kills the timers
	void			ReleaseFont();					// Releases the font direct draw surfaces
	unsigned int	DefaultColor();					// Returns the default font color
	String			WeaponName(int);				// returns the weapon name
	int				WeaponCost(int w, int q);		// returns the cost of the weapon
	String			ArmorName(int);					// returns the armor name
	int				ArmorCost(int a, int q);		// returns the cost of an armor
	String			QualityName(int);				// returns the quality name
	

	// map:
	LotaMap			map;							// the map class

	// other commonly used variables that don't need accessors
	CmdMode			commandMode;			// the current command mode
	char			currentCommand;			// the first letter of the current command being executed
	int				waitCommand;			// holds the time in msec to wait to give Enter Command
	int				walkTime;				// time to wait between steps

	int				screenLeft;				// these two values store the original position
	int				screenTop;				//		of the window so we can restore it upon exiting 
											//		full screen mode
	int				stdDisplay;				// heartbeat has control of the display when = 0
	bool			done;					
	bool			waterReset;				// reset the water dots
	bool			quickMenu;				// stores whether or not a small menu has control
	int				menuKey;				// stores the key pressed inside the menu
	int				vertLine;				// Vertical line dividing menu and map
	bool			newGraphics;			// are we displaying the new graphics?
	int				raftAnim;				// raft animation frame
	int				raftFacing;				// direction the raft is facing (lotaEast or lotaWest)
	int				charAnimCount;			// animation count for the player
	bool			ZBufferEnable;			// enables or disables the ZBuffer

	bool			d3dViewport;			// stores whether or not the d3d viewport has been set
	//ErikCollection<D3DFloor*> d3dFloor;		// first wall of the d3d area
	D3DFloor*		d3dFloor;
	bool			allowEnter;				// used when a player exits a town or dungeon
	bool			invisible;				// is the player invisible?
	bool			guard;					// is the player in guard colors?

	bool			disableEncounters;		// used to disable overworld encounters

	bool				titleScreen;		// are we at the title screen?
	TitleScreenState	titleState;			// state for the title screen function
	int				titleMenu;				// current menu location in the title screen
};


// LOTA.CPP
LRESULT CALLBACK WndProc (HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam);
BOOL CALLBACK OptionsDlgProc (HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam);
unsigned long __stdcall HeartBeatThread (void *param);
void HeartBeat (short mode);

void DisplayTitleScreen(LPDIRECTDRAWSURFACE7 pDDS);
void RunTitleScreen();
void SetMenu1();
void SetMenu2();
void SetNewGame();
void SetNewGameText();
void SetRestoreGame();
void SetEraseGame();
void TitleKey();

void WriteText ( LPDIRECTDRAWSURFACE7 pDDS, int px, int py, const char *theText, const unsigned int *coloring = NULL);
void WriteText ( LPDIRECTDRAWSURFACE7 pDDS, int px, int py, const char *theText, unsigned long c);
void DoCommand (char cmd);
void DrawTile( LPDIRECTDRAWSURFACE7 pDDS, int px, int py, int tile);
void DrawCharacter( LPDIRECTDRAWSURFACE7 pDDS, int anim );
void ChangeScreenMode();
void CheckAnim();
void wait (int howLong, bool keyBreak = false);
void WaitKey();
void DrawBorder( LPDIRECTDRAWSURFACE7 pDDS, unsigned int boxColor);
void DrawInnerBorder( LPDIRECTDRAWSURFACE7 pDDS, unsigned int innerColor);
void DrawLine(LPDIRECTDRAWSURFACE7 pDDS, int left, int top, int direction, 
			  int length, unsigned int boxColor);
void DrawInnerLine(LPDIRECTDRAWSURFACE7 pDDS, int left, int top, int direction, 
			  int length, unsigned int innerColor);
void CheckSurfaces();
int rnd(int lo, int hi);
double frnd(double lo, double hi);
int SubMenu(const MenuItemList &items);
int QuickMenu(const MenuItemList &items, int spacing, int value = 0, unsigned int clrInit = lotaWhite, unsigned int clrChanged = 0);
String space(int num);
void DrawMenu( LPDIRECTDRAWSURFACE7 pDDS );
void DrawRafts( LPDIRECTDRAWSURFACE7 pDDS );
void DrawMonster( LPDIRECTDRAWSURFACE7 pDDS, int px, int py, int monst);
void RaftAnim();
void CheckJoystick();
void DrawBottomText ( LPDIRECTDRAWSURFACE7 pDDS );
int ChooseNumber(int max);


// COMMANDS.CPP
void Armor();
void Climb();
void Disembark();
void End();
void Fight();
void GameSpeed();
void Hold();
void Inventory (int mode = 0);
void Leave();
void Magic();
void Open(bool textTake = false);
void Pass();
void Rob();
void Speak();
void Take();
void Weapon();
void Use();
void Xamine();

void DrawSpecial( LPDIRECTDRAWSURFACE7 pDDS);


#endif
