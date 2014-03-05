﻿using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Stores
{

	public class StoreLending : Store
	{
		public override int RobValue()
		{
			return XleCore.random.Next(180, 231);
		}
		protected override void GetColors(out Color backColor, out Color borderColor,
			out Color lineColor, out Color fontColor, out Color titleColor)
		{
			backColor = XleColor.DarkGray;
			borderColor = XleColor.LightGray;
			lineColor = XleColor.Yellow;
			fontColor = XleColor.White;
			titleColor = XleColor.White;
		}
		public override bool Speak(GameState state)
		{
			var player = state.Player;
			int i = 0;
			int max = 200 * player.Level;
			int choice;

			this.player = player;
			robbing = false;

			ClearWindow();
			LeftOffset = 6;

			theWindow[i++] = "Friendly";
			theWindow[i++] = "";
			theWindow[i++] = "   Lending Association";
			theWindow[i++] = "";
			theWindow[i++] = "";
			theWindow[i++] = "";
			theWindow[i++] = "";

			XleCore.TextArea.PrintLine();

			if (player.loan == 0)
			{
				theWindow[i++] = " We'd be happy to loan you";
				theWindow[i++] = " money at 'friendly' rates";
				theWindow[i++] = "";
				theWindow[i++] = "";
				theWindow[i++] = "";
				theWindow[i] = "You may borrow up to ";
				theWindow[i] += max;
				theWindow[i++] += " gold";

				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("Borrow how much?");

				choice = ChooseNumber(max);

				if (choice > 0)
				{
					player.Gold += choice;
					player.loan = (int)(choice * 1.5);
					player.dueDate = (int)(player.TimeDays + 0.999) + 120;

					XleCore.TextArea.PrintLine();
					XleCore.TextArea.PrintLine(choice.ToString() + " gold borrowed.");

					XleCore.Wait(1000, DrawStore);

					ColorStringBuilder b = new ColorStringBuilder();
					b.AddText("You'll owe ", XleColor.White);
					b.AddText(player.loan.ToString(), XleColor.Yellow);
					b.AddText(" gold", XleColor.Yellow);
					b.AddText(" in 120 days.", XleColor.White);

					g.AddBottom(b);

					StoreSound(LotaSound.Bad);

				}
			}
			else
			{
				String DueDate;
				max = Math.Max(player.Gold, player.loan);
				int min;
				Color[] color = new Color[44];

				if (player.dueDate - player.TimeDays > 0)
				{
					DueDate = ((int)(player.dueDate - player.TimeDays + 0.02)).ToString() + " days ";
					min = 0;
				}
				else
				{
					DueDate = "NOW!!   ";
					min = player.loan / 3;
				}

				theWindow[i++] = "You owe: " + player.loan.ToString() + " gold!";
				theWindow[i++] = "";
				theWindow[i++] = "";
				theWindow[i++] = "Due Date: " + DueDate;

				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("Pay how much?");

				if (min > 0)
				{
					for (i = 0; i < 13; i++, color[i] = XleColor.White) ;
					for (; i < 40; i++, color[i] = XleColor.Yellow) ;

					g.UpdateBottom("Pay how much? (At Least " + min.ToString() + " gold)", 0, color);

				}

				choice = ChooseNumber(max);

				if (choice > player.loan)
					choice = player.loan;

				player.Spend(choice);
				player.loan -= choice;

				if (player.loan <= 0)
				{
					XleCore.TextArea.PrintLine("Loan Repaid.");

					SoundMan.PlaySound(LotaSound.Sale);
				}
				else if (min == 0)
				{
					XleCore.TextArea.PrintLine("You Owe " + player.loan.ToString() + " gold.");
					XleCore.TextArea.PrintLine("Take your time.");

					SoundMan.PlaySound(LotaSound.Sale);
				}
				else if (choice >= min)
				{
					XleCore.TextArea.PrintLine("You have 14 days to pay the rest!");
					player.dueDate = (int)player.TimeDays + 14;

					SoundMan.PlaySound(LotaSound.Sale);
				}
				else
				{
					XleCore.TextArea.PrintLine("Better pay up!");

					//LotaPlaySound(snd_Bad);

				}


			}

			Wait(500);
			return true;

		}
	}
}
