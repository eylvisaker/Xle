using System;
using System.Collections.Generic;
using System.Text;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.Geometry;

namespace ERY.Xle
{
	public class Commands
	{
		//int			inventoryScreen = 0;
		Player player;

		public Commands(Player p)
		{
			player = p;
		}

		public void Prompt()
		{
			player.CheckDead();

			g.AddBottom("");
			g.AddBottom("Enter command:  ");

		}

		private bool CursorMovement(KeyCode cmd)
		{
			Direction dir;

			// Test for cursor movement so we can get the right command first
			switch (cmd)
			{
				case KeyCode.Right:
					dir = Direction.East;

					break;

				case KeyCode.Up:
					dir = Direction.North;

					break;

				case KeyCode.Left:
					dir = Direction.West;

					break;

				case KeyCode.Down:
					dir = Direction.South;

					break;

				default:
					return false;
			}

			XleCore.Map.PlayerCursorMovement(player, dir);



			return true;


		}



		public void DoCommand(KeyCode cmd)
		{
			if (cmd == KeyCode.None)
				return;

			int wasRaft = player.OnRaft;
			int waitTime = 700;

			if (CursorMovement(cmd))
			{
				if (g.Animating == false)
				{
					g.Animating = true;
					g.AnimFrame = 0;
				}

				waitTime = g.walkTime + XleCore.Map.TerrainWaitTime(player);

				g.charAnimCount = 0;


				if (wasRaft != player.OnRaft)
				{
					if (player.IsOnRaft)
					{
						g.AddBottom("");
						g.AddBottom("You climb onto a raft.");

						SoundMan.PlaySound(LotaSound.BoardRaft);
					}

				}

				// check for events
				XleCore.Map.PlayerStep(player);

				AfterDoCommand(waitTime, cmd);


				return;
			}

			if (XleCore.Menu(cmd) == "")
			{
				SoundMan.PlaySound(LotaSound.Invalid);

				XleCore.wait(waitTime);
				return;
			}
			else
			{
				UpdateCommand(XleCore.Menu(cmd));


				switch (cmd)
				{
					case KeyCode.A:
						Armor();
						break;
					case KeyCode.C:
						Climb();
						break;
					case KeyCode.D:
						Disembark();
						break;
					case KeyCode.E:
						End();
						break;
					case KeyCode.F:
						Fight();
						break;
					case KeyCode.G:
						GameSpeed();
						break;
					case KeyCode.H:
						Hold();
						break;
					case KeyCode.I:
						Inventory();
						break;
					case KeyCode.L:
						Leave();
						break;
					case KeyCode.M:
						Magic();
						break;
					case KeyCode.O:
						Open();
						break;
					case KeyCode.P:
						Pass();
						break;
					case KeyCode.R:
						Rob();
						break;
					case KeyCode.S:
						Speak();
						break;
					case KeyCode.T:
						Take();
						break;
					case KeyCode.U:
						Use();
						break;
					case KeyCode.W:
						Weapon();
						break;
					case KeyCode.X:
						Xamine();
						break;
					default:
						break;
				}
			}



			AfterDoCommand(waitTime, cmd);

		}

		public static void UpdateCommand(string command)
		{
			g.UpdateBottom("Enter Command: " + command);
		}


		private void AfterDoCommand(int waitTime, KeyCode cmd)
		{
			XleCore.Map.AfterExecuteCommand(player, cmd);

			XleCore.wait(XleCore.Redraw, waitTime, false);
			Prompt();
		}


		/*
	case 20:
		
		int dy = 0;
		
		// Test for cursor movement so we can move throught the menu command first
		switch (originalCmd)
		{
		case KeyCode.UP:
			dy = -1;
			
			break;
			
		case KeyCode.Down:
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
		
	}*/


		public void Armor()
		{
			MenuItemList theList = new MenuItemList();
			string tempstring;
			int value = 0;
			int j = 0;

			theList.Add("Nothing");

			for (int i = 1; i <= 5; i++)
			{
				if (player.ArmorType(i) > 0)
				{
					tempstring = XleCore.QualityList[player.ArmorQuality(i)] + " " +
								 XleCore.ArmorList[player.ArmorType(i)].Name;

					theList.Add(tempstring);
					j++;

					if (player.CurrentArmor == i)
						value = j;

				}

			}

			ColorStringBuilder builder = new ColorStringBuilder();
			builder.AddText(g.Bottom(0), XleCore.Map.DefaultColor);
			builder.AddText("-choose above", XleColor.Cyan);

			g.UpdateBottom(builder, 0);

			player.CurrentArmor = XleCore.SubMenu("Pick Armor", value, theList);

		}

		public void Climb()
		{
			if (XleCore.Map.PlayerClimb(player) == false)
			{
				g.AddBottom("");
				g.AddBottom("Nothing to climb");
			}
		}
		public void Disembark()
		{
			int newx, newy;

			g.UpdateBottom("Enter command: disembark raft");

			if (player.IsOnRaft)
			{

				g.AddBottom("");
				g.AddBottom("Disembark in which direction?");

				do
				{
					XleCore.Redraw();

				} while (!(
					Keyboard.Keys[KeyCode.Left] || Keyboard.Keys[KeyCode.Right] ||
					Keyboard.Keys[KeyCode.Up] || Keyboard.Keys[KeyCode.Down]));

				newx = player.X;
				newy = player.Y;

				Direction dir = Direction.East;

				if (Keyboard.Keys[KeyCode.Left])
					dir = Direction.West;
				else if (Keyboard.Keys[KeyCode.Up])
					dir = Direction.North;
				else if (Keyboard.Keys[KeyCode.Down])
					dir = Direction.South;
				else if (Keyboard.Keys[KeyCode.Right])
					dir = Direction.East;

				player.Disembark(dir);

				SoundMan.StopSound(LotaSound.Raft1);

			}
			else
			{
				g.AddBottom("");
				g.AddBottom("Nothing to disembark", XleColor.Yellow);

			}
		}
		public void End()
		{
			MenuItemList menu = new MenuItemList("Yes", "No");
			int choice;
			bool saved = false;

			g.AddBottom("");
			g.AddBottom("Would you like to save");
			g.AddBottom("the game in progress?");
			g.AddBottom("");

			choice = XleCore.QuickMenu(menu, 2);

			if (choice == 0)
			{
				player.SavePlayer();

				saved = true;

				g.AddBottom("Game Saved.");
				g.AddBottom("");
			}
			else
			{
				ColorStringBuilder builder = new ColorStringBuilder();

				builder.AddText("Game ", XleColor.White);
				builder.AddText("not", XleColor.Yellow);
				builder.AddText(" saved.", XleColor.White);

				g.AddBottom(builder);
				g.AddBottom("");
			}

			XleCore.wait(1500);

			g.AddBottom("Quit and return to title screen?");

			if (saved == false)
				g.AddBottom("Unsaved progress will be lost.", XleColor.Yellow);
			else
				g.AddBottom("");

			g.AddBottom("");

			choice = XleCore.QuickMenu(menu, 2, 1);

			if (choice == 0)
			{
				XleCore.ReturnToTitle = true;
			}
		}

		public void Fight()
		{
			XleCore.Map.PlayerFight(player);

		}

		public void GameSpeed()
		{
			MenuItemList theList = new MenuItemList("1", "2", "3", "4", "5");

			g.AddBottom("** Change game speed **", XleColor.Yellow);
			g.AddBottom("     (1 is fastest)", XleColor.Yellow);
			g.AddBottom("");

			player.Gamespeed = 1 + XleCore.QuickMenu(theList, 2, player.Gamespeed - 1);

			ColorStringBuilder builder = new ColorStringBuilder();

			builder.AddText("Gamespeed is: ", XleColor.White);
			builder.AddText(player.Gamespeed.ToString(), XleColor.Yellow);


			g.AddBottom(builder);


			XleCore.wait(300 + 200 * player.Gamespeed);


		}

		public void Hold()
		{
			MenuItemList theList = new MenuItemList();
			int value = 0;
			Color[] colors = new Color[40];

			theList.Add("Nothing");

			foreach (int i in XleCore.ItemList.Keys)
			{
				if (player.Item(i) > 0)
				{
					string itemName = XleCore.ItemList[i].Name;
					//TODO: Loadstring(g.hInstance(), i + 19, tempChars, 40);

					if (i == 9)			// mail
					{
						itemName = XleCore.GetMapName(player.mailTown) + " " + itemName;
					}

					if (i <= player.Hold)
					{
						value++;
					}

					theList.Add(itemName);
				}

			}

			ColorStringBuilder builder = new ColorStringBuilder();

			builder.AddText(g.Bottom(0), XleCore.Map.DefaultColor);
			builder.AddText("-choose above", XleColor.Cyan);

			g.UpdateBottom(builder, 0);

			player.HoldMenu(XleCore.SubMenu("Hold Item", value, theList));

		}

		public void Inventory()
		{
			int inventoryScreen = 0;

			string tempstring;
			Color bgcolor;
			Color fontcolor;

			// select the right colors for the screen.
			if (inventoryScreen == 0)
			{
				bgcolor = XleColor.Brown;
				fontcolor = XleColor.Yellow;
			}
			else
			{
				bgcolor = XleColor.Blue;
				fontcolor = XleColor.Cyan;
			}

			// Clear the back buffer
			Keyboard.ReleaseAllKeys();
			while (inventoryScreen < 2)
			{
				if (Keyboard.AnyKeyPressed)
				{
					Keyboard.ReleaseAllKeys();
					inventoryScreen++;
				}
				if (inventoryScreen == 0)
				{
					bgcolor = XleColor.Brown;
					fontcolor = XleColor.Yellow;
				}
				else
				{
					bgcolor = XleColor.Blue;
					fontcolor = XleColor.Cyan;
				}
				Display.BeginFrame();
				Display.Clear(bgcolor);

				// Draw the borders
				XleCore.DrawBorder(Color.Gray);
				XleCore.DrawLine(0, 128, 1, XleCore.myWindowWidth, XleColor.Gray);

				XleCore.DrawInnerBorder(Color.Yellow);
				XleCore.DrawInnerLine(0, 128, 1, XleCore.myWindowWidth, XleColor.Yellow);

				// Draw the title
				Display.FillRect(new Rectangle(176, 0, 288, 16), bgcolor);
				XleCore.WriteText(176, 0, " Player Inventory", fontcolor);

				// Draw the prompt
				Display.FillRect(144, 384, 336, 16, bgcolor);
				XleCore.WriteText(144, 384, " Hit key to continue", fontcolor);

				// Draw the top box
				XleCore.WriteText(48, 32, player.Name, fontcolor);

				tempstring = "Level         ";
				tempstring += player.Level;
				XleCore.WriteText(48, 64, tempstring, fontcolor);

				tempstring = "Time-days     ";
				tempstring += ((int)player.TimeDays).ToString();
				XleCore.WriteText(48, 96, tempstring, fontcolor);

				tempstring = "Dexterity     ";
				tempstring += player.Attribute[Attributes.dexterity];
				XleCore.WriteText(336, 32, tempstring, fontcolor);

				tempstring = "Strength      ";
				tempstring += player.Attribute[Attributes.strength];
				XleCore.WriteText(336, 48, tempstring, fontcolor);

				tempstring = "Charm         ";
				tempstring += player.Attribute[Attributes.charm];
				XleCore.WriteText(336, 64, tempstring, fontcolor);

				tempstring = "Endurance     ";
				tempstring += player.Attribute[Attributes.endurance];
				XleCore.WriteText(336, 80, tempstring, fontcolor);

				tempstring = "Intelligence  ";
				tempstring += player.Attribute[Attributes.intelligence];
				XleCore.WriteText(336, 96, tempstring, fontcolor);

				if (inventoryScreen == 0)
				{
					XleCore.WriteText(80, 160, "Armor & Weapons  -  Quality", fontcolor);

					int yy = 11;
					Color tempcolor;

					for (int i = 1; i <= 5; i++)
					{
						if (player.WeaponType(i) > 0)
						{

							if (player.CurrentWeapon == i)
							{
								tempcolor = XleColor.White;
							}
							else
							{
								tempcolor = fontcolor;
							}

							XleCore.WriteText(128, ++yy * 16, XleCore.WeaponList[player.WeaponType(i)].Name, tempcolor);
							XleCore.WriteText(416, yy * 16, XleCore.QualityList[player.WeaponQuality(i)], tempcolor);
						}

					}

					yy++;

					for (int i = 1; i <= 3; i++)
					{
						if (player.ArmorType(i) > 0)
						{

							if (player.CurrentArmor == i)
							{
								tempcolor = XleColor.White;
							}
							else
							{
								tempcolor = fontcolor;
							}

							XleCore.WriteText(128, ++yy * 16, XleCore.WeaponList[player.ArmorType(i)].Name, tempcolor);
							XleCore.WriteText(416, yy * 16, XleCore.QualityList[player.ArmorQuality(i)], tempcolor);
						}

					}

				}
				else if (inventoryScreen == 1)
				{

					// Draw the middle prompt
					Display.FillRect(160, 128, 288, 16, bgcolor);
					XleCore.WriteText(160, 128, " Other Possesions", fontcolor);

					string line;
					int yy = 9;
					int xx = 48;
					Color tempcolor;

					foreach (int i in XleCore.ItemList.Keys)
					{
						if (player.Item(i) > 0)
						{
							if (player.Hold == i)
							{
								tempcolor = XleColor.White;
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

							line = player.Item(i).ToString() + " ";

							if (i == 9)			// mail
							{
								line += XleCore.GetMapName(player.mailTown) + " ";
							}

							//TODO: Loadstring (g.hInstance(), i + 19, tempstring2, 39);

							// tempstring += tempstring2;
							line += XleCore.ItemList[i].Name;

							XleCore.WriteText(xx, ++yy * 16, line, tempcolor);
						}
					}

				}

				Display.EndFrame();
				Core.KeepAlive();

			}
		}

		public void Leave()
		{
			XleCore.Map.PlayerLeave(player);


		}

		public void Magic()
		{
		}

		public void Open()
		{
			if (XleCore.Map.PlayerOpen(player) == false)
			{
				g.AddBottom("");
				g.AddBottom("Nothing opens.");

				XleCore.wait(500);
			}

		}

		public void Pass()
		{
		}

		public void Rob()
		{
			/*
			for (j = -1; j < 3; j++)
			{
				for (i = -1; i < 3; i++)
				{
					if (Lota.Map.CheckSpecial(player.X + i, player.Y + j) >= 2 && found == false)
					{
						SpecialEvent dave = Lota.Map.GetSpecial(player.X + i, player.Y + j);

						found = true;

						Store(dave, true);

					}
				}
			}
			*/
			XleCore.Map.PlayerRob(player);
		}

		public void Speak()
		{
			XleCore.Map.PlayerSpeak(player);
		}

		public void Take()
		{
			if (XleCore.Map.PlayerTake(player) == false)
			{
				g.AddBottom("");
				g.AddBottom("Nothing to take.");

				XleCore.wait(500);
			}
		}

		public void Use()
		{
			string commandstring = string.Empty;
			bool noEffect = true;

			g.AddBottom("");

			string action = XleCore.ItemList[player.Hold].Action;
			if (string.IsNullOrEmpty(action))
				action = "Use";

			commandstring = action + " " + XleCore.ItemList[player.Hold].Name;

			g.AddBottom(commandstring);

			switch (player.Hold)
			{
				case 3:
					commandstring = "Eat Healing Herbs";
					noEffect = false;
					EatHealingHerbs();
					break;

				case 4:			// Iron Key
				case 5:			// Copper Key
				case 6:			// Brass Key
				case 7:			// Stone Key
				case 9:				// mail
				case 10:			// tulip
				case 11:			// compass
				case 13:			// scepter
				case 14:			// guard jewel
				case 15:			// compendium
				case 16:			// crown

				default:
					noEffect = !XleCore.Map.PlayerUse(player, player.Hold);

					break;
			}

			if (noEffect == true)
			{
				g.AddBottom("");
				XleCore.wait(400 + 100 * player.Gamespeed);
				g.UpdateBottom("No effect");
			}

		}

		private void EatHealingHerbs()
		{
			player.HP += player.MaxHP / 2;
			player.ItemCount(3, -1);
			SoundMan.PlaySound(LotaSound.Good);

			Color firstColor = g.HPColor;
			Color lastColor = XleColor.White;

			do
			{
				if (lastColor == XleColor.White)
					lastColor = XleColor.Cyan;
				else
					lastColor = XleColor.White;

				g.HPColor = lastColor;

				XleCore.wait(25);

			} while (SoundMan.IsPlaying(LotaSound.Good));

			g.HPColor = firstColor;
		}

		public void Weapon()
		{
			MenuItemList theList = new MenuItemList();
			string tempstring;
			int value = 0;
			int j = 0;
			theList.Add("Nothing");

			for (int i = 1; i <= 5; i++)
			{
				if (player.WeaponType(i) > 0)
				{
					tempstring = XleCore.QualityList[player.WeaponQuality(i)] + " " +
								 XleCore.WeaponList[player.WeaponType(i)].Name;

					theList.Add(tempstring);
					j++;

					if (player.CurrentWeapon == i)
					{
						value = j;
					}
				}

			}

			ColorStringBuilder builder = new ColorStringBuilder();
			builder.AddText(g.Bottom(0), XleCore.Map.DefaultColor);
			builder.AddText("-choose above", XleColor.Cyan);

			g.UpdateBottom(builder, 0);


			player.CurrentWeapon = XleCore.SubMenu("Pick Weapon", value, theList);
		}

		public void Xamine()
		{
			if (XleCore.Map.PlayerXamine(player) == false)
			{

			}


		}
		/*
		void DrawSpecial()
		{
			int i = 0;

			switch (g.stdDisplay)
			{
				case 'S':
					DrawStore(pDDS);
					break;

				case 'I':

			}

		}
		*/



	}



}