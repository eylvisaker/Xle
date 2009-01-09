using System;
using System.Collections.Generic;
using System.Text;

using ERY.AgateLib;
using ERY.AgateLib.Geometry;

namespace ERY.Xle
{
    [Serializable]
    public abstract class XleEvent
    {
        private Rectangle rect;

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

        public XleEvent()
        {
            /*
            data = new byte[g.map.SpecialDataLength() + 1];

            data[0] = 0;

            robbed = 0;
            id = -1;
            */
        }

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


    namespace XleEventTypes
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
            public Point Location
            {
                get { return mLocation; }
                set { mLocation = value; }
            }
            /// <summary>
            /// What map ID to go to.
            /// </summary>
            public int MapID
            {
                get { return mMapID; }
                set { mMapID = value; }
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
                        player.SetPos(mLocation.X,  this.mLocation.Y);

                        g.ClearBottom();

                        CheckLoan(player);
                    }
                    catch
                    {
                        SoundMan.PlaySound(LotaSound.Medium);

                        ColorStringBuilder builder = new ColorStringBuilder();

                        builder.AddText("Failed to load ", Color.White);
                        builder.AddText(newTownName, Color.Red);
                        builder.AddText(".", Color.White);

                        g.AddBottom(builder);
                        g.AddBottom("");

                        XleCore.wait(1500);
                    }
                }

                return true;
            }

            protected static void CheckLoan(Player player)
            {
                if (XleCore.Map.HasSpecialType(typeof(StoreLending)))
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

            protected  bool OpenImpl(Player player, bool isTaking)
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
                    string itemName = XleCore.ItemList[Contents].Name ;

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
                    theWindowColor[i] = Color.White;
            }
            public string ShopName
            {
                get { return mShopName; }
                set { mShopName = value; }
            }

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

            protected string[] theWindow = new string[20];
            protected Color[] theWindowColor = new Color[20];

            protected virtual void GetColors(out Color backColor, out Color borderColor,
                out Color lineColor, out Color fontColor, out Color titleColor)
            {
                backColor = Color.Green;
                borderColor = Color.Brown;
                lineColor = Color.Yellow;
                fontColor = Color.White;
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
                Display.FillRect(0, 296, 640, 400 - 296, Color.Black);

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

                XleCore.WriteText(320 - (tempString.Length / 2) * 16, 18 * 16, tempString, Color.White);

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
            protected bool CheckLoan(Player player, bool displayMessage)
            {
                if (player.loan > 0 && player.dueDate - player.TimeDays <= 0)
                    mLoanOverdue = true;

                if (mLoanOverdue && displayMessage)
                {
                    g.AddBottom("");
                    g.AddBottom("Sorry.  I Can't talk to you.");
                    XleCore.wait(500);
                }

                return true;
            }
        }

        [Serializable]
        public class StoreLending : Store
        {
            protected override void GetColors(out Color backColor, out Color borderColor,
                out Color lineColor, out Color fontColor, out Color titleColor)
            {
                backColor = Color.DarkGray;
                borderColor = Color.Gray;
                lineColor = Color.Yellow;
                fontColor = Color.White;
                titleColor = Color.Yellow;
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
                        for (i = 0; i < 13; i++, color[i] = Color.White) ;
                        for (; i < 40; i++, color[i] = Color.Yellow) ;

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
            protected override void GetColors(out Color backColor, out Color borderColor,
                out Color lineColor, out Color fontColor, out Color titleColor)
            {
                backColor = Color.DarkGray;
                borderColor = Color.Green;
                lineColor = Color.Yellow;
                fontColor = Color.White;
                titleColor = Color.Yellow;
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
                backColor = Color.Brown;
                borderColor = Color.Orange;
                lineColor = Color.Yellow;
                fontColor = Color.White;
                titleColor = Color.White;
            }

            public override bool Speak(Player player)
            {
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

                    if (choice == 0)			// buy weapon
                    {
                        g.AddBottom("");
                        g.AddBottom("Nothing purchased");
                        g.AddBottom("");

                        StoreSound(LotaSound.Medium);
                    }
                    else
                    {
                        // spend the cash, if they have it
                        if (player.Spend(priceList[choice]))
                        {
                            if (player.AddWeapon(itemList[choice], qualList[choice]))
                            {
                                tempString = g.QualityName(qualList[choice]);
                                tempString += " ";
                                tempString += g.WeaponName(itemList[choice]);
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
                }
                else if (choice == 1)		// sell weapon
                {

                }

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
            public override bool AllowedOnMapType(XleMap type)
            {
                if (type.Equals(typeof(XleMapTypes.Town)))
                    return true;
                else
                    return false;
            }
            public override bool Speak(Player player)
            {
                int choice;
                int raftCost = (int)(220 * this.CostFactor);
                int gearCost = (int)(28 * this.CostFactor);
                MenuItemList theList = new MenuItemList("Yes", "No");

                g.AddBottom("** " + this.ShopName + " **", Color.Yellow);
                g.AddBottom("");
                g.AddBottom("Want to buy a raft for " + raftCost.ToString() + " gold?");

                choice = XleCore.QuickMenu(theList, 3, 1);

                if (choice == 0)
                {
                    // Purchase raft
                    if (player.Spend(raftCost))
                    {
                        (XleCore.Map as XleMapTypes.Town).BuyRaft(player);

                        g.AddBottom("Raft purchased.");
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

                }

                return true;
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
                MenuItemList theList = new MenuItemList("Yes", "No");
                int choice;

                g.AddBottom(this.ShopName, Color.Green);
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
                backColor = Color.DarkGray;
                borderColor = Color.Green;
                lineColor = Color.Yellow;
                fontColor = Color.Yellow;
                titleColor = Color.White;

            }

            public override bool Speak(Player player)
            {
                string tempString;
                int i = 0;
                double cost = 15 / player.Attribute[Attributes.charm];
                int choice;
                int max = (int)(player.Gold / cost);

                theWindow[i++] = " " + ShopName + " ";

                this.player = player;
                this.robbing = false;

                Wait(1);


                if (player.mailTown == XleCore.Map.MapID)
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
                else
                {
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
                        player.Spend((int)(choice * cost));

                        player.Food += choice;


                        g.AddBottom(choice + " days of food bought.");

                        StoreSound(LotaSound.Sale);

                        XleMapTypes.Town twn = XleCore.Map as XleMapTypes.Town ;

                        if (player.Item(9) == 0 && twn != null && twn.Mail.Count > 0)
                        {
                            int mMap = XleCore.random.Next (twn.Mail.Count);
                            int target;
                            int count = 0;

                            do
                            {
                                target = twn.Mail[mMap];

                                if (XleCore.GetMapName(target) != "")
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
                                return true;
                            }

                            SoundMan.PlaySound(LotaSound.Question);

                            g.AddBottom("");
                            g.AddBottom("Would you like to earn some gold?");

                            MenuItemList menu = new MenuItemList("Yes", "No");

                            choice = QuickMenu(menu, 2);

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
                    }
                    else
                    {
                        g.AddBottom("");
                        g.AddBottom("Nothing Purchased");

                        StoreSound(LotaSound.Medium );
                    }
                }

                return true;

            }


            public override bool Rob(Player player)
            {
                /*	if (g.map.SetRobbed(shop.dave.id) < 4)
                    {
                        g.map.SetRobbed(shop.dave.id, 1);

                        choice = rnd(1, 15) + rnd(20, 35);

                        g.AddBottom("");
                        g.AddBottom("Stole " + String(choice) + " days of food.");

                        player.Food(choice);

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
                            */
                return false;
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
        };
    }
}