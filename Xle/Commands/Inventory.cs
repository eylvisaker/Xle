using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Commands
{
	public class Inventory : Command
	{
		public override void Execute(GameState state)
		{
			XleCore.TextArea.PrintLine();

			var player = state.Player;
			var renderer = XleCore.Renderer;

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
				XleCore.SetOrthoProjection(XleColor.DarkGray);
				Display.FillRect(new Rectangle(0, 0, 640, 400), bgcolor);

				// Draw the borders
				renderer.DrawFrame(Color.Gray);
				renderer.DrawFrameLine(0, 128, 1, XleCore.myWindowWidth, XleColor.Gray);

				renderer.DrawFrameHighlight(Color.Yellow);
				renderer.DrawInnerFrameHighlight(0, 128, 1, XleCore.myWindowWidth, XleColor.Yellow);

				// Draw the title
				Display.FillRect(new Rectangle(176, 0, 288, 16), bgcolor);
				renderer.WriteText(176, 0, " Player Inventory", fontcolor);

				// Draw the prompt
				Display.FillRect(144, 384, 336, 16, bgcolor);
				renderer.WriteText(144, 384, " Hit key to continue", fontcolor);

				// Draw the top box
				renderer.WriteText(48, 32, player.Name, fontcolor);

				tempstring = "Level        ";
				tempstring += player.Level.ToString().PadLeft(2);
				renderer.WriteText(48, 64, tempstring, fontcolor);

				string timeString = ((int)player.TimeDays).ToString().PadLeft(5);
				tempstring = "Time-days ";
				tempstring += timeString;
				renderer.WriteText(48, 96, tempstring, fontcolor);

				tempstring = "Dexterity     ";
				tempstring += player.Attribute[Attributes.dexterity];
				renderer.WriteText(336, 32, tempstring, fontcolor);

				tempstring = "Strength      ";
				tempstring += player.Attribute[Attributes.strength];
				renderer.WriteText(336, 48, tempstring, fontcolor);

				tempstring = "Charm         ";
				tempstring += player.Attribute[Attributes.charm];
				renderer.WriteText(336, 64, tempstring, fontcolor);

				tempstring = "Endurance     ";
				tempstring += player.Attribute[Attributes.endurance];
				renderer.WriteText(336, 80, tempstring, fontcolor);

				tempstring = "Intelligence  ";
				tempstring += player.Attribute[Attributes.intelligence];
				renderer.WriteText(336, 96, tempstring, fontcolor);

				if (inventoryScreen == 0)
				{
					renderer.WriteText(80, 160, "Armor & Weapons  -  Quality", fontcolor);

					int yy = 11;
					Color tempcolor;

					for (int i = 1; i <= 5; i++)
					{
						if (player.WeaponType(i) > 0)
						{

							if (player.CurrentWeaponIndex == i)
							{
								tempcolor = XleColor.White;
							}
							else
							{
								tempcolor = fontcolor;
							}

							renderer.WriteText(128, ++yy * 16, XleCore.WeaponList[player.WeaponType(i)].Name, tempcolor);
							renderer.WriteText(416, yy * 16, XleCore.QualityList[player.WeaponQuality(i)], tempcolor);
						}

					}

					yy++;

					for (int i = 1; i <= 3; i++)
					{
						if (player.ArmorType(i) > 0)
						{

							if (player.CurrentArmorIndex == i)
							{
								tempcolor = XleColor.White;
							}
							else
							{
								tempcolor = fontcolor;
							}

							renderer.WriteText(128, ++yy * 16, XleCore.ArmorList[player.ArmorType(i)].Name, tempcolor);
							renderer.WriteText(416, yy * 16, XleCore.QualityList[player.ArmorQuality(i)], tempcolor);
						}

					}

				}
				else if (inventoryScreen == 1)
				{

					// Draw the middle prompt
					Display.FillRect(160, 128, 288, 16, bgcolor);
					renderer.WriteText(160, 128, " Other Possesions", fontcolor);

					string line;
					int yy = 9;
					int xx = 48;
					Color tempcolor;

					foreach (int i in XleCore.ItemList.Keys)
					{
						if (player.Items[i] > 0)
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

							line = player.Items[i].ToString() + " ";

							if (i == 9)			// mail
							{
								line += XleCore.GetMapName(player.mailTown) + " ";
							}

							//TODO: Loadstring (g.hInstance(), i + 19, tempstring2, 39);

							// tempstring += tempstring2;
							line += XleCore.ItemList[i].Name;

							renderer.WriteText(xx, ++yy * 16, line, tempcolor);
						}
					}

				}

				Display.EndFrame();
				Core.KeepAlive();

			}
		}
	}
}
