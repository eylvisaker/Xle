
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

	/*
	ifstream fin;

	char mapName[25] = "MAP";
	char mapNum[5];
	int i, j;

	lstrcat(mapName, itoa(map, mapNum, 5));

	fin.open (mapName);

	if (fin.fail())
	{
		MessageBox(g.hwnd(), "Failed to load the map!", "Failed", 0);
		PostQuitMessage(1);

		return 1;
	}

	for (j = 0; j < 100 || fin.eof(); j++)
	{
		fin.getline(m[j], 100);
	}

	fin.close();

	for (j = 0; j < 100; j++)
	{
		for (i = 0; i < 100; i++)
		{
			m[j][i] -= 65;
		}
	}

	return 0;
	*/

	if (loaded)
	{
		FreeResource(hMap);
	}



	m = (char*)LockResource (hMap);
	loaded = true;

	return 0;
}


int LotaMap::M(int y, int x)
{

	return *(m + y * 100 + x) - 65;

}

void LotaMap::Draw( LPDIRECTDRAWSURFACE7 pDDS, int y, int x)
{
	int i, j;
	int initialxx = 272;
	int width = 23;
	int height = 17;

	width = width / 2;
	height = height / 2;

	int xx = initialxx;
	int yy = 16;

	for (j = y - height; j < y + height + 1; j++)
	{

		for (i = x - width; i < x + width; i++)
		{
			if (i == x && j == y)
			{
				int xlvj = 0;
			}

			DrawTile (pDDS, xx, yy, M(j,i));

			//DDPutPixel (pDDS, xx, yy, 30, 255, 255);

			xx += 16;
		}

		yy += 16;
		xx = initialxx;
	}
	
}

int LotaMap::MapType()
{

	return mapOutside;

}
