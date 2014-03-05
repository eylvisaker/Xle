using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Serialization.Xle;
using System.ComponentModel;
using ERY.Xle.XleEventTypes.Stores;

namespace ERY.Xle.XleEventTypes.Stores
{
	public abstract class Store : XleEvent
	{
		private double mCostFactor = 1.0;
		private bool mLoanOverdue = false;
		private bool mRobbed = false;
		private string mShopName;


		protected string[] theWindow = new string[20];
		protected Color[][] theWindowColor = new Color[20][];

		public Store()
		{
			LeftOffset = 2;

			for (int i = 0; i < theWindowColor.Length; i++)
			{
				for (int j = 0; j < 40; j++)
				{
					theWindowColor[i] = new Color[40];
				}
			}

			ClearWindow();

			BottomBackgroundColor = XleColor.Black;
		}
		public string ShopName
		{
			get { return mShopName; }
			set { mShopName = value; }
		}

		protected void ClearWindow()
		{
			for (int i = 0; i < theWindow.Length; i++)
			{
				theWindow[i] = string.Empty;

				for (int j = 0; j < theWindowColor[i].Length; j++)
					theWindowColor[i][j] = XleColor.White;
			}
		}

		protected void SetColor(int rowNumber, Color color)
		{
			SetColor(rowNumber, 0, 40, color);
		}
		protected void SetColor(int rowNumber, int start, int length, Color color)
		{
			for (int i = 0; i < length; i++)
				theWindowColor[rowNumber][start + i] = color;
		}

		protected override void WriteData(XleSerializationInfo info)
		{
			info.Write("ShopName", mShopName);
			info.Write("CostFactor", mCostFactor);
		}
		protected override void ReadData(XleSerializationInfo info)
		{
			mShopName = info.ReadString("ShopName");
			mCostFactor = info.ReadDouble("CostFactor");
		}

		[Browsable(false)]
		public bool Robbed
		{
			get { return mRobbed; }
			set { mRobbed = value; }
		}

		protected bool LoanOverdue
		{
			get { return mLoanOverdue; }
		}

		public double CostFactor
		{
			get { return mCostFactor; }
			set { mCostFactor = value; }
		}

		public virtual void CheckOfferMuseumCoin(Player player)
		{
			if (XleCore.random.Next(1000) < 45 && robbing == false)
			{
				OfferMuseumCoin(player);
			}
		}
		public static void OfferMuseumCoin(Player player)
		{
			int coin = -1;
			MenuItemList menu = new MenuItemList("Yes", "No");

			if (player.Level == 1)
				coin = XleCore.random.Next(2);
			else if (player.Level <= 3)
				coin = XleCore.random.Next(3);

			if (coin == -1)
				return;

			// TODO: only allow player to buy a coin if he has less than Level of that type of coins.
			int amount = 50 + (int)(XleCore.random.NextDouble() * 20 * player.Level);

			if (amount > player.Gold)
				amount /= 2;

			// shift value to index within items.
			coin += 17;

			SoundMan.PlaySound(LotaSound.Question);

			XleCore.TextArea.PrintLine("Would you like to buy a ");
			XleCore.Wait(1);

			XleCore.TextArea.PrintLine("museum coin for " + amount.ToString() + " gold?");
			XleCore.Wait(1);

			XleCore.TextArea.PrintLine();
			XleCore.Wait(1);

			int choice = XleCore.QuickMenu(menu, 3, 0);

			if (choice == 0)
			{
				if (player.Spend(amount))
				{
					string coinName = XleCore.ItemList[coin].Name;

					XleCore.TextArea.PrintLine("Use this " + coinName + " well!");

					player.ItemCount(coin, 1);

					SoundMan.PlaySound(LotaSound.Sale);
				}
				else
				{
					XleCore.TextArea.PrintLine("Not enough gold.");
					SoundMan.PlaySound(LotaSound.Medium);
				}

			}

		}
		protected virtual void GetColors(out Color backColor, out Color borderColor,
			out Color lineColor, out Color fontColor, out Color titleColor)
		{
			backColor = XleColor.Green;
			borderColor = XleColor.Brown;
			lineColor = XleColor.Yellow;
			fontColor = XleColor.White;
			titleColor = fontColor;
		}

		/// <summary>
		/// Stores the player object for use when redrawing.
		/// </summary>
		protected Player player;
		/// <summary>
		/// Bool indicating whether or not we are robbing this store.
		/// </summary>
		protected bool robbing;

		protected void RedrawStore()
		{
			Display.BeginFrame();

			DrawStore();

			Display.EndFrame();
			Core.KeepAlive();
		}

		protected int LeftOffset { get; set; }

		protected void DrawStore()
		{
			string tempString;

			Color storeBackColor;
			Color storeBorderColor;
			Color storeLineColor;
			Color storeFontColor;
			Color storeTitleColor;

			GetColors(out storeBackColor, out storeBorderColor, out storeLineColor,
				out storeFontColor, out storeTitleColor);

			// draw backgrounds
			Display.Clear(storeBackColor);
			Display.FillRect(0, 296, 640, 104, BottomBackgroundColor);

			// Draw the borders
			XleCore.DrawBorder(storeBorderColor);
			XleCore.DrawLine(0, 288, 1, 640, storeBorderColor);

			XleCore.DrawInnerBorder(storeLineColor);
			XleCore.DrawInnerLine(0, 288, 1, 640, storeLineColor);

			// Draw the title
			Display.FillRect(320 - (theWindow[0].Length + 2) / 2 * 16, 0,
				(theWindow[0].Length + 2) * 16, 16, storeBackColor);

			XleCore.WriteText(320 - (theWindow[0].Length / 2) * 16, 0, theWindow[0], storeTitleColor);

			for (int i = 1; i < 18; i++)
			{
				if (string.IsNullOrEmpty(theWindow[i]))
					continue;

				XleCore.WriteText((LeftOffset + 1) * 16, i * 16, theWindow[i], theWindowColor[i]);
			}

			if (robbing == false)
			{
				// Draw Gold
				tempString = " Gold: ";
				tempString += player.Gold;
				tempString += " ";
			}
			else
			{
				// don't need gold if we're robbing it!
				tempString = " Robbery in progress ";
			}

			Display.FillRect(320 - (tempString.Length / 2) * 16, 18 * 16, tempString.Length * 16, 14,
				storeBackColor);

			XleCore.WriteText(320 - (tempString.Length / 2) * 16, 18 * 16, tempString, XleColor.White);

			XleCore.DrawBottomText();

		}


		protected Color BottomBackgroundColor { get; set; }

		protected void StoreSound(LotaSound sound)
		{
			SoundMan.PlaySoundSync(RedrawStore, sound);
		}
		protected void Wait(int howLong)
		{
			XleCore.Wait(howLong, RedrawStore);
		}
		protected void WaitForKey(params KeyCode[] keys)
		{
			XleCore.WaitForKey(RedrawStore, keys);
		}

		protected int QuickMenu(MenuItemList menu, int spaces)
		{
			return XleCore.QuickMenu(RedrawStore, menu, spaces);
		}
		protected int QuickMenu(MenuItemList menu, int spaces, int value)
		{
			return XleCore.QuickMenu(RedrawStore, menu, spaces, value);
		}
		protected int QuickMenu(MenuItemList menu, int spaces, int value, Color clrInit)
		{
			return XleCore.QuickMenu(RedrawStore, menu, spaces, value, clrInit);
		}
		protected int QuickMenu(MenuItemList menu, int spaces, int value, Color clrInit, Color clrChanged)
		{
			return XleCore.QuickMenu(RedrawStore, menu, spaces, value, clrInit, clrChanged);
		}

		protected int ChooseNumber(int max)
		{
			return XleCore.ChooseNumber(RedrawStore, max);
		}
		/// <summary>
		/// Returns true if the loan for the player is over due and stores should not
		/// speak with him and optionally displays a message to the player.  
		/// Returns false if the current map has no lending association
		/// regardless of whether the player has an overdue loan.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="displayMessage">Pass true to display a default message indicating
		/// that the shop keeper will not talk to the player.</param>
		/// <returns>Returns true if the loan for the player is over due and stores should not
		/// speak with him and optionally displays a message to the player.  
		/// Returns false if the current map has no lending association
		/// regardless of whether the player has an overdue loan.</returns>
		protected bool CheckLoan(Player player, bool displayMessage)
		{
			if (XleCore.Map.Events.Any(x => x is StoreLending) == false)
				return false;

			if (player.loan > 0 && player.dueDate - player.TimeDays <= 0)
			{
				if (displayMessage)
				{
					XleCore.TextArea.PrintLine();
					XleCore.TextArea.PrintLine("Sorry.  I can't talk to you.");
					XleCore.Wait(500);
				}

				return true;
			}
			else
				return false;
		}

		public override bool Speak(GameState state)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine(ShopName, XleColor.Yellow);
			XleCore.TextArea.PrintLine("");
			XleCore.TextArea.PrintLine("A Sign Says, ");
			XleCore.TextArea.PrintLine("Closed for remodelling.");

			SoundMan.PlaySound(LotaSound.Medium);

			XleCore.TextArea.PrintLine();

			XleCore.Wait(1000);

			return true;
		}

		public override bool Rob(GameState state)
		{
			if (Robbed)
			{
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("No items within reach here.");
				XleCore.Wait(1000);
				return true;
			}

			int value = RobValue();

			if (value == 0)
			{
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("There's nothing to really carry here.");
				XleCore.Wait(1000);
				return true;
			}

			state.Player.Gold += value;
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("You get " + value.ToString() + " gold.", XleColor.Yellow);
			XleCore.Wait(1000);
			Robbed = true;

			return true;
		}

		public virtual int RobValue()
		{
			return 0;
		}
	}

}
