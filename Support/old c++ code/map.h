
#ifndef __MAPH__
#define __MAPH__

#include "lota.h"

class SpecialEvent 
{
    public:

		SpecialEvent();
		SpecialEvent(SpecialEvent&);
		~SpecialEvent();

		void operator=(SpecialEvent&);

		int				id;
		char			type;
		char			sx, sy;
		char			swidth, sheight;
		bool			marked;
		unsigned char	*data;
		const int		*robbed;

};

struct Monster {
	String		mName;
	int			mTerrain;
	int			mHP;
	int			mAttack;
	int			mDefense;
	int			mGold;
	int			mFood;
	int			mWeapon;
	int			mImage;
	bool		mTalks;
	int			mFriendly;

	Monster() : mTerrain(0), mHP(0), mAttack(0), mDefense(0), mGold(0),
	mFood(0), mWeapon(0), mImage(0), mTalks(false), mFriendly(0) 
	{ };

	Monster(Monster &m) { copyFrom(m); }
	Monster operator = (Monster &m) { copyFrom (m); return m; }

	void copyFrom(const Monster &m)
	{
		mName = m.mName;
		mTerrain = m.mTerrain;
		mHP = m.mHP;
		mAttack = m.mAttack;
		mDefense = m.mDefense;
		mGold = m.mGold;
		mFood = m.mFood;
		mWeapon = m.mWeapon;
		mImage = m.mImage;
		mTalks = m.mTalks;
		mFriendly = m.mFriendly;
		
	}

};

class LotaMap
{
	private:
		unsigned char			*m;						// resource data
		unsigned char			*mData;					// map values
		unsigned char			*mOriginalData;			// original values
		int						mapExtend;				// stores the number of extra tiles beyond the edges of the map
		char					*mRoof;					// stores whether or not a point is under a roof
		char					*s;						// special events
		HANDLE					hMap;					// resource handle
		bool					loaded;					// Loaded a map yet?
		int						mapWidth, mapHeight;	// dimensions of map
		int						mapType;				// type of the map
		int						offset;					// distance from the start of the file that the map data begins
		int						buyRaftMap, buyRaftX, buyRaftY;		// map and coors that mark where a purchased raft shows up
		int						mail[4];				// towns to carry mail to

		Monster					mMonst[32];				// stores monster structs;
		int						encounterState;
		int						stepCount;
		int						displayMonst;
		Direction				monstDir;
		POINT					monstPoint;
		int						monstCount, initMonstCount;
		Monster					currentMonst[6];

		int						mapNum;					// map number
		int						maxDungeonLevels;		// total number of dungeon levels
		int						tileSet;				// stores which bitmap contains map tiles
		int						roofOffset;				// stores the roof offset of the map
		//int					roofOpen;				// stores which roof is opened
		int						eachRoofOffset[40];		// stores roof offsets
		bool					roofOpen[40];			// stores open roofs
		int						robbed[40];				// stores which stores have been robbed
		bool					spcMarked[200];			// marked specials

		int						angry;					// whether or not the guards are chasing the player
		POINT					guard[101];				// the guards' locations on the map
		int						guardHP[101];			// stores the current hp for the guards
		Direction				guardFacing[101];		// direction the guards are facing
		int						guardAttack;			// attack and defense for the guards
		int						guardDefense;			
		unsigned long			guardColor;

		int						specialType(int);		// retusn the type of the special event
		int						specialx(int);			// retuns the x coordinate of the special event
		int						specialy(int);			// retuns the y coordinate of the special event
		int						specialwidth(int);		// retuns the width coordinate of the special event
		int						specialheight(int);		// retuns the height coordinate of the special event
		bool					specialmarked(int);		// returns whether or not the special has been marked (ie open chest.)
		void					specialData(int, unsigned char*);// sets char* to be the data at event given
		int						GetMapResource(int);	// returns that map resource number of the specified map
		String					name;					// stores the name of the map
		int						RoofIntOffset(int r);	// finds the roof offset given
		int						InIntRoof(int xx, int yy);	// returns if a point is in a roof

		void					LoadMonsters();
		int						ParseMonstValue(String &sTemp);

	public:
		// constructor & destructor:
		LotaMap();
		~LotaMap();

		String					GetName(int mapNum);	// returns the name of the specified map
										
		int						M(int yy, int xx);		// returns the tile at (xx,yy) on the map
		int						OriginalM(int yy, int xx);  // returns original tile at (xx, yy) on map
		void					SetM(int yy, int xx,	// sets the tile at xx, yy
									int val);
		int						LoadMap(int map);		// loads map# supplied
		int 					MapType();				// returns the map type
		void					Draw(					// draws the map with center point (x,y)
								LPDIRECTDRAWSURFACE7 pDDS, int y, int x);	
		int						MapWidth();				// returns the map width in tiles
		int						MapHeight();			// returns the map height in tiles
		int						CheckSpecial();			// checks for special events at player coordinates
		int						CheckSpecial(int x, int y);		// checks for special events at given coordinates
		SpecialEvent			GetSpecial();			// returns the special event at the player coordinates
		SpecialEvent			GetSpecial(int x, int y);		// returns the special event at the given coordinates
		bool					HasSpecialType(int type);		// returns whether or not the map has a particular special type (like a lender?)
		void					MarkSpecial(SpecialEvent);		// mark the special
		String					Name();					// returns the name of the map
		
		int						Mail(int);				// returns mail route
		bool					returnMail;

		int	BuyRaftX() { return buyRaftX; };
		int BuyRaftY() { return buyRaftY; };
		int BuyRaftMap() { return buyRaftMap; };

		void					MapMenu();				// sets the menu at the left equal to the map conditions
		int						MapNumber();			// returns the current map number
		int						SpecialDataLength();	// returns the length of the special data
		int						TileSet();				// returns the tileset of the map
		void					CheckRoof(int x, int y);	// checks for roofs at the given position and opens them
		//int						RoofOpen();				// returns which roof is open
		int						InRoof(int xx, int yy);	// returns if a point is in a roof
		int						SetRobbed(int se, int = 0);	// increments by value given and sets robbed flag
		bool					IsOpen(int);			// returns true if the specified roof is open
		int						Terrain(int xx, int yy);

		// Guard functions
		void					AnimateGuards();		// animates the guards
		void					Guards();				// Moves guards and handles their attacks
		int						AttackGuard(int grd);	// Player attacks a guard
		void					DrawGuards(				// Draws the guards on the map
								LPDIRECTDRAWSURFACE7 pDDS);
		bool					CheckGuard(POINT pt, int grd);	// checks a point on the map for walkability

		int						guardAnim;				// animation frame for the guards
		POINT					GuardPos(int i);		// returns the position of the given guard
		int						IsAngry(int set = -1);	// sets or returns anger state of guards
		POINT					RoofAnchor(int r);		// returns the anchor point of the roof
		POINT					RoofAnchorTarget(int r);// returns the anchor target of the roof
		POINT					RoofSize(int r);		// returns the size of the roof
		int						RoofOffset(int r);		// returns the start of the individual roof data
		int						RoofTile(int xx, int yy);// returns the roof tile as xx, yy

		// Monster functions
		bool					CheckEncounter(int xx, int yy, int dx, int dy);
		void					TestEncounter(int xx, int yy, int cursorKeys);
		int						EncounterState() 
								{ 	if (encounterState == -1)
									{	encounterState = 0;
										return -1; 	}
									return encounterState; };

		String					MonstName();
		int						attack();
		bool					KilledOne();
		void					MonstFight();
		bool					FinishedCombat(int &gold, int &food);
		bool					friendly;
		void					SpeakToMonster();

};


#endif
