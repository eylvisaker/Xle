
#ifndef __townh__
#define __townh__

#include "lota.h"

enum StoreType
{
	storeBank = 2,					// 2
	storeWeapon,					// 3
	storeArmor,						// 4
	storeWeaponTraining,			// 5
	storeArmorTraining,				// 6
	storeBlackjack,					// 7
	storeLending,					// 8
	storeRaft,						// 9
	storeHealer,					// 10
	storeJail,						// 11
	storeFortune,					// 12
	storeFlipFlop,					// 13
	storeBuyback,					// 14
	storeFood,						// 15
	storeVault,						// 16
	storeMagic						// 17
};

class StoreClass 
{
public:
	SpecialEvent	dave;
	String			name;
	double			costFactor;
	bool			robbing;
};

void Store(SpecialEvent dave, bool rob = false);			// processes stores

void DrawStore( LPDIRECTDRAWSURFACE7 pDDS );
void SetColors(UINT back, UINT border, UINT line, UINT font, UINT title);
void ClearTheWindow();
void SetStore();
void StoreDone(); 
void StoreSound(int snd);

void StoreBank(StoreClass &shop);
void StoreRaft (StoreClass &shop);
void StoreFortune (StoreClass &shop);
void StoreFood (StoreClass &shop);
void StoreWeapons (StoreClass &shop);
void StoreArmor (StoreClass &shop);
void StoreBuyback (StoreClass &shop);
void StoreBlackjack (StoreClass &shop);
void StoreLending (StoreClass &shop);
void StoreVault (StoreClass &shop);
void StoreMagic (StoreClass &shop);
void StoreHealer (StoreClass &shop);
void StoreTraining (StoreClass &shop, bool weapon);

void StoreMuseumCoin ();

void StdRob (StoreClass &shop, int amount);

#endif // __townh__