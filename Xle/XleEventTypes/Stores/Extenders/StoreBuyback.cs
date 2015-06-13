using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace ERY.Xle.XleEventTypes.Stores.Extenders
{
	public class StoreBuyback : StoreFront
	{
		public override bool AllowInteractionWhenLoanOverdue { get { return true; } }

		protected override void InitializeColorScheme(ColorScheme cs)
		{
			cs.BackColor = XleColor.Pink;
			cs.FrameColor = XleColor.Yellow;
			cs.FrameHighlightColor = XleColor.Yellow;
			cs.TextAreaBackColor = XleColor.Brown;
			cs.BorderColor = XleColor.Red;
		}

		protected override bool SpeakImpl(GameState state)
		{
			RunStore(state);
			return true;
		}

		void RunStore(GameState state)
		{
			var player = state.Player;

			int i = 0;
			int choice;
			int amount;

			this.player = player;
			robbing = false;

			ClearWindow();
			Title = TheEvent.ShopName;

			var wind = new TextWindow();
			wind.Location = new AgateLib.Geometry.Point(9, 4);

			wind.WriteLine("I will happily purchase");
			wind.WriteLine("your used arms and armor");

			var prompt = new TextWindow();

			prompt.Location = new Point(9, 9);
			prompt.WriteLine("Choose items to sell:");
			prompt.WriteLine();
			prompt.WriteLine(" 1.  Weapons");
			prompt.WriteLine(" 2.  Armor");

			Windows.Add(wind);
			Windows.Add(prompt);

			wind.SetColor(XleColor.Red);
			prompt.SetColor(XleColor.Red);

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Select (0 to cancel)");
			XleCore.TextArea.PrintLine();

			MenuItemList theList = new MenuItemList("0", "1", "2");
			choice = QuickMenu(theList, 2, 0);

			if (choice == 0)
				return;

			Windows.Remove(prompt);
			wind.Visible = false;

			ColorScheme.FrameColor = XleColor.Gray;
			ColorScheme.HorizontalLinePosition = 11;
			Title = "";
			ShowGoldText = false;

			Equipment item = null;
			XleCore.TextArea.Clear();

			TextWindow questionWindow = new TextWindow { Location = new Point(5, 16) };

			Windows.Add(questionWindow);

			switch (choice)
			{
				case 1:
					questionWindow.WriteLine("What weapon will you sell me?");
					item = XleCore.PickWeapon(state, null, ColorScheme.BackColor);
					break;

				case 2:
					questionWindow.WriteLine("What armor will you sell me?");
					item = XleCore.PickArmor(state, null, ColorScheme.BackColor);
					break;
			}

			if (item == null)
				return;

			Windows.Remove(questionWindow);

			ColorScheme.HorizontalLinePosition = 14;
			ColorScheme.TextAreaBackColor = XleColor.Black;

			Title = "Buy-back shop";
			wind.Visible = true;
			wind.SetColor(XleColor.White);
			wind.Location = new Point(9, 8);

			var ta = XleCore.TextArea;

			TextWindow offerText = new TextWindow();
			offerText.Location = new Point(2, 16);
			
			int charm = player.Attribute[Attributes.charm];
			charm = Math.Min(charm, 80);

			int maxAccept = (int)(item.Price * Math.Pow(charm, .7) / 11);
			int offer = (int)((6 + XleCore.random.NextDouble())* maxAccept / 14.0);

			choice = MakeOffer(item, offer, false);

			if (choice == 0)
			{
				CompleteSale(offer, item);
				return;
			}
			int ask = 0;

			Windows.Add(offerText);

			SetOfferText(offerText, offer, ask);

			ask = GetAskingPrice();

			if (ask == 0)
			{
				ta.PrintLine("\n\n\n\nSee you later.\n");
				return;
			}
			if (ask < 1.5 * offer)
			{
				CompleteSale(ask, item);
				return;
			}

			int spread = maxAccept - offer;

			if (ask > spread + maxAccept)
			{
				ComeBackWhenSerious();
				return;
			}

			spread = ask - offer;
			double scale = maxAccept / (double)spread;
			offer = (int)(offer + (1 + XleCore.random.NextDouble() * 5) * scale);
			maxAccept = spread;

			if (offer >= ask)
				offer = ask - 1;

			int lastAsk = ask;

			do
			{
				bool finalOffer = false;

				SetAskRejectPrice(offerText, ask, WayTooHigh(ask, offer, maxAccept));
				choice = MakeOffer(item, offer, finalOffer);

				if (choice == 0)
				{
					CompleteSale(offer, item);
					return;
				}
				else if (finalOffer)
				{
					MaybeDealLater();
					return;
				}

				SetOfferText(offerText, offer, lastAsk);
				ask = GetAskingPrice();

                if (ask == 0)
                {
                    MaybeDealLater();
                    return;
                }

				if (ask == lastAsk || 
					(ask > lastAsk && XleCore.random.NextDouble() < 0.5))
				{
					ComeBackWhenSerious();
					return;
				}

				double diff = lastAsk - ask;
				if (diff == 0) diff = XleCore.random.NextDouble() * 3;

				if (diff / maxAccept < 0.03)
					diff /= 1.3;

				lastAsk = ask;
				spread = (int)(offer + diff / 1.2 + XleCore.random.NextDouble() * diff / 1.6);

				if (spread > ask - 2 && XleCore.random.NextDouble() < .5)
				{
					CompleteSale(ask, item);
					return;
				}
				if (spread >= ask)
				{
					finalOffer = true;
				}
				else
				{
					offer = spread;

					if (ask - offer < 3)
						finalOffer = true;

					if (offer <= 0)
					{
						ComeBackWhenSerious();
						return;
					}
				}

			} while (true);
		}

		private static int GetAskingPrice()
		{
			XleCore.TextArea.Clear();
			
			return XleCore.ChooseNumber(32767);
		}

		private int MakeOffer(Equipment item, int offer, bool finalOffer)
		{
			var ta = XleCore.TextArea;

			ta.Clear();
			ta.PrintLine("I'll give " + offer + " gold for your");
			ta.Print(item.NameWithQuality);

			if (finalOffer)
			{
				ta.PrintLine(" -final offer!!!", XleColor.Yellow);
			}
			else
				ta.PrintLine(".");

			return XleCore.QuickMenuYesNo(true);
		}

		private bool WayTooHigh(double ask, int offer, int maxAccept)
		{
			return (ask / offer) > 1.4 && (ask / maxAccept > 1.3);
		}

		private void SetAskRejectPrice(TextWindow offerWind, int ask, bool wayTooHigh)
		{
			var clr = wayTooHigh ? XleColor.Yellow : XleColor.Cyan;

			offerWind.Clear();
			offerWind.WriteLine(" " + ask + " is " +
				(wayTooHigh ? "way " : "") + "too high!", clr);
		}

		private void SetOfferText(TextWindow offerWind, int offer, int ask)
		{
			offerWind.Clear();
			offerWind.Write("My latest offer: ", XleColor.White);
			offerWind.WriteLine(offer.ToString(), XleColor.Cyan);

			if (ask > 0)
			{
				offerWind.Write("You asked for: ");
				offerWind.WriteLine(ask.ToString(), XleColor.Cyan);
			}
			else
				offerWind.WriteLine();

			offerWind.Write("What will you sell for? ");
			offerWind.Write("(0 to quit)", XleColor.Purple);
		}

		private void ComeBackWhenSerious()
		{
			XleCore.TextArea.Clear();
			XleCore.TextArea.PrintLine("Come back when you're serious.");

			Wait(1500);
		}
		private void MaybeDealLater()
		{
			XleCore.TextArea.Clear();
			XleCore.TextArea.PrintLine("Maybe we can deal later.");

			Wait(1500);
		}

		private void CompleteSale(int offer, Equipment item)
		{
			XleCore.TextArea.Clear();
			XleCore.TextArea.PrintLine("It's a deal!");
			XleCore.TextArea.PrintLine(item.BaseName + " sold for " + offer + " gold.");

			player.Gold += offer;
			player.RemoveEquipment(item);

			StoreSound(LotaSound.Sale);
		}
	}
}
