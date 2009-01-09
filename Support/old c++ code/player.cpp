
#include "lota.h"

extern Global g;
	
Player::Player()
{
	NewPlayer("Kanato");

	gamespeed = 1;

	outx = 102;
	outy = 122;

	lastMap = 1;
	map = 1;

	x = 27;
	y = 94;	

	food = 100;
	gold = 200;



}

void Player::NewPlayer(String newName)
{
	int i;

	name = newName;

	for (i = 0; i < 5; i++)
	{
		atr[i] = 15;
	}

	goldBank = 0;
	gamespeed = 3;
	loan = 0;
	dueDate = 0;
	
	map = 1;
	dungeon = 0;
	food = 40;
	gold = 20;

	hp = 200;
	level = 1;

	x = 27;
	y = 94;	

	//  temporary, until the museum gets implemented.
	food = 100;
	gold = 200;
	/////////////////////////

	dungeonLevel = 0;
	faceDirection = lotaWest;
	mailTown = 0;

	currentArmor = 1;
	currentWeapon = 0;	
 
	for (i = 1; i <= 5; i++)
	{
		weapon[i] = 0;
		weaponQuality[i] = 0;
	}

	armor[1] = 1;
	armorQuality[1] = 0;

	for (i = 2; i <= 3; i++)
	{
		armor[i] = 0;
		armorQuality[i] = 0;
	}

	for (i = 0; i < 30; i++)
	{
		item[i] = 0;
	}

	item[1] = 1;			// gold armband
	item[15] = 1;			// compendium
	item[17] = 2;			// jade coins
	

	for (i = 0; i < 15; i++)
	{
		museum[i] = 0;
	}

	caretaker = 0;
	lastAttacked = 0;
	vaultGold = 17;
	
	//chests[50] = 0;
	
	guardian = 0;
	ambushed = 0;
	wizardOfPotions = 0;
	casandra = 0;

	ClearRafts();
	
	onRaft = false;

	SortEquipment();
}

int Player::TimeDays(double timePassed)
{
	if (int(timedays + timePassed) == int(timedays + 1))
	{
		food--;
		
		if (food < -14)
		{
			Dead();
		}
	}

	timedays += timePassed;

	return int(timedays);

}

int Player::Terrain()
{
	if (g.map.MapType())
	{
		return g.map.Terrain(x, y);
	}
	else
		return 0;
}


void Player::Dead()
{
	int i;
	int t;

	hp = 0;

	g.AddBottom("");
	g.AddBottom("");
	g.AddBottom("            You died!");
	g.AddBottom("");
	g.AddBottom("");

	LotaPlaySound(snd_VeryBad);

	g.commandMode = cmdBad;

	for (; LotaGetSoundStatus(snd_VeryBad) & DSBSTATUS_PLAYING && !g.done ;)
	{
		g.HPColor = lotaRed;
		wait (1);

		g.HPColor = lotaYellow;
		wait (1);
	}

	g.HPColor = lotaWhite;

	map = 1;
	g.map.LoadMap(map);

	do
	{
		x = rnd(0, g.map.MapWidth() - 1);
		y = rnd(0, g.map.MapHeight() - 1);

		t = g.player.Terrain();

	} while (t != mapGrass && t != mapForest);

	for (i = 1; i < 16; i++)
	{
		raft[i].x = -10;
		raft[i].y = -10;
	}

	hp = MaxHP();
	food = 30 + rnd(0, 10);
	gold = 30 + rnd(-5, 15);
	onRaft = false;

	wait (1000);

	g.AddBottom("The powers of the museum resurrect you from the grave!");
	g.AddBottom("");

	Stormy(0);

	LotaPlaySound(snd_VeryGood);

	for (; LotaGetSoundStatus(snd_VeryBad) & DSBSTATUS_PLAYING && !g.done;);

	g.commandMode = cmdEnterCommand;


}

int Player::OnRaft() const
{
	return onRaft;
}

int Player::BoardRaft(int raftNum)
{
	if (!(raftNum < 1 || raftNum > 16 || (raft[raftNum].x == -10 && raft[raftNum].y == -10)))
	{
		onRaft = raftNum;
	}

	return onRaft;
}

void Player::Disembark(Direction dir)
{
	onRaft = 0;

	FaceDirection (dir);

	switch (dir)
	{
	case lotaEast:
		Move(1, 0);
		break;
	case lotaNorth:
		Move(0, -1);
		break;
	case lotaWest:
		Move(-1, 0);
		break;
	case lotaSouth:
		Move(0, 1);
		break;
	}

}

int	Player::Hold(int h)
{
	if (h > 0 && h < 30)
	{
		if (item[h] > 0)
		{
			hold = h;
		}
	}

	if (h == 0)
	{
		hold = h;
	}

	return hold;

}

int	Player::HoldMenu(int h)
{
	int i;

	if (h != 0)
	{
		for (i = 1; i <= h && h < 30; i++)
		{
			if (g.player.item[i] == 0)
			{
				h++;
			}

		}
	}

	if (h < 30)
	{
		hold = h;
	}

	return hold;

}

int Player::X()
{
	return x;
}

int Player::Y()
{
	return y;
}

/****************************************************************************
 *  int Player::SetPos(int x, int y)										*
 *																			*
 *  Sets the positions of the player on the current map.  Returns a nonzero	*	
 *  value if they can't move that direction, or be in that position.		*
 ****************************************************************************/
int Player::SetPos(int xx, int yy)
{
	int i, j;
	int t = 0;

	int oldx = x;
	int oldy = y;

	int test;

	x = xx;
	y = yy;

	if (g.map.MapType() == mapOutside)
	{
		test = Terrain();

		if (test == 0)
		{
			t = 1;
		}
		else if (test == 1)
		{
			t = 2;
		}
	}

	switch (g.map.MapType())
	{
	case mapTown:
	case mapCastle:
		for (i = 0; i < 101; i++)
		{
			POINT gp = g.map.GuardPos(i);

			if (gp.x != 0 && gp.y != 0)
			{
				if ((gp.x == x - 1 || gp.x == x || gp.x == x + 1) &&
					(gp.y == y - 1 || gp.y == y || gp.y == y + 1))
				{
					t = 3;
				}
			}
		}
		// no break here!!!!
	case mapOutside:
		for (j = 0; j < 2; j++)
		{
			for (i = 0; i < 2; i++)
			{
				if (g.map.MapType() == mapOutside)
				{
					if (test == 0)
					{
						test = int(g.map.M(yy + j, xx + i) / 16) * 16;

						if (test == 0)
						{
							if (!OnRaft())
								t = 1;

						}
						else if (OnRaft())
						{
							t = 50;
						}
					}

				}
				else 
				{
					int xLimit = 7;
					const int yLimit = 8;

					if (g.map.MapType() == mapCastle)
						xLimit = 8;

					//test = int(g.map.M(yy + j, xx + i) / 16) * 16;
					test = g.map.M(yy + j, xx + i);

					if (test >= 16 * yLimit || test % 16 >= xLimit)
					{
						t = 3;
					}
				}

			}
		}

		break;

	case mapDungeon:
	case mapMuseum:
		if (g.map.M(yy, xx) >= 0x80)
		{
			t = 3;
		}
		else t = 0;

		break;
	}

	if (t == 1)
	{
		for (i = 1; i < 16; i++)
		{
			if (abs(raft[i].x - x) < 2 && abs(raft[i].y - y) < 2)
			{
				t = 0;
			}

			if (raft[i].x == x && raft[i].y == y && OnRaft() == 0)
			{
				BoardRaft(i);				
			}

		}

	}


	if (t == 2 && Hold() == 2)
	{
		t = 0;
	}

	if (t > 0)
	{
		x = oldx;
		y = oldy;
		return t;
	}

	x = xx;
	y = yy;

	if (g.map.MapType() != mapOutside && 
		(x < 0 || x + 1 >= g.map.MapWidth() || y < 0 || y + 1 >= g.map.MapHeight()) 
		&& g.map.MapType() != mapDungeon && g.map.MapType() != mapMuseum)
	{

		if (g.map.IsAngry() && g.map.MapType() == mapTown)
		{
			lastAttacked = map;
		}

		g.allowEnter = false;

		String temp = "Leave ";
		temp += g.map.Name();

		g.AddBottom("");
		g.AddBottom(temp);

		g.AddBottom("");

		g.commandMode = cmdBad;

		wait(2000);
		
		g.map.LoadMap(lastMap);
		NewMap();

		g.AddBottom("");

		g.commandMode = cmdPrompt;


	}

	if (OnRaft())
	{
		raft[OnRaft()].x = xx;
		raft[OnRaft()].y = yy;

		if (g.player.X() < -45 || g.player.X() > g.map.MapWidth() + 45 ||
			g.player.Y() < -45 || g.player.Y() > g.map.MapHeight() + 45)
		{
			Stormy(3);
		}
		else if (g.player.X() < -30 || g.player.X() > g.map.MapWidth() + 30 ||
			g.player.Y() < -30 || g.player.Y() > g.map.MapHeight() + 30)
		{
			Stormy(2);
		}
		else if (g.player.X() < -15 || g.player.X() > g.map.MapWidth() + 15 ||
			g.player.Y() < -15 || g.player.Y() > g.map.MapHeight() + 15)
		{
			Stormy(1);
		}
		else
		{
			Stormy(0);
		}


	}
	return 0;
}

int Player::Move(int dx, int dy)
{
	int i;

	if (dx != 0 || dy != 0)
	{
		if (g.map.CheckEncounter(x, y, dx, dy))
		{
			i = SetPos(x + dx, y + dy);
			g.map.CheckRoof(x, y);

			return i;
		}
		else
		{
			return 47;
		}
	}
	return 0;
}

void Player::NewMap(int xx, int yy)
{
	CmdMode sav;

	StopAllSounds();

	map = g.map.MapNumber();

	if (g.map.MapType() == mapOutside && outmap == map)
	{
		x = outx;
		y = outy;

		dungeonLevel = 0;

		g.map.IsAngry(false);
	}
	else
	{

		// store outside map coordinate
		outx = x;
		outy = y;

		// set new coordinate
		x = xx;
		y = yy;

		if (map == lastAttacked)
		{
			g.map.IsAngry(true);

			g.ClearBottom();
			g.AddBottom("We remember you - slime!");
			g.AddBottom("");
			g.AddBottom("");
			g.AddBottom("");

			sav = g.commandMode;
			g.commandMode = cmdBad;

			wait(2000);

			g.commandMode = sav;
		}
		else if (g.map.MapType() == mapTown)
		{
			lastAttacked = 0;
		}

	}
	
	if (g.map.MapType() == mapDungeon)
	{
		faceDirection = lotaEast;
		dungeonLevel = 1;
	}
	else
	{
		faceDirection = lotaNorth;
	}


}

int	Player::ArmorType(int i)
{
	if (i <= 0 || i > 3)
		return 0;

	return armor[i];

}

int	Player::WeaponType(int i)
{
	if (i <= 0 || i > 5)
		return 0;

	return weapon[i];

}

int	Player::ArmorQuality(int i)
{
	if (i <= 0 || i > 3)
		return 0;

	return armorQuality[i];

}

int	Player::WeaponQuality(int i)
{
	if (i <= 0 || i > 5)
		return 0;

	return weaponQuality[i];

}

int	Player::CurrentArmor(int i)
{
	if (i > -1)
	{
		if (i == 0)
		{
			currentArmor = i;
		}
		else
		{
			for (int j = 1; j <= i; j++)
			{
				if (armor[j] == 0)
					i++;
			} 

			currentArmor = i;
		}


	}
	return currentArmor;
}

int	Player::CurrentWeapon(int i)
{
	if (i > -1)
	{

		if (i == 0)
		{
			currentWeapon = i;
		}
		else
		{
			currentWeapon = 0;

			for (int j = 1; j <= i; j++)
			{
				if (weapon[j] == 0)
					i++;
			} 

			currentWeapon = i;
		}

	}

	return currentWeapon;

}

int Player::DungeonLevel(int i)
{
	if (i >= 0 && i <= 8)
	{
		dungeonLevel = i;
	}

	return dungeonLevel;

}

int Player::Item(int i)
{

	return item[i];

}

String Player::Name()
{
	return name;
}

int Player::Level(int i)
{
	if (i == 1)
	{
		level++;
	}

	return level;
}

int	Player::Attribute(int attr, int change)
{

	atr[attr] += change;

	return atr[attr];

}

Direction Player::FaceDirection(Direction dir)
{
	if (dir >= lotaEast && dir <= lotaSouth)
	{
		faceDirection = dir;
	}

	return faceDirection;
}

/****************************************************************************
 *  int Global::Gamespeed(int g)											*
 *																			*
 *  This function sets or returns the current gamespeed, and updates the	*
 *	timer values.															*	
 ****************************************************************************/
int Player::Gamespeed(int gs)
{
	if (gs > 0 && gs < 6)
	{
		gamespeed = gs;	
		g.ResetTimers();

	}


	return gamespeed;
}

int	Player::HP(int change)
{
	hp += change;

	if (hp > MaxHP())
		hp = MaxHP();

	if (hp < 0)
	{
		hp = 0;
		Dead();
	}

	return hp;

}

int	Player::Food(int change)
{
	if (food < 0 && change > 0)
		food = 0;

	food += change;

	if (food < 0)
		return 0;

	return food;

}

int	Player::Gold() const
{
	return gold;
}

int	Player::Spend(int amount)
{
	if (amount <= gold)
	{
		gold -= amount;
		return true;
	}

	return false;
}

void Player::GainGold(int amount)
{
	gold += amount;
}


int Player::MaxHP() const
{
	return 200 * level;
}

POINT Player::Raft(int r)
{
	POINT loc;

	if (r > 0 && r < 32)
	{
		loc.x = raft[r].x;
		loc.y = raft[r].y;
	}

	return loc;
}

int Player::RaftMap(int r)
{
	
	if (r > 0 && r < 32)
	{
		return raftMap[r];
	}
	else
	{
		return 0;
	}

}

void Player::AddRaft(int map, int x, int y)
{
	for (int i = 1; i < 31; i++)
	{
		if (raftMap[i] == 0)
		{
			break;
		}
	}

	raftMap[i] = map;
	raft[i].x = x;
	raft[i].y = y;
	
}

void Player::ClearRafts()
{

	for (int i = 0; i < 32; i++)
	{
		raft[i].x = -10;
		raft[i].y = -10;
		raftMap[i] = 0;
	}
	
}

int Player::Stormy(int s)
{
	if (s >= 0)
	{
		stormy = s;
	}

	return stormy;
}

int	Player::LastAttacked(int newAtt)
{
	lastAttacked = newAtt;

	return lastAttacked;

} 
int	Player::Hit(int defense)
{
	int wt = WeaponType(CurrentWeapon());
	int qt = WeaponQuality(CurrentWeapon());
	int hit;

	int dam = Attribute(strength) - 12;
 	dam += int( wt * (qt + 2) ) / 2;

	dam = int(dam * rnd(30, 150) / 100.0 + 0.5);
	dam += rnd(-2, 2);

	if (dam < 3)
		dam = rnd(1, 3);

	hit = Attribute(dexterity) - int(defense * 0.3);
	hit += qt;

	if (hit > 24)
		hit = 24;
	else if (hit < 4)
		hit = 4;

	hit -= rnd(0, 25);

	if (rnd(0, 99) < 4)
		hit -= 10000;

	if (hit < 0 || rnd(0, 99) < 2)
	{
		dam = 0;
	}

	OutputDebugString("Hit: " + String(hit) + " Dam: " + String(dam) + "\n");

	return dam;
}
int	Player::Damage(int attack)
{
	int dam = int(attack - (Attribute(endurance) + ArmorType(CurrentArmor()) * 4) * 0.8);

	dam += int(dam * rnd(-50, 100) / 100 + 0.5);

	if (dam < 0 || rnd(1, 60) + attack / 15 < Attribute(dexterity) + ArmorQuality(CurrentArmor()))
	{
		dam = 0;
	}

	HP(-dam);

	return dam;

}

int Player::ItemCount(int itm, int a)
{
	item[itm] += a;

	if (item[itm] <= 0)
	{
		item[itm] = 0;

		if (hold == itm)
		{
			hold = 0;
		}

	}

	return item[itm];

}

int Player::Map(int m)
{
	if (m > -1)
	{
		if (g.map.MapType() == mapOutside)
			outmap = g.map.MapNumber();

		lastMap = map;

		map = m;
	
		g.map.LoadMap(map);

	}

	return map;
}

int Player::GoldInBank(int a)
{	
	goldBank += a;

	if (goldBank < 0)
		goldBank = 0;
	
	return goldBank;
}

int Player::AddWeapon(int w, int q)
{
	int i;

	for (i = 1; i <= 5; i++)
	{
		if (WeaponType(i) == 0)
		{
			weapon[i] = w;
			weaponQuality[i] = q;

			SortEquipment();

			return true;
		}
	}

	return false;
}

int Player::RemoveWeapon(int w)
{
	return false;
}
int Player::AddArmor(int a, int q)
{
	int i;

	for (i = 1; i <= 3; i++)
	{
		if (ArmorType(i) == 0)
		{
			armor[i] = a;
			armorQuality[i] = q;

			SortEquipment();

			return true;
		}
	}

	return false;
}

int Player::RemoveArmor(int a)
{
	return false;
}

void Player::SortEquipment()
{
	int i = 0;
	int tempItem;
	int tempQuality;

	do
	{

		i++;

		if (weapon[i + 1] < weapon[i])
		{
			tempItem = weapon[i];
			tempQuality = weaponQuality[i];

			weapon[i] = weapon[i + 1];
			weaponQuality[i] = weaponQuality[i + 1];

			weapon[i + 1] = tempItem;
			weaponQuality[i + 1] = tempQuality;


			if (currentWeapon == i)
			{
				currentWeapon = i + 1;
			}
			else if (currentWeapon == i + 1)
			{
				currentWeapon = i;
			}

			i = 0;

		}
		else if (weapon[i + 1] == weapon[i] && weaponQuality[i + 1] < weaponQuality[i])
		{

			tempItem = weapon[i];
			tempQuality = weaponQuality[i];

			weapon[i] = weapon[i + 1];
			weaponQuality[i] = weaponQuality[i + 1];

			weapon[i + 1] = tempItem;
			weaponQuality[i + 1] = tempQuality;


			if (currentWeapon == i)
			{
				currentWeapon = i + 1;
			}
			else if (currentWeapon == i + 1)
			{
				currentWeapon = i;
			}

			i = 0;
		}

	} while (i < 4);

	i = 0;

	do
	{

		i++;

		if (armor[i + 1] < armor[i])
		{
			tempItem = armor[i];
			tempQuality = armorQuality[i];

			armor[i] = armor[i + 1];
			armorQuality[i] = armorQuality[i + 1];

			armor[i + 1] = tempItem;
			armorQuality[i + 1] = tempQuality;


			if (currentArmor == i)
			{
				currentArmor = i + 1;
			}
			else if (currentArmor == i + 1)
			{
				currentArmor = i;
			}

			i = 0;

		}
		else if (armor[i + 1] == armor[i] && armorQuality[i + 1] < armorQuality[i])
		{
			tempItem = armor[i];
			tempQuality = armorQuality[i];

			armor[i] = armor[i + 1];
			armorQuality[i] = armorQuality[i + 1];

			armor[i + 1] = tempItem;
			armorQuality[i + 1] = tempQuality;


			if (currentArmor == i)
			{
				currentArmor = i + 1;
			}
			else if (currentArmor == i + 1)
			{
				currentArmor = i;
			}

			i = 0;
		}

	} while (i < 2);
}

int Player::LastMap()
{
	return lastMap;
}

int Player::VaultGold(int setGold)
{
	if (setGold > 0 && setGold < 20)
		vaultGold = setGold;

	return vaultGold;

}


bool Player::SaveGame()
{
	if (g.commandMode != cmdPrompt && g.commandMode != cmdEnterCommand)
		return false;

	if (g.Menu('e') == "End")
	{
		const int ver = 2;
		char buffer[256];
		int count = 0;
//		HRESULT hr;
		DWORD bytes;
		DWORD err = GetLastError();

		/*DeleteFile(name + ".chr");
		err = GetLastError();
		*/

		HANDLE hFile = CreateFileA(name + ".chr", GENERIC_WRITE, 0, NULL,
			CREATE_ALWAYS, FILE_FLAG_SEQUENTIAL_SCAN | FILE_FLAG_WRITE_THROUGH, NULL);

		err = GetLastError();

		if (hFile == INVALID_HANDLE_VALUE)
		{

			hFile = NULL;
			
			hFile = CreateFileA(name + ".chr", GENERIC_WRITE, 0, NULL,
				OPEN_EXISTING, FILE_FLAG_SEQUENTIAL_SCAN | FILE_FLAG_WRITE_THROUGH, NULL);


			err = GetLastError();

		}

		if (hFile == INVALID_HANDLE_VALUE)
		{

			g.AddBottom("");
			g.AddBottom("Failed to save file.", lotaYellow);
			g.AddBottom("Error code: " + String((signed)err), lotaYellow);
 			g.AddBottom("");

			switch(err)
			{
				case 183:
					g.AddBottom("Cannot create a file when ");
					g.AddBottom("that file already exists. ");
					break;

			}

			LotaPlaySound(snd_Invalid);

			CmdMode cmd = g.commandMode;

			g.commandMode = cmdBad;
			wait(3000);
			g.commandMode = cmd;

			g.AddBottom("");
			g.AddBottom("Enter Command: ");

			CloseHandle(hFile);
			return false;

		}

		WriteFile(hFile, &ver, 4, &bytes, NULL);		// write the version to the file
		count += bytes;									// 4

		err = GetLastError();

		memset(buffer, 0, 255);
		memcpy(buffer, (char*) Name(), len(Name()));

		WriteFile(hFile, buffer, 60, &bytes, NULL);
		count += bytes;									// 64

		memset(buffer, 0, 255);
		WriteFile(hFile, buffer, 192, &bytes, NULL);
		count += bytes;									// 256

		WriteFile(hFile, this, sizeof(*this) - sizeof(name), &bytes, NULL);
		count += bytes;									// 1260

		err = GetLastError();

		//if (err)
		//{

		//	g.AddBottom("");
		//	g.AddBottom("Failed to save file.", lotaYellow);
		//	g.AddBottom("Error code: " + String((signed)err), lotaYellow);
 	//		g.AddBottom("");

		//	switch(err)
		//	{
		//		case 183:
		//			g.AddBottom("Cannot create a file when ");
		//			g.AddBottom("that file already exists. ");
		//			break;

		//	}

		//	LotaPlaySound(snd_Invalid);

		//	CmdMode cmd = g.commandMode;

		//	g.commandMode = cmdBad;
		//	wait(3000);
		//	g.commandMode = cmd;

		//	g.AddBottom("");
		//	g.AddBottom("Enter Command: ");

		//	CloseHandle(hFile);
		//	return false;

		//}

		SetEndOfFile(hFile);
		CloseHandle(hFile);

	
		g.AddBottom("");
		g.AddBottom("Game saved successfully.", lotaGreen);

		g.commandMode = cmdBad;
		wait(2000);
		g.commandMode = cmdPrompt;
		
	
		return true;

	}
	else
	{
		g.AddBottom("");
		g.AddBottom("Cannot save now.");
		g.AddBottom("");

		LotaPlaySound(snd_Invalid);

		CmdMode cmd = g.commandMode;

		g.commandMode = cmdBad;
		wait(750);
		g.commandMode = cmd;

		g.AddBottom("Enter Command: ");

		return false;

	}
}

bool Player::LoadGame(String nName)
{
	if (nName == "")
		return false;

	HANDLE hFile = CreateFile(nName, GENERIC_READ, 0, NULL,
		OPEN_EXISTING, FILE_FLAG_SEQUENTIAL_SCAN, NULL);

	int ver;
	char buffer[256];
	int count = 0;
	bool failed = false;
	DWORD bytes;
	DWORD err = GetLastError();

	if (err)
		failed = true;

	ReadFile(hFile, &ver, 4, &bytes, NULL);		// write the version to the file
	count += bytes;									// 4

	if (count != 4)
	{
		failed = true;
	}
	else
	{
		switch (ver)
		{
			case 1:

				failed = true;

				break;

			case 2:

				memset(buffer, 0, 255);
				ReadFile(hFile, buffer, 60, &bytes, NULL);
				count += bytes;

				if (count != 64)
				{
					failed = true;
					break;
				}
				
				name = buffer;


				ReadFile(hFile, buffer, 192, &bytes, NULL);
				count += bytes;									// 256

				if (count != 256)
				{
					failed = true;
					break;
				}

				ReadFile(hFile, this, sizeof(*this) - sizeof(name), &bytes, NULL);
				count += bytes;									// 1252
				
				if (count != 1252)
				{
					failed = true;
					break;
				}

				break;

			default:
				failed = true;

				break;

		}
	}


	CloseHandle(hFile);

	if (failed)
	{
		g.AddBottom("");
		g.AddBottom("Could not read file.");
		g.AddBottom("");

		LotaPlaySound(snd_Invalid);

		CmdMode cmd = g.commandMode;

		g.commandMode = cmdBad;
		wait(750);
		g.commandMode = cmd;
	}
	else
	{

		g.map.LoadMap(Map());

		g.invisible = false;
		g.guard = false;

		g.ClearBottom();

		g.AddBottom("");
		g.AddBottom("Game loaded successfully.", lotaGreen);

		g.commandMode = cmdBad;
		
		wait(2000);

		g.commandMode = cmdPrompt;
		
		return true;
	}


	return false;

}