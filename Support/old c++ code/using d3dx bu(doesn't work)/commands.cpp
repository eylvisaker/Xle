
#include "lota.h"

extern Global g;

int			inventoryScreen = 0;

/****************************************************************************
 *  void DoCommand (char cmd)												*
 *																			*
 *  This function will process user input and execute their commands.		*
 ****************************************************************************/
void DoCommand (char cmd)
{
	String		theCommand("Enter Command: ");
	int			last = g.cursor();
	char		cursorKeys = 0;
	String		command;
	char		originalCmd = cmd;
	int			terrain = 0;
	bool		badCommand = false;
	int			wasRaft = g.player.onRaft;

	cmd = toupper(cmd);

	switch (g.commandMode)
	{
	case 0:
	
		break;
	case 5:
		
		g.AddBottom("");
		g.AddBottom("Enter command:  ");

		g.commandMode = 10;

		break;
		
	case 'I':
		Inventory(cmd);

		break;
	case 10:
		if (cmd != 0)
		{

			cmd = g.cursor(cmd);
			
			if (cmd == -1)
			{
				cmd = originalCmd;
				badCommand = true;
			}

			// Test for cursor movement so we can get the right command first
			switch (originalCmd)
			{
			case VK_RIGHT:
				cursorKeys = 1;

				break;

			case VK_UP:
				cursorKeys = 2;

				break;

			case VK_LEFT:
				cursorKeys = 3;

				break;

			case VK_DOWN:
				cursorKeys = 4;

				break;
				
			}

			//if ((cmd >= 65 && cmd <= 91) || cursorKeys > 0 || originalCmd == 'Z')
			if (cmd > 0 || cursorKeys > 0)
			{
				
				String mcmd = g.Menu(cmd);
				
				if (mcmd != "")
				{
					command = mcmd;
				}

				if (cursorKeys)
				{

					g.waitCommand = 50 * (g.Gamespeed() + 1);

					if (!g.Animating())
					{
						g.Animating(true);
						g.AnimFrame(animInc);
					}

					if (g.map.MapType() == mapDungeon)
					{
						g.waitCommand = 500;
					}


				}
				else
				{
					g.waitCommand = 700;
				}


				switch (cursorKeys)
				{
				case lotaEast:
					switch (g.map.MapType())
					{
					case mapOutside:
						g.raftFacing = lotaEast;

						command = "Move East";

						terrain = g.SetPos(g.GetX() + 1, g.GetY());
					
						break;

					case mapTown:
					case mapCastle:
						g.FaceDirection(lotaEast);
						command = "Walk East";

						terrain = g.SetPos(g.GetX() + 1, g.GetY());

						break;

					case mapDungeon:
					case mapMuseum:


						command = "Turn Right";

						g.player.faceDirection--;

						if (g.player.faceDirection < lotaEast)
							g.player.faceDirection = lotaSouth;

						break;

					}
					break;

				case lotaNorth:

					switch (g.map.MapType())
					{
					case mapOutside:
						g.FaceDirection(lotaNorth);
						command = "Move North";

						terrain = g.SetPos(g.GetX(), g.GetY() - 1);
						break;

					case mapTown:
					case mapCastle:
						g.FaceDirection(lotaNorth);
						command = "Walk North";

						terrain = g.SetPos(g.GetX(), g.GetY() - 1);
						break;

					case mapDungeon:
					case mapMuseum:
						command = "Move Forward";
						
						if (g.player.faceDirection == lotaEast)
							terrain = g.SetPos(g.GetX() + 1, g.GetY());
						if (g.player.faceDirection == lotaNorth)
							terrain = g.SetPos(g.GetX(), g.GetY() - 1);
						if (g.player.faceDirection == lotaWest)
							terrain = g.SetPos(g.GetX() - 1, g.GetY());
						if (g.player.faceDirection == lotaSouth)
							terrain = g.SetPos(g.GetX(), g.GetY() + 1);

						break;

					}

					break;

				case lotaWest:
					switch (g.map.MapType())
					{
					case mapOutside:
						g.FaceDirection(lotaWest);
						g.raftFacing = lotaWest;

						command = "Move West";

						terrain = g.SetPos(g.GetX() - 1, g.GetY());
						break;

					case mapTown:
						g.FaceDirection(lotaWest);
						command = "Walk West";

						terrain = g.SetPos(g.GetX() - 1, g.GetY());
						break;

					case mapDungeon:
					case mapMuseum:
						command = "Turn Left";
						
						g.player.faceDirection++;

						if (g.player.faceDirection > lotaSouth)
							g.player.faceDirection = lotaEast;

						break;

					}

					break;

				case lotaSouth:

					switch (g.map.MapType())
					{
					case mapOutside:
						g.FaceDirection(lotaSouth);
						command = "Move South";

						terrain = g.SetPos(g.GetX(), g.GetY() + 1);
						break;

					case mapTown:
						g.FaceDirection(lotaSouth);
						command = "Walk South";

						terrain = g.SetPos(g.GetX(), g.GetY() + 1);
						break;

					case mapDungeon:
					case mapMuseum:
						command = "Move Backward";
					
						if (g.player.faceDirection == lotaEast)
							terrain = g.SetPos(g.GetX() - 1, g.GetY());
						if (g.player.faceDirection == lotaNorth)
							terrain = g.SetPos(g.GetX(), g.GetY() + 1);
						if (g.player.faceDirection == lotaWest)
							terrain = g.SetPos(g.GetX() + 1, g.GetY());
						if (g.player.faceDirection == lotaSouth)
							terrain = g.SetPos(g.GetX(), g.GetY() - 1);

						break;

					}

					break;

				}

				if (cursorKeys > 0)
				{
					g.charAnimCount = 0;
				}

				if (originalCmd == 'Z')
				{
					command = "Pass";
				}

				theCommand += command;
				g.UpdateBottom(theCommand);
				g.currentCommand = cmd;

				if (wasRaft != g.player.onRaft)
				{
					if (g.player.onRaft)
					{
						g.AddBottom ("");
						g.AddBottom ("You climb onto a raft.");
					}

				}

				if (terrain == 1)
				{
					LotaPlaySound (snd_Bump);
					
					unsigned int coloring[39];

					for (int count = 0; count < 39; count ++)
					{
						coloring[count] = lotaCyan;
					}

					g.AddBottom("");
					g.AddBottom("There is too much water for travel.", (unsigned int *)coloring);
				}
				else if (terrain == 2)
				{
					LotaPlaySound (snd_Bump);
					
					g.AddBottom("");
					g.AddBottom("You are not equipped to");
					g.AddBottom("cross the mountains.");

				}
				else if (terrain == 50)
				{
					LotaPlaySound (snd_Bump);
					
					unsigned int coloring[39];

					for (int count = 0; count < 39; count ++)
					{
						coloring[count] = lotaCyan;
					}

					g.AddBottom("");
					g.AddBottom("The raft must stay in the water.", (unsigned int *)coloring);
				}
				else
				{

					if (cursorKeys > 0)
					{

						if (g.map.MapType() == mapOutside)
						{

							if (!g.player.onRaft)
							{
								switch (g.player.Terrain())
								{
								case mapSwamp:
									LotaPlaySound (snd_Swamp);
									break;

								case mapDesert:
									LotaPlaySound (snd_Desert);
									break;

								case mapGrass:
								case mapForest:
								case mapMixed:
								default:
									LotaPlaySound (snd_WalkOutside);
									break;
								}
							}
							else
							{
								//PlaySound (MAKEINTRESOURCE(snd_Raft), g.hInstance(), SND_ASYNC | SND_RESOURCE | SND_LOOP | SND_NOSTOP);
							}

							switch (g.player.Terrain())
							{
							case mapWater:
							case mapGrass:
							case mapForest:
								g.player.TimeDays(.25);
								break;
							case mapSwamp:
								g.player.TimeDays(.5);
								break;
							case mapMountain:
								g.player.TimeDays(1);
								break;
							case mapDesert:
								g.player.TimeDays(1);
								break;
							case mapMixed:
								g.player.TimeDays(.5);
								break;
							}
						}
						
						if (g.map.CheckSpecial())
						{

							SpecialEvent dave = g.map.GetSpecial();

							switch (dave.type)
							{
							case 1:
								String tempLine = "Enter ";
								String tempName;

								g.map.GetName(dave.data[0], tempName);
								
								MenuItemList theList(2, "Yes", "No");
								
								tempLine += tempName;
								tempLine += "?";

								g.AddBottom("");
								g.AddBottom(tempLine);
								
								HeartBeat(redraw);

								LotaPlaySound (snd_Question);
								
								int choice = QuickMenu(theList, 3);

								if (choice == 0)
								{

									wait(500);

									g.map.LoadMap(dave.data[0]);
									g.player.x = dave.data[1];
									g.player.y = dave.data[2];

									//g.SetPos(dave.data[1], dave.data[2]);

									g.ClearBottom();

								}

								break;

							}

									
						}

						g.waterReset = true;
					}
				}

				if (!badCommand || cmd == 'Z')
				{
					switch (cmd)
					{
					case 'A':
						Armor();
						break;
					case 'D':
						Disembark();
						break;
					case 'E':
						End();
						break;
					case 'F':
						Fight();
						
						break;
					case 'G':
						GameSpeed();
						break;
					case 'H':
						Hold();
						break;

					case 'I':
						g.waitCommand = 0;
						Inventory(1);

						break;
					case 'L':
						Leave();
						break;
					case 'M':
						Magic();
						break;
					case 'O':
						Open();
						break;
					case 'P':
					case 'Z':
						Pass();
						break;
					case 'R':
						Rob();
						break;
					case 'S':
						Speak();
						break;
					case 'T':
						Take();
						break;
					case 'U':
						Use();
						break;
					case 'W':
						Weapon();
						break;
					case 'X':
						Xamine();
						break;
					default:
						break;
					}
				}
				else
				{
					if (cursorKeys == 0)
					{
						LotaPlaySound (snd_Invalid);
						g.waitCommand = 0;
					}
				}

				// waits before the next command
				if (g.waitCommand > 0)
				{

					g.commandMode = 0;

					wait(g.waitCommand);

					g.AddBottom("");
					g.AddBottom("Enter Command: ");

					g.commandMode = 10;

					g.waitCommand = 0;

					g.ResetTimers();
				}

			}
		}

		break;

	}

}


void Armor()
{
	MenuItemList theList;
	char		 tempChars[40];
	String		 tempString;
	unsigned int colors[40];

	theList.AddItem("Nothing");

	for (int i = 1; i <= 5; i++)
	{
		if (g.player.armor[i] > 0)
		{
			LoadString(g.hInstance(), g.player.armorQuality[i] + 14, tempChars, 40);
			tempString = tempChars;
			tempString += " ";

			LoadString(g.hInstance(), g.player.armor[i], tempChars, 40);
			tempString += tempChars;

			theList.AddItem (tempString);
		}

	}

	tempString = g.Bottom(0);

	for (i = 0; i < tempString.len(); i++)
	{
		colors[i] = lotaWhite;
	}

	for (; i < 40; i++)
	{
		colors[i] = lotaCyan;
	}

	tempString += "-choose above";

	g.UpdateBottom(tempString, 0, colors);

	g.subMenu.title = "Pick Armor";
	g.subMenu.value = g.player.currentArmor;

	g.player.currentArmor = SubMenu(theList);
}

void Disembark()
{
	int newx, newy;

	g.UpdateBottom ("Enter command: disembark raft");

	if (g.player.onRaft)
	{


		g.AddBottom("");
		g.AddBottom("Disembark in which direction?");

		g.commandMode = 0;
		g.quickMenu = true;

		do
		{
			wait(1);

		} while (g.menuKey != VK_LEFT && g.menuKey != VK_RIGHT && g.menuKey != VK_UP && g.menuKey != VK_DOWN);

		newx = g.GetX();
		newy = g.GetY();

		switch (g.menuKey)
		{
		case VK_LEFT:
			g.FaceDirection(lotaWest);
			newx--;
			break;

		case VK_UP:
			g.FaceDirection(lotaNorth);
			newy--;
			break;

		case VK_DOWN:
			g.FaceDirection(lotaSouth);
			newy++;
			break;

		case VK_RIGHT:
			g.FaceDirection(lotaEast);
			newx++;
			break;
		}

		g.quickMenu = false;
		g.commandMode = 10;

		g.player.onRaft = false;

		g.SetPos(newx, newy);

		g.menuKey = 0;
	}
	else
	{
		g.AddBottom ("");
		g.AddBottom ("Nothing to disembark", lotaYellow);
		
	}
}
void End()
{
	PostQuitMessage(0);

}

void Fight()
{
	ChangeScreenMode();
}

void Hold()
{
	MenuItemList theList;
	char		 tempChars[40];
	String		 tempString;
	int			 value = 0;
	unsigned int colors[40];

	theList.AddItem("Nothing");

	for (int i = 1; i <= 16; i++)
	{
		if (g.player.item[i] > 0)
		{
			LoadString(g.hInstance(), i + 19, tempChars, 40);
			tempString = tempChars;

			if (i <= g.player.hold)
			{
				value++;
			}

			theList.AddItem (tempString);
		}

	}

	g.subMenu.title = "Hold Item";
	g.subMenu.value = value;

	tempString = g.Bottom(0);

	for (i = 0; i < tempString.len(); i++)
	{
		colors[i] = lotaWhite;
	}

	for (; i < 40; i++)
	{
		colors[i] = lotaCyan;
	}

	tempString += "-choose above";

	g.UpdateBottom(tempString, 0, colors);

	g.player.hold = SubMenu(theList);

	for (i = 1; i <= g.player.hold; i++)
	{
		if (g.player.item[i] == 0)
		{
			g.player.hold++;
		}

	}
}

void GameSpeed()
{

	MenuItemList theList(5, "1", "2", "3", "4", "5");
	String		 strtmp = "Gamespeed is: ";
	String		 numtmp;
	unsigned int color[40];

	g.AddBottom("** Change game speed **", lotaYellow);
	g.AddBottom("     (1 is fastest)", lotaYellow);
	g.AddBottom("");

	g.Gamespeed(1 + QuickMenu(theList, 2, g.Gamespeed() - 1));
	
	strtmp += g.Gamespeed();

	for (UINT i = 0; i < 40; i++)
	{
		
		if (i == strlen(strtmp) - 1)
		{
			color[i] = lotaWhite;
		}
		else
			color[i] = lotaYellow;
	}

	g.AddBottom(strtmp, color);

	g.commandMode = 0;
	wait(500 + 100 * g.Gamespeed());
	g.commandMode = 10;

	
}

void Inventory(int mode)
{

	if (mode == 0 || mode == 1)
	{

		g.commandMode = 'I';

		g.stdDisplay = false;

		if (mode == 1)
		{
			g.DestroyTimers();
			inventoryScreen = 0;
		}


	}
	else if (mode >= 8)
	{

		inventoryScreen++;

		if (inventoryScreen > 1)
		{
			inventoryScreen = 0;

			g.ResetTimers();
			g.ClearBottom();
			g.stdDisplay = true;
			g.commandMode = 5;

		}

	}

}

void Leave()
{
}

void Magic()
{
}

void Open()
{
}

void Pass()
{
}

void Rob()
{
}

void Speak()
{
}

void Take()
{
}

void Use()
{
	String commandString;
	char	tempChars[40];

	bool noEffect = false;

	g.commandMode = 0;

	switch (g.player.hold)
	{
	case 0:
		commandString = "Use Nothing";
		noEffect = true;
		break;

	case 1:
		commandString = "Twist gold armband";
		noEffect = true;
		break;
	case 2:
		commandString = "Ready climbing gear";
		noEffect = true;
		break;
	default:
		commandString = "Use ";
		LoadString (g.hInstance(), g.player.hold + 19, tempChars, 40);
		commandString += tempChars;

		noEffect = true;
		
		break;
	}

	if (noEffect = true)
	{
		g.AddBottom("");
		g.AddBottom (commandString);
		g.AddBottom ("");
		wait (400 + 100 * g.Gamespeed());
		g.UpdateBottom ("No effect");


	}

	g.commandMode = 10;

}

void Weapon()
{
	MenuItemList theList;
	char		 tempChars[40];
	String		 tempString;
	unsigned int colors[40];

	theList.AddItem("Nothing");

	for (int i = 1; i <= 5; i++)
	{
		if (g.player.weapon[i] > 0)
		{
			LoadString(g.hInstance(), g.player.weaponQuality[i] + 14, tempChars, 40);
			tempString = tempChars;
			tempString += " ";

			LoadString(g.hInstance(), g.player.weapon[i], tempChars, 40);
			tempString += tempChars;

			theList.AddItem (tempString);
		}

	}

	tempString = g.Bottom(0);

	for (i = 0; i < tempString.len(); i++)
	{
		colors[i] = lotaWhite;
	}

	for (; i < 40; i++)
	{
		colors[i] = lotaCyan;
	}

	tempString += "-choose above";

	g.UpdateBottom(tempString, 0, colors);

	g.subMenu.title = "Pick Weapon";
	g.subMenu.value = g.player.currentWeapon;

	g.player.currentWeapon = SubMenu(theList);
}

void Xamine()
{
	String terrain;
	String travel;
	String food;
	String theString;
	int i, j;
	unsigned int color[40];

	switch (g.map.MapType())
	{
	case mapOutside:
		switch (g.player.Terrain())
		{
		case mapGrass:
			terrain = "grasslands";
			travel = "easy";
			food = "low";
			break;
		case mapWater:
			terrain = "water";
			travel = "easy";
			food = "low";
			break;
		case mapMountain:
			terrain = "the mountains";
			travel = "slow";
			food = "high";
			break;
		case mapForest:
			terrain = "a forest";
			travel = "easy";
			food = "low";
			break;
		case mapDesert:
			terrain = "a desert";
			travel = "slow";
			food = "high";
			break;
		case mapSwamp:
			terrain = "a swamp";
			travel = "average";
			food = "medium";
			break;
		case mapFoothills:
			terrain = "mountain foothills";
			travel = "average";
			food = "medium";
			break;
		case mapMixed:
			terrain = "mixed terrain";
			travel = "average";
			food = "medium";
			break;

		}

		g.AddBottom("");

		theString = "You are in ";
		theString += terrain;
		theString += ".";

		g.AddBottom (theString);

		theString = "Travel: ";
		theString += travel;
		theString += "  -  Food use: ";
		theString += food;

		for (i = 0; i < 8; i++)
			color[i] = lotaWhite;
		
		j = i;

		for (; i < j + travel.len(); i++)
			color[i] = lotaGreen;

		j = i;

		for (; i < j + 15; i++)
			color[i] = lotaWhite;

		//j = i;

		for (; i < theString.len(); i++)
			color[i] = lotaGreen;


		g.AddBottom(theString, color);

		break;
	}

}

void DrawSpecial ( LPDIRECTDRAWSURFACE7 pDDS )
{
	int i = 0;
	
	switch (g.commandMode)
	{
	case 'I':

		String		tempString;
		char		tempString2[40];
		unsigned int	bgcolor;
		unsigned int	fontcolor;

		// Clear the back buffer
		if (inventoryScreen == 0)
		{
			bgcolor = lotaBrown;
			fontcolor = lotaYellow;
		}
		else if (inventoryScreen == 1)
		{
			bgcolor = lotaBlue;
			fontcolor = lotaCyan;
		}

		// Clear the back buffer
		DDPutBox (pDDS, 0, 0, myWindowWidth, myWindowHeight, bgcolor);

		// Draw the borders
		DrawBorder(pDDS, lotaMdGray);
		DrawLine (pDDS, 0, 128, 1, myWindowWidth, lotaMdGray);

		DrawInnerBorder (pDDS, lotaYellow);
		DrawInnerLine (pDDS, 0, 128, 1, myWindowWidth, lotaYellow);

		// Draw the title
		DDPutBox (pDDS, 176, 0, 288, 16, bgcolor);
		WriteText (pDDS, 176, 0, " Player Inventory", fontcolor);

		// Draw the prompt
		DDPutBox (pDDS, 144, 384, 336, 16, bgcolor);
		WriteText (pDDS, 144, 384, " Hit key to continue", fontcolor);

		// Draw the top box
		WriteText (pDDS, 48, 32, g.player.name, fontcolor);

		tempString = "Level         ";
		tempString += g.player.level;
		WriteText (pDDS, 48, 64, tempString, fontcolor);

		tempString = "Time-days     ";
		tempString += g.player.TimeDays();
		WriteText (pDDS, 48, 96, tempString, fontcolor);

		tempString = "Dexterity     ";
		tempString += g.player.atr[0];
		WriteText (pDDS, 336, 32, tempString, fontcolor);

		tempString = "Strength      ";
		tempString += g.player.atr[1];
		WriteText (pDDS, 336, 48, tempString, fontcolor);

		tempString = "Charm         ";
		tempString += g.player.atr[2];
		WriteText (pDDS, 336, 64, tempString, fontcolor);

		tempString = "Endurance     ";
		tempString += g.player.atr[3];
		WriteText (pDDS, 336, 80, tempString, fontcolor);

		tempString = "Intelligence  ";
		tempString += g.player.atr[4];
		WriteText (pDDS, 336, 96, tempString, fontcolor);

		if (inventoryScreen == 0)
		{

			WriteText (pDDS, 80, 160, "Armor & Weapons  -  Quality", fontcolor);

			int yy = 11;
			unsigned long tempcolor;

			for (i = 1; i <= 5; i++)
			{
				if (g.player.weapon[i] > 0)
				{

					if (g.player.currentWeapon == i)
					{
						tempcolor = lotaWhite;
					}
					else
					{
						tempcolor = fontcolor;
					}

					LoadString (g.hInstance(), g.player.weapon[i], tempString2, 39);
					WriteText (pDDS, 128, ++yy * 16, tempString2, tempcolor);

					LoadString (g.hInstance(), g.player.weaponQuality[i] + 14, tempString2, 39);
					WriteText (pDDS, 416, yy * 16, tempString2, tempcolor);
				}

			}
			
			yy++;

			for (i = 1; i <= 3 ; i++)
			{
				if (g.player.armor[i] > 0)
				{

					if (g.player.currentArmor == i)
					{
						tempcolor = lotaWhite;
					}
					else
					{
						tempcolor = fontcolor;
					}

					LoadString (g.hInstance(), g.player.armor[i], tempString2, 39);
					WriteText (pDDS, 128, ++yy * 16, tempString2, tempcolor);

					LoadString (g.hInstance(), g.player.armorQuality[i] + 14, tempString2, 39);
					WriteText (pDDS, 416, yy * 16, tempString2, tempcolor);
				}

			}

		}
		else if (inventoryScreen == 1)
		{

			// Draw the middle prompt
			DDPutBox (pDDS, 160, 128, 288, 16, bgcolor);
			WriteText (pDDS, 160, 128, " Other Possesions", fontcolor);

			int yy = 9;
			int xx = 48;
			unsigned long tempcolor;

			for (i = 1; i <= 29; i++)
			{
				if (g.player.item[i] > 0)
				{
					if (g.player.hold == i)
					{
						tempcolor = lotaWhite;
					}
					else
					{
						tempcolor = fontcolor;
					}

					if (i == 17)
					{
						yy = 9;
						xx = 352;
					}
					if (i == 24)
					{
						yy++;
					}
					
					tempString = g.player.item[i];
					tempString += " ";

					LoadString (g.hInstance(), i + 19, tempString2, 39);
					
					tempString += tempString2;

					WriteText (pDDS, xx, ++yy * 16, tempString, tempcolor);
				}
			}

		}

		break;

	}

}