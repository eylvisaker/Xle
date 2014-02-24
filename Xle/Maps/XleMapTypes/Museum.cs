using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using AgateLib;
using AgateLib.InputLib;
using AgateLib.Geometry;
using AgateLib.Geometry.VertexTypes;
using AgateLib.Serialization.Xle;
using AgateLib.DisplayLib;

using Vertex = AgateLib.Geometry.VertexTypes.PositionTextureNormalTangent;

namespace ERY.Xle.XleMapTypes
{
	public class Museum : Map3D
	{
		int[] mData;
		int mHeight;
		int mWidth;
		Dictionary<int, MuseumDisplays.Exhibit> mExhibits = new Dictionary<int, MuseumDisplays.Exhibit>();

		public Museum()
		{
			mExhibits.Add(0x50, new MuseumDisplays.Information());
			mExhibits.Add(0x51, new MuseumDisplays.Welcome());
			mExhibits.Add(0x52, new MuseumDisplays.Weaponry());
			mExhibits.Add(0x53, new MuseumDisplays.Thornberry());
			mExhibits.Add(0x54, new MuseumDisplays.Fountain());
			mExhibits.Add(0x55, new MuseumDisplays.PirateTreasure());
			mExhibits.Add(0x56, new MuseumDisplays.HerbOfLife());
			mExhibits.Add(0x57, new MuseumDisplays.NativeCurrency());
			mExhibits.Add(0x58, new MuseumDisplays.StonesWisdom());
			mExhibits.Add(0x59, new MuseumDisplays.Tapestry());
			mExhibits.Add(0x5A, new MuseumDisplays.LostDisplays());
			mExhibits.Add(0x5B, new MuseumDisplays.KnightsTest());
			mExhibits.Add(0x5C, new MuseumDisplays.FourJewels());
			mExhibits.Add(0x5D, new MuseumDisplays.Guardian());
			mExhibits.Add(0x5E, new MuseumDisplays.Pegasus());
			mExhibits.Add(0x5F, new MuseumDisplays.AncientArtifact());
		}

		protected override void ReadData(XleSerializationInfo info)
		{
			mWidth = info.ReadInt32("Width");
			mHeight = info.ReadInt32("Height");
			mData = info.ReadInt32Array("Data");
		}
		protected override void WriteData(XleSerializationInfo info)
		{
			info.Write("Width", mWidth, true);
			info.Write("Height", mHeight, true);
			info.Write("Data", mData, NumericEncoding.Csv);
		}
		public override string[] MapMenu()
		{
			List<string> retval = new List<string>();

			retval.Add("Armor");
			retval.Add("Fight");
			retval.Add("Gamespeed");
			retval.Add("Hold");
			retval.Add("Inventory");
			retval.Add("Pass");
			retval.Add("Rob");
			retval.Add("Speak");
			retval.Add("Take");
			retval.Add("Use");
			retval.Add("Weapon");
			retval.Add("Xamine");

			return retval.ToArray();
		}
		public override IEnumerable<string> AvailableTileImages
		{
			get { yield return "DungeonTiles.png"; }
		}
		public override void InitializeMap(int width, int height)
		{
			mData = new int[height * width];
			mHeight = height;
			mWidth = width;
		}
		
		public override int Height
		{
			get { return mHeight; }
		}
		public override int Width
		{
			get { return mWidth; }
		}

		public override int this[int xx, int yy]
		{
			get
			{

				if (yy < 0 || yy >= Height || xx < 0 || xx >= Width)
				{
					return 0;
				}
				else
				{
					return mData[yy * mWidth + xx];
				}
			}
			set
			{
				if (yy < 0 || yy >= Height ||
					xx < 0 || xx >= Width)
				{
					return;
				}
				else
				{
					mData[yy * mWidth + xx] = value;
				}

			}
		}

		protected override double StepQuality
		{
			get
			{
				return 0.5;
			}
		}

		protected override Extenders.IMapExtender CreateExtenderImpl()
		{
			if (XleCore.Factory == null)
				return base.CreateExtenderImpl();

			return XleCore.Factory.CreateMapExtender(this);
		}
		public override void OnLoad(Player player)
		{
			base.OnLoad(player);

			CheckExhibitStatus(player);

			// face the player in a direction with an open passage
			// or to the nearest exhibit.
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					if (Math.Abs(i) + Math.Abs(j) == 2) continue;
					if (i == 0 && j == 0) continue;

					Point loc = new Point(player.X + i, player.Y + j);

					if (IsPassable(this[loc.X, loc.Y]))
					{
						player.FaceDirection = DirectionFromPoint(new Point(i, j));
					}
					else if (IsExhibit(this[loc.X, loc.Y]))
					{
						player.FaceDirection = DirectionFromPoint(new Point(i, j));
						goto exitloop;
					}
				}
			}

		exitloop:

			// check to see if the caretaker wants to see the player
			var info = (MuseumDisplays.Information)mExhibits[0x50];

			if (info.ShouldLevelUp(player))
			{
				g.ClearBottom();
				g.AddBottom("The caretaker wants to see you!");

				SoundMan.PlaySound(LotaSound.Good);

				while (SoundMan.IsPlaying(LotaSound.Good))
					XleCore.Wait(50);
			}
		}
		protected override void OnPlayerEnterPosition(Player player, int x, int y)
		{
			if (x == 12 && y == 13)
			{
				if (player.museum[1] < 3)
				{
					(mExhibits[0x51] as MuseumDisplays.Welcome).PlayGoldArmbandMessage(player);
					player.museum[1] = 3;

					CheckExhibitStatus(player);
				}
			}
		}

		protected override Color ExtraColor(Point location)
		{
			var ex = ExhibitAt(location.X, location.Y);

			if (ex == null)
				return base.ExtraColor(location);

			return ex.ExhibitColor;
		}

		protected override Map3D.ExtraType GetExtraType(int val, int side)
		{
			if (val >= 0x50 && val <= 0x5f)
			{
				if (side == -1) return ExtraType.DisplayCaseLeft;
				if (side == 1) return ExtraType.DisplayCaseRight;
			}
			if (val == 1)
			{
				if (side == -1) return ExtraType.TorchLeft;
				if (side == 1) return ExtraType.TorchRight;
			}

			return ExtraType.None;
		}
		protected override bool ExtraScale
		{
			get
			{
				return true;
			}
		}
		private bool IsExhibitAt(Point location)
		{
			return IsExhibit(this[location.X, location.Y]);
		}
		private bool IsExhibit(int value)
		{
			if ((value & 0xf0) == 0x50)
				return true;
			else
				return false;
		}

		#region --- Museum Exhibits ---

		MuseumDisplays.Exhibit mCloseup;
		bool mDrawStatic;

		protected override void DrawCloseupImpl(Rectangle inRect)
		{
			Rectangle displayRect = new Rectangle(inRect.X + 64, inRect.Y + 64, 240, 128);

			Surfaces.MuseumExhibitStatic.Draw(displayRect);
			Surfaces.MuseumExhibitCloseup.Draw(inRect);

			DrawExhibitText(inRect, mCloseup);

			if (mDrawStatic == false)
			{
				mCloseup.Draw(displayRect);
			}
		}

		private MuseumDisplays.Exhibit ExhibitAt(int x, int y)
		{
			int tileAt = this[x, y];

			if (mExhibits.ContainsKey(tileAt) == false)
				return null;

			return mExhibits[tileAt];
		}
		
		private bool InteractWithDisplay(Player player)
		{
			Point stepDir = StepDirection(player.FaceDirection);

			MuseumDisplays.Exhibit ex = ExhibitAt(player.X + stepDir.X, player.Y + stepDir.Y);

			if (ex == null)
				return false;

			DrawCloseup = true;
			mCloseup = ex;
			mDrawStatic = ex.StaticBeforeCoin;

			g.AddBottom("You see a plaque.  It Reads...");
			g.AddBottom();
			g.AddBottomCentered(ex.LongName, ex.TextColor);

			XleCore.PromptToContinueOnWait = true;

			if (ex.IsClosed(player))
			{
				g.AddBottomCentered("- Exhibit closed -", ex.TextColor);
				g.AddBottom();
				XleCore.WaitForKey();

				return true;
			}

			g.AddBottomCentered(ex.CoinString, ex.TextColor);
			g.AddBottom();
			XleCore.WaitForKey();

			if (ex.RequiresCoin == false)
			{
				mDrawStatic = false;
				RunExhibit(player, ex);
			}
			else
			{
				if (player.museum[(int)ex.ExhibitID] == 0)
					g.AddBottom("You haven't used this exhibit.");
				else
					g.AddBottom();

				if (PlayerHasCoin(player, ex.Coin) == false)
				{
					g.AddBottom("You'll need a " + ex.Coin.ToString() + " coin.");
					XleCore.Wait(500);

					return true;
				}
				else
				{
					g.AddBottom("insert your " + ex.Coin.ToString() + " coin?");
					g.AddBottom();

					int choice = XleCore.QuickMenu(new MenuItemList("Yes", "no"), 3);

					if (choice == 1)
						return true;

					UseCoin(player, ex.Coin);

					mDrawStatic = false;
					RunExhibit(player, ex);
				}
			}

			return true;
		}

		private void RunExhibit(Player player, MuseumDisplays.Exhibit ex)
		{
			ex.PlayerXamine(player);

			if (player.museum[(int)ex.ExhibitID] == 0)
				player.museum[(int)ex.ExhibitID] = 1;

			CheckExhibitStatus(player);
		}

		private void CheckExhibitStatus(Player player)
		{
			// lost displays
			if (player.museum[0xa] > 0)
			{
				for (int i = 0; i < Width; i++)
				{
					for (int j = 0; j < Height; j++)
					{
						if (this[i, j] == 0x5a)
							this[i, j] = 0x10;
					}
				}
			}

			// welcome exhibit
			if (player.museum[1] == 0)
			{
				this[4, 1] = 0;
				this[3, 10] = 0;
			}
			else if (player.museum[1] == 1)
			{
				this[4, 1] = 0;
				this[3, 10] = 16;
			}
			else
			{
				this[4, 1] = 16;
				this[3, 10] = 16;
			}
		}

		private void UseCoin(Player player, MuseumDisplays.Coin coin)
		{

		}

		private bool PlayerHasCoin(Player player, MuseumDisplays.Coin coin)
		{
			return true;
		}


		#endregion

		public override bool PlayerXamine(Player player)
		{
			g.AddBottom();

			if (InteractWithDisplay(player))
				return true;

			g.AddBottom("You are in an ancient museum.");

			return true;
		}
		public override bool PlayerUse(Player player, int item)
		{
			// twist gold armband
			if (item == 1)
			{
				Point faceDir = StepDirection(player.FaceDirection);
				Point test = new Point(player.X + faceDir.X, player.Y + faceDir.Y);

				// door value
				if (this[test.X, test.Y] == 0x02)
				{
					XleCore.Wait(1000);

					player.SetMap(1, 114, 42);

					g.ClearBottom();
				}
				else
				{
					g.AddBottom("The gold armband hums softly.");
				}

				return true;
			}

			return false;
		}
		public override bool PlayerFight(Player player)
		{
			g.AddBottom();
			g.AddBottom("There is nothing to fight.");

			return true;
		}
		public override bool PlayerRob(Player player)
		{
			g.AddBottom();
			g.AddBottom("There is nothing to rob.");

			return true;
		}
		protected override bool PlayerSpeakImpl(Player player)
		{
			g.AddBottom();
			g.AddBottom("There is no reply.");

			return true;
		}
		public override bool PlayerTake(Player player)
		{
			g.AddBottom();
			g.AddBottom("There is nothing to take.");

			return true;
		}

		protected override void DrawMuseumExhibit(int distance, Rectangle destRect, int val)
		{
			var exhibit = mExhibits[val];
			Color clr = exhibit.ExhibitColor;

			DrawExhibitStatic(destRect, clr, distance);

			if (distance == 1)
			{
				DrawExhibitText(destRect, exhibit);
			}
		}

		private static void DrawExhibitText(Rectangle destRect, MuseumDisplays.Exhibit exhibit)
		{
			int px = 176;
			int py = 208;

			int textLength = exhibit.Name.Length;

			px -= (textLength / 2) * 16;

			px += destRect.X;
			py += destRect.Y;

			AgateLib.DisplayLib.Display.FillRect(px, py, textLength * 16, 16, Color.Black);

			Color clr = exhibit.TextColor;
			XleCore.WriteText(px, py, exhibit.Name, clr);
		}

		int anim;
		int offset = 0;

		private void DrawExhibitStatic(Rectangle destRect, Color clr, int distance)
		{
			Rectangle destOffset = new Rectangle(96, 96, 160, 96);

			if (distance == 2)
			{
				destOffset.X = 128;
				destOffset.Y = 112;
				destOffset.Width = 112;
				destOffset.Height = 64;
			}

			Rectangle srcRect = new Rectangle(0, 0, destOffset.Width, destOffset.Height);
			Rectangle oldDest = destRect;

			oldDest.X += destOffset.X;
			oldDest.Y += destOffset.Y;
			oldDest.Width = srcRect.Width;
			oldDest.Height = srcRect.Height;

			int freq = 5;

			if (1 == 1)
			{
				srcRect.X = offset;
				srcRect.Width -= srcRect.X;

				destRect.X += destOffset.X;
				destRect.Y += destOffset.Y;
				destRect.Width = srcRect.Width;
				destRect.Height = srcRect.Height;

				Surfaces.MuseumExhibitStatic.Color = clr;
				Surfaces.MuseumExhibitStatic.Draw(srcRect, destRect);

				destRect = Rectangle.FromLTRB(destRect.Right, destRect.Top, oldDest.Right, destRect.Bottom);
				srcRect.X = 0;
				srcRect.Width = destRect.Width;

				Surfaces.MuseumExhibitStatic.Draw(srcRect, destRect);
			}
			else
			{

				destRect.X += destOffset.X;
				destRect.Y += destOffset.Y;
				destRect.Width = srcRect.Width;
				destRect.Height = srcRect.Height;

				Surfaces.MuseumExhibitStatic.Color = clr;
				Surfaces.MuseumExhibitStatic.Draw(srcRect, destRect);
			}

			anim++;
			if (anim % freq == 0)
				offset = XleCore.random.Next((destOffset.Width - 16) / 4) * 4;
		}

		protected override void PlayPlayerMoveSound()
		{
			SoundMan.PlaySound(LotaSound.WalkMuseum);
		}
	}
}