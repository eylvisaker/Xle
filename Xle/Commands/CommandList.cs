using System;
using System.Collections.Generic;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.Geometry;
using ERY.Xle.Commands;

namespace ERY.Xle.Commands
{
	public class CommandList
	{
		public GameState State { get; set; }
		Player player { get { return State.Player; } }

		Dictionary<KeyCode, Direction> mDirectionMap = new Dictionary<KeyCode, Direction>();

		public CommandList(GameState state)
		{
			State = state;

			mDirectionMap[KeyCode.Right] = Direction.East;
			mDirectionMap[KeyCode.Up] = Direction.North;
			mDirectionMap[KeyCode.Left] = Direction.West;
			mDirectionMap[KeyCode.Down] = Direction.South;

			mDirectionMap[KeyCode.OpenBracket] = Direction.North;
			mDirectionMap[KeyCode.Semicolon] = Direction.West;
			mDirectionMap[KeyCode.Quotes] = Direction.East;
			mDirectionMap[KeyCode.Slash] = Direction.South;

			Items = new List<Command>();

			Items.Add(new Armor());
			Items.Add(new Hold());
			Items.Add(new Use { ShowItemMenu = false });
			Items.Add(new Weapon());
			Items.Add(new Speak());
		}

		public void Prompt()
		{
			player.CheckDead();

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.Print("Enter command: ");
		}

		public List<Command> Items { get; set; }
		/// <summary>
		/// Returns true if the command is a cursor movement.
		/// </summary>
		/// <param name="cmd"></param>
		/// <returns></returns>
		private void CursorMovement(KeyCode cmd)
		{
			Direction dir = mDirectionMap[cmd];

			XleCore.Map.PlayerCursorMovement(player, dir);
		}

		bool IsCursorMovement(KeyCode cmd)
		{
			switch (cmd)
			{
				case KeyCode.Right:
				case KeyCode.Up:
				case KeyCode.Left:
				case KeyCode.Down:
					return true;

				default:
					return false;
			}
		}
		public void DoCommand(KeyCode cmd)
		{
			if (cmd == KeyCode.None)
				return;

			int waitTime = 700;

			if (IsCursorMovement(cmd))
			{
				ExecuteCursorMovement(cmd);
				return;
			}

			if (XleCore.Menu(cmd) == "")
			{
				SoundMan.PlaySound(LotaSound.Invalid);

				XleCore.Wait(waitTime);
				return;
			}
			else
			{
				var command = FindCommand(cmd);

				if (command != null)
				{
					XleCore.TextArea.Print(command.Name);

					command.Execute(State);
				}
				else
				{
					UpdateCommand(XleCore.Menu(cmd));

					switch (cmd)
					{
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
						case KeyCode.X:
							Xamine();
							break;
						default:
							break;
					}
				}
			}



			AfterDoCommand(waitTime, cmd);

		}

		private Command FindCommand(KeyCode cmd)
		{
			var command = Items.Find(x => x.Name.StartsWith(AgateLib.InputLib.Keyboard.GetKeyString(cmd,
				new KeyModifiers()), StringComparison.InvariantCultureIgnoreCase));

			return command;
			
		}

		private void ExecuteCursorMovement(KeyCode cmd)
		{
			int wasRaft = player.OnRaft;

			CursorMovement(cmd);

			if (g.Animating == false)
			{
				g.Animating = true;
				g.AnimFrame = 0;
			}

			var waitTime = g.walkTime;

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
		}

		public static void UpdateCommand(string command)
		{
			g.UpdateBottom("Enter Command: " + command);
		}


		private void AfterDoCommand(int waitTime, KeyCode cmd)
		{
			XleCore.Map.AfterExecuteCommand(player, cmd);

			XleCore.Wait(waitTime, false, XleCore.Redraw);
			Prompt();
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

			XleCore.Wait(1500);

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


			XleCore.Factory.SetGameSpeed(XleCore.GameState, player.Gamespeed);

			XleCore.Wait(XleCore.GameState.GameSpeed.AfterSetGamespeedTime);

		}

		public void Inventory()
		{
			int inventoryScreen = 0;

			string tempstring;
			Color bgcolor;
			Color fontcolor;

			// Clear the back buffer
			Keyboard.ReleaseAllKeys();
			while (inventoryScreen < 2)
			{
				if (Keyboard.AnyKeyPressed)
				{
					Keyboard.ReleaseAllKeys();
					inventoryScreen++;
				}
		
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

				tempstring = "Level        ";
				tempstring += player.Level.ToString().PadLeft(2);
				XleCore.WriteText(48, 64, tempstring, fontcolor);

				string timeString = ((int)player.TimeDays).ToString().PadLeft(5);
				tempstring = "Time-days ";
				tempstring += timeString;
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

							XleCore.WriteText(128, ++yy * 16, XleCore.ArmorList[player.ArmorType(i)].Name, tempcolor);
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

				XleCore.Wait(500);
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
		}

		public void Take()
		{
			if (XleCore.Map.PlayerTake(player) == false)
			{
				g.AddBottom("");
				g.AddBottom("Nothing to take.");

				XleCore.Wait(500);
			}
		}

		public void Xamine()
		{
			if (XleCore.Map.PlayerXamine(player) == false)
			{

			}


		}

	}
}