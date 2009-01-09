
#ifndef __PLAYERH__
#define __PLAYERH__

#include "lota.h"

enum Attributes
{
	dexterity = 0,
	strength,
	charm,
	endurance,
	intelligence
};

class Player
{
	public:
		Player();

		//		Character
		void		NewPlayer(String newName = "");		// reinitialize all the variables
		void		Dead();					// the player has died
		String		Name();					// returns the name of the character

		bool		SaveGame();				// saves the game
		bool		LoadGame(String game);  // loads a game

		int			Level(int = 0);			// returns current level of character; 1 increases level
		int			Attribute(int atr, int change = 0);	// returns or changes the specified attribute
		int			Gamespeed(int = 0);		// sets or returns the gamespeed
		int			HP(int change = 0);		// returns or adjusts current HP
		int			MaxHP() const;			// calculates max hp
		int			Food(int change = 0);	// returns or adjusts food
		int			Gold() const;			// returns current gold
		int			Spend(int amount);		// adjusts current gold if there's enough available and returns true if there is
		int			GoldInBank(int a = 0);		// increases or decreases gold in bank or returns current value
		void		GainGold(int amount);	// gains gold

		//		Time
		void		OneDay();				// one day has passed
		int			TimeDays(double time = 0);		// returns the time in days or increments them

		//		Map
		int			Terrain();				// the current outside terrain the player is standing on
		int			X();					// returns x position
		int			Y();					// returns y position
		int			SetPos(int px, int py);	// sets the position on the map for the PC
		int			Move(int dx, int dy);	// moves the PC in the specified direction
		void		NewMap(int xx = -1, int yy = -1);	// sets coordinates for a new map & loads coordinates for outside
		int			DungeonLevel(int i = -1);	// sets or returns the current dungeon level for the player
		Direction	FaceDirection(Direction dir = lotaNone);	// sets or returns the direction the player is facing
		int			LastAttacked(int newAtt); // stores the town number that was last attacked
		int			Map(int m = -1);		// sets or returns the current map
		int			LastMap();				// returns the last map the player was on
		int			VaultGold(int = -1);	// sets or returns the current gold in the vault

		//		Raft
		int			OnRaft() const;			// returns the raft that the player is on (or 0 if not)
		int			BoardRaft(int);			// boards a raft
		void		Disembark(Direction dir);		// disembarks a raft
		POINT		Raft(int r);			// returns the coordinates of a given raft
		int			RaftMap(int r);			// returns the map the given raft is on
		int			Stormy(int = -1);		// sets or returns whether or not the player is in stormy water
		void		AddRaft(int map, int x, int y);  // adds a raft to the map
		void		ClearRafts();			// Clears all rafts from all maps

		//		Items
		int			Hold(int h = -1);		// sets or returns the item that is currently being held
		int			HoldMenu(int h);		// sets the item that is currently being held (menu selection)
		int			ArmorType(int);			// returns the armor that the player is carrying in the specified slot
		int			WeaponType(int);		// returns the weapon that the player is carrying in the specified slot
		int			ArmorQuality(int);		// returns the armor quality
		int			WeaponQuality(int);		// returns the weapon quality
		int			CurrentArmor(int = -1);	// sets or returns the armor currently worn
		int			CurrentWeapon(int = -1);// sets or returns the armor currently equiped
		int			Item(int);				// returns the number of the specified items
		int			ItemCount(int itm, int inc);	// Adjusts the number of items the player has
		int			AddWeapon(int w, int q);	// Adds a weapon to inventory
		int			AddArmor(int a, int q);		// Adds a weapon to inventory
		int			RemoveWeapon(int w);		// Removes a weapon to inventory
		int			RemoveArmor(int a);			// Removes a weapon to inventory
		void		SortEquipment();			// sorts weapons and armor

		//		Combat
		int			Hit(int defense);		// player damages a creature returns damage
		int			Damage(int attack);		// player gets hit

	private:

		int			atr[5];
		int			food;
		int			gold;
		int			goldBank;
		double		timedays;

		int			stormy;
		int 		onRaft;

		int			gamespeed;
		int			map;
		int			lastMap;
		int			dungeon;
		int			hp;
		short		level;
		int			outx, outy, outmap;
		int			x, y;
		int			dungeonLevel;
		Direction	faceDirection;

		int			currentArmor;
		int			currentWeapon;
		int			weapon[6];
		int			armor[4];
		int			weaponQuality[6];
		int			armorQuality[4];
		int			item[30];
		int			hold;

		int			lastAttacked;
		int			vaultGold;

		int			chests[50];

		POINT		raft[32];
		int			raftMap[32];

	public:

		int			loan;					// loan amount
		int			dueDate;				// time in days that the money is due

		int			guardian;
		int			ambushed;
		int			wizardOfPotions;
		int			casandra;

		int			museum[15];
		int			caretaker;

		int			mailTown;

	private:
		String		name;

};



#endif