
#include "lota.h"

extern Global g;

LotaMap::LotaMap()
{
	loaded = false;
}

LotaMap::~LotaMap()
{
	FreeResource(hMap);
}

int LotaMap::LoadMap(int map)
{
	int resID;

	if (loaded)
	{
		FreeResource(hMap);
	}
	loaded = false;

	resID = GetMapResource(map);

	if (resID > 0)
	{
		hMap = LoadResource (g.hInstance(), 
			   FindResource (g.hInstance(), MAKEINTRESOURCE(resID), TEXT("map")));

		m = (unsigned char*)LockResource (hMap);
		loaded = true;

		offset = m[4];
		mapWidth = m[1];
		mapHeight = m[2];
		mapType = m[5];

		MapMenu();

	}
	else
	{
		return 1;
	}

	return 0;
}

int LotaMap::GetMapResource(int map)
{
	int resID = 0;

	switch (map)
	{
	case 1:
		resID = OUTSIDE1;

		break;

	case 2:
		resID = OUTSIDE2;

		break;
	case 3:
		//resID = OUTSIDE3;
		break;

	case 61:
		resID = Dng_Pirates;
	}
	
	return resID;

}

void LotaMap::GetName(int mapNum, String& szBuffer)
{
	HANDLE tempMap;
	int resID;
	String	tempName;
	resID = GetMapResource(mapNum);
	char*	tempM;

	if (resID > 0) 
	{
		tempMap = LoadResource (g.hInstance(), 
				  FindResource (g.hInstance(), MAKEINTRESOURCE(resID), TEXT("map")));
	
		tempM = (char*)LockResource(tempMap);

		for (int i = 0; i < 16; i++)
		{
			tempName += tempM[7 + i];
		}
		
		tempName.rtrim();

		szBuffer = tempName;

		FreeResource (tempMap);

	}

}

int LotaMap::M(int yy, int xx)
{

	if (yy < 0 || yy > mapWidth - 1 || xx < 0 || xx > mapWidth - 1)
	{
		return 0;
	}
	else
	{
		return m[yy * mapWidth + xx + offset];
	}

}

void LotaMap::SetM(int yy, int xx, int val)
{
	if (yy < 0 || yy > mapWidth - 1 || xx < 0 || xx > mapWidth -1)
	{
		return;
	}
	else
	{
		m[yy * mapWidth + xx + offset] = val;

	}

}
void LotaMap::Draw( LPDIRECTDRAWSURFACE7 pDDS, int y, int x)
{
	int i, j;
	int initialxx = g.vertLine + 16;
	int width = (624 - initialxx) / 16;
	int height = 17;
	int tile;
	static int lastTime = 0;
	int now = clock();

	width = width / 2;
	height = height / 2;

	int xx = initialxx;
	int yy = 16;

	if (mapType == mapDungeon || mapType == mapMuseum)
	{
		Render3DMap();

		return;
	}

	for (j = y - height; j < y + height + 1; j++)
	{

		for (i = x - width; i < x + width + 1; i++)
		{
			tile = M(j, i);

			if (lastTime + 150 < now || g.waterReset == true)
			{
				if (tile == 0 && rnd(1, 1000) < 5 && g.waterReset == false)
				{
					tile = 1;
					SetM(j, i, tile);

				}
				else if ((tile == 1 && rnd(1, 1000) < 50) || (g.waterReset == true && tile == 1))
				{
					tile = 0;
					SetM(j, i, tile);
					//*(m + j * mapWidth + i) = 65;
				}
			}

			DrawTile (pDDS, xx, yy, tile);

			//DDPutPixel (pDDS, xx, yy, 30, 255, 255);

			xx += 16;
		}

		yy += 16;
		xx = initialxx;
	}
	
	g.waterReset = false;

	if (lastTime + 150 < now)
	{
		lastTime = clock();
	}
}

int LotaMap::MapType()
{

	return mapType;

}

int LotaMap::MapHeight()
{
	return mapHeight;
}

int LotaMap::MapWidth()
{
	return mapWidth;
}

bool LotaMap::CheckSpecial()			// checks for special events at player coordinates
{

	SpecialEvent dave = GetSpecial();

	if (dave.type != 0)
	{
		return true;
	}
	else
	{
		return false;
	}

}

SpecialEvent LotaMap::GetSpecial()			// retuns the special event at the player coordinates
{
	int i;
	SpecialEvent dave;

	for (i = 0; i < 40; i++)
	{
		if (g.player.x == specialx(i) && g.player.y == specialy(i))
		{
			dave.sx = g.player.x;
			dave.sy = g.player.y;
			dave.type = specialType(i);
			specialData(i, dave.data);

			return dave;

		}
	}

	dave.type = 0;

	return dave;

}

int LotaMap::specialType(int i)
{
	int off = mapHeight * mapWidth;
	int type;

	off += i * 8;

	type = m[off];

	return type;
}

int LotaMap::specialx(int i)
{
	int off = (mapHeight) * mapWidth;
	int type;

	off += i * 8 + 1;

	type = m[off];

	return type;
}

int LotaMap::specialy(int i)
{
	int type;
	int off = mapHeight * mapWidth;
	
	off += i * 8 + 2;

	type = m[off];

	return type;
}

void LotaMap::specialData(int i, char* buffer)
{

	if (buffer)
	{
		int off = mapHeight * mapWidth;
		
		off += i * 8 + 3;

		for (int j = 0; j < 5; j++)
		{
			buffer[j] = m[off + j];
		}

		buffer[5] = 0;
	
	}

}

void LotaMap::MapMenu()
{
	int i = 0;

	switch (mapType)
	{
	case mapMuseum:

		break;
	case mapOutside:

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

		break;
	case mapTown:

		break;
	case mapDungeon:

		break;
	case mapCastle:
		break;

	}

}


SpecialEvent::SpecialEvent()
{
	data = new char[6];

	strcpy(data, "     ");

}

SpecialEvent::SpecialEvent(SpecialEvent& se)
{
	data = new char[6];

	for (int i = 0; i < 6; i++)
		data[i] = se.data[i];

	sx = se.sx;
	sy = se.sy;
	type = se.type;

}

SpecialEvent::~SpecialEvent()
{
	delete [] data;

}

