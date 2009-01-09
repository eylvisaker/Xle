
#ifndef __PLAYERH__
#define __PLAYERH__

#include "lota.h"

class Player
{
public:
	String		name;
	int			atr[5];
	int			food;
	int			gold;
	int			goldBank;
	int			gamespeed;
	int			map;
	int			dungeon;
	int			hp;
	short		level;
	int			x, y;
	int			dungeonLevel;
	int			faceDirection;
	int			currentArmor;
	int			currentWeapon;
	int			weapon[6];
	int			armor[4];
	int			weaponQuality[6];
	int			armorQuality[4];
	int			item[30];
	int			hold;
	int			museum[15];
	int			caretaker;
	int			lastAttacked;
	int			chests[50];
	
	int			guardian;
	int			ambushed;
	int			wizardOfPotions;
	int			cassandra;
	POINT		raft[16];
	int 		onRaft;

public:
	Player();

	void		NewPlayer(String newName = "");		// reinitialize all the variables
	void		OneDay();				// one day has passed
	void		Dead();					// the player has died
	int			TimeDays(double time = 0);		// returns the time in days
	int			Terrain();				// the current outside terrain the player is standing on


private:
	double		timedays;

};



#endif