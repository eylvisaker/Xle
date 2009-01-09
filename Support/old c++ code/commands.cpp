
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

	if (cmd == VK_RETURN)
		cmd = g.cursor();


	String		theCommand("Enter Command: ");
	int			last = g.cursor();
	char		cursorKeys = 0;
	String		command;
	char		originalCmd = cmd;
	int			terrain = 0;
	int			i;
	int			newDirection;
	bool		badCommand = false;
	int			wasRaft = g.player.OnRaft();
	int			wasStormy = g.player.Stormy();
	bool		wasRoof[40];
	bool		roofChanged = false;
	int			whichRoofChanged = -1;
	int			start = clock();
	bool		changedMap = false;

	for (i = 0; i < 40; i++)
	{
		wasRoof[i] = g.map.IsOpen(i);
	}

	cmd = toupper(cmd);
	
	switch (g.commandMode)
	{
	case 0:
		if (g.stdDisplay == 'I')
			Inventory(cmd);
		
	case 2:
	case 3:
	case 4:
		
		break;
	case 5:
		
		g.ResetTimers();
		
		g.AddBottom("");
		g.AddBottom("Enter command:  ");
		
		g.commandMode = cmdEnterCommand;
		
		break;
		
	case 'I':
		//Inventory(cmd);
		
		break;
	case 10:
		if (cmd != 0)
		{
			g.ResetTimers();

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
					
					if (g.map.MapType() == mapOutside)
						g.waitCommand = g.walkTime;
					else
						g.waitCommand = g.walkTime;
					
					if (!g.Animating())
					{
						g.Animating(true);
						g.AnimFrame(animInc);
					}
					
					if (g.map.MapType() == mapDungeon)
					{
						g.waitCommand = 1;
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
						g.player.FaceDirection(lotaEast);
						
						command = "Move East";
						
						terrain = g.player.Move(1, 0);
						
						break;
						
					case mapTown:
					case mapCastle:
						g.player.FaceDirection(lotaEast);
						command = "Walk East";
						
						terrain = g.player.Move(1, 0);
						
						break;
						
					case mapDungeon:
					case mapMuseum:
						
						command = "Turn Right";
						
						g.menuKey = g.player.FaceDirection();
						newDirection = g.player.FaceDirection() - 1;
						
						if (newDirection < lotaEast)
							newDirection = lotaSouth;

						g.commandMode = cmd3;						
						
						g.player.FaceDirection((Direction)newDirection);

						break;
						
					}
					break;
				case lotaNorth:
					
					switch (g.map.MapType())
					{
					case mapOutside:
						g.player.FaceDirection(lotaNorth);
						command = "Move North";
						
						terrain = g.player.Move(0, -1);
						
						break;
						
					case mapTown:
					case mapCastle:
						g.player.FaceDirection(lotaNorth);
						command = "Walk North";
						
						terrain = g.player.Move(0, -1);
						break;
						
					case mapDungeon:
					case mapMuseum:
						command = "Move Forward";
						
						g.commandMode = cmd2;
						g.menuKey = 1;
						
						if (g.player.FaceDirection() == lotaEast)
							terrain = g.player.Move(1, 0);
						if (g.player.FaceDirection() == lotaNorth)
							terrain = g.player.Move(0, -1);
						if (g.player.FaceDirection() == lotaWest)
							terrain = g.player.Move(-1, 0);
						if (g.player.FaceDirection() == lotaSouth)
							terrain = g.player.Move(0, 1);
						
						break;
						
					}
					
					break;
					
				case lotaWest:
					switch (g.map.MapType())
					{
					case mapOutside:
						g.player.FaceDirection(lotaWest);
						g.raftFacing = lotaWest;
						
						command = "Move West";
						
						terrain = g.player.Move(-1, 0);
						break;
						
					case mapCastle:
					case mapTown:
						g.player.FaceDirection(lotaWest);
						command = "Walk West";
						
						terrain = g.player.Move(-1, 0);
						break;
						
					case mapDungeon:
					case mapMuseum:
						command = "Turn Left";
						
						g.menuKey = g.player.FaceDirection();
						newDirection = g.player.FaceDirection() + 1;
						
						g.commandMode = cmd3;
						
						if (newDirection > lotaSouth)
							newDirection = lotaEast;

						g.player.FaceDirection((Direction)newDirection);
						
						break;
						
					}
					
					break;
		
				case lotaSouth:
					
					switch (g.map.MapType())
					{
					case mapOutside:
						g.player.FaceDirection(lotaSouth);
						command = "Move South";
						
						terrain = g.player.Move(0,1);
						break;
						
					case mapCastle:
					case mapTown:
						g.player.FaceDirection(lotaSouth);
						command = "Walk South";
						
						terrain = g.player.Move(0,1);
						break;
						
					case mapDungeon:
					case mapMuseum:
						command = "Move Backward";
						g.commandMode = cmd2;
						g.menuKey = -1;
						
						if (g.player.FaceDirection() == lotaEast)
							terrain = g.player.Move(-1, 0);
						if (g.player.FaceDirection() == lotaNorth)
							terrain = g.player.Move(0, 1);
						if (g.player.FaceDirection() == lotaWest)
							terrain = g.player.Move(1, 0);
						if (g.player.FaceDirection() == lotaSouth)
							terrain = g.player.Move(0, -1);
						
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
				
				if (g.allowEnter)
				{
					theCommand += command;
					g.UpdateBottom(theCommand);
					g.currentCommand = cmd;
				}
				
				if (wasRaft != g.player.OnRaft())
				{
					if (g.player.OnRaft())
					{
						g.AddBottom ("");
						g.AddBottom ("You climb onto a raft.");
						LotaPlaySound(snd_BoardRaft);
						
					}
					
				}

				if (g.map.MapType() == mapTown || g.map.MapType() == mapCastle)
				{
					for (i = 0; i < 40; i++)
					{
						if (g.map.IsOpen(i) != wasRoof[i])
						{
							roofChanged = true;
							whichRoofChanged = int((i + 1) * (g.map.IsOpen(i) - 0.5) * 2);
						}
					}
				}

				if (roofChanged && g.map.MapType() == mapTown)
				{
					HeartBeat(redraw);

					if (whichRoofChanged > 0)
					{
						LotaPlaySound (snd_BuildingOpen);
					}
					else if (whichRoofChanged < 0)
					{
						LotaPlaySound (snd_BuildingClose);
					}

					g.waitCommand += 50;

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
				else if (terrain == 3)
				{
					LotaPlaySound (snd_Invalid);
					
					g.UpdateBottom("Enter command: Move nowhere");
					g.waitCommand = 75;
					
					
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
				else if (terrain == 47)
				{
					LotaPlaySound(snd_Bump);

					g.AddBottom("");
					g.AddBottom("Attempt to disengage");
					g.AddBottom("is blocked.");
					
					wait(500);
	
				}
				else
				{
					
					if (cursorKeys > 0)
					{
						if (g.map.EncounterState() == -1)
						{
							g.AddBottom("");
							g.AddBottom("Attempt to disengage");
							g.AddBottom("is successful.");


							wait(500);
						}

						if (g.map.MapType() == mapOutside)
						{
							
							if (!g.player.OnRaft())
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
								
								if (g.player.Stormy() != wasStormy || g.player.Stormy() >= 2)
								{
									if (g.player.Stormy() == 1 && wasStormy == 0)
									{
										g.AddBottom("");
										g.AddBottom("You are sailing into stormy water.", lotaYellow);
									}
									else if (g.player.Stormy() == 2 || g.player.Stormy() == 3)
									{
										g.AddBottom("");
										g.AddBottom("The water is now very rough.", lotaWhite);
										g.AddBottom("It will soon swamp your raft.", lotaYellow);
									}
									else if (g.player.Stormy() == 1 && wasStormy == 2)
									{
										g.AddBottom("");
										g.AddBottom("You are out of immediate danger.", lotaYellow);
									}
									else if (g.player.Stormy() == 0 && wasStormy == 1)
									{
										g.AddBottom("");
										g.AddBottom("You leave the storm behind.", lotaCyan);
									}
									
									if (g.player.Stormy() == 3)
									{
										g.AddBottom("");
										g.AddBottom("Your raft sinks.", lotaYellow);
										g.AddBottom("");
									}
									
									wait(1000);
									g.waitCommand = 0;
									
									if (g.player.Stormy() == 3)
									{
										g.player.Dead();
									}
								}
								
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
						else if (g.map.MapType() == mapTown || g.map.MapType() == mapCastle)
						{
							LotaPlaySound(snd_TownWalk);
							
						}
						if (g.map.CheckSpecial() && g.allowEnter )
						{
							
							SpecialEvent dave = g.map.GetSpecial();
							
							switch (dave.type)
							{
							case 1:
								String tempLine = "Enter ";
								String tempName;
								int choice = 1;

								MenuItemList theList(2, "Yes", "No");
								
								tempName = g.map.GetName(dave.data[0] * 256 + dave.data[1]);
																
								tempLine += tempName + "?";

								if (dave.data[6] < 11 || dave.data[6] == 32)
								{
									g.AddBottom("");
									g.AddBottom(tempLine);
									wait(1);
									
									LotaPlaySound (snd_Question);
									
									choice = QuickMenu(theList, 3);
								}
								else
								{
									// for the castle
									g.AddBottom("");
									g.AddBottom("Leave " + g.map.Name());

									g.AddBottom("");
									wait(500);

									choice = 0;
								}

								
								if (choice == 0)
								{
									//wait(500);
				
									g.waitCommand = 1;
									
									g.player.Map(dave.data[0] * 256 + dave.data[1]);
									//g.map.LoadMap();
									
									g.player.NewMap(dave.data[2] * 256 + dave.data[3], 
													dave.data[4] * 256 + dave.data[5]);
									
									g.ClearBottom();
									cursorKeys = 0;
									changedMap = true;
									
									if (g.map.HasSpecialType(storeLending))
									{
										if (g.player.loan > 0 && g.player.dueDate - g.player.TimeDays() <= 0)
										{
											g.AddBottom("This is your friendly lender.");
											g.AddBottom("You owe me money!");

											wait(1000);
											
										}
									}
								}
								else
								{
									start = clock();
								}

								break;
								
							}
							
							
						}
						
						g.waterReset = true;
						g.allowEnter = true;
					}
				}
				
				if (!badCommand || cmd == 'Z')
				{
					int a = 0;

					switch (cmd)
					{
					case 'A':
						Armor();
						break;
					case 'C':
						Climb();
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
						a = 1;
						break;
					}
					if (!a)
						start = clock();
				}
				else
				{
					if (cursorKeys == 0 && !changedMap)
					{
						LotaPlaySound (snd_Invalid);
						g.waitCommand = 0;
					}
				}

				if (g.waitCommand > 0)
					g.waitCommand += start - clock();
				
				g.map.Guards();
				g.map.TestEncounter(g.player.X(), g.player.Y(), cursorKeys);

				if (cursorKeys && (g.map.MapType() == mapDungeon || g.map.MapType() == mapMuseum))
				{
					g.waitCommand += 1;
				}

				// waits before the next command				
				
				wait(g.waitCommand);
				/*
				g.AddBottom("");
				g.AddBottom("Enter Command: ");
				*/
				g.commandMode = cmdPrompt;
				g.waitCommand = 0;
				
				g.ResetTimers();

				g.menuKey = 0;
				
				
			}
		}
		
		break;
		
	case 20:
		
		int dy = 0;
		
		// Test for cursor movement so we can move throught the menu command first
		switch (originalCmd)
		{
		case VK_UP:
			dy = -1;
			
			break;
			
		case VK_DOWN:
			dy = 1;
			
			break;
			
		}
		
		if (dy)
		{
			// TODO:  wtf???
			g.commandMode = (CmdMode)21;
			
			g.cursorPos(dy);
			wait(125);
			
			g.commandMode = (CmdMode)20;
			
		}
		
	}
	
}


void Armor()
{
	MenuItemList theList;
	String		 tempString;
	unsigned int colors[40];
	int			 value = 0;
	int			 j = 0;
	
	theList.AddItem("Nothing");
	
	for (int i = 1; i <= 5; i++)
	{
		if (g.player.ArmorType(i) > 0)
		{
			tempString = g.QualityName(g.player.ArmorQuality(i)) + " " + 
						 g.ArmorName(g.player.ArmorType(i));
			
			theList.AddItem (tempString);
			j++;
			
			if (g.player.CurrentArmor() == i)
				value = j;
			
		}
		
	}
	
	tempString = g.Bottom(0);
	
	for (i = 0; i < len(tempString); i++)
	{
		colors[i] = g.DefaultColor();
	}
	
	for (; i < 40; i++)
	{
		colors[i] = lotaCyan;
	}
	
	tempString += "-choose above";
	
	g.UpdateBottom(tempString, 0, colors);
	
	g.subMenu.title = "Pick Armor";
	g.subMenu.value = value;
	
	g.player.CurrentArmor(SubMenu(theList));
	
}

void Climb()
{
	bool NothingToClimb = false;
	
	switch (g.map.MapType())
	{
	case mapDungeon:
		switch (g.map.M(g.player.Y(), g.player.X()))
		{
		case 0x0D:
			g.player.DungeonLevel(g.player.DungeonLevel() - 1);
			
			break;
		case 0x0A:
			g.player.DungeonLevel(g.player.DungeonLevel() + 1);
			
			break;
		default:
			NothingToClimb = true;
			break;
		}
		
		if (!NothingToClimb)
		{
			
			if (g.player.DungeonLevel() == 0)
			{
				g.AddBottom("");
				g.AddBottom("You climb out of the dungeon.");
				
				g.map.LoadMap(1);
				g.player.NewMap();
				
			}
			else
			{
				String tempString = "You are now at level " + String(g.player.DungeonLevel()) + String(".");
				
				g.AddBottom("");
				g.AddBottom(tempString, lotaWhite);
				
				delete g.d3dFloor;
				g.d3dFloor = NULL;
				
			}
		}
		
		break;
		default:
			NothingToClimb = true;
			
			break;
	}
	
	if (NothingToClimb)
	{
		g.AddBottom("");
		g.AddBottom("Nothing to climb");
	}
}
void Disembark()
{
	int newx, newy;
	
	g.UpdateBottom ("Enter command: disembark raft");
	
	if (g.player.OnRaft())
	{
		
		
		g.AddBottom("");
		g.AddBottom("Disembark in which direction?");
		
		g.quickMenu = true;
		
		do
		{
			wait(1);
			
		} while (g.menuKey != VK_LEFT && g.menuKey != VK_RIGHT && g.menuKey != VK_UP && g.menuKey != VK_DOWN);
		
		newx = g.player.X();
		newy = g.player.Y();
		
		Direction dir;
		
		switch (g.menuKey)
		{
		case VK_LEFT:
			dir = lotaWest;
			break;
			
		case VK_UP:
			dir = lotaNorth;
			break;
			
		case VK_DOWN:
			dir = lotaSouth;
			break;
			
		case VK_RIGHT:
			dir = lotaEast;
			break;
		}
		
		g.quickMenu = false;
		g.commandMode = cmdEnterCommand;
		
		g.player.Disembark(dir);
		
		LotaStopSound(snd_Raft1);
		
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
	MenuItemList menu(2, "Yes", "No");
	int choice;

	g.AddBottom("");
	g.AddBottom("Would you like to save");
	g.AddBottom("the game in progress?");
	g.AddBottom("");

	choice = QuickMenu(menu, 2);

	if (choice == 0)
	{
		g.player.SaveGame();
	}

	
}

void Fight()
{
	String tempString;
	String weaponName;
	POINT attackPt, attackPt2;
	int i = 0, j = 0;
	int dx = 0, dy = 0;
	bool attacked = false;
	int maxXdist = 1;
	int maxYdist = 1;
	int dam = 0;
	int tile = 0, tile1;
	int hit = 0;
	unsigned int colors[40];

	
	weaponName = space(25);
	
	LoadString(g.hInstance(), g.player.WeaponType(g.player.CurrentWeapon()), weaponName, 25);
	weaponName = rtrim(weaponName);
	
	if (weaponName == "")
		weaponName = "Bare hands";
			

	if (g.player.WeaponType(g.player.CurrentWeapon()) == 6 || 
		g.player.WeaponType(g.player.CurrentWeapon()) == 8)
	{
		maxXdist = 12;
		maxYdist = 8;
	}
	
	switch (g.map.MapType())
	{
		case mapTown:
		case mapCastle:
			
			g.AddBottom("");
			
			tempString = space(25);
			
			LoadString(g.hInstance(), g.player.WeaponType(g.player.CurrentWeapon()), tempString, 25);
			tempString = rtrim(tempString);
			
			if (tempString == "")
				tempString = "Bare hands";
			
			tempString = String("Fight with ") + tempString;
			
			g.AddBottom(tempString);
			
			tempString = "Enter direction: ";
			g.AddBottom(tempString);
			
			g.quickMenu = true;
			
			do
			{
				g.menuKey = 0;
				wait(1);
				
			} while (g.menuKey != VK_LEFT && g.menuKey != VK_RIGHT && g.menuKey != VK_UP &&
				g.menuKey != VK_DOWN && !g.done);
			
			g.quickMenu = false;
			
			attackPt.x = g.player.X();
			attackPt.y = g.player.Y();
			attackPt2.x = attackPt.x;
			attackPt2.y = attackPt.y;
			
			switch (g.menuKey)
			{
			case VK_RIGHT:
				tempString += "east";
				g.player.FaceDirection(lotaEast);
				attackPt.x++;
				attackPt2.x++;
				attackPt2.y++;
				dx = 1;
				break;
			case VK_UP:
				tempString += "north";
				g.player.FaceDirection(lotaNorth);
				dy = -1;
				attackPt2.x++;
				break;
			case VK_LEFT:
				tempString += "west";
				g.player.FaceDirection(lotaWest);
				dx = -1;
				attackPt2.y++;
				break;
			case VK_DOWN:
				tempString += "south";
				g.player.FaceDirection(lotaSouth);
				dy = 1;
				attackPt.y++;
				attackPt2.x++;
				attackPt2.y++;
				break;
			}
			
			g.UpdateBottom(tempString);
			
			for (i = 1; i <= maxXdist && attacked == false && tile < 128; i++)
			{
				for (j = 1; j <= maxYdist && attacked == false && tile < 128; j++)
				{
					
					for (int k = 0; k < 101; k++)
					{
						if((g.map.GuardPos(k).x == attackPt.x + dx * i
							|| g.map.GuardPos(k).x + 1 == attackPt.x + dx * i
							|| g.map.GuardPos(k).x == attackPt2.x + dx * i
							|| g.map.GuardPos(k).x + 1 == attackPt2.x + dx * i
							)
							&& 
							(g.map.GuardPos(k).y == attackPt.y + dy * j 
							|| g.map.GuardPos(k).y + 1 == attackPt.y + dy * j 
							|| g.map.GuardPos(k).y == attackPt2.y + dy * j 
							|| g.map.GuardPos(k).y + 1 == attackPt2.y + dy * j 
							)
							&&
							attacked == false)
						{
							g.map.AttackGuard(k);
							attacked = true;
						}
					}

					tile = g.map.M(attackPt.y + dy, attackPt.x + dx);
					tile1 = g.map.M(attackPt2.y + dy, attackPt2.x + dx);

					k = g.map.RoofTile (attackPt.x + dx, attackPt.y + dy);
					if (k != 127)
						tile = 128;
					
					k = g.map.RoofTile (attackPt2.x + dx, attackPt2.y + dy);
					if (k != 127)
						tile1 = 128;

					
					if (tile == 222 || tile == 223 || tile == 238 || tile == 239)
					{
						hit = 1;
					}
					else if (tile1 == 222 || tile1 == 223 || tile1 == 238 || tile1 == 239)
					{
						hit = 2;
					}
					if (hit)
					{
						if (hit == 1)
						{
							if (tile == 223)
							{
								attackPt.x--;
							}
							else if (tile == 238)
							{
								attackPt.y--;
							}
							else if (tile == 239)
							{
								attackPt.x--;
								attackPt.y--;
							}
						}
						else if (hit == 2)
						{
							attackPt = attackPt2;
							
							if (tile1 == 223)
							{
								attackPt.x--;
							}
							else if (tile1 == 238)
							{
								attackPt.y--;
							}
							else if (tile1 == 239)
							{
								attackPt.x--;
								attackPt.y--;
							}
						}
						
						dam = rnd(30, 39);
						
						tempString = (String)"Merchant killed by blow of " + (String)dam;
						
						g.AddBottom("");
						g.AddBottom(tempString);
						
						g.map.SetM(attackPt.y + dy, attackPt.x + dx, 0x52);
						g.map.SetM(attackPt.y + dy + 1, attackPt.x + dx, 0x52);
						g.map.SetM(attackPt.y + dy + 1, attackPt.x + dx + 1, 0x52);
						g.map.SetM(attackPt.y + dy, attackPt.x + dx + 1, 0x52);
						
						g.map.IsAngry(true);
						
						LotaPlaySound(snd_EnemyDie);
						
						attacked = true;
						
						
					}

					if (tile == 176 || tile1 == 176 || tile == 192 || tile == 192)
					{
						g.AddBottom("The prison bars hold.");

						LotaPlaySound(snd_Bump);

						attacked = true;
					}
				}
				
			}
			
			if (attacked == false)
			{
				g.AddBottom("Nothing hit");
			}
			
			wait (200 + 50 * g.player.Gamespeed(), true);
			
			break;
		
		case mapOutside:


			g.AddBottom("");
			

			if (g.map.EncounterState() == 10)
			{
				int dam = g.map.attack();

				for (i = 0; i < 7; i++)
					colors[i] = lotaWhite;
				for (; i < 39; i++)
					colors[i] = lotaCyan;

				g.AddBottom("Attack " + g.map.MonstName(), colors);

				for (i = 5; i < 39; i++)
					colors[i] = lotaCyan;

				g.AddBottom("With " + weaponName, colors);

				if (dam)
				{
					LotaPlaySound(snd_PlayerHit);
					tempString = "Enemy hit by blow of ";

					for (i = 0; i < len(tempString); i++)
						colors[i] = lotaWhite;

					tempString += String (dam);

					colors[len(tempString)] = lotaWhite;

					tempString += ".";

					g.AddBottom(tempString, colors);
				}
				else
				{
					LotaPlaySound(snd_PlayerMiss);

					g.AddBottom("Your Attack missed.", lotaYellow);
				}

				wait (250 + 100 * g.player.Gamespeed(), true);

				if (g.map.KilledOne())
				{
					LotaPlaySound(snd_EnemyDie);

					g.AddBottom("");
					g.AddBottom("the " + g.map.MonstName() + " dies.");

					int gold, food;
					bool finished = g.map.FinishedCombat(gold, food);

					wait (500 + 100 * g.player.Gamespeed());

					if (finished)
					{
						g.AddBottom("");

						if (food)
						{
 							MenuItemList menu(2, "Yes", "No");
							int choice;

							g.AddBottom("Would you like to use the");
							g.AddBottom(g.map.MonstName() + "'s flesh for food?");
							g.AddBottom("");

							choice = QuickMenu(menu, 3, 0);

							if (choice == 1)
								food = 0;
							else
							{
								for (i = 0; i < 40; i++)
									colors[i] = lotaWhite;

								for (i = 0; i <= int(log10(food)); i++)
									colors[i + 9] = lotaGreen;
								

								g.AddBottom("You gain " + String(food) + " days of food.", colors);

								g.player.Food(food);
							}

						}
						

						if (gold < 0)
						{
							// gain weapon or armor
						}
						else if (gold > 0)
						{
							for (i = 0; i < 40; i++)
								colors[i] = lotaWhite;

							for (i = 0; i <= int(log10(gold)); i++)
								colors[i + 9] = lotaYellow;
							
							g.AddBottom("You find " + String(gold) + " gold.", colors);
							g.player.GainGold(gold);


						}

						wait (400 + 100 * g.player.Gamespeed());

					}

				}

			}
			else if (g.map.EncounterState())
			{
				g.AddBottom("The unknown creature is not ");
				g.AddBottom("within range.");

				wait (300 + 100 * g.player.Gamespeed());
			}
			else
			{
				g.AddBottom("Nothing to fight.");

				wait (300 + 100 * g.player.Gamespeed());
			}

			break;
	}
	
	g.commandMode = cmdEnterCommand;
	
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
	
	g.player.Gamespeed(1 + QuickMenu(theList, 2, g.player.Gamespeed() - 1));
	
	strtmp += g.player.Gamespeed();
	
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
	
	g.commandMode = cmdBad;
	wait(500 + 100 * g.player.Gamespeed());
	g.commandMode = cmdEnterCommand;
	
	
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
		if (g.player.Item(i) > 0)
		{
			LoadString(g.hInstance(), i + 19, tempChars, 40);
			
			if (i == 9)			// mail
			{
				tempString = g.map.GetName(g.player.mailTown) + String(" ") + tempChars;
			}
			else
			{
				tempString = tempChars;
			}
			
			if (i <= g.player.Hold())
			{
				value++;
			}
			
			theList.AddItem (tempString);
		}
		
	}
	
	g.subMenu.title = "Hold Item";
	g.subMenu.value = value;
	
	tempString = g.Bottom(0);
	
	for (i = 0; i < len(tempString); i++)
	{
		colors[i] = g.DefaultColor();
	}
	
	for (; i < 40; i++)
	{
		colors[i] = lotaCyan;
	}
	
	tempString += "-choose above";
	
	g.UpdateBottom(tempString, 0, colors);
	
	g.player.HoldMenu(SubMenu(theList));
	
}

void Inventory(int mode)
{
	static bool waiting = false;

	if (mode == 0 || mode == 1)
	{
		
		g.commandMode = cmdBad;
		
		g.stdDisplay = 'I';
		
		if (mode == 1)
		{
			g.DestroyTimers();
			inventoryScreen = 0;
		}
		
		
	}
	else if (mode >= 8)
	{
	
		if (!waiting)
		{
			waiting = true;

			inventoryScreen++;
			
			if (inventoryScreen > 1)
			{
				inventoryScreen = 0;
				
				g.ResetTimers();
				g.ClearBottom();
				g.stdDisplay = 0;
				
				wait(250);

				g.commandMode = cmdPrompt;
				
			}

			wait(250);

			waiting = false;

		}
		
	}
	
}

void Leave()
{
	
	if (g.map.IsAngry())
	{
		g.AddBottom("");
		g.AddBottom("Walk out yourself.");
	}
	else
	{
		String name = g.map.Name();
		name = String("Leave ") + name;

		g.AddBottom("");
		g.AddBottom(name);
		g.AddBottom("");

		//g.commandMode = cmdBad;
		wait(200);
		//g.commandMode = cmdEnterCommand;

		g.player.Map(g.player.LastMap());
		g.player.NewMap();
		
	}
	
}

void Magic()
{
}

void Open(bool take)
{
	bool found = false;
	int type;

	for (int j = -1; j < 3; j++)
	{
		for (int i = -1; i < 3; i++)
		{
			type = g.map.CheckSpecial(g.player.X() + i, g.player.Y() + j);
			SpecialEvent dave = g.map.GetSpecial(g.player.X() + i, g.player.Y() + j);				

			if (((type == 23 && !take) || (type == 25 && take)) && found == false && dave.marked == false)
			{
				found = true;

				if (!take)
				{
					g.UpdateBottom("Enter Command: Open Chest");
					LotaPlaySound(snd_OpenChest);

					wait(750);

					for (j = dave.sy; j < dave.sy + dave.sheight; j++)
					{
						for (i = dave.sx; i < dave.sx + dave.swidth; i++)
						{
							int m = g.map.M(j, i);
							
							if (m % 16 >= 11 && m % 16 < 14 && m / 16 >= 13 && m / 16 < 15)
							{
								g.map.SetM(j, i, m - 3);
							}

						}
					}

					g.map.IsAngry(1);
				}

				if (dave.data[0] == 0)
				{
					char tempChars[40];
					int count = 1;

					LoadString (g.hInstance(), dave.data[1] + 19, tempChars, 40);
	
					if (dave.data[1] == 8)
						count = rnd(3, 5);

					g.player.ItemCount(dave.data[1], count);
					
					g.AddBottom("");


					if (!take)
					{
						g.AddBottom("You find a " + String(tempChars) + "!");
						LotaPlaySound(snd_VeryGood);
					}
					else
						g.AddBottom("You take " + String(count) + " " + String(tempChars) + ".");


				}
				else if (dave.data[0] == 1)
				{
					int gd;

					if (dave.data[1] == 0 && dave.data[2] <= 50)
						gd = rnd(dave.data[2] * 100, (dave.data[2] + 1) * 100);
					else
						gd = dave.data[1] * 256 + dave.data[2];

					g.AddBottom("");
					g.AddBottom("You find " + String(gd) + " gold.");

					g.player.GainGold(gd);
					LotaPlaySound(snd_Sale);

				}

				
				g.map.MarkSpecial(dave);

			}
		}
	}

	if (!found && !take)
	{

		g.AddBottom("");
		g.AddBottom("Nothing opens.");
		wait(500);
	
	}
	else if (!found && take)
	{
		g.AddBottom("");
		g.AddBottom("Nothing to take.");
		wait(500);
	}


}

void Pass()
{
}

void Rob()
{
	bool found = false;
	int i, j;
	
	for (j = -1; j < 3; j++)
	{
		for (i = -1; i < 3; i++)
		{
			if (g.map.CheckSpecial(g.player.X() + i, g.player.Y() + j) >= 2 && found == false)
			{
				SpecialEvent dave = g.map.GetSpecial(g.player.X() + i, g.player.Y() + j);
				
				found = true;

				Store(dave, true);

			}
		}
	}
	
}

void Speak()
{
	int i, j, k;
	bool found = false;
	String name;

	for (j = -1; j < 3; j++)
	{
		for (i = -1; i < 3; i++)
		{
			int sType = g.map.CheckSpecial(g.player.X() + i, g.player.Y() + j);
			
			if (sType >= 2 && sType < 20 && found == false && !g.map.IsAngry())
			{
				SpecialEvent dave = g.map.GetSpecial(g.player.X() + i, g.player.Y() + j);
				found = true;
				
				Store(dave);
				
			}
			else if (sType == 21 && found == false)
			{
				SpecialEvent dave = g.map.GetSpecial(g.player.X() + i, g.player.Y() + j);
				found = true;

				switch (dave.data[0])
				{
				case 1:			// casandra

					Casandra();

					break;

				case 2:			// wizard of potions


					break;
				}
			}

		}
	}

	if (g.map.MapType() == mapOutside)
	{
		if (g.map.EncounterState() == 0)
		{
			found = false;
		}
		else if (g.map.EncounterState() < 10)
		{
		}
		else if (g.map.EncounterState() == 10)
		{
			found = true;

			g.map.SpeakToMonster();

		}
		

	}


	if (!found)
	{
		for (j = -1; j < 3; j++)
		{
			for (i = -1; i < 3; i++)
			{
				for (k = 0; k < 101; k++)
				{
					if((g.map.GuardPos(k).x == g.player.X() + i ||
						g.map.GuardPos(k).x + 1 == g.player.X() + i) && (
						g.map.GuardPos(k).y == g.player.Y() + j ||
						g.map.GuardPos(k).y + 1 == g.player.Y() + j) && 							
						found == false)
					{
						switch (g.map.MapType())
						{
							case mapTown:
								g.AddBottom("");
								g.AddBottom("The guard salutes.");
								found = true;
								break;

							case mapCastle:
								g.AddBottom("");

								if (!g.invisible && !g.guard)
								{
									g.AddBottom("The guard ignores you.");
								}
								else if (g.invisible)
								{
									if (rnd(0, 999) < 998)
										g.AddBottom("The guard looks startled.");
									else
									{
										g.AddBottom("The guard looks startled,");
										g.AddBottom("and starts popping prozac pills.");
									}
								}
								else if (g.guard)  // for fortress
								{

								}
								

								found = true;
								break;

						}
					}
				}
			}
		}
	}

	if (found == false)
	{
		g.AddBottom("");
		g.AddBottom("No response");
	}
}

void Take()
{
	Open(true);
}

void Use()
{
	String commandString;
	char	tempChars[40];
	int i, j;
	unsigned int lastColor, firstColor = g.HPColor;
	bool found = false;

	bool noEffect = true;
	
	g.commandMode = cmdBad;
	
	g.AddBottom("");

	switch (g.player.Hold())
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

	case 3:

		commandString = "Eat Healing Herbs";
		g.AddBottom(commandString);

		g.player.HP(g.player.MaxHP() / 2);
		g.player.ItemCount(3, -1);
        LotaPlaySound(snd_Good);

		lastColor = lotaWhite;

		do
		{
			if (lastColor == lotaWhite)
				lastColor = lotaCyan;
			else
				lastColor = lotaWhite;

			g.HPColor = lastColor;

			wait(25);

		} while (LotaGetSoundStatus(snd_Good) == DSBSTATUS_PLAYING);

		g.HPColor = firstColor;

		noEffect = false;
		break;
	
	case 4:			// Iron Key
	case 5:			// Copper Key
	case 6:			// Brass Key
	case 7:			// Stone Key
		
		commandString = "Use ";
		LoadString (g.hInstance(), g.player.Hold() + 19, tempChars, 40);
		commandString += tempChars;

		for (j = -1; j < 3; j++)
		{
			for (i = -1; i < 3; i++)
			{
				if (g.map.CheckSpecial(g.player.X() + i, g.player.Y() + j) == 24 && found == false)
				{
					SpecialEvent dave = g.map.GetSpecial(g.player.X() + i, g.player.Y() + j);
					
					if (dave.data[0] == g.player.Hold())
					{
						found = true;

						g.AddBottom(commandString);
						LotaPlaySound(snd_UnlockDoor);
						wait(250);

						g.AddBottom("Unlock door");

						for (j = dave.sy; j < dave.sy + dave.sheight; j++)
						{
							for (i = dave.sx; i < dave.sx + dave.swidth; i++)
							{
								int m = g.map.M(j, i);
								
								if ((m % 16 < 4 && m / 16 == 13) || (m % 16 >= 2 && m % 16 < 4 && m / 16 == 14))
								{
									g.map.SetM(j, i, 0);
								}

							}
						}

						wait(750);

						break;
					}
					
				}
			}

			if (found) break;
		}

		if (found)
		{
			noEffect = false;
		}

		if (noEffect)
		{
			g.AddBottom (commandString);
			g.AddBottom ("");
			wait (400 + 100 * g.player.Gamespeed());
			g.UpdateBottom ("This key does nothing here.");

			noEffect = false;

		}
		break;

	case 8:				// magic seeds
		commandString = "Eat Magic Seeds";
		
		g.AddBottom(commandString);
		wait(150);

		g.invisible = true;
		g.AddBottom("You're invisible.");

		g.map.IsAngry(0);

		wait(500);

		g.player.ItemCount(8, -1);
		noEffect = false;

		break;

	case 12:				// magic ice

		commandString = "Throw magic ice";

		for (j = -1; j < 3; j++)
		{
			for (i = -1; i < 3; i++)
			{
				if (g.map.CheckSpecial(g.player.X() + i, g.player.Y() + j) == 22 && found == false)
				{
					SpecialEvent dave = g.map.GetSpecial(g.player.X() + i, g.player.Y() + j);
					found = true;

					g.AddBottom(commandString);
					wait(500);

					for (j = dave.sy; j < dave.sy + dave.sheight; j++)
					{
						for (i = dave.sx; i < dave.sx + dave.swidth; i++)
						{
							int m = g.map.M(j, i);
							
							if (m % 16 >= 13 && m / 16 <= 2)
							{
								g.map.SetM(j, i, m - 8);
							}

						}
					}
					
					break;
				}
			}

			if (found) break;
		}

		if (found)
		{
			noEffect = false;
		}

		break;


	case 9:				// mail
	case 10:			// tulip
	case 11:			// compass
	case 13:			// scepter
	case 14:			// guard jewel
	case 15:			// compendium
	case 16:			// crown

	default:
		commandString = "Use ";
		LoadString (g.hInstance(), g.player.Hold() + 19, tempChars, 40);
		commandString += tempChars;
		
		noEffect = true;
		
		break;
	}
	
	if (noEffect == true)
	{
		g.AddBottom (commandString);
		g.AddBottom ("");
		wait (400 + 100 * g.player.Gamespeed());
		g.UpdateBottom ("No effect");
		
		
	}
	
	g.commandMode = cmdEnterCommand;
	
}

void Weapon()
{
	MenuItemList theList;
	String		 tempString;
	unsigned int colors[40];
	int			 value = 0;
	int			 j = 0;
	theList.AddItem("Nothing");
	
	for (int i = 1; i <= 5; i++)
	{
		if (g.player.WeaponType(i) > 0)
		{
			tempString = g.QualityName(g.player.WeaponQuality(i)) + " " +
						 g.WeaponName(g.player.WeaponType(i));
			
			theList.AddItem (tempString);
			j++;
			
			if (g.player.CurrentWeapon() == i)
			{
				value = j;
			}
		}
		
	}
	
	tempString = g.Bottom(0);
	
	for (i = 0; i < len(tempString); i++)
	{
		colors[i] = g.DefaultColor();
	}
	
	for (; i < 40; i++)
	{
		colors[i] = lotaCyan;
	}
	
	tempString += "-choose above";
	
	g.UpdateBottom(tempString, 0, colors);
	
	g.subMenu.title = "Pick Weapon";
	g.subMenu.value = value;
	
	g.player.CurrentWeapon(SubMenu(theList));
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
		
		theString = "You are in " + terrain + (String)".";
		
		g.AddBottom (theString);
		
		theString = "Travel: " + travel + (String)"  -  Food use: " + food;

		for (i = 0; i < 8; i++)
			color[i] = lotaWhite;
		
		j = i;
		
		for (; i < j + len(travel); i++)
			color[i] = lotaGreen;
		
		j = i;
		
		for (; i < j + 15; i++)
			color[i] = lotaWhite;
		
		//j = i;
		
		for (; i < len(theString); i++)
			color[i] = lotaGreen;
		
		
		g.AddBottom(theString, color);
		
		break;
		
		case mapTown:
			g.AddBottom("");
			g.AddBottom(String ("You are in ") + g.map.Name() + ".");
			g.AddBottom("Look about to see more.");
			
			
	}
	
}

void DrawSpecial ( LPDIRECTDRAWSURFACE7 pDDS )
{
	int i = 0;
	
	switch (g.stdDisplay)
	{
	case 'S':
		DrawStore(pDDS);
		break;
		
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
		WriteText (pDDS, 48, 32, g.player.Name(), fontcolor);
		
		tempString = "Level         ";
		tempString += g.player.Level();
		WriteText (pDDS, 48, 64, tempString, fontcolor);
		
		tempString = "Time-days     ";
		tempString += g.player.TimeDays();
		WriteText (pDDS, 48, 96, tempString, fontcolor);
		
		tempString = "Dexterity     ";
		tempString += g.player.Attribute(0);
		WriteText (pDDS, 336, 32, tempString, fontcolor);
		
		tempString = "Strength      ";
		tempString += g.player.Attribute(1);
		WriteText (pDDS, 336, 48, tempString, fontcolor);
		
		tempString = "Charm         ";
		tempString += g.player.Attribute(2);
		WriteText (pDDS, 336, 64, tempString, fontcolor);
		
		tempString = "Endurance     ";
		tempString += g.player.Attribute(3);
		WriteText (pDDS, 336, 80, tempString, fontcolor);
		
		tempString = "Intelligence  ";
		tempString += g.player.Attribute(4);
		WriteText (pDDS, 336, 96, tempString, fontcolor);
		
		if (inventoryScreen == 0)
		{
			
			WriteText (pDDS, 80, 160, "Armor & Weapons  -  Quality", fontcolor);
			
			int yy = 11;
			unsigned long tempcolor;
			
			for (i = 1; i <= 5; i++)
			{
				if (g.player.WeaponType(i) > 0)
				{
					
					if (g.player.CurrentWeapon() == i)
					{
						tempcolor = lotaWhite;
					}
					else
					{
						tempcolor = fontcolor;
					}
					
					WriteText (pDDS, 128, ++yy * 16, g.WeaponName(g.player.WeaponType(i)), tempcolor);
					WriteText (pDDS, 416, yy * 16, g.QualityName(g.player.WeaponQuality(i)), tempcolor);
				}
				
			}
			
			yy++;
			
			for (i = 1; i <= 3 ; i++)
			{
				if (g.player.ArmorType(i) > 0)
				{
					
					if (g.player.CurrentArmor() == i)
					{
						tempcolor = lotaWhite;
					}
					else
					{
						tempcolor = fontcolor;
					}
					
					WriteText (pDDS, 128, ++yy * 16, g.ArmorName(g.player.ArmorType(i)), tempcolor);
					WriteText (pDDS, 416, yy * 16, g.QualityName(g.player.ArmorQuality(i)), tempcolor);
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
				if (g.player.Item(i) > 0)
				{
					if (g.player.Hold() == i)
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
					
					tempString = String(g.player.Item(i)) + " ";
					
					if (i == 9)			// mail
					{
						tempString += g.map.GetName(g.player.mailTown) + " ";
					}

					LoadString (g.hInstance(), i + 19, tempString2, 39);
					
					tempString += tempString2;
					
					WriteText (pDDS, xx, ++yy * 16, tempString, tempcolor);
				}
			}
			
		}
		
		break;
		
	}
	
}