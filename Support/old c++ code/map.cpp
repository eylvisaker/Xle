
#include "lota.h"

extern Global g;


LotaMap::LotaMap()
{
	loaded = false;
	mData = NULL;
	mRoof = NULL;

	displayMonst = -1;

}

LotaMap::~LotaMap()
{
	if (loaded)
	{
		FreeResource(hMap);
	}
	if (mData)
	{
		delete [] mData;
		mData = NULL;
	}
	if (mOriginalData)
	{
		delete [] mOriginalData;
		mOriginalData = NULL;
	}
	if (mRoof)
	{
		delete [] mRoof;
		mRoof = NULL;		
	}

}

void LotaMap::LoadMonsters()
{
	DWORD error;
	int ptr = 0;

	HANDLE hMonst = LoadResource (g.hInstance(), 
		FindResource (g.hInstance(), MAKEINTRESOURCE(dat_Monst1), TEXT("MONST")));

	error = GetLastError();
	_ASSERT(!FAILED(error));
		

	BYTE *data = (unsigned char*)LockResource (hMonst);

	error = GetLastError();
	_ASSERT(!FAILED(error));

	for (int i = 0; i < 32; i++)
	{
		Monster current;
		String sTemp, sTemp2;
		int j;

		while(data[ptr] != 0x0D)
		{
			if (data[ptr] != 0x0A)
				sTemp += (char)data[ptr++];
			else
				ptr++;
		}
		ptr++;

		for (j = 0; j < len(sTemp) && sTemp[j] != 0x09; j++);
		current.mName = ltrim(left(sTemp, j));
		sTemp = mid(sTemp, j, len(sTemp));

		if (current.mName == "Name" || current.mName == "")
		{
			i--;
			continue;
		}

		current.mTerrain = ParseMonstValue(sTemp);
		current.mHP = ParseMonstValue(sTemp);
		current.mAttack = ParseMonstValue(sTemp);
		current.mDefense = ParseMonstValue(sTemp);
		current.mGold = ParseMonstValue(sTemp);
		current.mFood = ParseMonstValue(sTemp);
		current.mWeapon = ParseMonstValue(sTemp);
		current.mImage = ParseMonstValue(sTemp);
		current.mTalks = (ParseMonstValue(sTemp)) ? true : false;
		current.mFriendly = ParseMonstValue(sTemp);

		mMonst[i] = current;
	}

	FreeResource(hMonst);

}

int LotaMap::ParseMonstValue(String &sTemp)
{
	for (int j = 0; j < len(sTemp) && sTemp[j] != 0x09; j++);

	sTemp = mid(sTemp, j, len(sTemp));

	while(sTemp[0] == 0x09) 
	{
		sTemp = mid(sTemp, 1, len(sTemp));
	}

	return (int) val(sTemp);

}


int LotaMap::LoadMap(int map)
{
	int resID;
	int off = 0;
	String search;
	int defHP = 0;

	if (loaded)
	{
		FreeResource(hMap);
	}
	if (mData)
	{
		delete [] mData;
		mData = NULL;
	}
	if (mOriginalData)
	{
		delete [] mOriginalData;
		mOriginalData = NULL;
	}
	if (mRoof)
	{
		delete [] mRoof;
		mRoof = NULL;		
	}
	loaded = false;

	resID = GetMapResource(map);

	if (mMonst[0].mName == "")
		LoadMonsters();

	displayMonst = -1;
	encounterState = 0;

	g.invisible = false;
	g.guard = false;

	if (resID > 0)
	{
		// Get the handle to the map in a resource
		// eventually, adapt this to get maps that are outside files.
		hMap = LoadResource (g.hInstance(), 
			   FindResource (g.hInstance(), MAKEINTRESOURCE(resID), TEXT("map")));

		m = (unsigned char*)LockResource (hMap);
		loaded = true;

		
		// Load the map variables
		mapWidth = m[0] * 256 + m[1];
		mapHeight = m[2] * 256 + m[3];
		offset = m[4] * 256 + m[5];
		mapType = m[6];
		mapNum = map;
		tileSet = m[33];
		defHP = m[25] * 256 + m[26];
		guardAttack = m[27] * 256 + m[28]; 
		guardDefense = m[29] * 256 + m[30];
		guardColor = m[31] * 256 + m[32];
		roofOffset = m[34] * 256 + m[35];

		buyRaftMap = m[36];
		buyRaftX = m[37] * 256 + m[38];
		buyRaftY = m[39] * 256 + m[40];

		// 41 is special count, not necessary to load here?

		mail[0] = m[42];
		mail[1] = m[43];
		mail[2] = m[44];
		mail[3] = m[45];

		returnMail = false;
		name = "";

		for (int i = 0; i < 16; i++)
		{
			name += (char)m[7 + i];
		}

		// reset per-visit settings
		for (i = 0; i < 40; i++)
		{
			robbed[i] = 0;
		}

		if (mapType == mapDungeon || mapType == mapMuseum)
		{
			maxDungeonLevels = m[24];
			mapExtend = 1;
		}
		else
		{
			mapExtend = 60;
			maxDungeonLevels = 1;
		}

		mData = new unsigned char[(mapWidth + 2 * mapExtend) * (mapHeight + 2 * mapExtend) * maxDungeonLevels];
		mOriginalData = new unsigned char[(mapWidth + 2 * mapExtend) * (mapHeight + 2 * mapExtend) * maxDungeonLevels];

		if (mapType == mapTown || mapType == mapCastle)
			mRoof = new char[mapWidth * mapHeight];

		for (i = 0; i < 200; i++)
			spcMarked[i] = false;

		for (i = 0; i < (mapWidth + 2 * mapExtend) * (mapHeight + 2 * mapExtend); i++)
		{
			if (mapType == mapOutside)
			{
				mOriginalData[i] = mData[i] = 0;
				
			}
			else if (mapType == mapDungeon || mapType == mapMuseum)
			{
				mOriginalData[i] = mData[i] = 0xff;
			}
			else if (mapType == mapCastle || mapType == mapTown)
			{
				mOriginalData[i] = mData[i] = m[24];
				
			}
		}

		for (int j = 0; j < mapHeight * maxDungeonLevels; j++)
		{
			for (i = 0; i < mapWidth; i++)
			{
				mOriginalData[(j + mapExtend) * (mapWidth + 2 * mapExtend) + (i + mapExtend)] = 
					mData[(j + mapExtend) * (mapWidth + 2 * mapExtend) + (i + mapExtend)] = m[j * mapWidth + i + offset];
			}
		}

		name = rtrim(name);

		MapMenu();

		if (mapType == mapTown || mapType == mapCastle)
		{

			off = offset + mapWidth * mapHeight;

			while (!(right(search, 7) == "5555557"))
			{
				search += (char)m[off++];
			}

			for (i = 0; i < 101; i++)
			{
				guard[i].x = m[off++] * 256;
				guard[i].x += m[off++];
				guard[i].y = m[off++] * 256;
				guard[i].y += m[off++];

				guardFacing[i] = lotaSouth;

				guardHP[i] = defHP + defHP * rnd(-10, 10) / 100;

			}

			for (i = 0; i < 40; i++)
			{
				eachRoofOffset[i] = RoofIntOffset(i);
			}

			for (j = 0; j < mapHeight; j++)
			{
				for (i = 0; i < mapWidth; i++)
				{
					mRoof[j * mapWidth + i] = InIntRoof(i, j);
				}
			}

			guardAnim = 2;

		}
		else if (mapType == mapOutside || mapType == mapDungeon || mapType == mapMuseum)
		{
			for (i = 0; i < 101; i++)
			{
				guard[i].x = 0;
				guard[i].y = 0;
			}
		}

		g.Unlock();
		g.LoadFont();
		g.Lock();

		for (i = 0; i < 40; i++)
		{
			roofOpen[i] = false;
		}

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
	case 0:
		return 0;

	case 1:
		resID = map_OUTSIDE1;

		break;

	case 2:
		resID = map_OUTSIDE2;

		break;
	case 3:
		//resID = map_OUTSIDE3;
		break;

	case 10:
		resID = map_ThompsonCrossing;
		break;
	
	case 11:
		resID = map_Thornberry;
		break;

	case 12:
		resID = map_Alanville;
		break;

	case 13:
		resID = map_IsleCity;
		break;

	case 14:
		resID = map_Cobbleton;
		break;

	case 15:
		resID = map_GrandLedge;
		break;

	case 16:
		resID = map_BigRapids;
		break;

	case 17:
		resID = map_Mazelton;
		break;

	case 18:
		resID = map_MerchantSquare;
		break;

	case 19:
		resID = map_Laingsburg;
		break;

	case 20:
		resID = map_HolyPoint;
		break;

	case 21:
		resID = map_EagleHollow;
		break;

	case 51:
		resID = map_Castle;
		break;

	case 52:
		resID = map_Castle2;
		break;

	case 61:
		resID = Dng_Pirates;
		break;

	default:
		OutputDebugString("GetMapResource: Failed to load map " + String(map) + "resource number.\n");

	}
	
	return resID;

}

String LotaMap::GetName(int mapNum)
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
		
		tempName = rtrim(tempName);

		FreeResource (tempMap);

	}
	
	if (tempName == "Thomson Crossing")
		tempName = "Thompson Crossing";

	return tempName;

}

int LotaMap::M(int yy, int xx)
{
	int inc = 0;

	if (yy < -mapExtend || yy > mapHeight - 1 + mapExtend ||
		xx < -mapExtend || xx > mapWidth - 1 + mapExtend)
	{
		if (mapType == mapOutside || mapType == mapDungeon || mapType == mapMuseum)
		{
			return 0;
		}
		else if (mapType == mapCastle || mapType == mapTown)
		{
			return m[24];
		}
		else
		{
			return 0;
		}
	}
	else
	{
		if (mapType == mapDungeon)
		{
			inc = mapHeight * mapWidth * (g.player.DungeonLevel() - 1);
		}

		return mData[(yy + mapExtend) * (mapWidth + 2 * mapExtend) + (xx + mapExtend) + inc];

	}
}


int LotaMap::OriginalM(int yy, int xx)
{
	int inc = 0;

	if (yy < -mapExtend || yy > mapHeight - 1 + mapExtend ||
		xx < -mapExtend || xx > mapWidth - 1 + mapExtend)
	{
		if (mapType == mapOutside || mapType == mapDungeon || mapType == mapMuseum)
		{
			return 0;
		}
		else if (mapType == mapCastle || mapType == mapTown)
		{
			return m[24];
		}
		else
		{
			return 0;
		}
	}
	else
	{
		if (mapType == mapDungeon)
		{
			inc = mapHeight * mapWidth * (g.player.DungeonLevel() - 1);
		}

		return mOriginalData[(yy + mapExtend) * (mapWidth + 2 * mapExtend) + (xx + mapExtend) + inc];

	}
}

void LotaMap::SetM(int yy, int xx, int val)
{
	if (yy < -mapExtend || yy > mapHeight - 1 + mapExtend ||
		xx < -mapExtend || xx > mapWidth - 1 + mapExtend)
	{
		return;
	}
	else
	{
		mData[(yy + mapExtend) * (mapWidth + 2 * mapExtend) + (xx + mapExtend)] = val;
	}

}
void LotaMap::Draw( LPDIRECTDRAWSURFACE7 pDDS, int y, int x)
{
	int i, j;
	int initialxx = g.vertLine + 16;
	int width = (624 - initialxx) / 16;
	int height = 17;
	int wAdjust = 0;
	int hAdjust = 0;
	int tile, t;
	int tx, ty;
	POINT mDrawMonst = {0,0};
	static int lastTime = 0;
	int now = clock();
	bool setLastTime = false;
	static long cycles = 0;

	width = width / 2;
	height = height / 2;

	wAdjust = 1;
	hAdjust = 1;

	/*if (width % 2 == 0)
	{
		wAdjust = 1;
	}
	if (height % 2 == 0)
	{
		hAdjust = 1;
	}*/

	int xx = initialxx;
	int yy = 16;

	if (mapType == mapDungeon || mapType == mapMuseum)
	{
		Render3DMap();

		return;
	}

	if (lastTime + 150 < now)
		setLastTime = true;


	for (j = y - height; j < y + height + hAdjust; j++)
	{

		for (i = x - width; i < x + width + wAdjust; i++)
		{
			tile = M(j, i);

			if ((setLastTime || g.waterReset == true) && g.map.MapType() == mapOutside)
			{
				if (tile == 0 && rnd(1, 1000) < 10 * (g.player.Stormy() + 1) && g.waterReset == false)
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
			else if (g.map.MapType() == mapCastle)
			{
				if (setLastTime && tile % 16 >= 13 && tile / 16 < 2)
				{
					tx = rnd(0xD, 0xF);
					ty = rnd(0x0, 0x1);

					tile = ty * 0x10 + tx;

					if (!((tile & 0x0F) >= 0x0D && (tile & 0x10) >> 4 <= 0x01))
					{
						int qweruio = 1;
						tile = 0x0F;
					}
					SetM(j, i, tile);
				}
				else if (setLastTime && (tile / 16 == 2 && tile % 16 < 8))
				{
					tile = cycles % 8 + 0x20;

					SetM(j, i, tile);
				}
				else if (setLastTime && (tile >= 0x40 && tile < 0x43))
				{
					tile = OriginalM(j, i);
					tile -= cycles % 3;
					
					while (tile < 0x40)
						tile += 3;

					SetM(j, i, tile);
				}

			}


			t = RoofTile(i, j);
			
			if (t != 127)
			{
				tile = t;
			}
			
			if (i == monstPoint.x && j == monstPoint.y)
			{
				mDrawMonst.x = xx;
				mDrawMonst.y = yy;
			}

			DrawTile (pDDS, xx, yy, tile);

			//DDPutPixel (pDDS, xx, yy, 30, 255, 255);

			xx += 16;
		}

		yy += 16;
		xx = initialxx;
	}
	
	g.waterReset = false;

	DrawGuards(pDDS);		
		
	if (setLastTime)
	{
		lastTime = clock();
		cycles ++;
	}

	if (displayMonst > -1)
	{
		DrawMonster(pDDS, mDrawMonst.x, mDrawMonst.y, displayMonst);
	}


}
/****************************************************************************
 *  void LotaMap::DrawGuards ( LPDIRECTDRAWSURFACE7 pDDS )					*
 *																			*
 *  This function displays the character sprite in the middle of the map	*
 *	subscreen.																*
 ****************************************************************************/
void LotaMap::DrawGuards( LPDIRECTDRAWSURFACE7 pDDS)
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
	
	
	for (i = 0; i < 101; i++)
	{
		if (guard[i].x != 0 && guard[i].y != 0 && InRoof(guard[i].x, guard[i].y) == -1)
		{
			tx = guardAnim * 32 + g.newGraphics * 96;
			
			if (IsAngry())
				ty = (guardFacing[i] + 3) * 32;
			else
				ty = 7 * 32;
			
			SetRect(&charRect, tx, ty, tx + 32, ty + 32);

			rx = px - (g.player.X() - guard[i].x) * 16;
			ry = py - (g.player.Y() - guard[i].y) * 16;
			
			if (rx >= lx && ry >= 16 && rx <= 592 && ry < 272)
			{

				SetRect(&destRect, rx, ry, rx + 32, ry + 32);
				//pDDS->Blt(&destRect, g.Character(), &charRect,  DDBLT_WAIT | DDBLT_KEYSRC, NULL);
				pDDS->BltFast(destRect.left, destRect.top, g.Character(), &charRect, DDBLTFAST_SRCCOLORKEY);

			}
		}
	}
}

void LotaMap::AnimateGuards()
{
	static int lastGuardAnim = 0;

	if (angry && lastGuardAnim + 150 <= clock())
	{
		guardAnim++;
		if (guardAnim > 2)
			guardAnim = 0;

		lastGuardAnim = clock();
	}
	else if (lastGuardAnim + 1000 <= clock())
	{
		guardAnim++;
		if (guardAnim > 2)
			guardAnim = 0;
	
		lastGuardAnim = clock();
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

int LotaMap::CheckSpecial()
{
	return CheckSpecial(g.player.X(), g.player.Y());
}

int LotaMap::CheckSpecial(int x, int y)			// checks for special events at player coordinates
{

	SpecialEvent dave = GetSpecial(x, y);

	if (dave.type != 0)
	{
		return dave.type;
	}
	else
	{
		return 0;
	}

}

bool LotaMap::specialmarked(int i)
{
	return spcMarked[i];
}

SpecialEvent LotaMap::GetSpecial()
{
	return GetSpecial(g.player.X(), g.player.Y());
}


SpecialEvent LotaMap::GetSpecial(int x, int y)			// retuns the special event at the player coordinates
{
	int i;
	SpecialEvent dave;

	for (i = 0; i < 120; i++)
	{
		if (x >= specialx(i) && y >= specialy(i) && 
			x <= specialx(i) + specialwidth(i) - 1 && y <= specialy(i) + specialheight(i) - 1)
		{
			dave.sx = specialx(i);
			dave.sy = specialy(i);
			dave.swidth = specialwidth(i);
			dave.sheight = specialheight(i);
			dave.type = specialType(i);
			dave.id = i;
			dave.marked = specialmarked(i);
			dave.robbed = &robbed[i];

			specialData(i, dave.data);

			return dave;

		}
	}

	dave.type = 0;

	return dave;

}

void LotaMap::MarkSpecial(SpecialEvent dave)
{
	spcMarked[dave.id] = true;
}

int LotaMap::specialType(int i)
{
	int off = mapHeight * mapWidth + offset;
	int type;

	off += i * (SpecialDataLength() + 5);

	type = m[off];

	return type;
}

int LotaMap::specialx(int i)
{
	int off = mapHeight * mapWidth + offset;
	int type;

	off += i * (SpecialDataLength() + 5) + 1;

	type = m[off++] * 256;
	type += m[off];

	return type;
}

int LotaMap::specialy(int i)
{
	int type;
	int off = mapHeight * mapWidth + offset;
	
	off += i * (SpecialDataLength() + 5) + 3;

	type = m[off++] * 256;
	type += m[off];

	return type;
}

int LotaMap::specialwidth(int i)
{
	int type;
	int off = mapHeight * mapWidth + offset;
	
	off += i * (SpecialDataLength() + 5) + 5;

	type = m[off++] * 256;
	type += m[off];

	return type;
}

int LotaMap::specialheight(int i)
{
	int type;
	int off = mapHeight * mapWidth + offset;
	
	off += i * (SpecialDataLength() + 5) + 7;

	type = m[off++] * 256;
	type += m[off];

	return type;
}

void LotaMap::specialData(int i, unsigned char* buffer)
{

	if (buffer)
	{
		int off = mapHeight * mapWidth + offset;
		
		off += i * (SpecialDataLength() + 5) + 9;

		for (int j = 0; j < SpecialDataLength(); j++)
		{
			buffer[j] = m[off + j];
		}

		buffer[j] = 0;
	
	}

}

void LotaMap::MapMenu()
{
	int i = 0;

	switch (mapType)
	{
	case mapMuseum:
		g.WriteMenu(i++, "Armor");
		g.WriteMenu(i++, "Fight");
		g.WriteMenu(i++, "Gamespeed");
		g.WriteMenu(i++, "Hold");
		g.WriteMenu(i++, "Inventory");
		g.WriteMenu(i++, "Pass");
		g.WriteMenu(i++, "Rob");
		g.WriteMenu(i++, "Speak");
		g.WriteMenu(i++, "Take");
		g.WriteMenu(i++, "Use");
		g.WriteMenu(i++, "Weapon");
		g.WriteMenu(i++, "Xamine");
		g.WriteMenu(i++, "");
		g.WriteMenu(i++, "");

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
		g.WriteMenu(i++, "Armor");
		g.WriteMenu(i++, "Fight");
		g.WriteMenu(i++, "Gamespeed");
		g.WriteMenu(i++, "Hold");
		g.WriteMenu(i++, "Inventory");
		g.WriteMenu(i++, "Leave");
		g.WriteMenu(i++, "Magic");
		g.WriteMenu(i++, "Pass");
		g.WriteMenu(i++, "Rob");
		g.WriteMenu(i++, "Speak");
		g.WriteMenu(i++, "Use");
		g.WriteMenu(i++, "Weapon");
		g.WriteMenu(i++, "Xamine");
		g.WriteMenu(i++, "");

		break;
	case mapDungeon:

		g.WriteMenu(i++, "Armor");
		g.WriteMenu(i++, "Climb");
		g.WriteMenu(i++, "End");
		g.WriteMenu(i++, "Fight");
		g.WriteMenu(i++, "Gamespeed");
		g.WriteMenu(i++, "Hold");
		g.WriteMenu(i++, "Inventory");
		g.WriteMenu(i++, "Magic");
		g.WriteMenu(i++, "Open");
		g.WriteMenu(i++, "Pass");
		g.WriteMenu(i++, "Use");
		g.WriteMenu(i++, "Weapon");
		g.WriteMenu(i++, "Xamine");
		g.WriteMenu(i++, "");


		break;
	case mapCastle:
		g.WriteMenu(i++, "Armor");
		g.WriteMenu(i++, "Fight");
		g.WriteMenu(i++, "Gamespeed");
		g.WriteMenu(i++, "Hold");
		g.WriteMenu(i++, "Inventory");
		g.WriteMenu(i++, "Magic");
		g.WriteMenu(i++, "Open");
		g.WriteMenu(i++, "Pass");
		g.WriteMenu(i++, "Speak");
		g.WriteMenu(i++, "Take");
		g.WriteMenu(i++, "Use");
		g.WriteMenu(i++, "Weapon");
		g.WriteMenu(i++, "Xamine");
		g.WriteMenu(i++, "");

		break;

	}

	g.cursor('P');

}

String LotaMap::Name()
{
	if (name == "Thomson Crossing")
		name = "Thompson Crossing";

	return name;

}

int	LotaMap::MapNumber()
{
	return mapNum;
}

void LotaMap::Guards()
{
	if (angry)
	{
		int i, j;
		double dist;
		int xdist;
		int ydist;
		int dx;
		int dy;
		POINT newPt;
		bool badPt = false;
		unsigned int color[40];
		String tempString;
		int dam;

		for (i = 0; i < 101; i++)
		{
			if (guard[i].x != 0 && guard[i].y != 0 && InRoof(guard[i].x, guard[i].y) == -1)
			{
				badPt = false;

				newPt = guard[i];

				xdist = g.player.X() - guard[i].x;
				ydist = g.player.Y() - guard[i].y;

				if (xdist != 0)
					dx = xdist / abs(xdist);
				else dx = 0;
				if (ydist != 0)
					dy = ydist / abs(ydist);
				else dy = 0;

				dist = sqrt(pow(xdist, 2) + pow(ydist, 2));

				if (abs(xdist) <= 2 && abs(ydist) <= 2)
				{

					g.AddBottom("");

					tempString = "Attacked by guard! -- ";
					dam = g.player.Damage(guardAttack);

					for (j = 0; j < len(tempString); j++)
					{
						color[j] = lotaWhite;
					}

					if (dam > 0)
					{
						tempString += "Blow ";

						for (; j < len(tempString); j++)
						{
							color[j] = lotaYellow;
						}
						tempString += dam;
						tempString += " H.P.";

						for (; j < len(tempString); j++)
						{
							color[j] = lotaWhite;
						}

						LotaPlaySound(snd_EnemyHit);

					}
					else
					{
						tempString += "Missed";

						for (; j < len(tempString); j++)
						{
							color[j] = lotaCyan;
						}
						LotaPlaySound(snd_EnemyMiss);
					}

					
					g.AddBottom(tempString, color);

					g.commandMode = cmdBad;

					wait(100 * g.player.Gamespeed());

					g.commandMode = cmdEnterCommand;


				}
				else if (dist < 25)
				{
					if (abs(xdist) > abs(ydist))
					{
						newPt.x += dx;
					}
					else
					{
						newPt.y += dy;
					}

					badPt = !CheckGuard(newPt, i);

					if (badPt == true)
					{
						newPt = guard[i];

						if (abs(xdist) > abs(ydist))
						{
							xdist = 0;

							if (ydist == 0)
							{
								dy = rnd(0, 1) * 2 - 1;
							}
							
							dx = 0;

							newPt.y += dy;
						}
						else
						{
							ydist = 0;

							if (xdist == 0)
							{
								dx = rnd(0, 1) * 2 - 1;
							}

							dy = 0;

							newPt.x += dx;
						}
						badPt = !CheckGuard(newPt, i);

						if (badPt == true)
							newPt = guard[i];

					}

					guard[i] = newPt;

					if (abs(xdist) > abs(ydist))
					{
						if (dx < 0)
						{
							guardFacing[i] = lotaWest;
						}
						else
						{
							guardFacing[i] = lotaEast;
						}
					}
					else
					{
						if (dy < 0)
						{
							guardFacing[i] = lotaNorth;
						}
						else
						{
							guardFacing[i] = lotaSouth;
						}
					}

				}
			}			// guard(x,y) != (0,0)
		}

	}
}

bool LotaMap::CheckGuard(POINT pt, int grd)
{
	int i, j, k;

	for (j = 0; j < 2; j++)
	{
		for (i = 0; i < 2; i++)
		{
			if (g.map.M(pt.y + j, pt.x + i) >= 128 || g.map.M(pt.y + j, pt.x + i) % 16 >= 7)
			{
				return false;
			}

			for (k = 0; k < 101; k++)
			{
				if (k != grd)
				{
					if ((guard[k].x == pt.x - 1 || guard[k].x == pt.x || guard[k].x == pt.x + 1) &&
						(guard[k].y == pt.y - 1 || guard[k].y == pt.y || guard[k].y == pt.y + 1))
					{
						return false;
					}
				}
			}
		}
	}

	return true;
}
int LotaMap::AttackGuard(int grd)
{
	int dam = 0;
	int i = 0;
	int hit = 0;
	String tempString;
	unsigned int color[40];
	
	dam = g.player.Hit(guardDefense);

	if (dam > 0)
	{
		angry = true;
		//g.player.lastAttacked = MapNumber();

		tempString = "Guard struck  ";

		for (i = 0; i < len(tempString); i++)
		{
			color[i] = lotaYellow;
		}

		tempString += (String)dam + " H.P. blow";

		for (; i < len(tempString); i++)
		{
			color[i] = lotaWhite;
		}

		g.AddBottom(tempString, color);

		guardHP[grd] -= dam;

		LotaPlaySound(snd_PlayerHit);

		if (guardHP[grd] <= 0)
		{
			g.AddBottom("Guard killed");

			guardHP[grd] = 0;
			guard[grd].x = 0;
			guard[grd].y = 0;

			wait(100);

			LotaStopSound(snd_PlayerHit);
			LotaPlaySound(snd_EnemyDie);

		}

	}
	else
	{
		g.AddBottom("Attack on guard missed", lotaPurple);
		LotaPlaySound(snd_PlayerMiss);
	}

	return 0;

}

POINT LotaMap::GuardPos(int i)
{
	POINT tempPt;

	tempPt.x = guard[i].x;
	tempPt.y = guard[i].y;

	return tempPt;

}

int LotaMap::SpecialDataLength()
{
	switch (MapType())
	{
	case mapOutside:
		return 104;
		break;
	case mapTown:
	case mapCastle:
		return 104;
		break;
	}

	return 5;
}

int LotaMap::IsAngry(int set)
{
	if (set == 1 || set == 0)
	{
		angry = set;
	}
	
	if (angry)
	{
		g.invisible = false;
		g.guard = false;
	}


	return angry;
}

int LotaMap::TileSet()
{
	return tileSet;
}

void LotaMap::CheckRoof(int ptx, int pty)
{
	if (mapType != mapTown && mapType != mapCastle)
	{
		return;
	}
	
	if (!IsAngry())
	{
		memset(roofOpen, 0, sizeof(bool) * 40);
	}

	if (InRoof(ptx, pty) >= 0)
	{
		roofOpen[InRoof(ptx, pty)] = true;
	}

}

int LotaMap::InIntRoof(int ptx, int pty)
{
	POINT anchor;
	POINT anchorTarget;
	POINT size;
	int roof = -1;

	for (int i = 0; i < 40 && roof == -1; i++)
	{
		anchor = RoofAnchor(i);
		anchorTarget = RoofAnchorTarget(i);
		size = RoofSize(i);

		if (ptx >= anchorTarget.x - anchor.x && ptx < anchorTarget.x - anchor.x + size.x &&
			pty >= anchorTarget.y - anchor.y && pty < anchorTarget.y - anchor.y + size.y && 
			RoofTile(ptx, pty) != 127)

		{
			roof = i;
			continue;
		}

		else if (ptx + 1 >= anchorTarget.x - anchor.x && ptx + 1 < anchorTarget.x - anchor.x + size.x &&
			pty >= anchorTarget.y - anchor.y && pty < anchorTarget.y - anchor.y + size.y && 
			RoofTile(ptx + 1, pty) != 127)
		{
			roof = i;
			continue;
		}

		else if (ptx >= anchorTarget.x - anchor.x && ptx < anchorTarget.x - anchor.x + size.x &&
			pty + 1>= anchorTarget.y - anchor.y && pty + 1< anchorTarget.y - anchor.y + size.y && 
			RoofTile(ptx, pty + 1) != 127)
		{
			roof = i;
			continue;
		}

		else if (ptx + 1>= anchorTarget.x - anchor.x && ptx + 1< anchorTarget.x - anchor.x + size.x &&
			pty + 1 >= anchorTarget.y - anchor.y && pty + 1< anchorTarget.y - anchor.y + size.y && 
			RoofTile(ptx + 1, pty + 1) != 127)
		{
			roof = i;
			continue;
		}
	}

	return roof;


}

int LotaMap::InRoof(int ptx, int pty)
{
	int roof = -1;

	if (pty >= 0 && pty < mapHeight && ptx >= 0 && ptx < mapWidth)
	{
		roof = mRoof[pty * mapWidth + ptx];

		if (IsOpen(roof))
		{
			roof = -1;
		}

	}

	return roof;

}

/*int LotaMap::RoofOpen()
{
	return roofOpen;

}
*/
bool LotaMap::IsOpen(int r)
{
	
	return roofOpen[r];

}

POINT LotaMap::RoofAnchor(int r)
{
	if (r == 39)
		int i = 0;

	int off = RoofOffset(r);
	POINT size;


	size.x = m[off] * 256 + m[off + 1];
	size.y = m[off + 2] * 256 + m[off + 3];

	return size;
}

POINT LotaMap::RoofAnchorTarget(int r)
{
	int off = RoofOffset(r);
	POINT size;

	size.x = m[off + 4] * 256 + m[off + 5];
	size.y = m[off + 6] * 256 + m[off + 7];

	return size;
}

POINT LotaMap::RoofSize(int r)
{
	int off = RoofOffset(r);
	POINT size;

	size.x = m[off + 8] * 256 + m[off + 9];
	size.y = m[off + 10] * 256 + m[off + 11];

	return size;
}

int LotaMap::RoofIntOffset(int r)
{
	POINT lastSize;
	int last;
	int me;

	if (r == 0)
	{
		return roofOffset;
	}

	last = RoofOffset(r - 1);
	lastSize = RoofSize(r - 1);

	me = last + 12 + lastSize.y * lastSize.x;

	return me;

}

int LotaMap::RoofOffset(int r)
{
	return eachRoofOffset[r];
}

int LotaMap::RoofTile(int xx, int yy)
{
	int i;
	POINT anchor;
	POINT anchorTarget;
	POINT size;

	for (i = 0; i < 40 && (MapType() == mapTown || MapType() == mapCastle); i++)
	{
		anchor = RoofAnchor(i);
		anchorTarget = RoofAnchorTarget(i);
		size = RoofSize(i);

		if (xx >= anchorTarget.x - anchor.x && xx < anchorTarget.x - anchor.x + size.x &&
			yy >= anchorTarget.y - anchor.y && yy < anchorTarget.y - anchor.y + size.y && 
			!roofOpen[i])
		{
			return m[RoofOffset(i) + (yy - anchorTarget.y + anchor.y) * size.x + xx - anchorTarget.x + anchor.x + 12];
		}
	}

	return 127;
}

int	LotaMap::SetRobbed(int se, int r)
{
	robbed[se] += r;

	return robbed[se];
}


bool LotaMap::HasSpecialType(int type)
{
	int t;

	for (int i = 0; i < 120; i++)
	{
		t = specialType(i);

		if (t == type)
			return true;
	}

	return false;
}

int LotaMap::Mail(int index)
{
	if (index > 3 || index < 0)
		return 0;

	return mail[index];

}


int LotaMap::Terrain(int xx, int yy)
{

	int		t[2][2] = {0,0,0,0};
	int		tc[8] = {0,0,0,0,0,0,0,0};
	int		i;

	for (int j = 0; j < 2; j++)
	{
		for ( i = 0; i < 2; i++)
		{
			t[j][i] = M(yy + j, xx + i);
		}
	}

	for (j = 0; j < 2; j++)
	{
		for (i = 0; i < 2; i++)
		{
			tc[t[j][i] / 32]++;
			
			if (t[j][i] % 32 <= 1)
				tc[t[j][i] / 32] += 1;
		}
	}

	if (tc[mapMountain] > 4)
	{
		return mapMountain;
	}	
	
	if (tc[mapMountain] > 0)
	{
		return mapFoothills;
	}

	if (tc[mapDesert] >= 1 )
	{
		return mapDesert;
	}

	if (tc[mapSwamp] > 1)
	{
		return mapSwamp;
	}

	for (i = 0; i < 8; i++)
	{
		if (tc[i] > 3)
		{
			return i;
		}
		else if (tc[i] == 2 && i != 1)
		{
			return mapMixed;
		}
	}

	return 2;
}

bool LotaMap::CheckEncounter(int xx, int yy, int dx, int dy)
{

	if (mapType != mapOutside)
		return true;

	if (encounterState == 1)
	{
		bool moveTowards = false;

		switch(monstDir)
		{

			case lotaEast:		if (dx > 0) moveTowards = true;				break;
			case lotaNorth:		if (dy < 0) moveTowards = true;				break;
			case lotaWest:		if (dx < 0) moveTowards = true;				break;
			case lotaSouth:		if (dy > 0) moveTowards = true;				break;

		}			

		if (!moveTowards)
		{
			if (rnd(0, 99) < 50)
			{

				encounterState = 3;			// avoided;
			}
		}

	}
	else if (encounterState == 10)
	{
		if (rnd(0, 99) < 40 && !friendly)
		{
			return false;
		}
		else
		{
			encounterState = -1;
			displayMonst = -1;
		}

	}

	return true;
}

void LotaMap::TestEncounter(int xx, int yy, int cursorKeys)
{
	int waitAtEnd = 0;
	bool keyBreak = false;
	bool firstTime = false;

	String dirName;

	if (mapType != mapOutside)
		return;

	if (g.disableEncounters)
		return;

	if (encounterState == 0 && stepCount <= 0)
	{
		stepCount = rnd(1, 15);
		int type = rnd(0, 20);

		if (cursorKeys == 0)
			type = 99;

		friendly = false;
		monstDir = (Direction) rnd(lotaEast, lotaSouth);
		

		if (type < 10)
		{


			monstPoint.x = xx - 1;
			monstPoint.y = yy - 1;

			switch(monstDir)
			{

				case lotaEast:	dirName = "East";		monstPoint.x += 2;		break;
				case lotaNorth:	dirName = "North";		monstPoint.y -= 2;		break;
				case lotaWest:	dirName = "West";		monstPoint.x -= 2;		break;
				case lotaSouth:	dirName = "South";		monstPoint.y += 2;		break;

			}	
			
			encounterState = 1;  // unknown creature
			LotaPlaySound(snd_Encounter, 0, 0);


			g.AddBottom("");
			g.AddBottom("An unknown creature is approaching ", lotaCyan);
			g.AddBottom("from the " + dirName + ".", lotaCyan);

			waitAtEnd = 1000;

		}
		else if (type < 15)
		{
			encounterState = 2;	// creature is appearing
			
			waitAtEnd = 1000;
		}

	}
 	else if (encounterState == 0 && stepCount > 0 && cursorKeys > 0)
	{
		stepCount--;
	}
	else if (encounterState == 1)
	{
		encounterState = 2;

		waitAtEnd = true;
	}

    if (encounterState == 2)
	{
		if (rnd(0, 99) < 55)
			encounterState = 5;		// monster has appeared
		else
			encounterState = 10;	// monster is ready

		int mCount = 0;
		int val, sel = -1;
		firstTime = true;

		LotaPlaySound(snd_Encounter, 0, true);

		for (int i = 0; i < 32; i++)
		{
			if (mMonst[i].mTerrain == -1 && Terrain(xx, yy) != 0)
				mCount ++;
			
			if (mMonst[i].mTerrain == Terrain(xx, yy))
				mCount += 3;

			if (Terrain(xx, yy) == mapFoothills && mMonst[i].mTerrain == mapMountain)
				mCount += 3;
		}

		val = rnd(1, mCount);

		for (i = 0; i < 32; i++)
		{
			if (mMonst[i].mTerrain == -1 && Terrain(xx, yy) != 0)
				val --;
			
			if (mMonst[i].mTerrain == Terrain(xx, yy))
				val -= 3;

			if (Terrain(xx, yy) == mapFoothills && mMonst[i].mTerrain == mapMountain)
				val -= 3;

			if (val == 0 || val == -1 || val == -2)
			{
				sel = i;
				break;
			}
		}

		_ASSERTE(sel > -1);

		displayMonst = sel;

		monstPoint.x = xx - 1;
		monstPoint.y = yy - 1;

		switch(monstDir)
		{

			case lotaEast:	dirName = "East";		monstPoint.x += 2;		break;
			case lotaNorth:	dirName = "North";		monstPoint.y -= 2;		break;
			case lotaWest:	dirName = "West";		monstPoint.x -= 2;		break;
			case lotaSouth:	dirName = "South";		monstPoint.y += 2;		break;

		}	


		int max = 1;
		initMonstCount = monstCount = rnd(1, max);
	
		for (int i = 0; i < monstCount; i++)
		{
			currentMonst[i] = mMonst[displayMonst];

			currentMonst[i].mHP = int(frnd(.8, 1.2) * currentMonst[i].mHP);
		}

		if (rnd(0, 255) <= currentMonst[0].mFriendly)
			friendly = true;
		else
			friendly = false;
		
		waitAtEnd = 2000;


	}
	
	if (encounterState == 3)
	{
		g.AddBottom("");
		g.AddBottom("You avoid the unknown creature.");
		waitAtEnd = g.player.Gamespeed() * 100 + 200;

		encounterState = 0;

	}
	else if (encounterState == 5)
	{
		unsigned int colors[40];
		String s = (monstCount > 1) ? "s" : "";
		
		for (int i = 0; i < 40; i++)
			colors[i] = lotaCyan;

		colors[0] = lotaWhite;

		encounterState = 10;			// appeared and ready

		g.AddBottom("");
		g.AddBottom(String(monstCount) + " " + currentMonst[0].mName + s, colors);

		colors[0] = lotaCyan;
		g.AddBottom("is approaching.", colors);

		waitAtEnd = 2000;

	}
	else if (encounterState == 10)
	{
		if (friendly)
		{
			unsigned int colors[40];

			for (int i = 0; i < 40; i++)
				colors[i] = lotaCyan;
			colors[0] = lotaWhite;

			g.AddBottom("");
			g.AddBottom(String(monstCount) + " " + currentMonst[0].mName, colors);
			g.AddBottom("Stands before you.");
			
			if (waitAtEnd == 0)
				waitAtEnd = 1500;

		}
		else
		{
			unsigned int colors[40];
			String text;

			text = "Attacked by ";

			for (int i = 0; i < len(text); i++)
				colors[i] = lotaWhite;

			text += String(monstCount);

			for (; i < len(text); i++)
				colors[i] = lotaYellow;

			text += " " + currentMonst[0].mName;

			for (; i < len(text); i++)
				colors[i] = lotaCyan;

			g.AddBottom("");
			g.AddBottom(text, colors);

			int dam = 0;
			int hits = 0;

			for (i = 0; i < monstCount; i++)
			{
				int t = g.player.Damage(currentMonst[i].mAttack);

				if (t > 0)
				{
					dam += t;
					hits ++;
				}
			}

			text = "Hits:  ";
			for (int i = 0; i < len(text); i++)
 				colors[i] = lotaWhite;

			text += String(hits);
			for (; i < len(text); i++)
				colors[i] = lotaYellow;


			text += "   Damage:  ";
			for (; i < len(text); i++)
				colors[i] = lotaWhite;

			text += String(dam);
			for (; i < len(text); i++)
				colors[i] = lotaYellow;


			g.AddBottom(text, colors);

			if (dam > 0)
			{
				LotaPlaySound(snd_EnemyHit);
			}
			else
			{
				LotaPlaySound(snd_EnemyMiss);
			}



		}

		waitAtEnd = 250;
		g.waitCommand = 1;

		if (!firstTime)
			keyBreak = true;
		
	}


 	if (waitAtEnd)
	{

		wait(waitAtEnd, keyBreak);

	}
}

String LotaMap::MonstName()
{
	return currentMonst[0].mName;

}

int LotaMap::attack()
{
	int damage = g.player.Hit(currentMonst[monstCount - 1].mDefense);

	if (currentMonst[monstCount - 1].mWeapon > 0)
	{
		if (g.player.WeaponType(g.player.CurrentWeapon()) == currentMonst[monstCount - 1].mWeapon)
		{
			damage += rnd(20, 30);
		}
		else
		{
			damage = rnd(1, (damage < 10) ? damage : 10);
		}
	}

	currentMonst[monstCount - 1].mHP -= damage;
	friendly = false;
 
	return damage;
}

bool LotaMap::KilledOne()
{
	if (currentMonst[monstCount - 1].mHP <= 0)
	{
		monstCount--;

		return true;

	}

	return false;
}

void LotaMap::MonstFight()
{

}

bool LotaMap::FinishedCombat(int &gold, int &food)
{
	bool finished = false;

	if (monstCount == 0)
	{
		finished = true;

		gold = 0; 
		food = 0;

		for (int i = 0; i < initMonstCount; i++)
		{
			gold += currentMonst[i].mGold;
			food += currentMonst[i].mFood;

		}

		gold = int(gold * frnd(0.5, 1.5));
		food = int(food * frnd(0.5, 1.5));

		if (rnd(0, 99) < 50)
			food = 0;

		encounterState = 0;
		displayMonst = -1;
	}

	return finished;

}


void LotaMap::SpeakToMonster()
{

	if (!friendly)
	{
		g.AddBottom("");
		g.AddBottom("The " + g.map.MonstName() + " does not reply.");

 		g.commandMode = cmdBad;
		wait(250);
		g.commandMode = cmdEnterCommand;

		return;
	}

	const int talkTypes = 5;
	int type = rnd(1, talkTypes);
	int qual = rnd(0, 4);
	int cost = 0;
	int item = 0;
	MenuItemList menu(2, "Yes", "No");

	String text1, text2;
	unsigned int colors1[40];
	unsigned int colors2[40];
	unsigned int qcolor = lotaWhite;

	String quality[5] = {" Well Crafted", " Slightly Used", " Sparkling New", " Wonderful", "n Awesome"};

	for (int i = 0; i < 40; i++)
	{
		colors1[i] = colors2[i] = lotaWhite;
	}

	while (g.player.MaxHP() == g.player.HP() && type == 4)
		type = rnd(1, talkTypes);

	switch(type)
 	{
		case 1:			// buy armor
			item = rnd(1, 4);
			cost = int(g.ArmorCost(item, qual) * frnd(0.6, 1.2));

			text1 = "Do you want to buy a";
			
			for (i = 0; i < len(text1); i++)
				colors1[i] = lotaCyan;

			text1 += quality[qual];
			
			text2 = g.ArmorName(item);
			i = len(text2);

			text2 += " for ";

			for (; i < len(text2); i++)
				colors2[i] = lotaCyan;

			text2 += String(cost);
			i = len(text2);

			text2 += " Gold?";
 			for (; i < len(text2); i++)
				colors2[i] = lotaCyan;

			qcolor = lotaCyan;

			break;
		case 2:			// buy weapon
			item = rnd(1, 7);
			cost = int(g.WeaponCost(item, qual) * frnd(0.6, 1.2));

			text1 = "Do you want to buy a ";
			
			for (i = 0; i < len(text1); i++)
				colors1[i] = lotaCyan;

			text1 += quality[qual];
			
			// line 2
			text2 = g.WeaponName(item);
			i = len(text2);

			text2 += " for ";

			for (; i < len(text2); i++)
				colors2[i] = lotaCyan;

			text2 += String(cost);
			i = len(text2);

			text2 += " Gold?";
 			for (; i < len(text2); i++)
				colors2[i] = lotaCyan;

			qcolor = lotaCyan;

			break;
		case 3:			// buy food
			for (i = 0; i < 40; i++)
				colors1[i] = colors2[i] = lotaGreen;

			item = rnd(20, 40);
			cost = int(item * frnd(0.8, 1.2));

			text1 = "Do you want to buy ";
			i = len(text1);

			text1 += String(item);
			for (; i < 40; i++)
				colors1[i] = lotaYellow;

			// line 2
			text2 = "Days of food for ";
			i = len(text2);

			text2 += String(cost);
			for (; i < len(text2); i++)
				colors2[i] = lotaYellow;

			text2 += " gold?";

			qcolor = lotaGreen;

			break;
		case 4:			// buy hp
			for (i = 0; i < 40; i++)
				colors1[i] = colors2[i] = lotaGreen;
			

			item = rnd(20, g.player.MaxHP() / 4);

			if (item > (g.player.MaxHP() - g.player.HP()))
				item = (g.player.MaxHP() - g.player.HP());

			cost = int(item * frnd(0.75, 0.9));

			text1 = "Do you want to buy a potion worth ";

			// line 2
			text2 = String(item);
			for (i = 0; i < len(text2); i++)
				colors2[i] = lotaYellow;

			text2 += " Hit Points for ";
			i = len(text2);

			text2 += String(cost);
			for (; i < len(text2); i++)
				colors2[i] = lotaYellow;

			text2 += " gold?";

			qcolor = lotaGreen;

			break;
		case 5:			// buy museum coin
			StoreMuseumCoin();

			break;

	}

	if (type != 5)
	{
		g.AddBottom(text1, colors1);
		g.AddBottom(text2, colors2);
		g.AddBottom("");

		int choice = QuickMenu(menu, 3, 0, qcolor);

		if (choice == 0)
		{
			if (g.player.Spend(cost))
			{
				LotaPlaySound(snd_Sale);
				
				g.AddBottom("");
				g.AddBottom("Purchase Completed.");

				bool flash = false;
				unsigned int  clr1 = lotaWhite;
				unsigned int  clr2 = lotaWhite;

				switch(type)
				{
					case 1:
						g.player.AddArmor(item, qual);

						break;
					case 2:
						g.player.AddWeapon(item, qual);

						break;
					case 3:
						g.player.Food(item);
						clr2 = lotaGreen;

						break;
					case 4:
						g.player.HP(item);
						clr2 = lotaGreen;

						break;
					case 5:
						break;
				}

				unsigned int lastColor = clr1;
				while (LotaGetSoundStatus(snd_Sale) && DSBSTATUS_PLAYING)
				{
					if (lastColor == clr2)
						lastColor = clr1;
					else
						lastColor = clr2;

					g.HPColor = lastColor;
	 
					wait(40);
				}

				g.HPColor = lotaWhite;
			}
			else
			{

 				LotaPlaySound(snd_Medium);

				g.AddBottom("");
				g.AddBottom("You don't have enough gold...");
			}

		}
		else
		{
 			LotaPlaySound(snd_Medium);

			g.AddBottom("");

			if (rnd(1, 2) == 1)
				g.AddBottom("Maybe Later...");
			else
				g.AddBottom("You passed up a good deal!");

		}
	}

	encounterState = 0;
	displayMonst = -1;
}


