using System;
using System.Collections.Generic;
using System.Text;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Serialization.Xle;
using System.ComponentModel;

namespace ERY.Xle.XleEventTypes
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

			g.AddBottom("Would you like to buy a ");
			XleCore.wait(1);

			g.AddBottom("museum coin for " + amount.ToString() + " gold?");
			XleCore.wait(1);

			g.AddBottom("");
			XleCore.wait(1);

			int choice = XleCore.QuickMenu(menu, 3, 0);

			if (choice == 0)
			{
				if (player.Spend(amount))
				{
					string coinName = XleCore.ItemList[coin].Name;

					g.AddBottom("Use this " + coinName + " well!");

					player.ItemCount(coin, 1);

					SoundMan.PlaySound(LotaSound.Sale);
				}
				else
				{
					g.AddBottom("Not enough gold.");
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
			XleCore.wait(RedrawStore, howLong);
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
			if (XleCore.Map.HasEventType(typeof(XleEventTypes.StoreLending)) == false)
				return false;

			if (player.loan > 0 && player.dueDate - player.TimeDays <= 0)
			{
				if (displayMessage)
				{
					g.AddBottom("");
					g.AddBottom("Sorry.  I can't talk to you.");
					XleCore.wait(500);
				}

				return true;
			}
			else
				return false;
		}

		public override bool Speak(Player player)
		{
			g.AddBottom(ShopName, XleColor.Yellow);
			g.AddBottom("");
			g.AddBottom("A Sign Says, ");
			g.AddBottom("Closed for remodelling.");

			SoundMan.PlaySound(LotaSound.Medium);

			g.AddBottom("");

			XleCore.wait(1000);

			return true;
		}
		public override bool Rob(Player player)
		{
			if (Robbed)
			{
				g.AddBottom();
				g.AddBottom("No items within reach here.");
				XleCore.wait(1000);
				return true;
			}

			int value = RobValue();

			if (value == 0)
			{
				g.AddBottom();
				g.AddBottom("There's nothing to really carry here.");
				XleCore.wait(1000);
				return true;
			}

			player.Gold += value;
			g.AddBottom();
			g.AddBottom("You get " + value.ToString() + " gold.", XleColor.Yellow);
			XleCore.wait(1000);
			Robbed = true;

			return true;
		}

		public virtual int RobValue()
		{
			return 0;
		}
	}

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
		public override bool Speak(Player player)
		{
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

				g.AddBottom("");
				g.AddBottom("Borrow how much?");

				choice = ChooseNumber(max);

				if (choice > 0)
				{
					player.Gold += choice;
					player.loan = (int)(choice * 1.5);
					player.dueDate = (int)(player.TimeDays + 0.999) + 120;

					g.AddBottom();
					g.AddBottom(choice.ToString() + " gold borrowed.");

					XleCore.wait(DrawStore, 1000);

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

				g.AddBottom("");
				g.AddBottom("Pay how much?");

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
					g.AddBottom("Loan Repaid.");

					SoundMan.PlaySound(LotaSound.Sale);
				}
				else if (min == 0)
				{
					g.AddBottom("You Owe " + player.loan.ToString() + " gold.");
					g.AddBottom("Take your time.");

					SoundMan.PlaySound(LotaSound.Sale);
				}
				else if (choice >= min)
				{
					g.AddBottom("You have 14 days to pay the rest!");
					player.dueDate = (int)player.TimeDays + 14;

					SoundMan.PlaySound(LotaSound.Sale);
				}
				else
				{
					g.AddBottom("Better pay up!");

					//LotaPlaySound(snd_Bad);

				}


			}

			Wait(500);
			return true;

		}
	}
	public class StoreBank : Store
	{
		public override int RobValue()
		{
			return XleCore.random.Next(180, 231);
		}
		public override bool AllowRobWhenNotAngry
		{
			get
			{
				return true;
			}
		}
		protected override void GetColors(out Color backColor, out Color borderColor,
			out Color lineColor, out Color fontColor, out Color titleColor)
		{
			backColor = XleColor.DarkGray;
			borderColor = XleColor.Green;
			lineColor = XleColor.Yellow;
			fontColor = XleColor.White;
			titleColor = XleColor.Yellow;
		}
		public override bool Speak(Player player)
		{
			int i = 0;
			int choice;
			int amount;

			this.player = player;
			robbing = false;

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

			MenuItemList theList = new MenuItemList("0", "1", "2", "3");
			choice = QuickMenu(theList, 2, 0);

			switch (choice)
			{

				case 1:
					g.AddBottom("");
					g.AddBottom("Deposit how much?");
					amount = ChooseNumber(player.Gold);

					player.Spend(amount);
					player.GoldInBank += amount;

					break;
				case 2:
					if (player.GoldInBank > 0)
					{

						g.AddBottom("");
						g.AddBottom("Withdraw how much?");
						amount = ChooseNumber(player.GoldInBank);

						player.Gold += amount;
						player.GoldInBank -= amount;
					}
					else
					{
						g.ClearBottom();
						g.AddBottom("Nothing to withdraw");

						StoreSound(LotaSound.Medium);
						choice = 0;

					}
					break;
			}

			if (choice > 0)
			{
				g.AddBottom("Current balance: " + player.GoldInBank + " gold.");

				if (choice != 3)
				{
					StoreSound(LotaSound.Sale);
				}

			}

			return true;
		}
	}
	public abstract class StoreEquipment : Store
	{
		protected override void ReadData(XleSerializationInfo info)
		{
			base.ReadData(info);

			if (info.ContainsKey("AllowedItemTypes"))
			{
				AllowedItemTypes = info.ReadInt32Array("AllowedItemTypes");
			}
		}

		protected override void WriteData(XleSerializationInfo info)
		{
			base.WriteData(info);

			info.Write("AllowedItemTypes", AllowedItemTypes);
		}

		protected abstract string StoreType { get; }
		public int[] AllowedItemTypes { get; set; }

		public override bool Speak(Player player)
		{
			if (CheckLoan(player, true))
				return true;

			MenuItemList theList = new MenuItemList("Buy", "Sell", "Neither");
			String tempString;
			int i = 0, j = 0;
			int max = 200 * player.Level;
			int choice;
			int[] itemList = new int[16];
			int[] qualList = new int[16];
			int[] priceList = new int[16];

			this.LeftOffset = 4;
			this.player = player;

			ClearWindow();
			theWindow[i++] = ShopName;
			theWindow[i++] = "";
			theWindow[i++] = "            " + StoreType;

			for (i = 1; i <= 8; i++)
			{
				int item = i;
				int quality = i % 5;

				itemList[i] = item;
				qualList[i] = quality;

				priceList[i] = XleCore.WeaponCost(item, quality);
			}

			for (; i < 16; i++)
			{
				itemList[i] = 0;
			}

			g.ClearBottom();
			choice = QuickMenu(theList, 2, 0);
			Wait(1);


			if (choice == 0)
			{

				theWindow[4] = "   Items               Prices";

				StoreSound(LotaSound.Sale);

				for (i = 1; i < 16 && itemList[i] > 0; i++)
				{
					if (itemList[i] > 0)
					{
						j = i + 5;

						theWindow[j] = "";
						theWindow[j] += i;
						theWindow[j] += ". ";
						theWindow[j] += XleCore.GetWeaponName(itemList[i], qualList[i]);


						theWindow[j] += new string(' ', 22 - XleCore.GetWeaponName(itemList[i], qualList[i]).Length);

						theWindow[j] += priceList[i];
						Wait(1);

					}

				}

				MenuItemList theList2 = new MenuItemList();

				for (int k = 0; k < i; k++)
				{
					theList2.Add(k.ToString());
				}

				g.AddBottom("");
				g.AddBottom("Make choice (hit 0 to cancel)");
				g.AddBottom("");

				choice = QuickMenu(theList2, 2, 0);

				if (choice == 0)
				{
					g.AddBottom("");
					g.AddBottom("Nothing purchased");
					g.AddBottom("");

					StoreSound(LotaSound.Medium);
				}
				else if (player.Spend(priceList[choice]))
				{
					// spend the cash, if they have it
					if (player.AddWeapon(itemList[choice], qualList[choice]))
					{
						tempString = XleCore.QualityList[qualList[choice]];
						tempString += " ";
						tempString += XleCore.WeaponList[itemList[choice]].Name;
						tempString += " purchased.";
						g.AddBottom(tempString);
						g.AddBottom("");

						StoreSound(LotaSound.Sale);
					}
					else
					{

						player.Gold += priceList[choice];
						g.AddBottom("No room in inventory");
					}

				}
				else
				{
					g.AddBottom("You're short on gold.");
					StoreSound(LotaSound.Medium);
				}
			}
			else if (choice == 1)		// sell weapon
			{

			}

			return true;
		}

	}
	public class StoreWeapon : StoreEquipment
	{
		protected override void GetColors(out Color backColor, out Color borderColor,
			out Color lineColor, out Color fontColor, out Color titleColor)
		{
			backColor = XleColor.Brown;
			borderColor = XleColor.Orange;
			lineColor = XleColor.Yellow;
			fontColor = XleColor.White;
			titleColor = XleColor.White;
		}

		protected override string StoreType
		{
			get { return "Weapon"; }
		}
	}

	public class StoreArmor : StoreEquipment
	{
		protected override string StoreType
		{
			get { return "armor"; }
		}
	}

	public class StoreWeaponTraining : Store
	{
	}
	public class StoreArmorTraining : Store
	{
	}
	public class StoreBlackjack : Store
	{
	}
	public class StoreRaft : Store
	{
		// map and coords that mark where a purchased raft shows up
		int mBuyRaftMap;
		Point mBuyRaftPt;

		protected override void WriteData(XleSerializationInfo info)
		{
			base.WriteData(info);

			info.Write("BuyRaftMap", mBuyRaftMap);
			info.Write("BuyRaftX", mBuyRaftPt.X);
			info.Write("BuyRaftY", mBuyRaftPt.Y);
		}
		protected override void ReadData(XleSerializationInfo info)
		{
			base.ReadData(info);

			mBuyRaftMap = info.ReadInt32("BuyRaftMap", 0);
			mBuyRaftPt.X = info.ReadInt32("BuyRaftX", 0);
			mBuyRaftPt.Y = info.ReadInt32("BuyRaftY", 0);
		}

		public int BuyRaftMap
		{
			get { return mBuyRaftMap; }
			set { mBuyRaftMap = value; }
		}
		public Point BuyRaftPt
		{
			get { return mBuyRaftPt; }
			set { mBuyRaftPt = value; }
		}

		public override bool AllowedOnMapType(XleMap type)
		{
			if (type.Equals(typeof(XleMapTypes.Town)))
				return true;
			else
				return false;
		}
		public override bool Speak(Player player)
		{
			int choice = 0;
			int raftCost = (int)(400 * this.CostFactor);
			int gearCost = (int)(50 * this.CostFactor);
			MenuItemList theList = new MenuItemList("Yes", "No");
			bool skipRaft = false;
			bool offerCoin = false;

			if (CheckLoan(player, true))
				return true;

			// check to see if there are any rafts near the raft drop point
			skipRaft = CheckForNearbyRaft(player, skipRaft);

			g.AddBottom("** " + this.ShopName + " **", XleColor.Yellow);
			g.AddBottom("");

			if (skipRaft == false)
			{
				g.AddBottom("Want to buy a raft for " + raftCost.ToString() + " gold?");

				choice = XleCore.QuickMenu(theList, 3, 1);

				if (choice == 0)
				{
					// Purchase raft
					if (player.Spend(raftCost))
					{
						player.AddRaft(BuyRaftMap, BuyRaftPt.X, BuyRaftPt.Y);

						g.AddBottom("Raft purchased.");
						SoundMan.PlaySound(LotaSound.Sale);
						XleCore.wait(1000);

						g.AddBottom("Board raft outside.");

						offerCoin = true;
					}
					else
					{
						g.AddBottom("Not enough gold.");
						SoundMan.PlaySound(LotaSound.Medium);
						XleCore.wait(750);
					}
				}
			}

			if (skipRaft == true || choice == 1)
			{
				g.AddBottom("How about some climbing gear");
				g.AddBottom("for " + gearCost.ToString() + " gold?");
				g.AddBottom("");

				choice = XleCore.QuickMenu(theList, 3, 1);

				if (choice == 0)
				{
					if (player.Spend(gearCost))
					{
						g.AddBottom("Climbing gear purchased.");

						player.ItemCount(2, 1);
						offerCoin = true;

						SoundMan.PlaySound(LotaSound.Sale);
					}
					else
					{
						g.AddBottom("Not enough gold.");
						SoundMan.PlaySound(LotaSound.Medium);
					}
				}
				else if (choice == 1)
				{
					g.AddBottom("");
					g.AddBottom("Nothing Purchased.");

					SoundMan.PlaySound(LotaSound.Medium);
				}

				XleCore.wait(750);

			}

			if (offerCoin)
				CheckOfferMuseumCoin(player);

			return true;
		}

		private bool CheckForNearbyRaft(Player player, bool skipRaft)
		{
			for (int i = 0; i < player.Rafts.Count; i++)
			{
				if (player.Rafts[i].MapNumber != BuyRaftMap)
					continue;

				int dist = Math.Abs(player.Rafts[i].X - BuyRaftPt.X) +
					Math.Abs(player.Rafts[i].Y - BuyRaftPt.Y);

				if (dist > 10)
					continue;

				skipRaft = true;
				break;
			}
			return skipRaft;
		}

	}
	public class StoreHealer : Store
	{
		bool buyHerbs = false;

		protected override void GetColors(out Color backColor, out Color borderColor,
			out Color lineColor, out Color fontColor, out Color titleColor)
		{
			backColor = XleColor.Green;
			borderColor = XleColor.LightGreen;
			lineColor = XleColor.Yellow;
			fontColor = XleColor.White;
			titleColor = XleColor.White;

			if (buyHerbs)
				backColor = XleColor.LightBlue;
		}

		public override bool Speak(Player player)
		{
			if (CheckLoan(player, true))
				return true;

			buyHerbs = false;
			int i = 0;
			this.player = player;
			int woundPrice = (int)((player.MaxHP - player.HP) * 0.75);
			int herbsPrice = (int)(player.Level * 300 * CostFactor);

			ClearWindow();

			theWindow[i++] = ShopName;
			theWindow[i++] = "";
			theWindow[i++] = "";
			theWindow[i++] = "    Our sect offers restorative";
			theWindow[i++] = "        cures for your wounds.";

			i += 4;

			string woundString = woundPrice.ToString() + " gold";

			if (woundPrice <= 0)
			{
				woundString = "Not needed";
				SetColor(i, 18, 12, XleColor.Yellow);
			}

			theWindow[i++] = "1. Wound Care  -  " + woundString;
			i += 2;

			theWindow[i++] = "2. Healing Herbs -  " + herbsPrice.ToString() + " apiece";

			// display ready message
			if (player.museum[6] == 3)
			{
				i += 2;

				// TODO: make it blue!
				theWindow[i++] = "You're ready for herbs!";

				SoundMan.PlaySound(LotaSound.VeryGood);
				while (SoundMan.IsPlaying(LotaSound.VeryGood))
				{
					XleCore.wait(RedrawStore, 20);
				}
			}

			MenuItemList theList = new MenuItemList("0", "1", "2");

			g.AddBottom("");
			g.AddBottom("Make choice (hit 0 to cancel)");
			g.AddBottom("");

			int choice = QuickMenu(theList, 2, 0);

			if (choice == 0)
			{
				g.AddBottom("");
				g.AddBottom("Nothing purchased");
				g.AddBottom("");

				StoreSound(LotaSound.Medium);
			}
			else if (choice == 1)
			{
				g.AddBottom("You are cured.");
				player.HP = player.MaxHP;

				StoreSound(LotaSound.VeryGood);
			}
			else if (choice == 2)
			{
				if (player.museum[6] <= 1)
				{
					g.AddBottom("You're not ready yet.");
					SoundMan.PlaySound(LotaSound.Medium);
				}
				else
				{
					int max = player.Gold / herbsPrice;
					max = Math.Min(max, 40 - player.Item(3));

					buyHerbs = true;

					g.AddBottom();
					g.AddBottom("Purchase how many healing herbs?");

					int number = ChooseNumber(max);

					if (number == 0)
					{
						g.AddBottom("Nothing purchased.");
						SoundMan.PlaySound(LotaSound.Medium);
					}
					else
					{
						if (player.Spend(number * herbsPrice) == false)
						{
							throw new Exception("Not enough money!");
						}

						player.ItemCount(3, number);

						g.AddBottom(number.ToString() + " healing herbs purchased.");
						player.museum[6] |= 0x04;

						StoreSound(LotaSound.Sale);
					}
				}
			}

			return true;
		}
	}

	public class StoreJail : Store
	{
	}
	public class StoreFortune : Store
	{
		public override bool Speak(Player player)
		{
			if (CheckLoan(player, true))
				return true;

			MenuItemList theList = new MenuItemList("Yes", "No");
			int choice;

			g.AddBottom(this.ShopName, XleColor.Green);
			g.AddBottom("");
			g.AddBottom("Read your fortune for " + (int)(6 * CostFactor) + " gold?");

			choice = XleCore.QuickMenu(theList, 3, 1);

			if (choice == 0)
			{


			}

			return true;
		}
	}

	public class StoreFlipFlop : Store
	{
	}
	public class StoreBuyback : Store
	{
	}
	public class StoreFood : Store
	{
		protected override void GetColors(out Color backColor, out Color borderColor,
			out Color lineColor, out Color fontColor, out Color titleColor)
		{
			backColor = XleColor.DarkGray;
			borderColor = XleColor.Green;
			lineColor = XleColor.Yellow;
			fontColor = XleColor.Yellow;
			titleColor = XleColor.White;

		}

		bool skipMailOffer = false;

		public override bool Speak(Player player)
		{
			if (CheckLoan(player, true))
				return true;

			string tempString;
			double cost = 15 / player.Attribute[Attributes.charm];
			int choice;
			int max = (int)(player.Gold / cost);

			SetTitle();

			this.player = player;
			this.robbing = false;

			Wait(1);


			if (player.mailTown == XleCore.Map.MapID)
			{
				PayForMail(player);
				skipMailOffer = true;
			}
			else
			{
				SetWindow(cost);

				tempString = "      Maximum purchase:  ";
				tempString += max;
				tempString += " days";

				g.AddBottom("");
				g.AddBottom(tempString, XleColor.Cyan);

				choice = ChooseNumber(max);

				if (choice > 0)
				{
					player.Spend((int)(choice * cost));
					player.Food += choice;

					g.AddBottom(choice + " days of food bought.");

					StoreSound(LotaSound.Sale);

					if (skipMailOffer == false)
						OfferMail(player);

					return true;
				}
				else
				{
					g.AddBottom("");
					g.AddBottom("Nothing Purchased");

					StoreSound(LotaSound.Medium);
				}
			}

			CheckOfferMuseumCoin(player);

			return true;

		}

		private void OfferMail(Player player)
		{
			XleMapTypes.Town twn = XleCore.Map as XleMapTypes.Town;

			if (player.Item(9) > 0) return;
			if (twn == null) return;
			if (twn.Mail.Count == 0) return;

			int mMap = XleCore.random.Next(twn.Mail.Count);
			int target;
			int count = 0;
			bool valid = false;

			// search for a valid map
			do
			{
				target = twn.Mail[mMap];

				if (XleCore.GetMapName(target) != "")
					valid = true;
				else
				{
					mMap++;
					if (mMap == twn.Mail.Count) mMap = 0;
				}

				count++;

			} while (count < 6 && valid == false);

			if (valid == false)
				return;

			SoundMan.PlaySound(LotaSound.Question);

			g.AddBottom("");
			g.AddBottom("Would you like to earn some gold?");

			MenuItemList menu = new MenuItemList("Yes", "No");

			int choice = QuickMenu(menu, 2);

			if (choice == 0)
			{
				player.ItemCount(9, 1);
				player.mailTown = target;

				g.AddBottom("");
				g.AddBottom("Here's some mail to");
				g.AddBottom("deliver to " + XleCore.GetMapName(target) + ".");
				g.AddBottom("");
				g.AddBottom("        Press Key to Continue");

				WaitForKey();
			}
		}
		private void SetWindow(double cost)
		{
			LeftOffset = 9;

			int i = 1;
			theWindow[i++] = "";
			theWindow[i++] = "";
			theWindow[i++] = "";
			theWindow[i++] = "    Food & water";
			theWindow[i++] = "";
			theWindow[i++] = "";
			theWindow[i++] = "We sell food for travel.";
			theWindow[i++] = "Each 'day' of food will ";
			theWindow[i++] = "keep you fed for one day";
			theWindow[i++] = "of travel (on foot).    ";
			theWindow[i++] = "";
			theWindow[i++] = "";

			if (robbing == false)
			{
				theWindow[i] = "Cost is ";
				theWindow[i] += cost;
				theWindow[i++] += " gold per 'day'";
			}
			else
				theWindow[i] = "Robbery in progress";

			for (i = 1; i < theWindowColor.Length; i++)
				SetColor(i, XleColor.Yellow);

		}

		private int SetTitle()
		{
			int i = 0;
			theWindow[0] = ShopName;
			return i;
		}
		private void PayForMail(Player player)
		{
			int gold = XleCore.random.Next(1, 4);

			switch (gold)
			{
				case 1: gold = 95; break;
				case 2: gold = 110; break;
				case 3: gold = 125; break;
			}

			g.AddBottom("");
			g.AddBottom("Thanks for the delivery. ");
			g.AddBottom("Here's " + gold.ToString() + " gold.");
			g.AddBottom("");
			g.AddBottom("");

			StoreSound(LotaSound.Good);
			g.UpdateBottom("        Press Key to Continue");
			WaitForKey();

			player.Gold += gold;
			player.ItemCount(9, -1);
			player.mailTown = 0;
		}

		int robCount;

		public override bool Rob(Player player)
		{
			this.player = player;

			SetTitle();
			Wait(1);
			SetWindow(0);

			g.ClearBottom();

			if (robCount < 4)
			{
				robCount++;
				robbing = true;

				int choice = XleCore.random.Next(1, 16) + XleCore.random.Next(20, 36);

				g.AddBottom("");
				g.AddBottom("Stole " + choice.ToString() + " days of food.", XleColor.Yellow);

				player.Food += choice;
				SoundMan.PlaySound(LotaSound.Sale);

				if (XleCore.random.NextDouble() < 0.25)
					robCount = 4;

			}
			else
			{
				g.AddBottom("");
				g.AddBottom("No items within reach now.", XleColor.Yellow);

				SoundMan.PlaySound(LotaSound.Medium);
			}

			g.AddBottom();
			Wait(2000);

			return true;
		}
	}

	public class StoreVault : Store
	{
		public override bool Speak(Player player)
		{
			return false;
		}
	}

	public class StoreMagic : Store
	{
		protected override void GetColors(out Color backColor, out Color borderColor, out Color lineColor,
			out Color fontColor, out Color titleColor)
		{
			backColor = XleColor.LightBlue;
			borderColor = XleColor.Cyan;
			lineColor = XleColor.Yellow;
			fontColor = XleColor.Cyan;
			titleColor = XleColor.White;
		}
		public override bool Speak(Player player)
		{
			if (CheckLoan(player, true))
				return true;

			this.player = player;

			LeftOffset = 7;

			theWindow[0] = ShopName;
			BottomBackgroundColor = XleColor.Blue;

			int i = 1;
			theWindow[i++] = "";
			SetColor(i, XleColor.Blue);
			theWindow[i++] = "General Purpose      Prices";
			theWindow[i++] = "";
			theWindow[i++] = "1. Magic flame        " + MagicPrice(1);
			theWindow[i++] = "2. Firebolt           " + MagicPrice(2);
			theWindow[i++] = "";
			SetColor(i, XleColor.Blue);
			theWindow[i++] = "Dungeon use only     Prices";
			theWindow[i++] = "";
			theWindow[i++] = "3. Befuddle spell     " + MagicPrice(3);
			theWindow[i++] = "4. Psycho strength    " + MagicPrice(4);
			theWindow[i++] = "5. Kill Flash         " + MagicPrice(5);
			theWindow[i++] = "";
			SetColor(i, XleColor.Blue);
			theWindow[i++] = "Outside use only     Prices";
			theWindow[i++] = "";
			theWindow[i++] = "6. Seek spell         " + MagicPrice(6);

			g.ClearBottom();
			g.AddBottom("Make choice (hit 0 to cancel)");
			g.AddBottom();

			int choice = QuickMenu(new MenuItemList("0", "1", "2", "3", "4", "5", "6"), 2);

			if (choice == 0)
				return true;

			int maxCount = player.Gold / MagicPrice(choice);

			int purchaseCount = XleCore.ChooseNumber(maxCount);

			if (purchaseCount == 0)
				return true;




			return true;
		}

		int MagicPrice(int index)
		{
			int[] prices = { 32, 63, 152, 189, 379, 51 };

			return (int)(XleCore.MagicSpells[index].BasePrice * this.CostFactor);
		}
	}

}
