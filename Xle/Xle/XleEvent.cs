using System;
using System.Collections.Generic;
using System.Text;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Serialization.Xle;
using System.ComponentModel;

namespace ERY.Xle
{
	[Serializable]
	public abstract class XleEvent : IXleSerializable
	{
		private Rectangle rect;

		[Browsable(false)]
		public Rectangle Rectangle
		{
			get { return rect; }
			set { rect = value; }
		}

		public int X
		{
			get { return rect.X; }
			set { rect.X = value; }
		}
		public int Y
		{
			get { return rect.Y; }
			set { rect.Y = value; }
		}
		public int Width
		{
			get { return rect.Width; }
			set { rect.Width = value; }
		}
		public int Height
		{
			get { return rect.Height; }
			set { rect.Height = value; }
		}

		/// <summary>
		/// Gets whether or not this type of event can be placed on
		/// the specified map type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public virtual bool AllowedOnMapType(XleMap type)
		{
			return true;
		}

		/// <summary>
		/// Gets whether or not this type of event allows the player
		/// to rob it when the town isn't angry at him.
		/// </summary>
		[Browsable(false)]
		public virtual bool AllowRobWhenNotAngry { get { return false; } }

		/// <summary>
		/// Method called when the player attempts to rob and should get the 
		/// message "the merchant won't let you rob."
		/// </summary>
		public virtual void RobFail()
		{
			g.AddBottom();
			g.AddBottom("The merchant won't let you rob.");

			XleCore.wait(1000);
		}
		public XleEvent()
		{
			/*
			data = new byte[g.map.SpecialDataLength() + 1];

			data[0] = 0;

			robbed = 0;
			id = -1;
			*/
		}

		#region IXleSerializable Members

		void IXleSerializable.WriteData(XleSerializationInfo info)
		{
			info.Write("X", rect.X);
			info.Write("Y", rect.Y);
			info.Write("Width", rect.Width);
			info.Write("Height", rect.Height);

			WriteData(info);
		}
		void IXleSerializable.ReadData(XleSerializationInfo info)
		{
			rect.X = info.ReadInt32("X");
			rect.Y = info.ReadInt32("Y");
			rect.Width = info.ReadInt32("Width");
			rect.Height = info.ReadInt32("Height");

			ReadData(info);
		}

		/// <summary>
		///  Override this in a derived class to write data to a map file.
		/// </summary>
		/// <param name="info"></param>
		protected virtual void WriteData(XleSerializationInfo info)
		{
		}
		/// <summary>
		///  Override this in a derived class to read data from a map file.
		/// </summary>
		/// <param name="info"></param>
		protected virtual void ReadData(XleSerializationInfo info)
		{
		}

		#endregion

		/// <summary>
		/// Function called when player speaks in a square inside or next
		/// to the LotaEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool Speak(Player player)
		{
			return false;
		}
		/// <summary>
		/// Function called when player executes Rob in a square inside or next
		/// to the LotaEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool Rob(Player player)
		{
			return false;
		}
		/// <summary>
		/// Function called when the player executes the Open command inside
		/// or next to the LotaEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool Open(Player player)
		{
			return false;
		}
		/// <summary>
		/// Function called when the player executes the Take command inside
		/// or next to the LotaEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool Take(Player player)
		{
			return false;
		}
		/// <summary>
		/// Function called when the player walks inside
		/// the LotaEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool StepOn(Player player)
		{
			return false;
		}
		/// <summary>
		/// Function called when the player tries to walk inside
		/// the LotaEvent.
		/// 
		/// This is before the position is updated.  Returns false to 
		/// block the player from stepping there, and true if the
		/// player can walk there.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		/// <returns></returns>
		public virtual bool TryToStepOn(Player player, int dx, int dy)
		{
			return true;
		}
		/// <summary>
		/// Function called when the player uses an item
		/// or next to the LotaEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual bool Use(Player player, int item)
		{
			return false;
		}
		/// <summary>
		/// Function called when the player eXamines next
		/// to the LotaEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool Xamine(Player player)
		{
			return false;
		}

	}
}

namespace ERY.Xle.XleEventTypes
{
	[Serializable]
	public class ChangeMapEvent : XleEvent
	{
		private int mMapID;
		private bool mAsk = true;
		private Point mLocation;
		private string mCommandText = "";

		/// <summary>
		/// Text used as a command.
		/// Use {0} to indicate town we are in, {1} to indicate town we are 
		/// going to.
		/// </summary>
		public string CommandText
		{
			get { return mCommandText; }
			set { mCommandText = value; }
		}

		/// <summary>
		/// Whether or not to ask the player to change maps
		/// </summary>
		public bool Ask
		{
			get { return mAsk; }
			set { mAsk = value; }
		}
		/// <summary>
		/// What point on the new map to go to
		/// </summary>
		[Browsable(false)]
		public Point Location
		{
			get { return mLocation; }
			set { mLocation = value; }
		}


		public int TargetX
		{
			get { return mLocation.X; }
			set { mLocation.X = value; }
		}
		public int TargetY
		{
			get { return mLocation.Y; }
			set { mLocation.Y = value; }
		}

		/// <summary>
		/// What map ID to go to.
		/// </summary>
		public int MapID
		{
			get { return mMapID; }
			set { mMapID = value; }
		}

		protected override void WriteData(XleSerializationInfo info)
		{
			base.WriteData(info);

			info.Write("MapID", MapID);
			info.Write("AskUser", mAsk);
			info.Write("TargetX", TargetX);
			info.Write("TargetY", TargetY);
		}
		protected override void ReadData(XleSerializationInfo info)
		{
			MapID = info.ReadInt32("MapID");
			mAsk = info.ReadBoolean("AskUser");
			TargetX = info.ReadInt32("TargetX");
			TargetY = info.ReadInt32("TargetY");
		}

		public override bool StepOn(Player player)
		{
			if (player.X < X) return false;
			if (player.Y < Y) return false;
			if (player.X >= X + Width) return false;
			if (player.Y >= Y + Height) return false;

			string line = "Enter ";
			string newTownName;
			int choice = 0;

			MenuItemList theList = new MenuItemList("Yes", "No");

			try
			{
				newTownName = XleCore.GetMapName(mMapID);
			}
			catch
			{
				SoundMan.PlaySound(LotaSound.Medium);

				g.AddBottom("");
				g.AddBottom("Map ID " + mMapID + " not found.");
				g.AddBottom("");

				XleCore.wait(1500);

				return false;
			}
			line += newTownName + "?";

			if (Ask)
			{
				g.AddBottom("");
				g.AddBottom(line);

				SoundMan.PlaySound(LotaSound.Question);

				choice = XleCore.QuickMenu(theList, 3);
			}
			else
			{
				g.AddBottom("");
				g.AddBottom("Leave " + XleCore.Map.MapName);

				XleCore.wait(1000);
			}
			if (string.IsNullOrEmpty(CommandText) == false)
			{
				g.AddBottom("");
				g.AddBottom(string.Format(CommandText, XleCore.Map.MapName, newTownName));

				g.AddBottom("");
				XleCore.wait(500);

				choice = 0;
			}


			if (choice == 0)
			{

				try
				{
					player.Map = mMapID;
					player.SetPos(mLocation.X, this.mLocation.Y);

					g.ClearBottom();

					CheckLoan(player);
				}
				catch (Exception e)
				{
					System.Diagnostics.Debug.WriteLine(e.Message);

					SoundMan.PlaySound(LotaSound.Medium);

					ColorStringBuilder builder = new ColorStringBuilder();

					builder.AddText("Failed to load ", XleColor.White);
					builder.AddText(newTownName, XleColor.Red);
					builder.AddText(".", XleColor.White);

					g.AddBottom(builder);
					g.AddBottom("");

					XleCore.wait(1500);
				}
			}

			return true;
		}

		protected static void CheckLoan(Player player)
		{
			if (XleCore.Map.HasEventType(typeof(StoreLending)))
			{
				if (player.loan > 0 && player.dueDate - player.TimeDays <= 0)
				{
					g.AddBottom("This is your friendly lender.");
					g.AddBottom("You owe me money!");

					XleCore.wait(1000);

				}
			}
		}
	}

	[Serializable]
	public abstract class ItemAvailableEvent : XleEvent
	{
		private bool mClosed = true;
		private bool mContainsItem = false;

		private int mContents = 100;

		public bool ContainsItem
		{
			get { return mContainsItem; }
			set { mContainsItem = value; }
		}
		public int Contents
		{
			get { return mContents; }
			set { mContents = value; }
		}

		public bool Closed
		{
			get { return mClosed; }
			set { mClosed = value; }
		}

		protected bool OpenImpl(Player player, bool isTaking)
		{
			Commands.UpdateCommand("Open Chest");

			if (Closed == false)
			{
				g.AddBottom("");
				g.AddBottom("Chest already open.");

				return true;
			}

			SoundMan.PlaySound(LotaSound.OpenChest);

			XleCore.wait(750);


			for (int j = this.Rectangle.Top; j < this.Rectangle.Bottom; j++)
			{
				for (int i = this.Rectangle.Left; i < this.Rectangle.Right; i++)
				{
					int m = XleCore.Map[i, j];

					if (m % 16 >= 11 && m % 16 < 14 && m / 16 >= 13 && m / 16 < 15)
					{
						XleCore.Map[i, j] = m - 3;
					}

				}
			}

			if (XleCore.Map is IHasGuards)
			{
				(XleCore.Map as IHasGuards).IsAngry = true;
			}

			if (ContainsItem)
			{
				int count = 1;
				string itemName = XleCore.ItemList[Contents].Name;

				//TODO: Loadstring (g.hInstance(), dave.data[1] + 19, tempChars, 40);

				if (Contents == 8)
					count = XleCore.random.Next(3, 6);

				player.ItemCount(Contents, count);

				g.AddBottom("");


				if (isTaking == false)
				{
					g.AddBottom("You find a " + itemName + "!");
					SoundMan.PlaySound(LotaSound.VeryGood);
				}
				else
					g.AddBottom("You take " + count.ToString() + " " + itemName + ".");


			}
			else
			{
				int gd = mContents;

				g.AddBottom("");
				g.AddBottom("You find " + gd.ToString() + " gold.");

				player.Gold += gd;
				SoundMan.PlaySound(LotaSound.Sale);
			}

			mClosed = false;

			XleCore.wait(500 + 200 * player.Gamespeed);

			return true;
		}
	}

	[Serializable]
	public class TakeEvent : ItemAvailableEvent
	{

		public override bool Take(Player player)
		{
			return OpenImpl(player, true);
		}

	}
	[Serializable]
	public class TreasureChestEvent : ItemAvailableEvent
	{

		public override bool Open(Player player)
		{
			return OpenImpl(player, false);
		}
	}

	[Serializable]
	public class Door : XleEvent
	{
		int mItem;

		public int RequiredItem
		{
			get { return mItem; }
			set { mItem = value; }
		}

		public override bool Use(Player player, int item)
		{
			g.AddBottom("");
			g.AddBottom("The door just laughs at you.");

			return true;
		}
	}
	[Serializable]
	public abstract class Store : XleEvent
	{
		private double mCostFactor = 1.0;
		private bool mLoanOverdue = false;
		private bool mRobbed = false;
		private string mShopName;

		public Store()
		{
			for (int i = 0; i < theWindowColor.Length; i++)
				theWindowColor[i] = XleColor.White;
		}
		public string ShopName
		{
			get { return mShopName; }
			set { mShopName = value; }
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
			OfferMuseumCoin(player);
			if (XleCore.random.Next(100) < 3 && robbing == false)
			{
				OfferMuseumCoin(player);
			}
		}
		public static void OfferMuseumCoin(Player player)
		{
			int amount = XleCore.random.Next(30) + 40;
			int coin = -1;
			int choice;
			MenuItemList menu = new MenuItemList("Yes", "No");

			if (player.Level == 1)
				coin = XleCore.random.Next(2);
			else if (player.Level <= 3)
				coin = XleCore.random.Next(3);

			if (coin == -1)
				return;

			coin += 17;

			g.AddBottom("Would you like to buy a ");
			XleCore.wait(1);

			g.AddBottom("museum coin for " + amount.ToString() + " gold?");
			XleCore.wait(1);

			g.AddBottom("");
			XleCore.wait(1);

			SoundMan.PlaySound(LotaSound.Question);

			choice = XleCore.QuickMenu(menu, 3, 0);

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

		protected string[] theWindow = new string[20];
		protected Color[] theWindowColor = new Color[20];

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
			Display.FillRect(0, 296, 640, 400 - 296, XleColor.Black);

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

				XleCore.WriteText(320 - (theWindow[i].Length / 2) * 16, i * 16, theWindow[i], theWindowColor[i]);
			}

			if (!robbing)
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

			Display.FillRect(320 - (tempString.Length / 2) * 16, 18 * 16, tempString.Length * 16, 16, storeBackColor);

			XleCore.WriteText(320 - (tempString.Length / 2) * 16, 18 * 16, tempString, XleColor.White);

			XleCore.DrawBottomText();

		}


		protected void Clear()
		{
			for (int i = 0; i < 18; i++)
			{
				theWindow[i] = "";
			}
		}

		protected void StoreSound(LotaSound sound)
		{
			SoundMan.PlaySound(sound);
			Wait(2000);
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
		/// <param name="displayMessage"></param>
		/// <returns></returns>
		protected bool CheckLoan(Player player, bool displayMessage)
		{
			if (XleCore.Map.HasEventType(typeof(XleEventTypes.StoreLending)) == false)
				return false;

			if (player.loan > 0 && player.dueDate - player.TimeDays <= 0)
			{
				if (displayMessage)
				{
					g.AddBottom("");
					g.AddBottom("Sorry.  I Can't talk to you.");
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

	[Serializable]
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
			borderColor = XleColor.Gray;
			lineColor = XleColor.Yellow;
			fontColor = XleColor.White;
			titleColor = XleColor.Yellow;
		}
		public override bool Speak(Player player)
		{
			int i = 0;
			int max = 200 * player.Level;
			int choice;

			this.player = player;
			robbing = false;

			theWindow[i++] = " Friendly ";
			theWindow[i++] = "";
			theWindow[i++] = "Lending Association";
			theWindow[i++] = "";
			theWindow[i++] = "";
			theWindow[i++] = "";
			theWindow[i++] = "";

			if (player.loan == 0)
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
					player.Gold += choice;
					player.loan = (int)(choice * 1.5);
					player.dueDate = (int)player.TimeDays + 120;

					g.AddBottom("Borrowed " + choice.ToString() + " gold.");
					g.AddBottom("");
					g.AddBottom("You'll owe " + player.loan.ToString() + " gold in 120 days!");

					StoreSound(LotaSound.Sale);

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
					DueDate = ((int)(player.dueDate - player.TimeDays)).ToString() + " days ";
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
					g.AddBottom("Loan Repaid");

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
	[Serializable]
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

	[Serializable]
	public class StoreWeapon : Store
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

			this.player = player;

			theWindow[i++] = " " + ShopName + " ";
			theWindow[i++] = "";
			theWindow[i++] = "Weapons";


			for (i = 1; i <= 8; i++)
			{
				itemList[i] = i;
				qualList[i] = i % 5;
				priceList[i] = g.WeaponCost(itemList[i], qualList[i]);
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

				theWindow[4] = "Items                   Prices";

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

			CheckOfferMuseumCoin(player);

			return true;
		}
	}

	[Serializable]
	public class StoreArmor : Store
	{
	}

	[Serializable]
	public class StoreWeaponTraining : Store
	{
	}
	[Serializable]
	public class StoreArmorTraining : Store
	{
	}

	[Serializable]
	public class StoreBlackjack : Store
	{
	}

	[Serializable]
	public class StoreRaft : Store
	{
		// map and coors that mark where a purchased raft shows up
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

						CheckOfferMuseumCoin(player);

						g.AddBottom("Board raft outside.");
					}
					else
					{
						g.AddBottom("Not enough gold.");
						SoundMan.PlaySound(LotaSound.Medium);
						XleCore.wait(750);

						CheckOfferMuseumCoin(player);
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
				CheckOfferMuseumCoin(player);

			}

			return true;
		}

		private bool CheckForNearbyRaft(Player player, bool skipRaft)
		{
			for (int i = 0; i < player.Rafts.Count; i++)
			{
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

	[Serializable]
	public class StoreHealer : Store
	{
	}

	[Serializable]
	public class StoreJail : Store
	{
	}
	[Serializable]
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

	[Serializable]
	public class StoreFlipFlop : Store
	{
	}

	[Serializable]
	public class StoreBuyback : Store
	{
	}

	[Serializable]
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
			}
			else
			{
				SetWindow(cost);

				tempString = "Maximum purchase ";
				tempString += max;
				tempString += " days";

				g.AddBottom("");
				g.AddBottom(tempString);

				choice = ChooseNumber(max);

				if (choice > 0)
				{
					player.Spend((int)(choice * cost));
					player.Food += choice;

					g.AddBottom(choice + " days of food bought.");

					StoreSound(LotaSound.Sale);

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
			int i = 1;
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

			if (robbing == false)
			{
				theWindow[i] = "Cost is ";
				theWindow[i] += cost;
				theWindow[i++] += " gold per 'day'";
			}
			else
				theWindow[i] = "Robbery in progress";
		}
		private int SetTitle()
		{
			int i = 0;
			theWindow[i++] = " " + ShopName + " ";
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

			player.GainGold(gold);
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

	[Serializable]
	public class StoreVault : Store
	{
	}

	[Serializable]
	public class StoreMagic : Store
	{
	}

	[Obsolete]
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
	}
}