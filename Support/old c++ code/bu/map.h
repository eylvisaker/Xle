
#ifndef __MAPH__
#define __MAPH__

#include "lota.h"

class SpecialEvent {
public:

	SpecialEvent();
	SpecialEvent(SpecialEvent&);
	~SpecialEvent();

	char			type;
	char			sx, sy;
	char			*data;


};

class LotaMap
{
private:
	unsigned char			*m;						// map value in [y][x] format
	char					*s;						// special events
	HANDLE					hMap;					// resource handle
	bool					loaded;					// Loaded a map yet?
	int						mapWidth, mapHeight;	// dimensions of map
	int						mapType;				// type of the map
	int						offset;					// distance from the start of the file that the map data begins

	int						specialType(int);		// retusn the type of the special event
	int						specialx(int);			// retuns the x coordinate of the special event
	int						specialy(int);			// retuns the x coordinate of the special event
	void					specialData(int, char*);// sets char* to be the data at event given
	int						GetMapResource(int);	// returns that map resource number of the specified map

public:
	// constructor & destructor:
	LotaMap();
	~LotaMap();

	void					GetName(int mapNum,		// returns the name of the specified map
									String& szBuffer);
	int						M(int yy, int xx);		// returns the tile at (xx,yy) on the map
	void					SetM(int yy, int xx,	// sets the tile at xx, yy
								 int val);
	int						LoadMap(int map);		// loads map# supplied
	int 					MapType();				// returns the map type
	void					Draw(					// draws the map with center point (x,y)
							LPDIRECTDRAWSURFACE7 pDDS, int y, int x);	
	int						MapWidth();				// returns the map width in tiles
	int						MapHeight();			// returns the map height in tiles
	bool					CheckSpecial();			// checks for special events at player coordinates
	SpecialEvent			GetSpecial();			// retuns the special event at the player coordinates

	void					MapMenu();				// sets the menu at the left equal to the map conditions

};


#endif
