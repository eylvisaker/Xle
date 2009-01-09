
#include "lota.h"

extern Global g;

unsigned int	storeBackColor,
				storeBorderColor,
				storeLineColor,
				storeFontColor,
				storeTitleColor;

bool			resetColors;
String			theWindow[18];
unsigned int	theWindowColor[18];
StoreClass		*pShop;

void Store(SpecialEvent dave, bool rob)
{
	StoreClass shop;
	String tempString, name;
	int i;
	bool overDue = false;

	resetColors = false;

	for (i = 0; i < lstrlen((LPCSTR)dave.data) && dave.data[i] != '\\'; i++)
	{
		name += (char)dave.data[i];
	}

	shop.costFactor = dave.data[i + 2] * 256 + dave.data[i + 3];
	shop.name = name;
	shop.dave = dave;
	shop.robbing = rob;

	pShop = &shop;

	g.commandMode = cmdBad;

	if (g.player.loan > 0 && g.player.dueDate - g.player.TimeDays() <= 0)
		overDue = true;

	if (rob)					// we're allowed to rob shops even when our loan is overdue
	{
		overDue = false;

	}


	switch (dave.type)
	{
		case storeBank:
			StoreBank(shop);
			overDue = false;

			break;
		case storeRaft:
			if (!overDue)
				StoreRaft (shop);

			break;
		case storeFortune:
			if (!overDue)
				StoreFortune (shop);

			break;
		case storeFood:
			if (!overDue)
				StoreFood (shop);

			break;
		case storeWeapon:
			if (!overDue)
				StoreWeapons (shop);

			break;
		case storeArmor:
			if (!overDue)
				StoreArmor (shop);

			break;
		case storeLending:
			StoreLending (shop);
			overDue = false;

			break;

		case storeVault:
			StoreVault (shop);

			break;

		case storeHealer:
			if (!overDue)
				StoreHealer (shop);

			break;

		case storeMagic:
			if (!overDue)
				StoreMagic (shop);

			break;

		case storeWeaponTraining:
			if (!overDue)
				StoreTraining (shop, true);

			break;

		case storeArmorTraining:
			if (!overDue)
				StoreTraining (shop, false);

			break;

		case storeBuyback:
			StoreBuyback (shop);
			overDue = false;

			
		default:
			g.AddBottom (shop.name, lotaYellow);
			g.AddBottom ("");
			g.AddBottom ("A Sign Says, ");
			g.AddBottom ("Closed for remodelling.");
			
			LotaPlaySound (snd_Medium);
			overDue = false;

			g.AddBottom("");
			g.waitCommand = 500;
			
			wait(1000);

			break;
	}

	g.commandMode = cmdEnterCommand;

	if (overDue && shop.robbing == false)
	{
		g.AddBottom("");
		g.AddBottom("Sorry.  I Can't talk to you.");
		wait(500);
	}
	else if (rnd(0, 99) < 3 && shop.robbing == false)
		StoreMuseumCoin();

	pShop = NULL;
}

void SetColors(UINT back, UINT border, UINT line, UINT font, UINT title)
{
	storeBackColor = back;
	storeBorderColor = border;
	storeLineColor = line;
	storeFontColor = font;
	storeTitleColor = title;

	if (resetColors)
	{
		for (int i = 0; i < 18; i++)
		{
			theWindowColor[i] = storeFontColor;
		}

		resetColors = false;
	}

}

void DrawStore( LPDIRECTDRAWSURFACE7 pDDS )
{
	String tempString;

	// Clear the back buffer
	DDPutBox (pDDS, 0, 0, myWindowWidth, myWindowHeight, storeBackColor);
	DDPutBox (pDDS, 0, 296, myWindowWidth, 400 - 296, lotaBlack);

	// Draw the borders
	DrawBorder(pDDS, storeBorderColor);
	DrawLine(pDDS, 0, 288, 1, myWindowWidth, storeBorderColor);

	DrawInnerBorder (pDDS, storeLineColor);
	DrawInnerLine (pDDS, 0, 288, 1, myWindowWidth, storeLineColor);

	// Draw the title
	DDPutBox (pDDS, 320 - int((len(theWindow[0]) + 2) / 2) * 16, 0, 
			  (len(theWindow[0]) + 2) * 16, 16, storeBackColor);
	WriteText (pDDS, 320 - int(len(theWindow[0]) / 2) * 16, 0, theWindow[0], storeTitleColor);

	for (int i = 1; i < 18; i++)
	{
		WriteText (pDDS, 320 - int(len(theWindow[i]) / 2) * 16, i * 16, theWindow[i], theWindowColor[i]);	
	}

	if (!pShop->robbing)
	{
		// Draw Gold
		tempString = " Gold: ";
		tempString += g.player.Gold();
		tempString += " ";
	}
	else
	{
		// don't need gold if we're robbing it!
		tempString = " Robbery in progress ";
	}
	
	DDPutBox (pDDS, 320 - int(len(tempString) / 2) * 16, i * 16, 
			  len(tempString) * 16, 16, storeBackColor);
	WriteText (pDDS, 320 - int(len(tempString) / 2) * 16, i * 16, tempString, lotaWhite);	

	DrawBottomText (pDDS);

}

void ClearTheWindow()
{
	for (int i = 0; i < 18; i++)
	{
		theWindow[i] = "";
	}
}

void SetStore()
{

	g.stdDisplay = 'S';
	ClearTheWindow();

	resetColors = true;
	
}

void StoreSound(int snd)
{
	if (snd)
	{
		LotaPlaySound(snd);
		wait(2000);

	}

}

void StoreDone()
{

	g.stdDisplay = 0;

}

void StoreRaft(StoreClass &shop)
{
	String tempString;
	int choice;
	int raftCost = int(2.20 * shop.costFactor);
	int gearCost = int(0.28 * shop.costFactor);
	MenuItemList theList(2, "Yes", "No");

	if (shop.robbing)
	{
		StdRob(shop, 0);
		return;
	}

	tempString = String("** ") + shop.name + String(" **");

	g.AddBottom(tempString, lotaYellow);
	g.AddBottom("");

	tempString = "Want to buy a raft for " + String(raftCost) + " gold?";
	
	g.AddBottom(tempString);

	choice = QuickMenu(theList, 3, 1);

	if (choice == 0)
	{
		// Purchase raft
		if (g.player.Spend(raftCost))
		{
			g.player.AddRaft(g.map.BuyRaftMap(), g.map.BuyRaftX(), g.map.BuyRaftY());

			g.AddBottom("Raft purchased.");
			StoreSound(snd_Sale);

		}
		else
		{
			g.AddBottom("Not enough gold.");
			StoreSound(snd_Medium);

		}
	}
	else if (choice == 1)
	{
		g.AddBottom("How about some climbing gear");
		tempString = "for " + String(gearCost) + " gold?";
		
		g.AddBottom(tempString);
		g.AddBottom("");

		choice = QuickMenu(theList, 3, 1);

		if (choice == 0)
		{
			if (g.player.Spend(gearCost))
			{

				g.AddBottom("Climbing gear purchased.");
				
				g.player.ItemCount(2, 1);

				StoreSound(snd_Sale);

			}
			else
			{
				g.AddBottom("Not enough gold.");
				StoreSound(snd_Medium);

			}
		}
		else if (choice == 1)
		{
			g.AddBottom("");
			g.AddBottom("Nothing Purchased.");

			StoreSound(snd_Medium);

		}

	}
}
	
void StoreFortune (StoreClass &shop)
{
	String tempString;
	MenuItemList theList (2, "Yes", "No");
	int choice;

	if (shop.robbing)
	{
		StdRob(shop, 0);
		return;
	}

	g.AddBottom(shop.name, lotaGreen);
	g.AddBottom("");

	tempString = "Read your fortune for " + String(int(6 * shop.costFactor)) + " gold?";

	g.AddBottom(tempString);

	choice = QuickMenu(theList, 3, 1);

	if (choice == 0)
	{


	}


}

void StoreFood (StoreClass &shop)
{
	String tempString;
	int i = 0;
	double cost = 15 / g.player.Attribute(charm) * 1;
	int choice;
	int max = int(g.player.Gold() / cost);

	SetStore();
	SetColors(lotaDkGray, lotaGreen, lotaYellow, lotaYellow, lotaWhite);

	theWindow[i++] = String(" ") + shop.name + String(" ");

	wait(1);

	if (!shop.robbing && g.player.mailTown == g.map.MapNumber())
	{
		int gold = rnd(1, 3);

		switch (gold)
		{
			case 1: gold = 95; break;
			case 2: gold = 110; break;
			case 3: gold = 125; break;
		}

		g.AddBottom("");
		g.AddBottom("Thanks for the delivery. ");
		g.AddBottom("Here's "+ String(gold) + " gold.");
		g.AddBottom("");
		g.AddBottom("");

		StoreSound(snd_Good);
		g.UpdateBottom("        Press Key to Continue");
		WaitKey();

		g.player.GainGold(gold);
		g.player.ItemCount(9, -1);
		g.player.mailTown = 0;
		g.map.returnMail = true;

	}
	else if (!shop.robbing)
	{
		// I think this is probably a bug in the original game, but the
		// text for food shops did not show up when you were delivering
		// mail.
		//
		theWindow[i++] = "";
		theWindow[i++] = "";
		theWindow[i++] = "";
		theWindow[i++] = "Food & water";
		theWindow[i++] = "";
		theWindow[i++] = "";
		theWindow[i++] = "We sell food for travel.";
		theWindow[i++] = "Each 'day' of food will ";
		theWindow[i++] = "keep you fed for one day";
		theWindow[i++] = "of travel (on foot).    ";
		theWindow[i++] = "";
		theWindow[i++] = "";
		theWindow[i] = "Cost is ";
		theWindow[i] += cost;
		theWindow[i++] += " gold per 'day'";

		tempString = "Maximum purchase ";
		tempString += max;
		tempString += " days";

		g.AddBottom("");
		g.AddBottom(tempString);

		choice = ChooseNumber(max);

		if (choice > 0)
		{
			g.player.Spend (int(choice * cost));
			g.player.Food (choice);
			
			tempString = "";
			tempString += choice; 
			tempString += " days of food bought.";

			g.AddBottom(tempString);

			StoreSound(snd_Sale);

			if (g.player.Item(9) == 0 && g.map.returnMail == false)
			{
				int mMap = rnd(0, 3);
				int target;
				int count = 0;

				do
				{
					target = g.map.Mail(mMap);

					if (g.map.GetName(target) != "")
					{
						break;
					}
					else
					{
						mMap++;

						if (mMap == 4)
							mMap = 0;
					}
					
					count++;

				} while (count < 6);

				if (count == 6)
				{
					StoreDone();
					return;
				}

				LotaPlaySound(snd_Question);

				g.AddBottom("");
				g.AddBottom("Would you like to earn some gold?");

				MenuItemList menu(2, "Yes", "No");

				choice = QuickMenu(menu, 2);

				if (choice == 0)
				{
					g.player.ItemCount(9, 1);
					g.player.mailTown = target;

					g.AddBottom("");
					g.AddBottom("Here's some mail to");
					g.AddBottom("deliver to " + g.map.GetName(target) + ".");
					g.AddBottom("");
					g.AddBottom("        Press Key to Continue");

					WaitKey();
				}
			}
			
		}
		else
		{
			g.AddBottom("");
			g.AddBottom("Nothing Purchased");

			StoreSound(snd_Medium);
		}
	}
	else
	{
		if (g.map.SetRobbed(shop.dave.id) < 4)
		{
			g.map.SetRobbed(shop.dave.id, 1);

			choice = rnd(1, 15) + rnd(20, 35);

			g.AddBottom("");
			g.AddBottom("Stole " + String(choice) + " days of food.");

			g.player.Food(choice);

			StoreSound(snd_Sale);

			if (rnd(0, 99) < 25)
				g.map.SetRobbed(shop.dave.id, 4);

		}
		else
		{
			g.AddBottom("");
			g.AddBottom("No items within reach now.");

			StoreSound(snd_Medium);
		}

	}

	StoreDone();

}

void StoreBank(StoreClass &shop)
{
	String tempString;
	int i = 0;
//	int cost = 15 / g.player.Attribute(charm) * 1;
	int choice;
	int amount;

	if (shop.robbing)
	{
		StdRob(shop, 200);
		return;
	}

	SetStore();
	SetColors(lotaDkGray, lotaGreen, lotaYellow, lotaWhite, lotaYellow);

	theWindow[i++] = "Convenience Bank";
	theWindow[i++] = "";
	theWindow[i++] = "";
	theWindow[i++] = "Our services   ";
	theWindow[i++] = "---------------";
	theWindow[i++] = "";
	theWindow[i++] = "";
	theWindow[i++] = "1.  Deposit Funds   ";
	theWindow[i++] = "";
	theWindow[i++] = "2.  Withdraw Funds  ";
	theWindow[i++] = "";
	theWindow[i++] = "3.  Balance Inquiry  ";

	g.AddBottom("");
	g.AddBottom("Make choice (Hit 0 to cancel)");
	g.AddBottom("");

	MenuItemList theList (4, "0", "1", "2", "3");
	choice = QuickMenu(theList, 2, 0);

	switch (choice)
	{

	case 1:
		g.AddBottom("");
		g.AddBottom("Deposit how much?");
		amount = ChooseNumber(g.player.Gold());

		g.player.Spend(amount);
		g.player.GoldInBank(amount);

		break;
	case 2:
		if (g.player.GoldInBank() > 0)
		{

			g.AddBottom("");
			g.AddBottom("Withdraw how much?");
			amount = ChooseNumber(g.player.GoldInBank());

			g.player.Spend(-amount);
			g.player.GoldInBank(-amount);
		}
		else
		{
			g.ClearBottom();
			g.AddBottom("Nothing to withdraw");

			StoreSound(snd_Medium);
			choice = 0;

		}
		break;
	}

	if (choice > 0)
	{
		tempString = "Current balance: ";
		tempString += g.player.GoldInBank();
		tempString += " gold.";

		g.AddBottom(tempString);

		if (choice != 3)
		{
			StoreSound (snd_Sale);
		}

	}

	StoreDone();

}

void StoreWeapons (StoreClass &shop)
{
	MenuItemList theList(3, "Buy", "Sell", "Neither");
	String tempString;
	int i = 0, j = 0;
	int max = 200 * g.player.Level();
	int choice;
	int	itemList[16];
	int qualList[16];
	int priceList[16];

	SetStore();
	SetColors(lotaBrown, lotaOrange, lotaYellow, lotaWhite, lotaWhite);

	theWindow[i++] = String(" ") + shop.name + String(" ");
	theWindow[i++] = "";
	theWindow[i++] = "Weapons";


	for (i = 1; i <= 8; i++)
	{
		itemList[i] = i;
		qualList[i] = i % 5;
		priceList[i] = g.WeaponCost(itemList[i], qualList[i]);
	}

	for (;i < 16; i++)
	{
		itemList[i] = 0;
	}

	if (!shop.robbing)
	{
		g.ClearBottom();
		choice = QuickMenu(theList, 2, 0);
		wait(1);
	}
	else
		choice = 0;

	if (choice == 0)
	{

		theWindow[4] = "Items                   Prices";

		if (!shop.robbing)
			StoreSound(snd_Sale);

		for (i = 1; i < 16 && itemList[i] > 0; i++)
		{
			if (itemList[i] > 0)
			{
				j = i + 5;

				theWindow[j] = "";
				theWindow[j] += i;
				theWindow[j] += ". ";
				theWindow[j] += g.QualityName(qualList[i]);
				theWindow[j] += " ";
				theWindow[j] += g.WeaponName(itemList[i]);
				theWindow[j] += space(20 - len(g.QualityName(qualList[i])) 
									- len(g.WeaponName(itemList[i])));
				theWindow[j] += priceList[i];
				wait(1);

			}

		}

		MenuItemList theList2;

		for (int j = 0; j < i; j++)
		{
			theList2.AddItem(String(j));
		}

		g.AddBottom("");
		g.AddBottom("Make choice (hit 0 to cancel)");
		g.AddBottom("");

		choice = QuickMenu(theList2, 2, 0);

		if (choice == 0)			// buy weapon
		{
			if (!shop.robbing)
			{
				g.AddBottom("");
				g.AddBottom("Nothing purchased");
				g.AddBottom("");
			}
			else
			{
				g.AddBottom("");
				g.AddBottom("Nothing stolen");
				g.AddBottom("");
			}

			StoreSound(snd_Medium);
		}
		else
		{
			if (!shop.robbing)
			{
				// spend the cash, if they have it
				if (g.player.Spend(priceList[choice]))
				{
					if (g.player.AddWeapon(itemList[choice], qualList[choice]))
					{
						tempString = g.QualityName(qualList[choice]);
						tempString += " ";
						tempString += g.WeaponName(itemList[choice]);
						tempString += " purchased.";
						g.AddBottom(tempString);
						g.AddBottom("");

						StoreSound(snd_Sale);
					}
					else
					{

						g.player.GainGold(-priceList[choice]);
						g.AddBottom("No room in inventory");
					}
				
				}
				else
				{
					g.AddBottom("You're short on gold.");
					StoreSound(snd_Medium);
				}

			}
			else		// robbed
			{
				if (g.map.SetRobbed(shop.dave.id) < 4)
				{
					g.map.SetRobbed(shop.dave.id, 1);

					if (g.player.AddWeapon(itemList[choice], qualList[choice]))
					{
						tempString = g.QualityName(qualList[choice]);
						tempString += " ";
						tempString += g.WeaponName(itemList[choice]);
						tempString += " stolen.";
						g.AddBottom(tempString);
						g.AddBottom("");

						StoreSound(snd_Sale);

						if (rnd(0, 99) < 40)
						{
							g.map.SetRobbed(shop.dave.id, 4);
						}
					}
					else
					{
						g.AddBottom("No room in inventory");
						StoreSound(snd_Medium);
					}
				}
				else
				{
					g.AddBottom("No items within reach here.");
					StoreSound(snd_Medium);
				}
			}

		}
	}
	else if (choice == 1)		// sell weapon
	{
		
	}

	StoreDone();

}
void StoreArmor (StoreClass &shop)
{
	MenuItemList theList(3, "Buy", "Sell", "Neither");
	String tempString;
	int i = 0, j = 0;
	int max = 200 * g.player.Level();
	int choice;
	int	itemList[16];
	int qualList[16];
	int priceList[16];

	SetStore();
	SetColors(lotaPurple, lotaBlue, lotaYellow, lotaWhite, lotaWhite);

	theWindow[i++] = String(" ") + shop.name + String(" ");
	theWindow[i++] = "";
	theWindow[i++] = "Armor";

	for (i = 1; i < 5; i++)
	{
		itemList[i] = i;
		qualList[i] = i % 5;
		priceList[i] = g.ArmorCost(itemList[i], qualList[i]);
	}
	for (;i < 16; i++)
	{
		itemList[i] = 0;
	}

	if (!shop.robbing)
	{
		g.ClearBottom();
		choice = QuickMenu(theList, 2, 0);
		wait(1);
	}
	else
		choice = 0;

	if (choice == 0)
	{
		
		theWindow[4] = "Items                   Prices";

		StoreSound (snd_Sale);

		for (i = 1; i < 16 && itemList[i] > 0; i++)
		{
			if (itemList[i] > 0)
			{
				j = i + 5;

				theWindow[j] = "";
				theWindow[j] += i;
				theWindow[j] += ". ";
				theWindow[j] += g.QualityName(qualList[i]);
				theWindow[j] += " ";
				theWindow[j] += g.ArmorName(itemList[i]);
				theWindow[j] += space(20 - len(g.QualityName(qualList[i])) 
									- len(g.ArmorName(itemList[i])));
				theWindow[j] += priceList[i];
				wait(1);

			}

		}

		MenuItemList theList2;

		for (int j = 0; j < i; j++)
		{
			theList2.AddItem(String(j));
		}

		g.AddBottom("");
		g.AddBottom("Make choice (hit 0 to cancel)");
		g.AddBottom("");

		choice = QuickMenu(theList2, 2, 0);

		if (choice == 0)			// buy armor
		{
			if (!shop.robbing)
			{
				g.AddBottom("");
				g.AddBottom("Nothing purchased");
				g.AddBottom("");
			}
			else
			{
				g.AddBottom("");
				g.AddBottom("Nothing stolen");
				g.AddBottom("");
			}

			StoreSound(snd_Medium);
		}
		else
		{
			if (!shop.robbing)
			{
				if (g.player.Spend(priceList[choice]))
				{
					if (g.player.AddArmor(itemList[choice], qualList[choice]))
					{
						tempString = g.QualityName(qualList[choice]);
						tempString += " ";
						tempString += g.ArmorName(itemList[choice]);
						tempString += " purchased.";
						g.AddBottom(tempString);
						g.AddBottom("");

						StoreSound(snd_Sale);
					}
					else
					{
						g.player.Spend(-priceList[choice]);
						g.AddBottom("No room in inventory");
					}
				
				}
			}
			else		// robbed
			{
				if (g.map.SetRobbed(shop.dave.id) < 4)
				{
					g.map.SetRobbed(shop.dave.id, 1);

					if (g.player.AddArmor(itemList[choice], qualList[choice]))
					{
						tempString = g.QualityName(qualList[choice]);
						tempString += " ";
						tempString += g.ArmorName(itemList[choice]);
						tempString += " stolen.";
						g.AddBottom(tempString);
						g.AddBottom("");

						StoreSound(snd_Sale);

						if (rnd(0, 99) < 40)
						{
							g.map.SetRobbed(shop.dave.id, 4);
						}
					}
					else
					{
						g.AddBottom("No room in inventory");
						StoreSound(snd_Medium);
					}
				}
				else
				{
					g.AddBottom("No items within reach here.");
					StoreSound(snd_Medium);
				}
			}
		}
	}
	else if (choice == 1)
	{

	}

	StoreDone();

}
void StoreBuyback (StoreClass &shop)
{
}
void StoreBlackjack (StoreClass &shop)
{
}
void StoreLending (StoreClass &shop)
{
	String tempString;
	int i = 0;
	int max = 200 * g.player.Level();
	int choice;

	if (shop.robbing)
	{
		StdRob(shop, 50);
		return;
	}


	SetStore();
	SetColors(lotaDkGray, lotaMdGray, lotaYellow, lotaWhite, lotaYellow);

	theWindow[i++] = " Friendly ";
	theWindow[i++] = ""; 
	theWindow[i++] = "Lending Association";
	theWindow[i++] = "";
	theWindow[i++] = "";
	theWindow[i++] = "";
	theWindow[i++] = "";

	if (g.player.loan == 0)
	{
		theWindow[i++] = "We'd be happy to loan you";
		theWindow[i++] = "money at 'friendly' rates";
		theWindow[i++] = "";
		theWindow[i] = "You may borrow up to ";
		theWindow[i] += max;
		theWindow[i++] += " gold";
	
		g.AddBottom("");
		g.AddBottom("Borrow how much?");

		choice = ChooseNumber(max);

		if (choice > 0)
		{
			g.player.GainGold(choice);
			g.player.loan = int(choice * 1.5);
			g.player.dueDate = g.player.TimeDays() + 120;

			g.AddBottom("Borrowed " + String(choice) + " gold.");
			g.AddBottom("");
			g.AddBottom("You'll owe " + String(g.player.loan) + " gold in 120 days!");

			StoreSound (snd_Sale);

		}
	}
	else
	{
		String DueDate;
		max = g.player.Gold();
		int min;
		unsigned int color[44];

		if (g.player.dueDate - g.player.TimeDays() > 0)
		{
			DueDate = String(g.player.dueDate - g.player.TimeDays()) + String(" days ");
			min = 0;
		}
		else
		{
			DueDate = "NOW!!   ";
			min = g.player.loan / 3;
		}

		theWindow[i++] = "You owe: " + String(g.player.loan) + " gold!";
		theWindow[i++] = "";
		theWindow[i++] = "";
		theWindow[i++] = "Due Date: " + DueDate;

		g.AddBottom("");
		g.AddBottom("Pay how much?");

		if (min > 0)
		{
			for (i = 0; i < 13; i++, color[i] = lotaWhite);
			for (; i < 40; i++, color[i] = lotaYellow);

			g.UpdateBottom("Pay how much? (At Least " + String(min) + " gold)", 0, color);

		}

		choice = ChooseNumber(max);

		g.player.GainGold(-choice);
		g.player.loan -= choice;

		if (g.player.loan == 0)
		{
			g.AddBottom("Loan Repaid");
			LotaPlaySound(snd_Sale);

		}
		else if (min == 0)
		{

			g.AddBottom("You Owe " + String(g.player.loan) + " gold.");
			g.AddBottom("Take your time.");

			LotaPlaySound(snd_Sale);
		}
		else if (choice >= min)
		{
			g.AddBottom("You have 14 days to pay the rest!");
			g.player.dueDate = g.player.TimeDays() + 14;

			LotaPlaySound(snd_Sale);
		}
		else
		{
			g.AddBottom("Better pay up!");

			//LotaPlaySound(snd_Bad);

		}
		
	}

	wait(500);
	StoreDone();

}

void StoreVault (StoreClass &shop)
{
	if (shop.robbing == false)
		return;

	if (g.map.SetRobbed(shop.dave.id))
	{
		g.AddBottom("");
		g.AddBottom("The vault is empty.");
	}
	else
	{

		int total = 0;
		int bags = g.player.VaultGold();

		for (int i = 0; i < bags; i++)
		{
			total += rnd(i + 25, i * 5 + 75);
		}

		g.AddBottom("");
		g.AddBottom("You grab " + String(bags) + " bags of gold!");
		g.map.SetRobbed(shop.dave.id, 1);

		StoreSound (snd_VeryGood);

		g.player.VaultGold(--bags);

		g.player.GainGold(total);

	}

}

void StoreHealer (StoreClass &shop)
{
	int i = 0;
	int wound = int((g.player.MaxHP() - g.player.HP()) * 0.7);
	int herb = 250;
	String strWound;
	int choice;
	MenuItemList menu(3, "0", "1", "2");

	if (wound <= 0)
	{
		strWound = "NOT NEEDED";
	}
	else
	{
		strWound = String(wound) + " gold ";
	}

	SetStore();
	SetColors(lotaGreen, lotaLtGreen, lotaYellow, lotaWhite, lotaYellow);

	theWindow[i++] = String(" ") + shop.name + String(" ");
	theWindow[i++] = "";
	theWindow[i++] = "";
	theWindow[i++] = "Our Sect Offers Restorative";
	theWindow[i++] = "Cures For Your Wounds";
	theWindow[i++] = "";
	theWindow[i++] = "";
	theWindow[i++] = "";
	theWindow[i++] = "";

	theWindow[i++] = left("1. Wound Care  -  " + strWound + space(34), 34);
	theWindow[i++] = "";
	theWindow[i++] = left("2. Healing Herbs -  " + String(herb) + " apiece" + space(34), 34);
	
	g.ClearBottom();
	g.AddBottom("Make Choice (Hit 0 to cancel)");
	g.AddBottom("");

	choice = QuickMenu(menu, 2, 0);

	if (choice == 1)
	{
		if (g.player.Spend(wound))
		{
			g.AddBottom("");
			g.AddBottom("");
			g.AddBottom("You are healed.");

			g.player.HP(g.player.MaxHP() - g.player.HP());

			LotaPlaySound(snd_VeryGood,0,0);


			g.commandMode = cmdBad;
			wait(1500);

			g.commandMode = cmdEnterCommand;
		}
		else
		{
			g.AddBottom("You're short on gold.");
			LotaPlaySound(snd_Medium);

			wait(750);
		}

	}
	else if (choice == 2)
	{
		if (g.player.Gold() < herb)
		{
			g.AddBottom("You're short on gold.");
			LotaPlaySound(snd_Medium);

			wait(750);

		}
		else
		{
			int max = g.player.Gold() / herb;

			SetColors(lotaLtBlue,  lotaLtGreen, lotaYellow, lotaWhite, lotaYellow);
			
			g.ClearBottom();
			g.AddBottom("Purchase how many healing herbs?");
			
			choice = ChooseNumber(max);

			if (g.player.Item(3) + choice > 40)
			{
				g.AddBottom("You can't carry this many.");
				LotaPlaySound(snd_Medium);

				wait(750);
				
			}
			else
			{
				g.AddBottom("Healing Herbs Purchased.");
				LotaPlaySound(snd_Sale);

				g.player.ItemCount(3, choice);
				g.player.GainGold(-choice * herb);

				wait(1000);
			}

		}
	}


	StoreDone();
}

void StoreMagic (StoreClass &shop)
{
	int i = 0;
	int spell, choice; 
	int prices[7];
	String names[7];
	char tempChars[40];

	int maxSp[7];
	MenuItemList menu(7, "0", "1", "2", "3", "4", "5", "6");

	prices[1] = (int)shop.costFactor / 50;
 	prices[2] = (int)shop.costFactor / 25;
	prices[3] = (int)shop.costFactor / 10;
	prices[4] = (int)shop.costFactor / 8;
	prices[5] = (int)shop.costFactor / 4;
	prices[6] = (int)(shop.costFactor / 7.70);

	for (i = 1; i <= 6; i++)
	{
		LoadString (g.hInstance(), i + 42, tempChars, 39);
					
		names[i] = tempChars;
	}

	maxSp[1] = 100;
	maxSp[2] = 100;
	maxSp[3] = 40;
	maxSp[4] = 40;
	maxSp[5] = 20;
	maxSp[6] = 50;

	SetStore();
	SetColors(lotaLtBlue, lotaCyan, lotaYellow, lotaWhite, lotaWhite);

	i = 0;

	theWindow[i++] = String(" ") + shop.name + String(" ");
	theWindow[i++] = "";
	theWindow[i++] = "General Purpose       Prices";   theWindowColor[i-1] = lotaBlue;
	theWindow[i++] = "";
	theWindow[i++] = left(" 1. Magic Flame        " + String(prices[1]) + space(30), 30);
	theWindow[i++] = left(" 2. Firebolt           " + String(prices[2]) + space(30), 30);
	theWindow[i++] = "";
	theWindow[i++] = "Dungeon Use Only      Prices";   theWindowColor[i-1] = lotaBlue;
	theWindow[i++] = "";
	theWindow[i++] = left(" 3. Befuddle Spell     " + String(prices[3]) + space(30), 30);
	theWindow[i++] = left(" 4. Psycho Strength    " + String(prices[4]) + space(30), 30);
	theWindow[i++] = left(" 5. Kill Flash         " + String(prices[5]) + space(30), 30);
	theWindow[i++] = "";
	theWindow[i++] = "Outside Use Only      Prices";   theWindowColor[i-1] = lotaBlue;
	theWindow[i++] = "";
	theWindow[i++] = left(" 6. Seek Spell         " + String(prices[6]) + space(30), 30);

	g.ClearBottom();
	g.AddBottom("Make Choice (Hit 0 to cancel)");
	g.AddBottom("");

	spell = QuickMenu(menu, 2, 0);

	if (spell > 0)
	{

		if (g.player.Gold() < prices[spell])
		{
			g.AddBottom("You're short on gold.");
			LotaPlaySound(snd_Medium);

			wait(750);
		}
		else
		{
			int max = g.player.Gold() / prices[spell];

			SetColors(lotaLtBlue,  lotaLtGreen, lotaYellow, lotaWhite, lotaYellow);
			
			g.ClearBottom();
			g.AddBottom("Purchase how many " + names[spell] + "?");
			
			choice = ChooseNumber(max);

			if (g.player.Item(3) + choice > maxSp[spell])
			{
				g.AddBottom("You can't carry this many.");
				LotaPlaySound(snd_Medium);

				wait(750);
				
			}
			else
			{
				
				g.AddBottom(names[spell] + "s Purchased.");
				LotaPlaySound(snd_Sale);

				g.player.ItemCount(spell + 23, choice);
				g.player.GainGold(-choice * prices[spell]);

				wait(1000);
			}

		}

	}

	StoreDone();

}

void StoreTraining (StoreClass &shop, bool weapon)
{
	g.AddBottom(shop.name + (weapon ? " Weapon " : " Armor ") + "training.", lotaYellow);
	
	g.AddBottom("");
	g.AddBottom("Unfortunately, this store");
	g.AddBottom("is not implemented yet.");
	g.AddBottom("");

	LotaPlaySound(snd_Medium);
	wait(2000);

	g.AddBottom("");
	g.AddBottom(String("However, you can buy ") + (weapon ? "Dexterity " : "Endurance "), lotaGreen);
	wait(1);

	g.AddBottom("for 10 gold per point.", lotaGreen);
	g.AddBottom("");

	MenuItemList menu(2, "Yes", "No");
	int choice = QuickMenu(menu, 3, 0, lotaGreen, lotaWhite);
	

	if (choice == 0)
	{

		choice = ChooseNumber(g.player.Gold() / 10);

		if (choice > 0)
		{
			int attr = weapon ? dexterity : endurance;
			String attrName = weapon ? "Dexterity" : "Endurance";

			if (g.player.Attribute(attr) + choice > 36)
			{
				g.AddBottom("");
				g.AddBottom("You can't buy this much.");

				LotaPlaySound(snd_Medium);
			}
			else
			{
				g.player.Attribute(attr, choice);
				g.player.Spend(choice * 10);

				g.AddBottom("");
				g.AddBottom("Your " + attrName + " is now " + g.player.Attribute(attr) + "!");

				LotaPlaySound(snd_VeryGood);
			}

		}
		else
		{
			g.AddBottom("");
			g.AddBottom("No transaction.");

			LotaPlaySound(snd_Medium);
		}
		
	}

	g.commandMode = cmdBad;
	wait(500);
	g.commandMode = cmdPrompt;

}



void StoreMuseumCoin ()//StoreClass &shop)
{
	int amount = rnd(40, 79);
	int coin = -1;
	int choice;
	MenuItemList menu(2, "Yes", "No");

	if (g.player.Level() == 1)
	{
		coin = rnd(0, 1);
	}
	else if (g.player.Level() <= 3)
	{
		coin = rnd(0, 2);
	}

	if (coin == -1)
		return;

	coin += 17;
	
	g.AddBottom("Would you like to buy a ");
	wait(1);

	g.AddBottom("museum coin for " + String(amount) + " gold?");
	wait(1);

	g.AddBottom("");
	wait(1);

	LotaPlaySound(snd_Question);

	choice = QuickMenu(menu, 3, 0);

	if (choice == 0)
	{
		if (g.player.Spend(amount))
		{
			char tempChars[40];
			String tempString;
			
			LoadString(g.hInstance(), coin + 19, tempChars, 40);
			tempString = tempChars;

			g.AddBottom("Use this " + tempString + " well!");

			g.player.ItemCount(coin, 1);

			LotaPlaySound(snd_Sale);
		}
		else
		{
			g.AddBottom("Not enough gold.");
			LotaPlaySound(snd_Medium);
		}

	}



}
void StdRob (StoreClass &shop, int amount)
{
	if (g.map.SetRobbed(shop.dave.id))
	{
		g.AddBottom("");
		g.AddBottom("Nothing to really rob here.");
	}	
	else if (amount > 0)
	{
		g.map.SetRobbed(shop.dave.id, 1);

		amount = int(amount * rnd(80, 120) / 100.0);
		
		g.AddBottom("");
		g.AddBottom("You get " + String(amount) + " gold.");
		
		g.player.GainGold(amount);
		g.map.IsAngry(true);
	}
	else
	{
		g.AddBottom("");
		g.AddBottom("Nothing to really rob here.");
	}
	
	wait (1200);	
	
}
