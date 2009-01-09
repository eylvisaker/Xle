
#include "lota.h"

extern Global g;

Player::Player()
{
	NewPlayer("Kanato");

	food = 1000;
	gold = 123456;

	for (int i = 0; i < 30; i++)
	{
		item[i] = 5;
	}

	raft[1].x = 43;
	raft[1].y = 49;

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
	
	map = 1;
	dungeon = 0;
	food = 40;
	gold = 20;

	hp = 200;
	level = 1;

	x = 50;
	y = 50;

	dungeonLevel = 0;
	faceDirection = lotaWest;

	currentArmor = 1;
	currentWeapon = 0;	
 
	for (i = 1; i <= 5; i++)
	{
		weapon[i] = 0;
		weaponQuality[i] = 0;
	}

	armor[1] = 9;
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

	item[1] = 1;
	item[15] = 1;
	item[17] = 2;

	for (i = 0; i < 15; i++)
	{
		museum[i] = 0;
	}

	caretaker = 0;
	lastAttacked = 0;
	
	//chests[50] = 0;
	
	guardian = 0;
	ambushed = 0;
	wizardOfPotions = 0;
	cassandra = 0;

	for (i = 0; i < 16; i++)
	{
		raft[i].x = -10;
		raft[i].y = -10;
	}
	onRaft = false;

}

int Player::TimeDays(double timePassed)
{
	if (int(timedays + timePassed) == int(timedays + 1))
	{
		food--;
		
		if (food < 14)
		{
			Dead();
		}
	}

	timedays += timePassed;

	return int(timedays);

}

int Player::Terrain()
{
	int		t[2][2] = {0,0,0,0};
	int		tc[8] = {0,0,0,0,0,0,0,0};
	int		i;

	for (int j = 0; j < 2; j++)
	{
		for ( i = 0; i < 2; i++)
		{
			t[j][i] = g.map.M(y + j, x + i) / 16;
		}
	}

	for (j = 0; j < 2; j++)
	{
		for (i = 0; i < 2; i++)
		{
			tc[t[j][i] / 2]++;
		}
	}

	if (tc[mapMountain] > 1)
	{
		return mapMountain;
	}	
	
	if (tc[mapSwamp] > 0 && tc[mapGrass] + tc[mapForest] == 4 - tc[mapSwamp])
	{
		return mapSwamp;
	}

	if (tc[mapDesert] > 0 && tc[mapGrass] + tc[mapForest] + tc[mapSwamp] == 4 - tc[mapDesert])
	{
		return mapDesert;
	}

	if (tc[mapMountain] == 1)
	{
		return mapFoothills;
	}

	for (i = 0; i < 8; i++)
	{
		if (tc[i] == 3 || tc[i] == 4)
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

void Player::Dead()
{

}
